
#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections;
using XNAExtras;

#endregion

namespace Dune_2_Remade
{   
    public class DuneGame : Microsoft.Xna.Framework.Game
    {
        List<Unit> AllMyUnits = new List<Unit>();
        List<Building> AllMyBuildings = new List<Building>();
        List<Unit> AllEnemyUnits = new List<Unit>();
        List<Building> AllEnemyBuildings = new List<Building>();        
        List<BaseObject> SelectedObjects = new List<BaseObject>();
        List<Unit>[] SquadSelected = new List<Unit>[10];
        public static AudioEngine audio_engine;
        public static WaveBank MiscWB, RacesWB;
        public static SoundBank MiscSB, RacesSB;
        Cue music;        
        GraphicsDeviceManager graphics;
        ContentManager content;
        SpriteBatch spriteBatch;
        Map map;
        Camera camera;
        BitmapFont font;
        Rectangle selectRectangle = new Rectangle(-1,0,0,0);
        int fps = 0;
        int fpsTime = 0;
        int fpsCount = 0;
        Command currentMouseCommand=Command.None;
        Building currentMouseBuilding = null;
        bool currentMouseBuildingDraw = false;

        private Unit FindUnit(ID id)
        {
            if (id.userID == GlobalData.me.userID)
            {
                foreach (Unit u in AllMyUnits)
                {
                       if (u.objectID.objectID == id.objectID)
                         return u;
                }
            }
            else
            {
                foreach (Unit u in AllEnemyUnits)
                {
                    if ((u.objectID.objectID == id.objectID)&&(u.objectID.userID==id.userID))
                        return u;
                }
            }
            return null;
        }
        private BaseObject FindObject(ID id)
        {
            if (id.userID == GlobalData.me.userID)
            {
                foreach (Unit u in AllMyUnits)
                {
                       if (u.objectID.objectID == id.objectID)
                         return u;
                }
                foreach (Building b in AllMyBuildings)
            	{
                    if(b.objectID.objectID==id.objectID)
                        return b;
	            }
            }
            else
            {
                foreach (Unit u in AllEnemyUnits)
                {
                    if ((u.objectID.objectID == id.objectID)&&(u.objectID.userID==id.userID))
                        return u;
                }
                foreach (Building b in AllEnemyBuildings)
	            {
		            if ((b.objectID.objectID == id.objectID)&&(b.objectID.userID==id.userID))
                        return b;
            	}
            }
            return null;
        }
        private void Selectobjects(Vector4 selectVector)
        {
            foreach (BaseObject bo in SelectedObjects)
                bo.selected = false;
            SelectedObjects.Clear();            
            //units
            foreach (Unit u in AllMyUnits)
            {                
                if (u.position.X <= selectVector.W)
                {
                    if (u.position.Y <= selectVector.Z)
                    {
                        if (u.position.X + 1.0f >= selectVector.X)
                        {
                            if (u.position.Y + 1.0f >= selectVector.Y)
                            {
                                if (!u.IsBeingBuilt)
                                {
                                    SelectedObjects.Add(u);
                                    u.selected = true;
                                }
                            }
                        }
                    }
                }
            }
            if (SelectedObjects.Count == 1)
            {
                UserInterface.SelectObject(SelectedType.Unit,SelectedObjects[0]);                
                return;
            }
            if (SelectedObjects.Count > 1)
            {
                UserInterface.SelectObject(SelectedType.MultipleUnits, SelectedObjects[0]);                
                return;
            }
            //buildings            
                foreach (Building b in AllMyBuildings)
                {
                    if (b.position.X <= selectVector.W)
                    {
                        if (b.position.Y <= selectVector.Z)
                        {
                            if (b.position.X + b.size.X >= selectVector.X)
                            {
                                if (b.position.Y + b.size.Y >= selectVector.Y)
                                {                                    
                                    SelectedObjects.Add(b);
                                    b.selected = true;
                                }
                            }
                        }
                    }
                }
                if (SelectedObjects.Count == 1)
                {
                    UserInterface.SelectObject(SelectedType.Building,SelectedObjects[0]);                    
                    return;
                }
                if (SelectedObjects.Count > 1)
                {
                    UserInterface.SelectObject(SelectedType.MultipleBuildings,SelectedObjects[0]);                    
                    return;
                }
                UserInterface.SelectObject(SelectedType.None,null);                
        }
        private void MoveUnits(Vector2 destination)
        {
            foreach (BaseObject bo in SelectedObjects)
            {
                Unit u;
                if (bo is Unit)
                {                    
                    u = (Unit)bo;
                    if (u.target != null)
                        continue;
                    u.target=null;
                    if (Network.isHost == false)
                    {
                        MoveMessage mm = new MoveMessage();
                        mm.senderID = GlobalData.me.userID;
                        mm.objectID = u.objectID;
                        List<Point> list = new List<Point>();

                        list.Add(new Point((int)u.position.X, (int)u.position.Y));
                        list.Add(new Point((int)destination.X, (int)destination.Y));
                        mm.way = list;
                        Network.SendMessage(mm);
                        u.Moving = false;
                    }
                    else
                    {
                        MoveMessage mm = new MoveMessage();
                        mm.senderID = GlobalData.me.userID;
                        mm.objectID = u.objectID;

                        u.way = map.FindWay(new Point((int)u.position.X, (int)u.position.Y), new Point((int)destination.X, (int)destination.Y), u);
                        mm.way = u.way;
                        Network.SendMessage(mm);
                        u.Moving = true;
                        u.way.RemoveAt(0);
                    }
                }
            }
        }
        public DuneGame()
        {
            GameSettings.LoadSettings(null);
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            content = new ContentManager(Services);
            graphics.SynchronizeWithVerticalRetrace = false;
            IsMouseVisible = true;            
            this.graphics.IsFullScreen = true;
            for (int i = 0; i < SquadSelected.Length; i++)
                SquadSelected[i] = new List<Unit>();
            UserInterface.ScreenSize = new Point(graphics.PreferredBackBufferWidth,
                                                graphics.PreferredBackBufferHeight);

        }        
        protected override void Initialize()
        {            
            // TODO: Add your initialization logic here
            Supplies.Initialize(content);
                
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            font = new BitmapFont("sylfaen16.xml");

            audio_engine = new AudioEngine("Content\\Audio\\Dune_Project.xgs");
            MiscWB = new WaveBank(audio_engine, "Content\\Audio\\Misc.xwb");
            MiscSB = new SoundBank(audio_engine, "Content\\Audio\\MiscSB.xsb");
            RacesWB = new WaveBank(audio_engine, "Content\\Audio\\Races.xwb");
            RacesSB = new SoundBank(audio_engine, "Content\\Audio\\RacesSB.xsb");

            base.Initialize();
        }
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                // TODO: Load any ResourceManagementMode.Automatic content

                //Load units
                Textures.Add("Unit_Carryall", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_Carryall"), 8, 2, 0));
                Textures.Add("Unit_Devastator", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_Devastator"), 8, 1, 0));
                Textures.Add("Unit_DevastatorBase", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_DevastatorBase"), 8, 1, 0));
                Textures.Add("Unit_Frigate", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_Frigate"), 8, 1, 0));
                Textures.Add("Unit_Harvester", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_Harvester"), 8, 1, 0));
                Textures.Add("Unit_HarvesterSand", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_HarvesterSand"), 8, 3, 0));
                Textures.Add("Unit_Infantry", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_Infantry"), 4, 3, 500));
                Textures.Add("Unit_Launcher", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_Launcher"), 8, 1, 0));
                Textures.Add("Unit_MCV", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_MCV"), 8, 1, 0));
                Textures.Add("Unit_Ornithopter", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_Ornithopter"), 8, 3, 500));
                Textures.Add("Unit_Quad", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_Quad"), 8, 1, 0));
                Textures.Add("Unit_Saboteur", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_Saboteur"), 4, 3, 500));
                Textures.Add("Unit_Sandworm", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_Sandworm"), 1, 5, 500));
                Textures.Add("Unit_SiegeTank", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_SiegeTank"), 8, 1, 0));
                Textures.Add("Unit_SiegeTankBase", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_SiegeTankBase"), 8, 1, 0));
                Textures.Add("Unit_SonicTank", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_SonicTank"), 8, 1, 0));
                Textures.Add("Unit_Tank", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_Tank"), 8, 1, 0));
                Textures.Add("Unit_TankBase", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_TankBase"), 8, 1, 0));
                Textures.Add("Unit_Trike", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_Trike"), 8, 1, 0));
                Textures.Add("Unit_Trooper", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Unit_Trooper"), 4, 3, 500));

                //Load Terrain
                Textures.Add("Terrain_Dunes", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Terrain_Dunes"), 16, 1, 0));
                Textures.Add("Terrain_Mountain", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Terrain_Mountain"), 16, 1, 0));
                Textures.Add("Terrain_Rock", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Terrain_Rock"), 16, 1, 0));
                Textures.Add("Terrain_Spice", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Terrain_Spice"), 16, 1, 0));

                //Load Pictures
                Textures.Add("Picture_MCV", content.Load<Texture2D>(@"Content\Textures\Picture_MCV"));
                Textures.Add("Picture_Tank", content.Load<Texture2D>(@"Content\Textures\Picture_Tank"));
                Textures.Add("Picture_ConstructionYard", content.Load<Texture2D>(@"Content\Textures\Picture_ConstructionYard"));
                Textures.Add("Picture_WindTrap", content.Load<Texture2D>(@"Content\Textures\Picture_WindTrap"));
                Textures.Add("Picture_Barracks", content.Load<Texture2D>(@"Content\Textures\Picture_Barracks"));
                Textures.Add("Picture_Carryall", content.Load<Texture2D>(@"Content\Textures\Picture_Carryall"));
                Textures.Add("Picture_DeathHand", content.Load<Texture2D>(@"Content\Textures\Picture_DeathHand"));
                Textures.Add("Picture_Devastator", content.Load<Texture2D>(@"Content\Textures\Picture_Devastator"));
                Textures.Add("Picture_Deviator", content.Load<Texture2D>(@"Content\Textures\Picture_Deviator"));
                Textures.Add("Picture_Fatr", content.Load<Texture2D>(@"Content\Textures\Picture_Fatr"));
                Textures.Add("Picture_Fhark", content.Load<Texture2D>(@"Content\Textures\Picture_Fhark"));
                Textures.Add("Picture_Fordos", content.Load<Texture2D>(@"Content\Textures\Picture_Fordos"));
                Textures.Add("Picture_Fremen", content.Load<Texture2D>(@"Content\Textures\Picture_Fremen"));
                Textures.Add("Picture_Frigate", content.Load<Texture2D>(@"Content\Textures\Picture_Frigate"));
                Textures.Add("Picture_GunTurret", content.Load<Texture2D>(@"Content\Textures\Picture_GunTurret"));
                Textures.Add("Picture_Harvester", content.Load<Texture2D>(@"Content\Textures\Picture_Harvester"));
                Textures.Add("Picture_HeavyFactory", content.Load<Texture2D>(@"Content\Textures\Picture_HeavyFactory"));
                Textures.Add("Picture_HeavyInfantry", content.Load<Texture2D>(@"Content\Textures\Picture_HeavyInfantry"));
                Textures.Add("Picture_HighTechFactory", content.Load<Texture2D>(@"Content\Textures\Picture_HighTechFactory"));
                Textures.Add("Picture_Infantry", content.Load<Texture2D>(@"Content\Textures\Picture_Infantry"));
                Textures.Add("Picture_IX", content.Load<Texture2D>(@"Content\Textures\Picture_IX"));
                Textures.Add("Picture_Launcher", content.Load<Texture2D>(@"Content\Textures\Picture_Launcher"));
                Textures.Add("Picture_LightFactory", content.Load<Texture2D>(@"Content\Textures\Picture_LightFactory"));
                Textures.Add("Picture_Ornithopter", content.Load<Texture2D>(@"Content\Textures\Picture_Ornithopter"));
                Textures.Add("Picture_Palace", content.Load<Texture2D>(@"Content\Textures\Picture_Palace"));
                Textures.Add("Picture_Quad", content.Load<Texture2D>(@"Content\Textures\Picture_Quad"));
                Textures.Add("Picture_Radar", content.Load<Texture2D>(@"Content\Textures\Picture_Radar"));
                Textures.Add("Picture_Raider", content.Load<Texture2D>(@"Content\Textures\Picture_Raider"));
                Textures.Add("Picture_Refinery", content.Load<Texture2D>(@"Content\Textures\Picture_Refinery"));
                Textures.Add("Picture_RepairYard", content.Load<Texture2D>(@"Content\Textures\Picture_RepairYard"));
                Textures.Add("Picture_RocketTurret", content.Load<Texture2D>(@"Content\Textures\Picture_RocketTurret"));
                Textures.Add("Picture_Saboteur", content.Load<Texture2D>(@"Content\Textures\Picture_Saboteur"));
                Textures.Add("Picture_Sandworm", content.Load<Texture2D>(@"Content\Textures\Picture_Sandworm"));
                Textures.Add("Picture_Sardaukar", content.Load<Texture2D>(@"Content\Textures\Picture_Sardaukar"));
                Textures.Add("Picture_SiegeTank", content.Load<Texture2D>(@"Content\Textures\Picture_SiegeTank"));
                Textures.Add("Picture_Silo", content.Load<Texture2D>(@"Content\Textures\Picture_Silo"));
                Textures.Add("Picture_Slab1", content.Load<Texture2D>(@"Content\Textures\Picture_Slab1"));
                Textures.Add("Picture_Slab4", content.Load<Texture2D>(@"Content\Textures\Picture_Slab4"));
                Textures.Add("Picture_SonicTank", content.Load<Texture2D>(@"Content\Textures\Picture_SonicTank"));
                Textures.Add("Picture_Starport", content.Load<Texture2D>(@"Content\Textures\Picture_Starport"));
                Textures.Add("Picture_Trike", content.Load<Texture2D>(@"Content\Textures\Picture_Trike"));
                Textures.Add("Picture_Wall", content.Load<Texture2D>(@"Content\Textures\Picture_Wall"));



                //Load Structures

                Textures.Add("Structure_ConstructionYard", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_ConstructionYard"), 1, 2, 300));
                Textures.Add("Structure_WindTrap", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_WindTrap"), 1, 2, 300));
                Textures.Add("Structure_Barracks", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Barracks"), 1, 2, 300));
                Textures.Add("Structure_BuildingConyard", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_BuildingConyard"), 1, 1, 300));
                Textures.Add("Structure_BuildingTurret", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_BuildingTurret"), 1, 1, 300));
                Textures.Add("Structure_Building22", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Building22"), 1, 1, 300));
                Textures.Add("Structure_Building23", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Building23"), 1, 1, 300));
                Textures.Add("Structure_Building33", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Building33"), 1, 1, 300));
                Textures.Add("Structure_GunTurret", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_GunTurret"), 8, 1, 300));
                Textures.Add("Structure_HeavyFactory", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_HeavyFactory"), 1, 2, 300));
                Textures.Add("Structure_HighTechFactory", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_HighTechFactory"), 1, 2, 300));
                Textures.Add("Structure_IX", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_IX"), 1, 2, 300));
                Textures.Add("Structure_LightFactory", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_LightFactory"), 1, 2, 300));
                Textures.Add("Structure_Palace", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Palace"), 1, 2, 300));
                Textures.Add("Structure_Radar", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Radar"), 4, 2, 300));
                Textures.Add("Structure_Refinery", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Refinery"), 4, 2, 300));
                Textures.Add("Structure_RepairYard", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_RepairYard"), 3, 2, 300));
                Textures.Add("Structure_RocketTurret", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_RocketTurret"), 8, 1, 300));
                Textures.Add("Structure_Silo", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Silo"), 1, 2, 300));
                Textures.Add("Structure_Slab1", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Slab1"), 1, 1, 300));
                Textures.Add("Structure_Slab4", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Slab4"), 1, 1, 300));
                Textures.Add("Structure_StarPort", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_StarPort"), 1, 2, 300));
                Textures.Add("Structure_Wall", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Wall"), 1, 1, 300));
                Textures.Add("Structure_WOR", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_WOR"), 1, 2, 300));



                //Load global graphics
                GlobalData.objectSelected = new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Selection"), 1, 2, 500);
                GlobalData.Structure_Building[2][2] = new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Building22"), 1, 1, 0);
                GlobalData.Structure_Building[3][2] = new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Building23"), 1, 1, 0);
                GlobalData.Structure_Building[3][3] = new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\Structure_Building33"), 1, 1, 0);
                Textures.Add("MiniMap_Object", content.Load<Texture2D>(@"Content\Textures\MiniMap_Object"));
                Textures.Add("UI_Selection", new ExtendedTexture(content.Load<Texture2D>(@"Content\Textures\UI\UI_Selection"), 1, 1, 0));

                //Load UI graphics
                UserInterface.uiTopBkg = content.Load<Texture2D>(@"Content\Textures\UI\UI_Background");
                UserInterface.uiRightBkg = content.Load<Texture2D>(@"Content\Textures\UI\UI_RightBackground");
                UserInterface.uiRightBottomBkg = content.Load<Texture2D>(@"Content\Textures\UI\UI_RightBottomBkg");
                UserInterface.uiRightTopBkg = content.Load<Texture2D>(@"Content\Textures\UI\UI_RightTopBkg");
                UserInterface.uiRightMiddleBkg = content.Load<Texture2D>(@"Content\Textures\UI\UI_RightMiddleBkg");
                UserInterface.uiHorizontalBkg = content.Load<Texture2D>(@"Content\Textures\UI\UI_HorizontalBkg");
                UserInterface.Evaluate();

                UserInterface.uiButtonMove = content.Load<Texture2D>(@"Content\Textures\UI\UI_ButtonMove");
                UserInterface.uiButtonAttack = content.Load<Texture2D>(@"Content\Textures\UI\UI_ButtonAttack");
                UserInterface.uiButtonDeploy = content.Load<Texture2D>(@"Content\Textures\UI\UI_ButtonDeploy");
                UserInterface.uiButtonDestroy = content.Load<Texture2D>(@"Content\Textures\UI\UI_ButtonDestroy");
                UserInterface.uiButtonHarvest = content.Load<Texture2D>(@"Content\Textures\UI\UI_ButtonHarvest");
                UserInterface.uiButtonRepair = content.Load<Texture2D>(@"Content\Textures\UI\UI_ButtonRepair");
                UserInterface.uiButtonStop = content.Load<Texture2D>(@"Content\Textures\UI\UI_ButtonStop");


                map = new Map("Content\\Maps\\Map1.map");
                int topMenuSize = UserInterface.GetUITopRect.Height;
                int rightMenuSize = UserInterface.GetUIRightRect.Width;
                camera = new Camera(spriteBatch, new Vector4(7.9f, 7.5f, 20, 20), map.size, new Rectangle(0, topMenuSize, Window.ClientBounds.Width - rightMenuSize, Window.ClientBounds.Height - topMenuSize));

                font.Reset(graphics.GraphicsDevice);
                UserInterface.bf.Reset(graphics.GraphicsDevice);
            }
            // TODO: Load any ResourceManagementMode.Manual content
        }        

        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent == true)
            {
                content.Unload();
            }
        }
        private void RemoveObject(ID id)
        {
            if (id.userID == GlobalData.me.userID)
            {
                //My Units
                for (int i = 0; i < AllMyUnits.Count; i++)
                {
                    if (AllMyUnits[i].objectID.objectID == id.objectID)
                    {
                        if (AllMyUnits[i].selected)
                        {
                            for (int j = 0; j < SelectedObjects.Count; j++)
                            {
                                if (SelectedObjects[j].objectID.objectID == id.objectID)
                                {
                                    SelectedObjects.RemoveAt(j);
                                    if (SelectedObjects.Count == 0)
                                        UserInterface.SelectObject(SelectedType.None, null);
                                    if (SelectedObjects.Count == 1)
                                        UserInterface.SelectObject(SelectedType.Unit, SelectedObjects[0]);
                                    if (SelectedObjects.Count > 1)
                                        UserInterface.SelectObject(SelectedType.MultipleUnits, SelectedObjects[0]);
                                    break;
                                }
                            }
                            for (int j = 0; j < SquadSelected.Length; j++)
                            {
                                List<Unit> list = SquadSelected[j];
                                for (int k = 0; k < list.Count; k++)
                                {
                                    if (list[k].objectID.objectID == id.objectID)
                                    {
                                        list.RemoveAt(k);
                                        break;
                                    }
                                }
                            }
                        }

                        switch (AllMyUnits[i].name)
                        {
                            case "Infantry":
                            case "Trooper":
                                DuneGame.MiscSB.PlayCue("scream");
                                break;
                            default:
                                DuneGame.MiscSB.PlayCue("mediumExplosion");
                                break;
                        }
                        AllMyUnits.RemoveAt(i);
                        return;
                    }
                }
                //My Buildings
                for (int i = 0; i < AllMyBuildings.Count; i++)
                {
                    if (AllMyBuildings[i].objectID.objectID == id.objectID)
                    {
                        if (AllMyBuildings[i].selected)
                        {
                            for (int j = 0; j < SelectedObjects.Count; j++)
                            {
                                if (SelectedObjects[j].objectID.objectID == id.objectID)
                                {
                                    SelectedObjects.RemoveAt(j);
                                    if (SelectedObjects.Count == 0)
                                        UserInterface.SelectObject(SelectedType.None, null);
                                    if (SelectedObjects.Count == 1)
                                        UserInterface.SelectObject(SelectedType.Building, SelectedObjects[0]);
                                    if (SelectedObjects.Count > 1)
                                        UserInterface.SelectObject(SelectedType.MultipleBuildings, SelectedObjects[0]);
                                    break;
                                }
                            }
                        }

                        DuneGame.MiscSB.PlayCue("mediumExplosion");

                        AllMyBuildings.RemoveAt(i);
                        return; ;
                    }
                }
            }
            else
            {
                //enemy Units
                for (int i = 0; i < AllEnemyUnits.Count; i++)
                {
                    if ((AllEnemyUnits[i].objectID.objectID == id.objectID) && (AllEnemyUnits[i].objectID.userID == id.userID))
                    {
                        switch (AllEnemyUnits[i].name)
                        {
                            case "Infantry":
                            case "Trooper":
                                DuneGame.MiscSB.PlayCue("scream");
                                break;
                            default:
                                DuneGame.MiscSB.PlayCue("mediumExplosion");
                                break;

                        }

                        AllEnemyUnits.RemoveAt(i);
                        return;
                    }
                }
                //enemy Buildings
                for (int i = 0; i < AllEnemyBuildings.Count; i++)
                {
                    if ((AllEnemyBuildings[i].objectID.objectID == id.objectID) && (AllEnemyBuildings[i].objectID.userID == id.userID))
                    {
                        DuneGame.MiscSB.PlayCue("mediumExplosion");

                        AllEnemyBuildings.RemoveAt(i);
                        return;
                    }
                }
            }
        }
        private bool mayMove(Vector2 position, Vector2 moveVector)
        {
            if (moveVector.X > 0)
                position.X += 1.0f;
            else if (moveVector.X < 0)
                position.X -= 1.0f;
            if (moveVector.Y > 0)
                position.Y += 1;
            else if (moveVector.Y < 0)
                position.Y -= 1;
            return true;
            return (mayBuild(new Point((int)position.X, (int)position.Y), new Point(1, 1),null,false));     
        }
        private bool mayBuild(Point position, Point size, Unit unit, bool considerFog)
        {
            if (position.X < 0|| position.Y<0||(position.X+size.X>map.size.X)||(position.Y+size.Y>map.size.Y))
                return false;
            for (int a = 0; a < size.X; a++)
            {
                for (int b = 0; b < size.Y; b++)
                {
                    if (map.terrain[position.X + a + map.size.X * (position.Y + b)].LogicalValue != 2)
                        return false;
                    if(considerFog)
                        if (map.fog[position.X + a + map.size.X * (position.Y + b)])
                            return false;
                }
            }
            foreach (Unit u in AllMyUnits)
            {
                if (u != unit)
                {
                    if (!u.isFlying)
                    {
                        if ((int)u.position.X >= position.X)
                        {
                            if ((int)u.position.Y >= position.Y)
                            {
                                if ((int)u.position.X < position.X + size.X)
                                {
                                    if ((int)u.position.Y < position.Y + size.Y)
                                        return false;
                                }
                            }
                        }
                    }
                }
            }
            foreach (Unit u in AllEnemyUnits)
            {
                if (!u.isFlying)
                {
                    if ((int)u.position.X >= position.X)
                    {
                        if ((int)u.position.Y >= position.Y)
                        {
                            if ((int)u.position.X < position.X + size.X)
                            {
                                if ((int)u.position.Y < position.Y + size.Y)
                                    return false;
                            }
                        }
                    }
                }
            }
            foreach (Building u in AllMyBuildings)
            {
                if ((int)u.position.X + u.size.X > position.X)
                {
                    if ((int)u.position.Y + u.size.Y > position.Y)
                    {
                        if ((int)u.position.X < position.X + size.X)
                        {
                            if ((int)u.position.Y < position.Y + size.Y)
                                return false;
                        }
                    }
                }             
            }
            foreach (Building u in AllEnemyBuildings)
            {
                if ((int)u.position.X + u.size.X > position.X)
                {
                    if ((int)u.position.Y + u.size.Y > position.Y)
                    {
                        if ((int)u.position.X < position.X + size.X)
                        {
                            if ((int)u.position.Y < position.Y + size.Y)
                                return false;
                        }
                    }
                }             
            }
            return true;
        }        
        protected override void Update(GameTime gameTime)
        {
            audio_engine.Update();
            #region InitializeNewGame
            if (GlobalData.playersReadyToPlay != null)
            {
                if (Network.isHost == false)
                {
                    Network.SendMessage(new ReadyToPlayMessage());
                    GlobalData.playersReadyToPlay = null;
                }
                else
                {
                    for (int i = 0; i < GlobalData.playersReadyToPlay.Length; i++)
                    {
                        if (GlobalData.playersReadyToPlay[i] == false)
                            return;
                    }
                    GlobalData.playersReadyToPlay = null;
                    Random random = new Random();
                    Unit unit;
                    int x, y;
                    foreach (User u in GlobalData.usersList)
                    {
                        x = random.Next(map.size.X);
                        y = random.Next(map.size.Y);
                        Network.SendMessage(new CreateMessage(new ID(u.userID, 0), "MCV", new Point(x, y),false));
                        unit = Unit.CreateNewUnit("MCV", u.userID, new Point(x, y), u.House);
                        unit.objectID.objectID = 0;
                        unit.IsBeingBuilt = false;
                        unit.currentHealth = unit.maxHealth;
                        if (u.userID == GlobalData.me.userID)
                            AllMyUnits.Add(unit);
                        else
                            AllEnemyUnits.Add(unit); 
                    }
                   /* x = random.Next(map.size.X);
                    y = random.Next(map.size.Y);
                    AllMyUnits.Add(Unit.CreateNewUnit("MCV", GlobalData.me.userID, new Point(x, y), GlobalData.me.house));*/
                    camera.Center(new Vector2(AllMyUnits[0].position.X, AllMyUnits[0].position.Y));
                }
            }
            #endregion
            #region MaintainNewMessages
            lock (Network.messages)
            {
                for (int i = 0; i < Network.messages.Count; i++)
                {
                    NetworkMessage nm = Network.messages.Dequeue();
                    if (nm is CreateMessage)
                    {
                        CreateMessage cm = (CreateMessage)nm;
                        if (cm.isBuilding)
                        {
                            if (Network.isHost)
                            {
                                if (mayBuild(cm.position, GlobalData.gameSettings.GetBuildingByName(cm.name).size, null,false))
                                {
                                    Network.SendMessage(cm);
                                    Building b = Building.CreateNewBuilding(cm.name, cm.objectID.userID, cm.position, GlobalData.GetHouse(cm.objectID.userID));
                                    b.objectID.objectID = cm.objectID.objectID;
                                    if (cm.objectID.userID == GlobalData.me.userID)
                                        AllMyBuildings.Add(b);
                                    else
                                        AllEnemyBuildings.Add(b);
                                }
                            }
                            else
                            {
                                Building b = Building.CreateNewBuilding(cm.name, cm.objectID.userID, cm.position, GlobalData.GetHouse(cm.objectID.userID));
                                b.objectID.objectID = cm.objectID.objectID;
                                if (cm.objectID.userID == GlobalData.me.userID)
                                    AllMyBuildings.Add(b);
                                else
                                    AllEnemyBuildings.Add(b);
                            }
                        }
                        else
                        {
                            if (Network.isHost)
                                Network.SendMessage(cm);
                            Unit u = Unit.CreateNewUnit(cm.name, cm.objectID.userID, cm.position, GlobalData.GetHouse(cm.objectID.userID));
                            u.objectID = cm.objectID;
                            if (u.objectID.objectID == 0)
                            {
                                u.currentHealth = u.maxHealth;
                                u.IsBeingBuilt = false;
                            }
                            if (cm.objectID.userID == GlobalData.me.userID)
                            {
                                AllMyUnits.Add(u);
                                if (cm.objectID.objectID == 0)
                                    camera.Center(new Vector2(cm.position.X, cm.position.Y));
                            }
                            else
                                AllEnemyUnits.Add(u);                            
                        }
                    }
                    if (nm is MoveMessage)
                    {
                        if (Network.isHost == false)    //client
                        {
                            Unit u = FindUnit(((MoveMessage)nm).objectID);
                            if (u != null)
                            {
                                u.Moving = true;
                                u.position = new Vector3(((MoveMessage)nm).way[0].X, ((MoveMessage)nm).way[0].Y, 0);
                                ((MoveMessage)nm).way.RemoveAt(0);
                                u.way = ((MoveMessage)nm).way;
                            }
                        }
                        else
                        {
                            //serwer
                            Unit u = FindUnit(((MoveMessage)nm).objectID);
                            MoveMessage mm = new MoveMessage();
                            List<Point> way = map.FindWay(((MoveMessage)nm).way[0], ((MoveMessage)nm).way[1], u);
                            mm.senderID = GlobalData.me.userID;
                            mm.objectID = u.objectID;
                            mm.way = way;
                            Network.SendMessage(mm);
                            u.Moving = true;
                            way.RemoveAt(0);
                            u.way = way;
                        }
                    }
                    if(nm is AttackMessage)
                    {
                        BaseObject bo=FindObject(((AttackMessage)nm).idAttacked);
                        if(bo!=null)
                            bo.currentHealth-=((AttackMessage)nm).damage;
                    }
                    if (nm is DestroyMessage)
                    {
                        ID id = ((DestroyMessage)nm).objectID;                        
                        RemoveObject(id);
                    }
                }
            }
            #endregion 
            KeyboardInput(gameTime);
            MouseInput(gameTime);
            Audio();
            #region MaintainUnits
            for(int i=0;i<AllMyUnits.Count;i++)
            {
                Unit u = AllMyUnits[i];
                if (u.target != null)
                {
                    if (u.target.currentHealth <= 0)
                        u.target = null;
                }
                map.removeFog(new Point((int)u.position.X, (int)u.position.Y),u.viewRange);
                if (u.IsBeingBuilt)
                {
                    u.currentHealth += (float)(u.buildSpeed * gameTime.ElapsedGameTime.Milliseconds) / 1000.0f;
                    if (u.currentHealth >= (float)u.maxHealth)
                    {
                        u.currentHealth = (float)u.maxHealth;
                        u.IsBeingBuilt = false;
                    }
                }
                if(u.reloadTimePassed>=0)
                {
                    u.reloadTimePassed+=gameTime.ElapsedGameTime.Milliseconds;
                    if(u.reloadTimePassed>=u.reloadTime)
                        u.reloadTimePassed=-1;
                }
                if(u.target!=null)
                {
                    if(((Vector3)(u.position-u.target.position)).Length()>u.fireRange)
                    {
                        if(!u.Moving)
                        {
                            u.Moving = true;
                            MoveMessage mm = new MoveMessage();
                            mm.objectID=u.objectID;
                            mm.senderID=GlobalData.me.userID;
                            if(Network.isHost)
                            {
                                u.way = map.FindWay(new Point((int)u.position.X,(int)u.position.Y),new Point((int)u.target.position.X,(int)u.target.position.Y),u);
                                mm.way=u.way;
                            }
                            else
                            {
                                mm.way=new List<Point>();
                                mm.way.Add(new Point((int)u.position.X,(int)u.position.Y));
                                mm.way.Add(new Point((int)u.target.position.X,(int)u.target.position.Y));
                            }                
                            Network.SendMessage(mm);
                        }
                    }
                    else
                    {
                        if(u.reloadTimePassed<0)
                        {
                        u.Moving=false;
                        u.way = null;
                        if (u.turretTexture != null)
                            u.turretDirection = new Vector2(((Vector3)(u.target.position - u.position)).X, ((Vector3)(u.target.position - u.position)).Y);
                        AttackMessage am = new AttackMessage();
                        am.senderID=GlobalData.me.userID;
                        am.idAttacker=u.objectID;
                        am.idAttacked=u.target.objectID;
                        am.damage=u.damage;
                        Network.SendMessage(am);
                        u.reloadTimePassed=0;
                        if(Network.isHost)
                            u.target.currentHealth-=u.damage;                            
                        }
                        
                    }
                }
                if (u.Moving&&u.way!=null)
                {                    
                    Vector2 v2Position=new Vector2(u.position.X, u.position.Y);
                    Vector2 v2Way = new Vector2(u.way[0].X,u.way[0].Y);
                    Vector2 dv = v2Position - v2Way;                    
                    u.direction = v2Way - v2Position;
                    if (dv.Length() < 0.1f)
                    {
                        u.position = new Vector3(v2Way.X, v2Way.Y, 0);
                        u.way.RemoveAt(0);
                        if (u.way.Count == 0)
                            u.Moving = false;
                        continue;
                    }
                    u.direction.Normalize();
                    Vector2 moveVector=u.direction*((u.speed*(float)gameTime.ElapsedGameTime.Milliseconds)/1000.0f);
                    if (dv.Length() <= moveVector.Length())
                    {
                        u.position = new Vector3(v2Way.X, v2Way.Y, 0);
                        u.way.RemoveAt(0);
                        if (u.way.Count == 0)
                            u.Moving = false;
                    }
                    else
                    {
                        if (mayMove(new Vector2(u.position.X,u.position.Y), moveVector))
                            u.position += new Vector3(moveVector.X,moveVector.Y,0);
                        else
                        {
                            MoveMessage mm=new MoveMessage();
                            mm.objectID=u.objectID;
                            mm.senderID=GlobalData.me.userID;                            
                            if (Network.isHost)
                            {
                                u.way=map.FindWay(new Point((int)u.position.X,(int) u.position.Y), u.way[u.way.Count - 1], u);
                                mm.way=u.way;
                            }
                            else
                            {
                                mm.way=new List<Point>();
                                mm.way.Add(new Point((int)u.position.X,(int)u.position.Y));
                                mm.way.Add(u.way[u.way.Count-1]);
                                u.Moving=false;
                            }
                            Network.SendMessage(mm);
                        }
                    }                    
                }
                if(u.currentHealth<=0)
                {
                    DestroyMessage dm=new DestroyMessage();
                    dm.objectID=u.objectID;
                    dm.senderID=GlobalData.me.userID;
                    Network.SendMessage(dm);                    
                    RemoveObject(u.objectID);
                }
            }
            foreach (Unit u in AllEnemyUnits)
            {
                if (u.IsBeingBuilt)
                {
                    u.currentHealth += (float)(u.buildSpeed * gameTime.ElapsedGameTime.Milliseconds) / 1000.0f;
                    if (u.currentHealth >= (float)u.maxHealth)
                    {
                        u.currentHealth = (float)u.maxHealth;
                        u.IsBeingBuilt = false;
                    }
                }
                if (u.Moving)
                {
                    Vector2 v2Position = new Vector2(u.position.X, u.position.Y);
                    Vector2 v2Way = new Vector2(u.way[0].X, u.way[0].Y);
                    Vector2 dv = v2Position - v2Way;
                    u.direction = v2Way - v2Position;
                    if (dv.Length() < 0.1f)
                    {
                        u.position = new Vector3(v2Way.X, v2Way.Y, 0);
                        u.way.RemoveAt(0);
                        if (u.way.Count == 0)
                            u.Moving = false;
                        continue;
                    }
                    u.direction.Normalize();
                    Vector2 moveVector = u.direction * ((u.speed * (float)gameTime.ElapsedGameTime.Milliseconds) / 1000.0f);
                    if (dv.Length() <= moveVector.Length())
                    {
                        u.position = new Vector3(v2Way.X, v2Way.Y, 0);
                        u.way.RemoveAt(0);
                        if (u.way.Count == 0)
                            u.Moving = false;
                    }
                    else
                    {
                        u.position += new Vector3(moveVector.X, moveVector.Y, 0);
                    }
                }
            }
            #endregion
            #region MaintainBuildings
            Supplies.energy = 0;
            map.radarNumber = 0;
            for(int i=0;i<AllMyBuildings.Count;i++)
            {                
                Building b = AllMyBuildings[i];
                if (b.name == "Radar"&&b.IsBeingBuilt==false)
                    map.radarNumber++;
                map.removeFog(new Point((int)b.position.X,(int) b.position.Y),b.viewRange);
                if (b.IsBeingBuilt)
                {
                    b.currentHealth += (float)(b.buildSpeed * gameTime.ElapsedGameTime.Milliseconds) / 1000.0f;
                    if (b.currentHealth >= (float)b.maxHealth)
                    {
                        b.currentHealth = (float)b.maxHealth;
                        b.IsBeingBuilt = false;
                    }
                }
                else
                {                    
                    Supplies.energy -= b.energyConsumption;
                }
                if (b.currentHealth <= 0)
                {
                    DestroyMessage dm = new DestroyMessage();
                    dm.objectID = b.objectID;
                    dm.senderID = GlobalData.me.userID;
                    Network.SendMessage(dm);
                    RemoveObject(b.objectID);
                }
            }
            foreach (Building b in AllEnemyBuildings)
            {
                if (b.IsBeingBuilt)
                {
                    b.currentHealth += (float)(b.buildSpeed * gameTime.ElapsedGameTime.Milliseconds) / 1000.0f;
                    if (b.currentHealth >= (float)b.maxHealth)
                    {
                        b.currentHealth = (float)b.maxHealth;
                        b.IsBeingBuilt = false;
                    }
                }             
            }
            #endregion
            base.Update(gameTime);
        }
        private void Audio()
        {
            if (music == null)
            {
                music = MiscSB.GetCue("musicPeace");
                music.Play();
            }
            else if (music.IsPaused)
            {
                music.Resume();
            }
            else if (music.IsStopped)
            {
                music = MiscSB.GetCue("musicPeace");
                music.Play();
            }
        }
        private void MouseInput(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            //Zoom
            float d = MouseControl.lastScrollValue - ms.ScrollWheelValue;
            Vector2 cv=camera.GetCenterPosition();
            camera.Zoom(1.0f + d / 1000.0f);
            camera.Center(cv);
            MouseControl.lastScrollValue = ms.ScrollWheelValue;

            //Scroll map
            if (ms.MiddleButton == ButtonState.Pressed && MouseControl.isMiddleDown)
            {
                camera.Move(new Vector2((ms.X - MouseControl.lastPos.X)/25.0f, (ms.Y - MouseControl.lastPos.Y) /25.0f));
                Mouse.SetPosition(MouseControl.lastPos.X, MouseControl.lastPos.Y);
            }

            //On Right click
            if (ms.RightButton == ButtonState.Pressed && !MouseControl.isRightDown)
            {
                Vector2 destination;
                destination.X = -1; destination.Y = -1;
                if (ms.X >= 0 && ms.X < camera.mapWindowPosition.Width && ms.Y >= camera.mapWindowPosition.Y && ms.Y < (camera.mapWindowPosition.Y + camera.mapWindowPosition.Height))
                {
                    currentMouseBuildingDraw = false;
                    currentMouseBuilding = null;
                    currentMouseCommand = Command.None;
                    destination = camera.GetMapPosition(new Point(ms.X, ms.Y));
                    if (currentMouseCommand == Command.None)
                    {
                        Vector2 v2 = camera.GetMapPosition(new Point(ms.X, ms.Y));
                        Point target = new Point((int)v2.X, (int)v2.Y);
                        foreach (Building b in AllEnemyBuildings)
                        {
                            Point pos = new Point((int)b.position.X, (int)b.position.Y);
                            if (pos.X <= target.X)
                            {
                                if (pos.X + b.size.X > target.X)
                                {
                                    if (pos.Y <= target.Y)
                                    {
                                        if (pos.Y + b.size.Y > target.Y)
                                        {

                                            foreach (BaseObject bo in SelectedObjects)
                                            {
                                                ((Unit)bo).target = null;
                                                if (bo is Unit)
                                                {
                                                    ((Unit)bo).target = b;
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        foreach (Unit enemy in AllEnemyUnits)
                        {
                            Point pos = new Point((int)enemy.position.X, (int)enemy.position.Y);
                            if (pos == target)
                            {
                                foreach (BaseObject bo in SelectedObjects)
                                {
                                    ((Unit)bo).target = null;
                                    if (bo is Unit)
                                    {
                                        ((Unit)bo).target = enemy;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }                  
                Rectangle miniMap = UserInterface.GetMiniMapLocation();
                if (ms.X >= miniMap.Left && ms.X < miniMap.Right && ms.Y >= miniMap.Top && ms.Y < miniMap.Bottom)
                {
                    destination = map.GetMapPositionFromMinimap(new Point(ms.X - UserInterface.GetMiniMapLocation().X, ms.Y - UserInterface.GetMiniMapLocation().Y));
                }
                if (destination.X>=0)
                {
                    MoveUnits(destination);
                }                
            }
            currentMouseBuildingDraw = false;
            if (ms.LeftButton == ButtonState.Released && ms.RightButton == ButtonState.Released)
            {
                //inside map
                if ((ms.X < camera.mapWindowPosition.Width) && (ms.X >= 0) && (ms.Y >= camera.mapWindowPosition.Y) && (ms.Y < camera.mapWindowPosition.Y + camera.mapWindowPosition.Height))
                {
                    if (currentMouseCommand == Command.Build)
                    {
                        if(UserInterface.objectToBuildIsBuilding)
                        {
                            currentMouseBuildingDraw = true;
                        }
                    }
                }
            }
            //On left down
            if (ms.LeftButton == ButtonState.Pressed && MouseControl.isLeftDown == false)
            {
                Rectangle miniMap = UserInterface.GetMiniMapLocation();                
                //inside map
                if ((ms.X < camera.mapWindowPosition.Width) && (ms.X >= 0) && (ms.Y >= camera.mapWindowPosition.Y) && (ms.Y < camera.mapWindowPosition.Y + camera.mapWindowPosition.Height))
                {
                    if (currentMouseCommand == Command.None)
                    {
                        selectRectangle.X = ms.X;
                        selectRectangle.Y = ms.Y;
                        selectRectangle.Width = 0;
                        selectRectangle.Height = 0;
                    }
                    else if (currentMouseCommand == Command.Move)
                    {
                        currentMouseCommand = Command.None;
                        MoveUnits(camera.GetMapPosition(new Point(ms.X,ms.Y)));
                    }
                    else if (currentMouseCommand == Command.Build)
                    {
                        if (mayBuild(new Point((int)currentMouseBuilding.position.X, (int)currentMouseBuilding.position.Y), currentMouseBuilding.size,null,true))
                        {
                            int cost=GlobalData.gameSettings.GetBuildingByName(currentMouseBuilding.name).cost;
                            if (Supplies.spice >= cost)
                            {
                                CreateMessage cm=new CreateMessage(new ID(GlobalData.me.userID,Unit.GetNewID()),currentMouseBuilding.name,new Point((int)camera.GetMapPosition(new Point(ms.X, ms.Y)).X, (int)camera.GetMapPosition(new Point(ms.X, ms.Y)).Y),true);
                                Network.SendMessage(cm);
                                Supplies.spice -= cost;
                                if (Network.isHost)
                                {                                    
                                    currentMouseBuilding.currentHealth = 1;
                                    currentMouseBuilding.IsBeingBuilt = true;
                                    currentMouseBuilding.objectID = cm.objectID;
                                    AllMyBuildings.Add(currentMouseBuilding);                                    
                                }
                                currentMouseBuildingDraw = false;
                                currentMouseBuilding = null;
                                currentMouseCommand = Command.None;
                                MiscSB.PlayCue("placeStructure");
                            }
                            else
                            {
                                MiscSB.PlayCue("cannot");
                            }
                        }
                        else
                            MiscSB.PlayCue("cannot");
                    }
                }           
                //inside Mini Map
                if (ms.X >= miniMap.Left && ms.X < miniMap.Right && ms.Y >= miniMap.Top && ms.Y < miniMap.Bottom)
                {
                    if(currentMouseCommand==Command.None)
                        camera.Center(map.GetMapPositionFromMinimap(new Point(ms.X-UserInterface.GetMiniMapLocation().X,ms.Y-UserInterface.GetMiniMapLocation().Y)));
                    else if (currentMouseCommand == Command.Move)
                    {                        
                        MoveUnits(map.GetMapPositionFromMinimap(new Point(ms.X - UserInterface.GetMiniMapLocation().X, ms.Y - UserInterface.GetMiniMapLocation().Y)));
                        currentMouseCommand = Command.None;                     
                    }
                }
                #region MaintainInterface                
                Command command = UserInterface.GetCommand(new Point(ms.X, ms.Y));
                switch (command )
                {
                    case Command.Deploy:
                    for (int i = 0; i < SelectedObjects.Count; i++)
                    {
                        if (SelectedObjects[i] is Unit)
                        {
                            if (SelectedObjects[i].name == "MCV")
                            {
                                if (!mayBuild(new Point((int)SelectedObjects[i].position.X,(int)SelectedObjects[i].position.Y),new Point(2,2),(Unit)SelectedObjects[i],false))
                                    continue;
                                Point position = new Point((int)SelectedObjects[i].position.X, (int)SelectedObjects[i].position.Y);
                                Unit u = (Unit)SelectedObjects[i];
                                DestroyMessage dm = new DestroyMessage();
                                dm.senderID = GlobalData.me.userID;
                                dm.objectID = u.objectID;
                                Network.SendMessage(dm);
                                if (Network.isHost)
                                    RemoveObject(u.objectID);

                                CreateMessage cm = new CreateMessage(new ID(GlobalData.me.userID, Unit.GetNewID()), "ConstructionYard", position, true);
                                cm.senderID = GlobalData.me.userID;

                                Network.SendMessage(cm);
                                if (Network.isHost)
                                {
                                    Building b = Building.CreateNewBuilding("ConstructionYard", GlobalData.me.userID, position, GlobalData.me.House);
                                    b.objectID = cm.objectID;
                                    AllMyBuildings.Add(b);
                                }
                            }
                        }
                    }
                    break;
                    case Command.Destroy:
                        DestroyMessage destroyMessage = new DestroyMessage();
                        destroyMessage.senderID = GlobalData.me.userID;
                        for(int i=0;i<SelectedObjects.Count;i++)
                        {
                            destroyMessage.objectID = SelectedObjects[i].objectID;
                            Network.SendMessage(destroyMessage);
                            if (Network.isHost)
                                RemoveObject(SelectedObjects[i].objectID);
                        }
                        break;
                    case Command.Move:
                        currentMouseCommand = Command.Move;
                        break;
                    case Command.Stop:
                        foreach (Unit u in SelectedObjects)
                        {
                            u.Moving = false;
                            u.way = null;
                        }
                        break;
                    case Command.Exit:
                        Exit();
                        break;
                    case Command.Build:
                        if (UserInterface.objectToBuildIsBuilding)
                        {
                            currentMouseCommand = Command.Build;
                            currentMouseBuilding = Building.CreateNewBuilding(UserInterface.objectToBuildName, 0, new Point(0, 0), GlobalData.me.House);
                            currentMouseBuilding.IsBeingBuilt = false;
                        }
                        else
                        {
                            UnitData ud = GlobalData.gameSettings.GetUnitByName(UserInterface.objectToBuildName);
                            if (ud.cost <= Supplies.spice)
                            {
                                Supplies.spice -= ud.cost;
                                int []ridableFields=GlobalData.gameSettings.GetBuildingByName(SelectedObjects[0].name).rideableFields;
                                Point size = GlobalData.gameSettings.GetBuildingByName(SelectedObjects[0].name).size;
                                Random rand = new Random(ridableFields.Length);
                                int x,y=0;
                                if (ridableFields.Length > 0)
                                {
                                    x = rand.Next(ridableFields.Length);
                                    x = ridableFields[x];
                                    while (true)
                                    {
                                        if (x >= size.X)
                                        {
                                            x -= size.X;
                                            y++;
                                        }
                                        else
                                            break;
                                    }
                                }
                                else
                                {
                                    x = 1; y = 1;
                                }
                                size.X=(int)SelectedObjects[0].position.X+x;
                                size.Y=(int)SelectedObjects[0].position.Y+y;
                                CreateMessage cm=new CreateMessage(new ID(GlobalData.me.userID,Unit.GetNewID()),ud.name,size,false);
                                Network.SendMessage(cm);
                                if (Network.isHost)
                                {
                                    Unit u = Unit.CreateNewUnit(ud.name, GlobalData.me.userID, size, GlobalData.me.House);
                                    u.objectID = cm.objectID;
                                    AllMyUnits.Add(u);
                                }
                            }

                        }
                        break;
                }


                #endregion
            }
            //On Left click
            if (ms.LeftButton == ButtonState.Released && MouseControl.isLeftDown)
            {                
                //Select unit/building
                if (selectRectangle.X >= 0)
                {
                    selectRectangle.Width = ms.X - selectRectangle.X;
                    selectRectangle.Height = ms.Y - selectRectangle.Y;
                    if (selectRectangle.Width < 0)
                    {
                        selectRectangle.Width *= -1;
                        selectRectangle.X -= selectRectangle.Width;
                    }
                    if (selectRectangle.Height < 0)
                    {
                        selectRectangle.Height *= -1;
                        selectRectangle.Y -= selectRectangle.Height;
                    }
                    Vector2 v2=camera.GetMapPosition(new Point(selectRectangle.X,selectRectangle.Y));
                    Vector4 v4 = new Vector4(v2.X,v2.Y,0,0);
                    v2=camera.GetMapPosition(new Point(selectRectangle.X + selectRectangle.Width,selectRectangle.Y + selectRectangle.Height));
                    v4.W=v2.X;
                    v4.Z=v2.Y;
                    Selectobjects(v4);
                    if (SelectedObjects.Count > 0)
                    {
                        if(SelectedObjects[0] is Unit)
                            MiscSB.PlayCue("response");
                        else
                            MiscSB.PlayCue("buttonClick");
                    }
                    selectRectangle.X = -1;
                }
            }
            if (selectRectangle.X >= 0)
            {
                if((ms.X<0)||(ms.Y<camera.mapWindowPosition.Y)||(ms.X>=camera.mapWindowPosition.Width)||(ms.Y>=camera.mapWindowPosition.Y+camera.mapWindowPosition.Height))
                    Mouse.SetPosition(MouseControl.lastPos.X, MouseControl.lastPos.Y);
            }
            MouseControl.isLeftDown = (ms.LeftButton == ButtonState.Pressed) ? true : false;
            MouseControl.isMiddleDown = (ms.MiddleButton == ButtonState.Pressed) ? true : false;
            MouseControl.isRightDown = (ms.RightButton == ButtonState.Pressed) ? true : false;
            //MouseControl.lastPos = new Point(ms.X, ms.Y);
            MouseControl.lastPos = new Point(Mouse.GetState().X, Mouse.GetState().Y);
        }
        private void KeyboardInput(GameTime gameTime)
        {
            Keys[] ks = Keyboard.GetState().GetPressedKeys();
            float speed = 0.015f;
            foreach (Keys k in ks)
            {
                if (k == Keys.Up)
                    camera.Move(new Vector2(0.0f, -speed * gameTime.ElapsedGameTime.Milliseconds));
                if (k == Keys.Down)
                    camera.Move(new Vector2(0.0f, speed * gameTime.ElapsedGameTime.Milliseconds));
                if (k == Keys.Right)
                    camera.Move(new Vector2(speed * gameTime.ElapsedGameTime.Milliseconds, 0.0f));
                if (k == Keys.Left)
                    camera.Move(new Vector2(-speed * gameTime.ElapsedGameTime.Milliseconds, 0.0f));

            }
        }
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            Viewport oldVievport = graphics.GraphicsDevice.Viewport;
            double speed = 0.001f;
            speed *= gameTime.TotalGameTime.TotalMilliseconds;

            //draw map
            Viewport v = new Viewport();
            v.Y = camera.mapWindowPosition.Y;
            v.X = camera.mapWindowPosition.X;
            v.Width = camera.mapWindowPosition.Width;
            v.Height = camera.mapWindowPosition.Height;
            graphics.GraphicsDevice.Viewport = v;
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            
            map.Render(camera);            
            foreach (Building b in AllMyBuildings)
            {
                b.Render(camera, gameTime, GlobalData.me.House,map);                
            }
            foreach (Building b in AllEnemyBuildings)
            {
                b.Render(camera, gameTime, GlobalData.GetHouse(b.objectID.userID),map);
            }
            foreach (Unit u in AllMyUnits)
            {
                u.Render(camera, gameTime, GlobalData.me.House, map);
            }
            foreach (Unit u in AllEnemyUnits)
            {
                u.Render(camera, gameTime, GlobalData.GetHouse(u.objectID.userID), map);
            }
            if (currentMouseBuildingDraw)
            {
                MouseState ms = Mouse.GetState();
                Point pos = new Point((int)camera.GetMapPosition(new Point(ms.X, ms.Y)).X, (int)camera.GetMapPosition(new Point(ms.X, ms.Y)).Y);
                currentMouseBuilding.position = new Vector3(pos.X, pos.Y, 0);
                if (mayBuild(pos, currentMouseBuilding.size, null,true))
                    currentMouseBuilding.Render(camera, gameTime, GlobalData.me.House, map, new Color(100, 255, 100, 128));
                else
                    currentMouseBuilding.Render(camera, gameTime, GlobalData.me.House, map, new Color(255, 100, 100, 128));                
            }
            spriteBatch.End();
            graphics.GraphicsDevice.Viewport = oldVievport;

            UserInterface.Draw(spriteBatch);

            //selection rectangle
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);            
            if (selectRectangle.X >= 0)
            {
                Rectangle rec = new Rectangle(selectRectangle.X, selectRectangle.Y, Mouse.GetState().X - selectRectangle.X, Mouse.GetState().Y - selectRectangle.Y);
                if (rec.Width < 0)
                {
                    rec.Width *= -1;
                    rec.X -= rec.Width;
                }
                if (rec.Height < 0)
                {
                    rec.Height *= -1;
                    rec.Y -= rec.Height;
                }
                spriteBatch.Draw(Textures.GetTextureReference("UI_Selection").Texture, new Rectangle(rec.X,rec.Y,rec.Width,3), new Color(0, 255, 0, 128));
                spriteBatch.Draw(Textures.GetTextureReference("UI_Selection").Texture, new Rectangle(rec.X, rec.Y, 3, rec.Height), new Color(0, 255, 0, 128));
                spriteBatch.Draw(Textures.GetTextureReference("UI_Selection").Texture, new Rectangle(rec.X+rec.Width-3, rec.Y, 3, rec.Height), new Color(0, 255, 0, 128));
                spriteBatch.Draw(Textures.GetTextureReference("UI_Selection").Texture, new Rectangle(rec.X, rec.Y+rec.Height-3, rec.Width, 3), new Color(0, 255, 0, 128));

            }
            //Draw Mini map  
            if (map.radarNumber>0)
                map.RenderMiniMap(spriteBatch,UserInterface.GetMiniMapLocation(), AllMyUnits,AllMyBuildings,AllEnemyUnits,AllEnemyBuildings,camera);
            
            spriteBatch.End();

            fpsCount++;
            fpsTime += gameTime.ElapsedGameTime.Milliseconds;
            if (fpsTime >= 1000)
            {
                fps = (1000 * fpsCount) / fpsTime;
                fpsCount = 0;
                fpsTime = 0;
            }
            font.DrawString(10, 30, Color.Red, "FPS: " + fps.ToString());           
        }
    }
}