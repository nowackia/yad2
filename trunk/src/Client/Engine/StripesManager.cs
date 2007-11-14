using System;
using System.Collections.Generic;
using System.Text;
using Yad.Board.Common;
using Yad.Config.Common;
using Yad.Engine.Common;
using Yad.Config;
using System.IO;
using Yad.Properties.Common;
using Yad.Properties;
using Client.Properties;

namespace Client.Engine {

    public interface IManageableStripe {
         void Add(short id,string name, String pictureName);
         void Remove(short id);
         void AddPercentCounter(short id);
         void SetPercentValue(short id,int val);
         void RemovePercentCounter();
         void RemoveAll();
    }
    public class StripesManager {
        IManageableStripe unitStripe;
        IManageableStripe buildingStripe;
        List<short> buildingsOnMenu = new List<short>();
        List<short> unitsOnMenu = new List<short>();
        Simulation simulation;
        short race;
        TechnologyDependences deps;
        public StripesManager(Simulation sim, short race, IManageableStripe unitStripe,IManageableStripe buildingStripe) {
            this.buildingStripe = buildingStripe;
            this.unitStripe = unitStripe;
            simulation = sim;
            this.race = race;
            ExtractRaceData();
        }

        /// <summary>
        /// if player creates places the building menu can have more options to build
        /// </summary>
        /// <param name="name"></param>
        public void AddBuilding(short id) {
            if (simulation.GameSettingsWrapper.buildingsMap.ContainsKey(id) == false)
                throw new NotSupportedException("uknown building name");
            if (buildingsOnMenu.Contains(id) == false) {

                buildingsOnMenu.Add(id);
                String name = simulation.GameSettingsWrapper.buildingsMap[id].Name;
                buildingStripe.Add(id, name, Path.Combine(Settings.Default.Pictures, name + ".png"));//TODO add picture name to xsd.
            }
            foreach (TechnologyDependence techRef in deps) {
                short ids = simulation.GameSettingsWrapper.namesToIds[techRef.BuildingName];
                if (buildingsOnMenu.Contains(ids) == true) continue;
                if (CheckReqBuildingsToAddNewBuilding(techRef.RequiredBuildings)) {
                    // adds new building to strip
                    buildingsOnMenu.Add(ids);
                    String name = simulation.GameSettingsWrapper.buildingsMap[id].Name;
                    buildingStripe.Add(ids, name, Path.Combine(Settings.Default.Pictures,name+".png"));//TODO add picture name to xsd.
                }
            }      
        }

        private bool CheckReqBuildingsToAddNewBuilding(BuildingsNames coll) {
            
            foreach (String  buildingName in coll) {
                short id;
                if (simulation.GameSettingsWrapper.namesToIds.TryGetValue(buildingName, out id)) {
                    if (buildingsOnMenu.Contains(id) == false)
                        return false;
                }
                
            }
            return true;
        }

        private void ExtractRaceData() {
            RaceData raceData = simulation.GameSettingsWrapper.racesMap[race];
            this.deps = raceData.TechnologyDependences;
        }

        /// <summary>
        /// checks if this building can produce units - if true - put units on menu
        /// </summary>
        /// <param name="name">name of the building</param>
        public void BuildingClickedOnMap(short name) {
           //simulation.GameSettingsWrapper.
            if (buildingsOnMenu.Contains(name)) {
                BuildingData data = simulation.GameSettingsWrapper.buildingsMap[name];
            }
        }

        public void ClearUnits() {
            unitsOnMenu.Clear();
        }

        public void RemoveBuildingFromMenu(short name) {
            //Todo: check dependencies.
            buildingStripe.Remove(name);
        }


    }
}
