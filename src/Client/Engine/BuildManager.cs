#region MOD

/*
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
using Yad.UI.Common;

namespace Yad.Engine {
    public delegate void CreateUnitHandler(int createObjectID, short typeID);
    enum RightStripState {
        Building,
        Normal,
        Placing
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
        int _currentObjectID = -1;
        object cObjLock = new object();

        object cuLock = new object();

        object methodLock = new object();

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

        /// <summary>
        /// Inicjalizuje prawy pasek wszystkimi mozliwymi opcjami budowy
        /// </summary>
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

        public void OnBadLocation(int id) {
            lock (methodLock) {
                if (id == -1)
                    return;
                InfoLog.WriteInfo("lock leftState ", EPrefix.BMan);
                if (!_leftState.ContainsKey(id))
                    return;
                _leftState[id] = RightStripState.Normal;
                InfoLog.WriteInfo("release leftState ", EPrefix.BMan);
                InfoLog.WriteInfo("lock stripdata ", EPrefix.BMan);
                if (!_stripData.ContainsKey(id))
                    return;
                ActivateForObject(id);
                InfoLog.WriteInfo("release stripdata", EPrefix.BMan);
                UpdateView(id, false);
            }
        }

        private void ActivateForObject(int objectID) {
            foreach (StateWrapper sw in _stripData[objectID].Values) {
                sw.State = StripButtonState.Active;
            }
        }

        public void RemoveBuilding(ObjectID objectID, int creatorID, short typeID) {
            lock (methodLock) {
                bool contains = false;
                if (_stripData.ContainsKey(objectID.ObjectId))
                    contains = true;
                if (contains) {
                    _leftStripe.Remove(objectID.ObjectId);


                    if (_leftState.ContainsKey(objectID.ObjectId))
                        _leftState.Remove(objectID.ObjectId);


                    if (_stripData.ContainsKey(objectID.ObjectId)) {
                        _stripData.Remove(objectID.ObjectId);
                        foreach (int key in _stripData.Keys) {
                            ObjectID obj = new ObjectID(_gameLogic.CurrentPlayer.Id, key);
                            UpdateDependencies(obj, _gameLogic.CurrentPlayer.GetBuilding(obj).TypeID);
                        }
                    }

                    bool needsRebuild = false;

                    if (_currentObjectID == objectID.ObjectId) {
                        _currentObjectID = -1;
                        needsRebuild = true;
                    }

                    if (needsRebuild)
                        UpdateView(-1, false);
                }
                else {
                    //building was being built
                    bool containsCreator = false;
                    if (_stripData.ContainsKey(creatorID))
                        containsCreator = true;
                    if (containsCreator) {
                        _leftState[creatorID] = RightStripState.Normal;

                        foreach (StateWrapper sw in _stripData[creatorID].Values)
                            sw.State = StripButtonState.Active;
                        UpdateView(creatorID, false);
                    }
                }
            }
        }


        public void AddBuilding(ObjectID objectID, short typeID) {
            lock (methodLock) {
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
                int current = -1;
                if (_currentObjectID != -1)
                    current = _currentObjectID;
                if (current != -1)
                    UpdateView(current, false);
            }
        }

        public void SwitchCurrentBuilding(int id) {
            lock (methodLock) {
                InfoLog.WriteInfo("lock cObjLock ", EPrefix.BMan);
                int current = -1;
                int lastCurrent = -1;

                if (_currentObjectID != id) {
                    lastCurrent = _currentObjectID;
                    _currentObjectID = id;
                    current = id;
                }

                bool updateview = false;
                if (_leftState.ContainsKey(lastCurrent)) {
                    if (_leftState[lastCurrent] == RightStripState.Placing) {
                        updateview = true;
                        _leftState[lastCurrent] = RightStripState.Normal;
                    }
                }
                if (lastCurrent != -1 && updateview) {
                    foreach (short key in _stripData[lastCurrent].Keys) {
                        _stripData[lastCurrent][key].State = StripButtonState.Active;
                    }
                }

                if (current != -1)
                    UpdateView(current, true);
                InfoLog.WriteInfo("release cObjLock ", EPrefix.BMan);
            }
        }

        public void UpdateView(int id, bool rewind) {
            InfoLog.WriteInfo("UpdateView",EPrefix.UIManager);
            if (id == -1) {
                _rightStripe.HideAll();
                return;
            }
            _rightStripe.SwitchUpdate(_stripData[id], rewind);
        }
        public int RightBuildingClick(int id, bool isUnit) {
            lock (methodLock) {
                int current = -1;
                current = _currentObjectID;
                if (current != -1) {
                    StripButtonState state = _stripData[current][(short)id].State;
                    switch (state) {
                        case StripButtonState.Active:
                            RightBuildActiveClick(id, isUnit);
                            return current;
                    }
                }
                return -1;
            }
        }

        private void RightBuildActiveClick(int id, bool isUnit) {
            lock (methodLock) {
                int current = -1;
                InfoLog.WriteInfo("lock cObjLock", EPrefix.BMan);
                current = _currentObjectID;
                if (current == -1)
                    return;
                InfoLog.WriteInfo("release cObjLock", EPrefix.BMan);
                InfoLog.WriteInfo("lock leftState", EPrefix.BMan);
                _leftState[current] = RightStripState.Placing;
                InfoLog.WriteInfo("release leftState", EPrefix.BMan);
                if (isUnit) {
                    DeactivateOther(-1);
                    BuildUnitMessage buMessage = (BuildUnitMessage)MessageFactory.Create(MessageType.BuildUnitMessage);
                    buMessage.UnitType = (short)id;
                    buMessage.CreatorID = current;
                    buMessage.IdPlayer = _gameLogic.CurrentPlayer.Id;
                    Connection.Instance.SendMessage(buMessage);
                }
                else {
                    InfoLog.WriteInfo("lock stripData", EPrefix.BMan);
                    _stripData[current][(short)id].State = StripButtonState.Ready;
                    InfoLog.WriteInfo("release stripData", EPrefix.BMan);
                    DeactivateOther((short)id);
                }

                UpdateView(current, false);
            }
        }

        private void DeactivateOther(short typeid) {
            lock (methodLock) {
                Dictionary<short, StateWrapper> toDeactivate = null;
                int current = -1;
                InfoLog.WriteInfo("lock cObjLock", EPrefix.BMan);
                current = _currentObjectID;
                InfoLog.WriteInfo("release cObjLock", EPrefix.BMan);
                InfoLog.WriteInfo("lock stripData", EPrefix.BMan);
                toDeactivate = _stripData[current];
                foreach (short key in toDeactivate.Keys)
                    if (key != typeid)
                        toDeactivate[key].State = StripButtonState.Inactive;
                _stripData[current] = toDeactivate;
                InfoLog.WriteInfo("release stripData", EPrefix.BMan);
            }
        }

        public void ReadyReset(int id) {
            lock (methodLock) {
                InfoLog.WriteInfo("lock leftState", EPrefix.BMan);
                _leftState[id] = RightStripState.Normal;
                InfoLog.WriteInfo("release leftState", EPrefix.BMan);
                int current = -1;
                InfoLog.WriteInfo("lock cObjLock", EPrefix.BMan);

                current = _currentObjectID;
                InfoLog.WriteInfo("release cObjLock", EPrefix.BMan);
                InfoLog.WriteInfo("lock stripData", EPrefix.BMan);
                foreach (short key in _stripData[current].Keys) {
                    _stripData[current][key].State = Yad.UI.StripButtonState.Active;
                }
                InfoLog.WriteInfo("release stripData", EPrefix.BMan);
                UpdateView(current, false);
            }
        }
        private void UpdateDependencies(ObjectID objectID, short typeID) {
            lock (methodLock) {
                BuildingData bdata = GlobalSettings.Wrapper.buildingsMap[typeID];

                foreach (String bname in bdata.BuildingsCanProduce) {
                    short idb = GlobalSettings.Wrapper.namesToIds[bname];
                    if (CheckBuildingDependencies(bname)) {
                        InfoLog.WriteInfo("lock stripData [UDep]", EPrefix.BMan);

                        if (!_stripData[objectID.ObjectId].ContainsKey(idb)) {
                            int buildSpeed = GlobalSettings.Wrapper.buildingsMap[idb].BuildSpeed;
                            StateWrapper wrapper = new StateWrapper();
                            if (_leftState[objectID.ObjectId] == RightStripState.Building)
                                wrapper.State = StripButtonState.Inactive;
                            _stripData[objectID.ObjectId].Add(idb, wrapper);
                        }

                        InfoLog.WriteInfo("release stripData [Udep]", EPrefix.BMan);
                    }
                    else {
                        InfoLog.WriteInfo("lock stripData [UDep]", EPrefix.BMan);
                        RemoveBuildStatus(objectID, idb);
                        InfoLog.WriteInfo("release stripData [UDep]", EPrefix.BMan);
                    }
                }
                foreach (String uname in bdata.UnitsCanProduce) {
                    short idu = GlobalSettings.Wrapper.namesToIds[uname];
                    if (CheckUnitDependencies(uname)) {
                        InfoLog.WriteInfo("lock stripData [UDep]", EPrefix.BMan);

                        if (!_stripData[objectID.ObjectId].ContainsKey(idu)) {
                            int buildSpeed = GetUnitBuildSpeed(idu);
                            StateWrapper wrapper = new StateWrapper();
                            if (_leftState[objectID.ObjectId] == RightStripState.Building)
                                wrapper.State = StripButtonState.Inactive;
                            _stripData[objectID.ObjectId].Add(idu, wrapper);
                        }
                        InfoLog.WriteInfo("release stripData [UDep]", EPrefix.BMan);
                    }
                    else {
                        InfoLog.WriteInfo("lock stripData [UDep]", EPrefix.BMan);
                        RemoveBuildStatus(objectID, idu);
                        InfoLog.WriteInfo("release stripData [UDep]", EPrefix.BMan);
                    }

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
            lock (methodLock) {
                InfoLog.WriteInfo("lock stripData [Update strip]", EPrefix.BMan);
                if (_stripData.ContainsKey(id)) {
                    if (percent == -1) {
                        lock (((ICollection)_leftState).SyncRoot)
                            _leftState[id] = RightStripState.Normal;
                        lock (((ICollection)_stripData).SyncRoot)
                            ActivateForObject(id);
                        bool needsUpdate = false;

                        if (_currentObjectID == id)
                            needsUpdate = true;

                        if (needsUpdate)
                            UpdateView(id, false);
                    }
                    else {

                        _stripData[id][typeID].State = StripButtonState.Percantage;
                        _stripData[id][typeID].Percent = percent;


                        _leftState[id] = RightStripState.Building;

                        InfoLog.WriteInfo("lock cObjLock [Update Strip]", EPrefix.BMan);
                        bool needsUpdate = false;

                        if (_currentObjectID == id)
                            needsUpdate = true;
                        if (needsUpdate)
                            _rightStripe.UpdatePercent(typeID, percent);
                        InfoLog.WriteInfo("release cObjLock [Update strip]", EPrefix.BMan);
                    }
                }
            }
            InfoLog.WriteInfo("release stripData [Update strip]", EPrefix.BMan);
        }

        private bool CheckBuildingDependencies(string name) {
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
        private bool CheckBuildingUnitDeps(BuildingsNames bnames) {
            foreach (string name in bnames) {
                short idname = GlobalSettings.Wrapper.namesToIds[name];
                if (!_gameLogic.hasBuilding(idname))
                    return false;
            }
            return true;
        }

        private bool CheckUnitDependencies(string name) {
            short id = GlobalSettings.Wrapper.namesToIds[name];
            BuildingsNames bnames = null;
            if (GlobalSettings.Wrapper.harvestersMap.ContainsKey(id))
                bnames = GlobalSettings.Wrapper.harvestersMap[id].BuildingDependency;
            else if (GlobalSettings.Wrapper.mcvsMap.ContainsKey(id))
                bnames = GlobalSettings.Wrapper.mcvsMap[id].BuildingDependency;
            else if (GlobalSettings.Wrapper.tanksMap.ContainsKey(id))
                bnames = GlobalSettings.Wrapper.tanksMap[id].BuildingDependency;
            else if (GlobalSettings.Wrapper.troopersMap.ContainsKey(id))
                bnames = GlobalSettings.Wrapper.troopersMap[id].BuildingDependency;
            if (bnames == null)
                throw new Exception(name + "not found in building-unit dependencies!");
            return CheckBuildingUnitDeps(bnames);
        }

        private object Exception(string p) {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}*/
#endregion 

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
using Yad.UI.Common;

namespace Yad.Engine {
    public delegate void CreateUnitHandler(int createObjectID, short typeID);
    enum RightStripState {
        Building,
        Normal,
        Placing
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
        int _currentObjectID = -1;
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

        /// <summary>
        /// Inicjalizuje prawy pasek wszystkimi mozliwymi opcjami budowy
        /// </summary>
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

        public void OnBadLocation(int id) {
            InfoLog.WriteInfo("Enter OnBadLocation",EPrefix.BMan);
            if (id == -1)
                return;
            InfoLog.WriteInfo("lock leftState in OnBadLocation", EPrefix.BMan);
            lock (((ICollection)_leftState).SyncRoot)
            {
                if (!_leftState.ContainsKey(id))
                    return;
                _leftState[id] = RightStripState.Normal;
            }
            InfoLog.WriteInfo("release leftState in OnBadLocation", EPrefix.BMan);
            
            InfoLog.WriteInfo("lock stripdata in OnBadLocation", EPrefix.BMan);
            lock (((ICollection)_stripData).SyncRoot)
            {
                if (!_stripData.ContainsKey(id))
                    return;
                ActivateForObject(id);
            }
            InfoLog.WriteInfo("release stripdata in OnBadLocation", EPrefix.BMan);
            _rightStripe.ActivateAll();
            InfoLog.WriteInfo("Exit OnBadLocation",EPrefix.BMan);
        }

        private void ActivateForObject(int objectID) {
            foreach (StateWrapper sw in _stripData[objectID].Values) {
                sw.State = StripButtonState.Active;
            }
        }

        public void RemoveBuilding(ObjectID objectID, int creatorID, short typeID) {
            bool contains = false;
            lock (((ICollection)_stripData).SyncRoot)
                if (_stripData.ContainsKey(objectID.ObjectId))
                    contains = true;
            if (contains) {
                _leftStripe.Remove(objectID.ObjectId);

                lock (((ICollection)_leftState).SyncRoot){
                    if (_leftState.ContainsKey(objectID.ObjectId))
                        _leftState.Remove(objectID.ObjectId);
                }
                lock (((ICollection)_stripData).SyncRoot) {
                    if (_stripData.ContainsKey(objectID.ObjectId)) {
                        _stripData.Remove(objectID.ObjectId);
                        foreach (int key in _stripData.Keys) {
                            ObjectID obj = new ObjectID(_gameLogic.CurrentPlayer.Id, key);
                            UpdateDependencies(obj, _gameLogic.CurrentPlayer.GetBuilding(obj).TypeID);
                        }
                    }
                }
                bool needsRebuild = false;
                lock (cObjLock) {
                    if (_currentObjectID == objectID.ObjectId) {
                        _currentObjectID = -1;
                        needsRebuild = true;
                    }
                }
                if (needsRebuild)
                    lock (((ICollection)_stripData).SyncRoot)
                        UpdateView(-1, false);
            }
            else {
                //building was being built
                bool containsCreator = false;
                lock (((ICollection)_stripData).SyncRoot)
                    if (_stripData.ContainsKey(creatorID))
                    containsCreator = true;
                if (containsCreator) {
                    lock (((ICollection)_leftState).SyncRoot)
                        _leftState[creatorID] = RightStripState.Normal;
                    lock (((ICollection)_stripData).SyncRoot) {
                        foreach (StateWrapper sw in _stripData[creatorID].Values)
                            sw.State = StripButtonState.Active;
                    }
                    lock (((ICollection)_stripData).SyncRoot)
                        UpdateView(creatorID, false);
                }
            }
        }

        public void AddBuilding(ObjectID objectID, short typeID) {
            BuildingData bs = GlobalSettings.Wrapper.buildingsMap[typeID];
            string name = GlobalSettings.Wrapper.buildingsMap[typeID].Name;
            if (bs.BuildingsCanProduce.Count != 0 || bs.UnitsCanProduce.Count != 0) {
              
                _leftStripe.Add(objectID.ObjectId, name, name, true);
                lock (((ICollection)_leftState).SyncRoot)
                    _leftState.Add(objectID.ObjectId, RightStripState.Normal);
                lock (((ICollection)_stripData).SyncRoot)
                    _stripData.Add(objectID.ObjectId, new Dictionary<short, StateWrapper>());
            }
            lock (((ICollection)_stripData).SyncRoot) {
                foreach (int key in _stripData.Keys) {
                    ObjectID obj = new ObjectID(_gameLogic.CurrentPlayer.Id, key);
                    if (key == objectID.ObjectId)
                        UpdateDependencies(obj, typeID);
                    else
                        UpdateDependencies(obj, _gameLogic.CurrentPlayer.GetBuilding(obj).TypeID);
                }
            }
            int current = -1;
            lock (cObjLock){
                if (_currentObjectID != -1)
                    current = _currentObjectID;
            }
            if (current != -1)
                lock (((ICollection)_stripData).SyncRoot)
                    _rightStripe.VisibilityUpdate(_stripData[current], true);
                //UpdateView(current, false);
        }

        public void SwitchCurrentBuilding(int id) {
            InfoLog.WriteInfo("lock cObjLock ", EPrefix.BMan);
            if (id == -1)
                InfoLog.WriteInfo("ID == -1 w SwitchCurrentBuilding!", EPrefix.BMan);
            int current = -1;
            int lastCurrent = -1;

            lock (cObjLock) {
                lastCurrent = _currentObjectID;
                if (id != -1)
                    _currentObjectID = id;
                 current = id;
            }


            bool updateview = false;
            lock (((ICollection)_leftState).SyncRoot) {
                if (_leftState.ContainsKey(lastCurrent)) {
                    if (_leftState[lastCurrent] == RightStripState.Placing) {
                        updateview = true;
                        _leftState[lastCurrent] = RightStripState.Normal;
                    }
                }
            }
            if (lastCurrent != -1 && updateview) {
                lock (((ICollection)_stripData).SyncRoot) {
                    foreach (short key in _stripData[lastCurrent].Keys) {
                        _stripData[lastCurrent][key].State = StripButtonState.Active;
                    }
                }
            }
            if (current != -1)
                lock (((ICollection)_stripData).SyncRoot)
                    _rightStripe.SwitchUpdate(_stripData[current], true);
            InfoLog.WriteInfo("release cObjLock ", EPrefix.BMan);
        }

        public void UpdateView(int id, bool rewind) {
            InfoLog.WriteInfo("UpdateView",EPrefix.BMan);
            if (id == -1) {
                _rightStripe.HideAll();
                return;
            }
            _rightStripe.SwitchUpdate(_stripData[id], rewind);
        }
        public int RightBuildingClick(int id, bool isUnit) {
            InfoLog.WriteInfo("Enter RightBuildingClick" + id, EPrefix.BMan);
            int current = -1;
            InfoLog.WriteInfo("Lock cObjLock in RightBuildingClick", EPrefix.BMan);
            lock (this.cObjLock)
                current = _currentObjectID;
            InfoLog.WriteInfo("Unlock cObjLock in RightBuildingClick", EPrefix.BMan);
            if (current != -1)
            {
                StripButtonState state = _stripData[current][(short)id].State;
                InfoLog.WriteInfo("State: " + state + " " + Enum.GetName(typeof(StripButtonState), state), EPrefix.BMan);
                switch (state)
                {
                    case StripButtonState.Active:
                        RightBuildActiveClick(id, isUnit);
                        InfoLog.WriteInfo("Exit RightBuildingClick", EPrefix.BMan);
                        return current;
                }
            }
            InfoLog.WriteInfo("Exit RightBuildingClick", EPrefix.BMan);
            return -1;
        }

        private void RightBuildActiveClick(int id, bool isUnit) {
            InfoLog.WriteInfo("Enter RightBuildActiveClick", EPrefix.BMan);
            InfoLog.WriteInfo("lock cObjLock in RightBuildActiveClick", EPrefix.BMan);

            int current = -1;
            lock (cObjLock)
                current = _currentObjectID;

            InfoLog.WriteInfo("unlock cObjLock in RightBuildActiveClick", EPrefix.BMan);
            
            if (current == -1) {
                InfoLog.WriteInfo("Exit RightBuildingClick", EPrefix.BMan);
                return;
            }

            InfoLog.WriteInfo("lock leftState in RightBuildActiveClick", EPrefix.BMan);

            lock (((ICollection)_leftState).SyncRoot)
                _leftState[current] = RightStripState.Placing;

            InfoLog.WriteInfo("lock leftState in RightBuildActiveClick", EPrefix.BMan);
            if (isUnit) {
                DeactivateOther(-1);
                _rightStripe.DeactivateAll();
                BuildUnitMessage buMessage = (BuildUnitMessage) MessageFactory.Create(MessageType.BuildUnitMessage);
                buMessage.UnitType = (short)id;
                buMessage.CreatorID = current;
                buMessage.IdPlayer = _gameLogic.CurrentPlayer.Id;
                Connection.Instance.SendMessage(buMessage);
                InfoLog.WriteInfo("Exit RightBuildingClick", EPrefix.BMan);
                return;
            }
            else {
                InfoLog.WriteInfo("lock stripData in RightBuildActiveClick", EPrefix.BMan);
                lock (((ICollection)_stripData).SyncRoot)
                    _stripData[current][(short)id].State = StripButtonState.Ready;
                InfoLog.WriteInfo("unlock stripData in RightBuildActiveClick", EPrefix.BMan);
                DeactivateOther((short)id);
                _rightStripe.DeactivateAll();
                _rightStripe.SetState(id, StripButtonState.Ready);
                return;
            }
            InfoLog.WriteInfo("lock stripData in RightBuildActiveClick", EPrefix.BMan);
            lock (((ICollection)_stripData).SyncRoot)
                UpdateView(current,false);
            InfoLog.WriteInfo("unlock stripData in RightBuildActiveClick", EPrefix.BMan);
            InfoLog.WriteInfo("Exit RightBuildingClick", EPrefix.BMan);
        }

        private void DeactivateOther(short typeid) {
            Dictionary<short, StateWrapper> toDeactivate = null;
            int current = -1;
            InfoLog.WriteInfo("lock cObjLock", EPrefix.BMan);
            lock (cObjLock) {
                current = _currentObjectID;
            }
            InfoLog.WriteInfo("release cObjLock", EPrefix.BMan);
            InfoLog.WriteInfo("lock stripData", EPrefix.BMan);
            lock (((ICollection)_stripData).SyncRoot) {
                toDeactivate = _stripData[current];
                foreach (short key in toDeactivate.Keys)
                    if (key != typeid)
                        toDeactivate[key].State = StripButtonState.Inactive;
                _stripData[current] = toDeactivate;
            }
            InfoLog.WriteInfo("release stripData", EPrefix.BMan);
        }

        public void ReadyReset(int id) {
            InfoLog.WriteInfo("Enter ReadyReset", EPrefix.BMan);
            InfoLog.WriteInfo("lock leftState", EPrefix.BMan);
            lock (((ICollection)_leftState).SyncRoot)
                _leftState[id] = RightStripState.Normal;
            InfoLog.WriteInfo("release leftState", EPrefix.BMan);
            int current = -1;
            InfoLog.WriteInfo("lock cObjLock", EPrefix.BMan);
            lock (cObjLock)
                current = _currentObjectID;
            InfoLog.WriteInfo("release cObjLock", EPrefix.BMan);
            InfoLog.WriteInfo("lock stripData", EPrefix.BMan);
            lock (((ICollection)_stripData).SyncRoot) {
                foreach (short key in _stripData[current].Keys) {
                    _stripData[current][key].State = Yad.UI.StripButtonState.Active;
                }
            }
            InfoLog.WriteInfo("release stripData", EPrefix.BMan);
            InfoLog.WriteInfo("Leave ReadyReset", EPrefix.BMan);
            _rightStripe.ActivateAll();
        }

        private void UpdateDependencies(ObjectID objectID, short typeID) {
            BuildingData bdata = GlobalSettings.Wrapper.buildingsMap[typeID];
        
            foreach (String bname in bdata.BuildingsCanProduce) {
                    short idb = GlobalSettings.Wrapper.namesToIds[bname];
                if (CheckBuildingDependencies(idb)) {
                    InfoLog.WriteInfo("lock stripData [UDep]", EPrefix.BMan);
                    lock (((ICollection)_stripData).SyncRoot) {
                        if (!_stripData[objectID.ObjectId].ContainsKey(idb)) {
                            int buildSpeed = GlobalSettings.Wrapper.buildingsMap[idb].BuildSpeed;
                            StateWrapper wrapper = new StateWrapper();
                            if (_leftState[objectID.ObjectId] == RightStripState.Building)
                                wrapper.State = StripButtonState.Inactive;
                            _stripData[objectID.ObjectId].Add(idb, wrapper);
                        }
                    }
                    InfoLog.WriteInfo("release stripData [Udep]", EPrefix.BMan);
                }
                else {
                    InfoLog.WriteInfo("lock stripData [UDep]", EPrefix.BMan);
                    lock (((ICollection)_stripData).SyncRoot)
                        RemoveBuildStatus(objectID, idb);
                    InfoLog.WriteInfo("release stripData [UDep]", EPrefix.BMan);
                }
            }
            foreach (String uname in bdata.UnitsCanProduce) {
                short idu = GlobalSettings.Wrapper.namesToIds[uname];
                if (CheckHouseCanBuildUnit(idu)){
                    if (CheckUnitDependencies(idu)) {
                        InfoLog.WriteInfo("lock stripData [UDep]", EPrefix.BMan);
                        lock (((ICollection)_stripData).SyncRoot) {
                            if (!_stripData[objectID.ObjectId].ContainsKey(idu)) {
                                int buildSpeed = GetUnitBuildSpeed(idu);
                                StateWrapper wrapper = new StateWrapper();
                                if (_leftState[objectID.ObjectId] == RightStripState.Building)
                                    wrapper.State = StripButtonState.Inactive;
                                _stripData[objectID.ObjectId].Add(idu, wrapper);
                            }
                        }
                        InfoLog.WriteInfo("release stripData [UDep]", EPrefix.BMan);
                    }
                    else {
                        InfoLog.WriteInfo("lock stripData [UDep]", EPrefix.BMan);
                        lock (((ICollection)_stripData).SyncRoot)
                            RemoveBuildStatus(objectID, idu);
                        InfoLog.WriteInfo("release stripData [UDep]", EPrefix.BMan);
                    }
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
            InfoLog.WriteInfo("Enter UpdateStrip", EPrefix.BMan);
                if (_stripData.ContainsKey(id)) {
                    if (percent == -1) {
                        InfoLog.WriteInfo("lock leftState in UpdateStrip", EPrefix.BMan);
                        lock (((ICollection)_leftState).SyncRoot) {
                            if (!_leftState.ContainsKey(id)) {
                                InfoLog.WriteInfo("Leave UpdateStrip", EPrefix.BMan);
                                return;
                            }
                            _leftState[id] = RightStripState.Normal;
                        }
                        InfoLog.WriteInfo("unlock leftState in UpdateStrip", EPrefix.BMan);
                        InfoLog.WriteInfo("lock stripdata in UpdateStrip", EPrefix.BMan);
                        lock (((ICollection)_stripData).SyncRoot) {
                            if (!_stripData.ContainsKey(id)) {
                                InfoLog.WriteInfo("Leave UpdateStrip", EPrefix.BMan);
                                return;
                            }
                            ActivateForObject(id);
                        }
                        InfoLog.WriteInfo("unlock stripdata in UpdateStrip", EPrefix.BMan);
                        bool needsUpdate = false;
                        InfoLog.WriteInfo("lock cObjLock in UpdateStrip", EPrefix.BMan);
                        lock(cObjLock){
                            if (_currentObjectID == id)
                                needsUpdate = true;
                        }
                        InfoLog.WriteInfo("unlock stripdata in UpdateStrip", EPrefix.BMan);
                        if (needsUpdate)
                            _rightStripe.ActivateAll();
                    }
                    else {
                        InfoLog.WriteInfo("lock stripdata in UpdateStrip", EPrefix.BMan);
                        lock (((ICollection)_stripData).SyncRoot) {
                            if (!_stripData.ContainsKey(id))
                                return;
                            _stripData[id][typeID].State = StripButtonState.Percantage;
                            _stripData[id][typeID].Percent = percent;
                        }
                        InfoLog.WriteInfo("unlock stripdata in UpdateStrip", EPrefix.BMan);
                        InfoLog.WriteInfo("lock leftState in UpdateStrip", EPrefix.BMan);
                        lock (((ICollection)_leftState).SyncRoot) {
                            if (!_leftState.ContainsKey(id)) {
                                InfoLog.WriteInfo("Leave UpdateStrip", EPrefix.BMan);
                                return;
                            }
                            _leftState[id] = RightStripState.Building;
                        }
                        InfoLog.WriteInfo("unlock leftState in UpdateStrip", EPrefix.BMan);
                      
                        bool needsUpdate = false;
                        InfoLog.WriteInfo("lock cObjLock [Update Strip]", EPrefix.BMan);
                        lock (cObjLock)
                        {
                            if (_currentObjectID == id)
                                needsUpdate = true;
                        }
                          InfoLog.WriteInfo("unlock cObjLock [Update Strip]", EPrefix.BMan);
                        if (needsUpdate)
                                _rightStripe.UpdatePercent(typeID, percent);
                       
                        }
                }
            InfoLog.WriteInfo("Leave UpdateStrip", EPrefix.BMan);
        }
        /*
        private bool CheckBuildingDependencies(string name) {
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
        }*/

        private bool CheckBuildingDependencies(short id) {
            TechnologyDependences deps = GlobalSettings.Wrapper.racesMap[_gameLogic.CurrentPlayer.House].TechnologyDependences;
            foreach (TechnologyDependence dep in deps.TechnologyDependenceCollection) {
                short bid = GlobalSettings.Wrapper.namesToIds[dep.BuildingName];
                if (bid == id)
                    foreach (string n in dep.RequiredBuildings) {
                        short rbid = GlobalSettings.Wrapper.namesToIds[n];
                        if (_gameLogic.hasBuilding(rbid) == false) return false;
                    }
            }
            return true;
        }
        /*
        private bool CheckHouseCanBuildUnit(string name) {
            UnitsNames un = GlobalSettings.Wrapper.racesMap[_gameLogic.CurrentPlayer.House].__UnitsCanProduce;
            foreach (string uname in un)
                if (uname.CompareTo(name) == 0)
                    return true;
            return false;
        }*/

        private bool CheckHouseCanBuildUnit(short id) {
            UnitsNames un = GlobalSettings.Wrapper.racesMap[_gameLogic.CurrentPlayer.House].__UnitsCanProduce;
            foreach (string uname in un) {
                short uid = GlobalSettings.Wrapper.namesToIds[uname];
                if (id == uid)
                    return true;
            }
            return false;
        }
        /*
        private bool CheckUnitDependencies(string name) {
            short id = GlobalSettings.Wrapper.namesToIds[name];
            BuildingsNames bnames = null;
            if (GlobalSettings.Wrapper.harvestersMap.ContainsKey(id))
                bnames = GlobalSettings.Wrapper.harvestersMap[id].BuildingDependency;
            else if (GlobalSettings.Wrapper.mcvsMap.ContainsKey(id))
                bnames = GlobalSettings.Wrapper.mcvsMap[id].BuildingDependency;
            else if (GlobalSettings.Wrapper.tanksMap.ContainsKey(id))
                bnames = GlobalSettings.Wrapper.tanksMap[id].BuildingDependency;
            else if (GlobalSettings.Wrapper.troopersMap.ContainsKey(id))
                bnames = GlobalSettings.Wrapper.troopersMap[id].BuildingDependency;
            if (bnames == null)
                throw new Exception(name + "not found in building-unit dependencies!");
            return CheckBuildingUnitDeps(bnames);
        }*/

        private bool CheckUnitDependencies(short id) {
            BuildingsNames bnames = null;
            if (GlobalSettings.Wrapper.harvestersMap.ContainsKey(id))
                bnames = GlobalSettings.Wrapper.harvestersMap[id].BuildingDependency;
            else if (GlobalSettings.Wrapper.mcvsMap.ContainsKey(id))
                bnames = GlobalSettings.Wrapper.mcvsMap[id].BuildingDependency;
            else if (GlobalSettings.Wrapper.tanksMap.ContainsKey(id))
                bnames = GlobalSettings.Wrapper.tanksMap[id].BuildingDependency;
            else if (GlobalSettings.Wrapper.troopersMap.ContainsKey(id))
                bnames = GlobalSettings.Wrapper.troopersMap[id].BuildingDependency;
            if (bnames == null)
                throw new Exception(id + "not found in building-unit dependencies!");
            return CheckBuildingUnitDeps(bnames);
        }

        private bool CheckBuildingUnitDeps(BuildingsNames bnames) {
            foreach (string name in bnames) {
                short idname = GlobalSettings.Wrapper.namesToIds[name];
                if (!_gameLogic.hasBuilding(idname))
                    return false;
            }
            return true;
        }

        private object Exception(string p) {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}

