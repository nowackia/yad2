using System;
using System.Collections.Generic;
using System.Text;
using Yad.Engine.Client;
using Yad.Config.Common;
using Yad.Config;
using Yad.UI.Client;
using Yad.Board;
using Yad.Net.Client;
using Yad.UI;
using System.Collections;

namespace Yad.Engine {
    public delegate void CreateUnitHandler(int createObjectID, short typeID);
    public class BuildManager {

        GameLogic _gameLogic;
        BuildStripe _leftStripe;
        BuildStripe _rightStripe;
        Dictionary<int, Dictionary<short, BuildStatus>> _stripData = new Dictionary<int, Dictionary<short, BuildStatus>>();
        int _currentObjectID=-1;
        List<BuildStatus> _buildList=null;
        object cuLock = new object();

        private event CreateUnitHandler _createUnit = null;

        public BuildManager(GameLogic gameLogic, BuildStripe leftStripe, BuildStripe rightStripe) {
            _gameLogic = gameLogic;
            _leftStripe = leftStripe;
            _rightStripe = rightStripe;
            _buildList = new List<BuildStatus>();
            InitRightStripe();
        }

        public event CreateUnitHandler CreateUnit {
            add {
                lock(cuLock)
                    _createUnit += value;
            }
            remove {
                lock (cuLock)
                    _createUnit -= value;
            }
        }
        public void InitRightStripe() {
            foreach (short id in GlobalSettings.Wrapper.buildingsMap.Keys) {
                BuildingData bdata = GlobalSettings.Wrapper.buildingsMap[id];
                foreach (String bname in bdata.BuildingsCanProduce) {
                    short idb = GlobalSettings.Wrapper.namesToIds[bname];
                    if (!_rightStripe.Contains((int)idb))
                        _rightStripe.Add((int)idb, bname, bname, true);

                }
                foreach (String uname in bdata.UnitsCanProduce) {
                    short idu = GlobalSettings.Wrapper.namesToIds[uname];
                    if (!_rightStripe.Contains((int)idu))
                        _rightStripe.Add((int)idu, uname, uname, false);
                }
            }
            _rightStripe.HideAll();
        }

        public void ProcessTurn() {
            Yad.Log.Common.InfoLog.WriteInfo("BuildManager ProcessTurn start");
            lock (((ICollection)_buildList).SyncRoot) {
                for (int i = 0; i < _buildList.Count; ++i) {
                    BuildStatus bs = _buildList[i];
                    if (bs.DoTurn()) {
                        _buildList.Remove(bs);
                        --i;
                        OnBuildEnd(bs);
                    }
                    else
                        if (bs.ObjectId == _currentObjectID)
                            _rightStripe.Update(bs);
                }
            }
            Yad.Log.Common.InfoLog.WriteInfo("BuildManager ProcessTurn end");
        }

        public void OnBuildEnd(BuildStatus bstatus) {
            if (bstatus.BuildType == BuildType.Unit) {
                lock (cuLock) {
                    if (_createUnit != null)
                        _createUnit(bstatus.ObjectId, bstatus.Typeid);
                }
                ActivateForObject(bstatus.ObjectId);
                if (_currentObjectID == bstatus.ObjectId)
                    UpdateView(_currentObjectID, false);
            }
            if (bstatus.BuildType == BuildType.Building) {
                bstatus.State = StripButtonState.Ready;
                if (_currentObjectID == bstatus.ObjectId)
                    _rightStripe.Update(bstatus);
            }
        }

        private void ActivateForObject(int objectID) {
            foreach (BuildStatus bs in _stripData[objectID].Values) {
                bs.State = StripButtonState.Active;
            }
        }

        public void RemoveBuilding(ObjectID objectID, short typeID) {
            if (_stripData.ContainsKey(objectID.ObjectId)) {
                _leftStripe.Remove(objectID.ObjectId);
                _stripData.Remove(objectID.ObjectId);
                lock (((ICollection)_buildList).SyncRoot) {
                    for (int i = 0; i < _buildList.Count; ++i) {
                        BuildStatus bs = _buildList[i];
                        if (bs.ObjectId == objectID.ObjectId) {
                            _buildList.Remove(bs);
                            --i;
                        }
                    }
                }
                foreach (int key in _stripData.Keys) {
                    ObjectID obj = new ObjectID(_gameLogic.CurrentPlayer.Id, key);
                    UpdateDependencies(obj, _gameLogic.CurrentPlayer.GetBuilding(obj).TypeID);
                }
                if (_currentObjectID == objectID.ObjectId) {
                    _currentObjectID = -1;
                    UpdateView(_currentObjectID, false);
                }
            }
            
        }
        public void AddBuilding(ObjectID objectID, short typeID) {
            BuildingData bs = GlobalSettings.Wrapper.buildingsMap[typeID];
            string name = GlobalSettings.Wrapper.buildingsMap[typeID].Name;
            if (bs.BuildingsCanProduce.Count != 0 || bs.UnitsCanProduce.Count != 0) {
                _leftStripe.Add(objectID.ObjectId, name, name, true);
                _stripData.Add(objectID.ObjectId, new Dictionary<short, BuildStatus>());
            }
            
            foreach (int key in _stripData.Keys) {
                ObjectID obj = new ObjectID(_gameLogic.CurrentPlayer.Id, key);
                if (key == objectID.ObjectId)
                    UpdateDependencies(obj, typeID);
                else
                    UpdateDependencies(obj, _gameLogic.CurrentPlayer.GetBuilding(obj).TypeID);
            }
            if (_currentObjectID != -1)
                UpdateView(_currentObjectID, false);
        }

        public void SwitchCurrentBuilding(int id) {
            if (_currentObjectID != id) {
                _currentObjectID = id;
                UpdateView(id, true);
               
            }
        }

        public void UpdateView(int id, bool rewind) {
            if (id == -1) {
                _rightStripe.HideAll();
                return;
            }
            List<BuildStatus> listStatus = new List<BuildStatus>(_stripData[id].Values);
            _rightStripe.SwitchUpdate(listStatus, rewind);
        }
        public bool RightBuildingClick(int id) {
            StripButtonState state = _stripData[_currentObjectID][(short)id].State;
            switch (state) {
                case StripButtonState.Active:
                    RightBuildActiveClick(id);
                    return false;
                case StripButtonState.Ready:
                    RightBuildReadyClick(id);
                    return true;

            }
            return false;
        }

        private void RightBuildActiveClick(int id) {
            BuildStatus bs = _stripData[_currentObjectID][(short)id];
            if (bs.BuildType == BuildType.Unit) {
                bs.State = StripButtonState.Percantage;
                lock(((ICollection)_buildList).SyncRoot)
                    _buildList.Add(bs);
                DeactivateOther(bs.Typeid);
            }
            if (bs.BuildType == BuildType.Building) {
                //_stripData[_currentObjectID][(short)id].State = Yad.UI.StripButtonState.Ready;
                bs.State = StripButtonState.Percantage;
                lock (((ICollection)_buildList).SyncRoot)
                    _buildList.Add(bs);
                DeactivateOther(bs.Typeid);
            }
           
            UpdateView(_currentObjectID,false);
        }

        private void DeactivateOther(short typeid) {
            foreach (BuildStatus bs in _stripData[_currentObjectID].Values) {
                if (typeid != bs.Typeid)
                    bs.State = Yad.UI.StripButtonState.Inactive;
            }
        }
        private void RightBuildReadyClick(int id) {
            foreach (BuildStatus bs in _stripData[_currentObjectID].Values) {
                bs.State = Yad.UI.StripButtonState.Active;
            }
            UpdateView(_currentObjectID,false);
        }
        private void UpdateDependencies(ObjectID objectID, short typeID) {
            BuildingData bdata = GlobalSettings.Wrapper.buildingsMap[typeID];
            foreach (String bname in bdata.BuildingsCanProduce) {
                short idb = GlobalSettings.Wrapper.namesToIds[bname];
                if (CheckDependencies(bname)) {
                    if (!_stripData[objectID.ObjectId].ContainsKey(idb)) {
                        int buildSpeed = GlobalSettings.Wrapper.buildingsMap[idb].BuildSpeed;
                        BuildStatus bs = new BuildStatus(objectID.ObjectId, idb, (short)buildSpeed, BuildType.Building);
                        _stripData[objectID.ObjectId].Add(idb, bs);
                    }
                }
                else
                    RemoveBuildStatus(objectID, idb);      
            }
            foreach (String uname in bdata.UnitsCanProduce) {
                short idu = GlobalSettings.Wrapper.namesToIds[uname];
                if (CheckDependencies(uname)) {
                    if (!_stripData[objectID.ObjectId].ContainsKey(idu)) {
                        int buildSpeed = GetUnitBuildSpeed(idu);
                        BuildStatus bs = new BuildStatus(objectID.ObjectId, idu, (short)buildSpeed, BuildType.Unit);
                        _stripData[objectID.ObjectId].Add(idu, bs);
                    }
                }
                else
                    RemoveBuildStatus(objectID, idu);
            }
        }

        private void RemoveBuildStatus(ObjectID objectID, short idb) {
            if (_stripData[objectID.ObjectId].ContainsKey(idb)) {
                BuildStatus bs = _stripData[objectID.ObjectId][idb];
                lock (((ICollection)_buildList).SyncRoot) {
                    if (_buildList.Contains(bs))
                        _buildList.Remove(bs);
                }
                _stripData[objectID.ObjectId].Remove(idb);
            }
        }

        private int GetUnitBuildSpeed(short id) {
            if (GlobalSettings.Wrapper.troopersMap.ContainsKey(id))
                return GlobalSettings.Wrapper.troopersMap[id].BuildSpeed;
            if (GlobalSettings.Wrapper.tanksMap.ContainsKey(id))
                return GlobalSettings.Wrapper.tanksMap[id].BuildSpeed;
            if (GlobalSettings.Wrapper.harvestersMap.ContainsKey(id))
                return GlobalSettings.Wrapper.harvestersMap[id].BuildSpeed;
            return 0;
        }

        private bool CheckDependencies(string name) {
            TechnologyDependences deps = GlobalSettings.Wrapper.racesMap[_gameLogic.CurrentPlayer.House].TechnologyDependences;
            foreach (TechnologyDependence dep in deps.TechnologyDependenceCollection) {
                if (dep.BuildingName.Equals(name)) {
                    foreach (string n in dep.RequiredBuildings) {
                        short id = GlobalSettings.Wrapper.namesToIds[n];
                        if (_gameLogic.hasBuilding(id) == false) return false;
                    }
                }
            }
            return true;
        }
    }
}
