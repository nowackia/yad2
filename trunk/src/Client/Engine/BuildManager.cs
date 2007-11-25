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

namespace Yad.Engine {
    public class BuildManager {
        GameLogic _gameLogic;
        BuildStripe _leftStripe;
        BuildStripe _rightStripe;
        Dictionary<int, Dictionary<short, BuildStatus>> _stripData = new Dictionary<int, Dictionary<short, BuildStatus>>();
        int _currentObjectID=-1;

        public BuildManager(GameLogic gameLogic, BuildStripe leftStripe, BuildStripe rightStripe) {
            _gameLogic = gameLogic;
            _leftStripe = leftStripe;
            _rightStripe = rightStripe;
            InitRightStripe();
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
                UpdateView(_currentObjectID);
        }

        public void SwitchCurrentBuilding(int id) {
            if (_currentObjectID != id) {
                _currentObjectID = id;
                UpdateView(id);
               
            }
        }

        public void UpdateView(int id) {
            ObjectID obj = new ObjectID(_gameLogic.CurrentPlayer.Id, id);
            short typeID = _gameLogic.Simulation.Players[obj.PlayerID].GetBuilding(obj).TypeID;
            List<BuildStatus> listStatus = new List<BuildStatus>(_stripData[id].Values);
            _rightStripe.SwitchUpdate(listStatus);
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
            _stripData[_currentObjectID][(short)id].State = Yad.UI.StripButtonState.Ready;
            foreach (BuildStatus bs in _stripData[_currentObjectID].Values) {
                if (bs.Typeid != id)
                    bs.State = Yad.UI.StripButtonState.Inactive;
            }
            UpdateView(_currentObjectID);
        }

        private void RightBuildReadyClick(int id) {
            foreach (BuildStatus bs in _stripData[_currentObjectID].Values) {
                bs.State = Yad.UI.StripButtonState.Active;
            }
            UpdateView(_currentObjectID);
        }
        private void UpdateDependencies(ObjectID objectID, short typeID) {
            BuildingData bdata = GlobalSettings.Wrapper.buildingsMap[typeID];
            foreach (String bname in bdata.BuildingsCanProduce) {
                short idb = GlobalSettings.Wrapper.namesToIds[bname];
                if (CheckDependencies(bname)) {
                    BuildStatus bs = new BuildStatus(objectID.ObjectId, idb);
                    if (!_stripData[objectID.ObjectId].ContainsKey(idb))
                        _stripData[objectID.ObjectId].Add(idb, bs);
                }
                else
                    if (_stripData[objectID.ObjectId].ContainsKey(idb))
                        _stripData[objectID.ObjectId].Remove(idb);

            }
            foreach (String uname in bdata.UnitsCanProduce) {
                short idu = GlobalSettings.Wrapper.namesToIds[uname];
                if (CheckDependencies(uname)) {
                  
                    BuildStatus bs = new BuildStatus(objectID.ObjectId, idu);
                    if (!_stripData[objectID.ObjectId].ContainsKey(idu))
                        _stripData[objectID.ObjectId].Add(idu, bs);
                }
                else
                    if (_stripData[objectID.ObjectId].ContainsKey(idu))
                        _stripData[objectID.ObjectId].Remove(idu);
            }
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
