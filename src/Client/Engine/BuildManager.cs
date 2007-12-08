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
using Yad.Log.Common;
using Yad.Net.Messaging;
using Yad.Net.Messaging.Common;

namespace Yad.Engine {
    public delegate void CreateUnitHandler(int createObjectID, short typeID);
    enum RightStripState {
        Building,
        Normal
    }

    public class StateWrapper {
        public StripButtonState State = StripButtonState.Active;
        private int _percent = 0;

        public int Percent {
            get { return _percent; }
            set { _percent = value; }
        }
    }
    public class BuildManager {

        GameLogic _gameLogic;
        BuildStripe _leftStripe;
        BuildStripe _rightStripe;
        Dictionary<int, Dictionary<short, StateWrapper>> _stripData = new Dictionary<int, Dictionary<short, StateWrapper>>();
        Dictionary<int, RightStripState> _leftState = new Dictionary<int, RightStripState>();
        int _currentObjectID=-1;
        object cObjLock = new object();

        object cuLock = new object();

        private event CreateUnitHandler _createUnit = null;

        public BuildManager(GameLogic gameLogic, BuildStripe leftStripe, BuildStripe rightStripe) {
            _gameLogic = gameLogic;
            _leftStripe = leftStripe;
            _rightStripe = rightStripe;
            InitRightStripe();
        }

        public event CreateUnitHandler CreateUnit {
            add {
                _createUnit += value;
            }
            remove {
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
        /*
        public void ProcessTurn() {
            Yad.Log.Common.InfoLog.WriteInfo("BuildManager ProcessTurn start");
            List<BuildStatus> _bsToEnd = new List<BuildStatus>();
            lock (((ICollection)_buildList).SyncRoot) {
                for (int i = 0; i < _buildList.Count; ++i) {
                    BuildStatus bs = _buildList[i];
                    if (bs.DoTurn()) {
                        _buildList.Remove(bs);
                        --i;
                        _bsToEnd.Add(bs);
                        /* OLD--> InfoLog.WriteInfo("Before OnBuildEnd");
                        OnBuildEnd(bs);
                        InfoLog.WriteInfo("After OnBuildEnd")
                    }
                    else
                        if (bs.ObjectId == _currentObjectID) {
                            InfoLog.WriteInfo("Before _rightStripe.Update");
                            _rightStripe.Update(bs);
                            InfoLog.WriteInfo("After _rightStripe.Update");
                        }
                }
            }
            //NEW
            foreach (BuildStatus bs in _bsToEnd) {
                InfoLog.WriteInfo("Before OnBuildEnd");
                OnBuildEnd(bs);
                InfoLog.WriteInfo("After OnBuildEnd");
            }
            Yad.Log.Common.InfoLog.WriteInfo("BuildManager ProcessTurn end");
        }

        public void OnBuildEnd(BuildStatus bstatus) {
            if (bstatus.BuildType == BuildType.Unit) {
                _leftState[bstatus.ObjectId] = RightStripState.Normal;
                InfoLog.WriteInfo("Creating unit");
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
                InfoLog.WriteInfo("Create Building");
                if (_currentObjectID == bstatus.ObjectId)
                    _rightStripe.Update(bstatus);

                /* Sound */

        public void OnBadLocation(int id) {
            if (id == -1)
                return;
            InfoLog.WriteInfo("lock leftState ", EPrefix.LockInfo);
            lock (((ICollection)_leftState).SyncRoot)
                _leftState[id] = RightStripState.Normal;
            InfoLog.WriteInfo("release leftState ", EPrefix.LockInfo);
            InfoLog.WriteInfo("lock stripdata ", EPrefix.LockInfo);
            lock (((ICollection)_stripData).SyncRoot)
                ActivateForObject(id);
            InfoLog.WriteInfo("release stripdata", EPrefix.LockInfo);
            UpdateView(id, false);
        }
        private void ActivateForObject(int objectID) {
            foreach (StateWrapper sw in _stripData[objectID].Values) {
                sw.State = StripButtonState.Active;
            }
        }

        public void RemoveBuilding(ObjectID objectID, short typeID) {
            if (_stripData.ContainsKey(objectID.ObjectId)) {
                _leftStripe.Remove(objectID.ObjectId);
                _leftState.Remove(objectID.ObjectId);
                _stripData.Remove(objectID.ObjectId);
                
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
                _leftState.Add(objectID.ObjectId, RightStripState.Normal);
                _stripData.Add(objectID.ObjectId, new Dictionary<short, StateWrapper>());
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
            InfoLog.WriteInfo("lock cObjLock ", EPrefix.LockInfo);
            lock (cObjLock) {
                if (_currentObjectID != id) {
                    _currentObjectID = id;
                    UpdateView(_currentObjectID, true);
                }
            }
            InfoLog.WriteInfo("release cObjLock ", EPrefix.LockInfo);
        }

        public void UpdateView(int id, bool rewind) {
            InfoLog.WriteInfo("UpdateView");
            if (id == -1) {
                _rightStripe.HideAll();
                return;
            }
            _rightStripe.SwitchUpdate(_stripData[id], rewind);
        }
        public int RightBuildingClick(int id, bool isUnit) {
            int current = -1;
            lock (this.cObjLock)
                current = _currentObjectID;
            StripButtonState state = _stripData[current][(short)id].State;
            switch (state) {
                case StripButtonState.Active:
                    RightBuildActiveClick(id, isUnit);
                    return current;
            }
            return -1;
        }

        private void RightBuildActiveClick(int id, bool isUnit) {
            int current = -1;
            InfoLog.WriteInfo("lock cObjLock", EPrefix.LockInfo);
            lock (cObjLock)
                current = _currentObjectID;
            InfoLog.WriteInfo("release cObjLock", EPrefix.LockInfo);
            InfoLog.WriteInfo("lock leftState", EPrefix.LockInfo);
            lock (((ICollection)_leftState).SyncRoot)
                _leftState[current] = RightStripState.Building;
            InfoLog.WriteInfo("release leftState", EPrefix.LockInfo);
            if (isUnit) {
                DeactivateOther(-1);
                BuildUnitMessage buMessage = (BuildUnitMessage) MessageFactory.Create(MessageType.BuildUnitMessage);
                buMessage.UnitType = (short)id;
                buMessage.CreatorID = current;
                buMessage.IdPlayer = _gameLogic.CurrentPlayer.Id;
                Connection.Instance.SendMessage(buMessage);
            }
            else {
                InfoLog.WriteInfo("lock stripData", EPrefix.LockInfo);
                lock (((ICollection)_stripData).SyncRoot)
                    _stripData[current][(short)id].State = StripButtonState.Ready;
                InfoLog.WriteInfo("release stripData", EPrefix.LockInfo);
                DeactivateOther((short)id);
            }
           
            UpdateView(_currentObjectID,false);
        }

        private void DeactivateOther(short typeid) {
            Dictionary<short, StateWrapper> toDeactivate = null;
            int current = -1;
            InfoLog.WriteInfo("lock cObjLock", EPrefix.LockInfo);
            lock (cObjLock) {
                current = _currentObjectID;
            }
            InfoLog.WriteInfo("release cObjLock", EPrefix.LockInfo);
            InfoLog.WriteInfo("lock stripData", EPrefix.LockInfo);
            lock (((ICollection)_stripData).SyncRoot) {
                toDeactivate = _stripData[current];
                foreach (short key in toDeactivate.Keys)
                    if (key != typeid)
                        toDeactivate[key].State = StripButtonState.Inactive;
                _stripData[current] = toDeactivate;
            }
            InfoLog.WriteInfo("release stripData", EPrefix.LockInfo);
        }

        public void ReadyReset(int id) {
            InfoLog.WriteInfo("lock leftState", EPrefix.LockInfo);
            lock (((ICollection)_leftState).SyncRoot)
                _leftState[id] = RightStripState.Normal;
            InfoLog.WriteInfo("release leftState", EPrefix.LockInfo);
            int current = -1;
            InfoLog.WriteInfo("lock cObjLock", EPrefix.LockInfo);
            lock (cObjLock)
                current = _currentObjectID;
            InfoLog.WriteInfo("release cObjLock", EPrefix.LockInfo);
            InfoLog.WriteInfo("lock stripData", EPrefix.LockInfo);
            lock (((ICollection)_stripData).SyncRoot) {
                foreach (short key in _stripData[current].Keys) {
                    _stripData[current][key].State = Yad.UI.StripButtonState.Active;
                }
            }
            InfoLog.WriteInfo("release stripData", EPrefix.LockInfo);
            UpdateView(current,false);
        }
        private void UpdateDependencies(ObjectID objectID, short typeID) {
            BuildingData bdata = GlobalSettings.Wrapper.buildingsMap[typeID];
            
            foreach (String bname in bdata.BuildingsCanProduce) {
                short idb = GlobalSettings.Wrapper.namesToIds[bname];
                if (CheckDependencies(bname)) {
                    InfoLog.WriteInfo("lock stripData [UDep]", EPrefix.LockInfo);
                    lock (((ICollection)_stripData).SyncRoot) {
                        if (!_stripData[objectID.ObjectId].ContainsKey(idb)) {
                            int buildSpeed = GlobalSettings.Wrapper.buildingsMap[idb].BuildSpeed;
                            StateWrapper wrapper = new StateWrapper();
                            if (_leftState[objectID.ObjectId] == RightStripState.Building)
                                wrapper.State = StripButtonState.Inactive;
                            _stripData[objectID.ObjectId].Add(idb, wrapper);
                        }
                    }
                    InfoLog.WriteInfo("release stripData [Udep]", EPrefix.LockInfo);
                }
                else {
                    InfoLog.WriteInfo("lock stripData [UDep]", EPrefix.LockInfo);
                    lock (((ICollection)_stripData).SyncRoot)
                        RemoveBuildStatus(objectID, idb);
                    InfoLog.WriteInfo("release stripData [UDep]", EPrefix.LockInfo);
                }
            }
            foreach (String uname in bdata.UnitsCanProduce) {
                short idu = GlobalSettings.Wrapper.namesToIds[uname];
                if (CheckDependencies(uname)) {
                    InfoLog.WriteInfo("lock stripData [UDep]", EPrefix.LockInfo);
                    lock (((ICollection)_stripData).SyncRoot) {
                        if (!_stripData[objectID.ObjectId].ContainsKey(idu)) {
                            int buildSpeed = GetUnitBuildSpeed(idu);
                            StateWrapper wrapper = new StateWrapper();
                            if (_leftState[objectID.ObjectId] == RightStripState.Building)
                                wrapper.State = StripButtonState.Inactive;
                            _stripData[objectID.ObjectId].Add(idu, wrapper);
                        }
                    }
                    InfoLog.WriteInfo("release stripData [UDep]", EPrefix.LockInfo);
                }
                else {
                    InfoLog.WriteInfo("lock stripData [UDep]", EPrefix.LockInfo);
                    lock (((ICollection)_stripData).SyncRoot)
                        RemoveBuildStatus(objectID, idu);
                    InfoLog.WriteInfo("release stripData [UDep]", EPrefix.LockInfo);
                }
            }
        }

        private void RemoveBuildStatus(ObjectID objectID, short idb) {
            if (_stripData[objectID.ObjectId].ContainsKey(idb)) {
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

        public void UpdateStrip(int id, short typeID, int percent) {
            InfoLog.WriteInfo("lock stripData [Update strip]", EPrefix.LockInfo);
            lock (((ICollection)_stripData).SyncRoot) {
                if (_stripData.ContainsKey(id)) {
                    if (percent == -1) {
                        _leftState[id] = RightStripState.Normal;
                        ActivateForObject(id);
                        if (_currentObjectID == id)
                            UpdateView(id, false);
                    }
                    else {
                        _stripData[id][typeID].State = StripButtonState.Percantage;
                        _stripData[id][typeID].Percent = percent;
                        InfoLog.WriteInfo("lock cObjLock [Update Strip]", EPrefix.LockInfo);
                        if (_currentObjectID == id)
                                _rightStripe.UpdatePercent(typeID, percent);
                        InfoLog.WriteInfo("release cObjLock [Update strip]", EPrefix.LockInfo);
                    }
                }
            }
            InfoLog.WriteInfo("release stripData [Update strip]", EPrefix.LockInfo);
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
