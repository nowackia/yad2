using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using XNAExtras;

namespace Dune_2_Remade
{
    public enum Command { Exit, Move, Attack, Stop, Harvest, Repair, Destroy, None, Deploy, Build };
    public enum SelectedType { Unit, Building, MultipleUnits, MultipleBuildings, None };

    public static class UserInterface
    {
        public static Texture2D uiTopBkg = null;
        public static Texture2D uiRightBkg = null;
        public static Texture2D uiRightTopBkg = null;
        public static Texture2D uiRightBottomBkg = null;
        public static Texture2D uiRightMiddleBkg = null;
        public static Texture2D uiHorizontalBkg = null;
        public static Texture2D uiHouse = null;

        public static Texture2D uiButtonMove = null;
        public static Texture2D uiButtonAttack = null;
        public static Texture2D uiButtonStop = null;
        public static Texture2D uiButtonDeploy = null;
        public static Texture2D uiButtonDestroy = null;
        public static Texture2D uiButtonHarvest = null;
        public static Texture2D uiButtonRepair = null;

        public static int frameW = 14;
        public static int frameHorizH = 24;
        public static int frameH = 14;

        public static BitmapFont bf = new BitmapFont("sylfaen16.xml");

        static BaseObject SelectedObject = null;
        static SelectedType SelectedObjectType = SelectedType.None;

        static Rectangle exitButton;
        static Rectangle uiTop, uiRight, uiRightTop, uiRightMiddle, uiRightBottom, uiHoriz1, uiHoriz2;
        static Rectangle uiCmd1, uiCmd2, uiCmd3, uiCmd4;
        static Rectangle uiPicture, uiHealth;


        static Rectangle[] objectsToBuild = new Rectangle[9];
        static List<string> objectsToBuildNames = new List<string>(9);
        static List<bool> objectsToBuildIsBuilding = new List<bool>(9);
        public static string objectToBuildName;
        public static bool objectToBuildIsBuilding;

        public static void SelectObject(SelectedType t, BaseObject bo)
        {
            SelectedObjectType = SelectedType.None;
            objectsToBuildNames.Clear();
            objectsToBuildIsBuilding.Clear();

            SelectedObject = bo;
            if (SelectedObject == null)
                return;

            if (t == SelectedType.Building)
            {
                Building b = SelectedObject as Building;
                for (int i = 0; i < b.canProduceBuildings.Count; i++)
                {
                    objectsToBuildNames.Add(b.canProduceBuildings[i]);
                    objectsToBuildIsBuilding.Add(true);
                }
                for (int i = 0; i < b.canProduceUnits.Count; i++)
                {
                    objectsToBuildNames.Add(b.canProduceUnits[i]);
                    objectsToBuildIsBuilding.Add(false);
                }
            }
            else
            {
                objectsToBuildNames.Clear();
                objectsToBuildIsBuilding.Clear();
            }

            SelectedObjectType = t;
        }

        public static Rectangle GetUITopRect
        {
            get { return uiTop; }
        }        
        public static Rectangle GetUIRightRect
        {
            get { return uiRight; }
        }
        public static Rectangle GetMiniMapLocation()
        {
            return uiRightBottom;
        }
        static private Point screenSize = new Point(1024, 768);
        static public Point ScreenSize
        {
            get { return screenSize; }
            set { screenSize = value; Evaluate(); }
        }			

        static private float menuSize = 0.05f;
        static public float MenuSize
        {
            get { return menuSize; }
            set { menuSize = value; Evaluate(); }
        }

        static private float scaleWidth = 6.0f;

        static public float ScaleWidth
        {
            get { return scaleWidth; }
            set { scaleWidth = value; Evaluate(); }
        }


        static public void Evaluate()
        {
            int height = (int)(screenSize.Y * menuSize);
            int width = (int)(screenSize.Y * menuSize * scaleWidth);

            uiTop = new Rectangle(0, 0, screenSize.X, height);
            uiRight = new Rectangle(screenSize.X - width, height, width, screenSize.Y - height);

            if (uiRightBkg != null)
            {
                frameW = 14 * width / uiRightBkg.Width;
                frameH = 14 * uiRight.Height / uiRightBkg.Height;
                frameHorizH = 24 * uiRight.Height / uiRightBkg.Height;
            }

            uiRightTop = new Rectangle(screenSize.X - width + frameW, height + frameH,
                                        width - 2 * frameW, width - 2* frameW);
            if (uiRightTopBkg != null)
            {
                //2 to szerokoœæ/wysokoœæ ramki w oryginalnej grafice
                int dBorderX = (uiRightTop.Width / uiRightTopBkg.Width * 2),
                    dBorderY = (uiRightTop.Height / uiRightTopBkg.Height * 2);

                uiPicture = new Rectangle(uiRightTop.X + dBorderX, uiRightTop.Y + dBorderY,
                                        uiRightTop.Width / 2 - dBorderX, uiRightTop.Height / 2 - dBorderY);
                uiHealth = new Rectangle(uiPicture.Right + 1, uiPicture.Y, uiPicture.Width, uiPicture.Height / 2);
            }

            
            int cmdH = uiRightTop.Height / 8;
            uiCmd4 = uiCmd3 = uiCmd2 = uiCmd1 =
                    new Rectangle(uiRightTop.X, uiRightTop.Bottom - cmdH, uiRightTop.Width, cmdH);
            uiCmd2.Y -= cmdH;
            uiCmd3.Y -= 2*cmdH;
            uiCmd4.Y -= 3*cmdH;

            uiRightBottom = new Rectangle(uiRightTop.X, screenSize.Y - uiRightTop.Width - frameH,
                                        uiRightTop.Width, uiRightTop.Height);
            uiRightMiddle = new Rectangle(uiRightTop.X, uiRightTop.Bottom + frameH,
                                        uiRightTop.Width, uiRightBottom.Top - frameH - (uiRightTop.Bottom + frameH));

            int dH = (frameHorizH - frameH) / 2;
            uiHoriz1 = new Rectangle(uiRight.Left, uiRightTop.Bottom - dH, uiRight.Width, frameHorizH);
            uiHoriz2 = new Rectangle(uiHoriz1.X, uiRightMiddle.Bottom - dH, uiHoriz1.Width, frameHorizH);
            
            dH = (uiTop.Height - bf.LineHeight) / 2;
            if (uiTopBkg != null)
            {
                exitButton = new Rectangle(uiTop.X + 10 * uiTop.Width / uiTopBkg.Width, uiTop.Y + dH, bf.MeasureString("Exit"), uiTop.Height - 2 * dH);
            }
            else
            {
                exitButton = new Rectangle(0, 0, 10, 10);
            }

            int mBorderX = 0, mBorderY = 0;
            if (uiRightMiddleBkg != null)
            {
                mBorderX = (uiRightMiddle.Width / uiRightMiddleBkg.Width) * 2;
                mBorderY = (uiRightMiddle.Height / uiRightMiddleBkg.Height) * 2;
            }
            int mdh = (uiRightMiddle.Height - 2 * mBorderY) / 3;
            int mdw = (uiRightMiddle.Width - 2 * mBorderX) / 3;
            for (int x = 0; x < 3; x++)
                for (int y=0; y< 3; y++)
                {
                    objectsToBuild[3 * y + x] = new Rectangle(uiRightMiddle.X + mBorderX + x * mdw, uiRightMiddle.Y+  mBorderY + mdh*y, mdw, mdh);
                }
        }

        public static void Draw(SpriteBatch sB)
        {
            sB.Begin();

            //Draw layout
            if (uiTopBkg != null)
            {
                sB.Draw(uiTopBkg, uiTop, Color.White);
            }
            if (uiRightBkg != null)
            {
                sB.Draw(uiRightBkg, uiRight, Color.White);
            }
            if (uiRightBottomBkg != null)
            {
                sB.Draw(uiRightBottomBkg, uiRightBottom, Color.White);
            }
            if (uiRightTopBkg != null)
            {
                sB.Draw(uiRightTopBkg, uiRightTop, Color.White);
            }
            if (uiRightMiddleBkg != null)
            {
                sB.Draw(uiRightMiddleBkg, uiRightMiddle, Color.White);
            }
            if (uiHorizontalBkg != null)
            {
                sB.Draw(uiHorizontalBkg, uiHoriz1, Color.White);
                sB.Draw(uiHorizontalBkg, uiHoriz2, Color.White);
            }

            //Draw selection-specific commands
            if (SelectedObjectType == SelectedType.MultipleUnits || SelectedObjectType == SelectedType.Unit)
            {
                if (uiButtonDestroy != null)
                    sB.Draw(uiButtonDestroy, uiCmd1, Color.White);
                if (uiButtonStop != null)
                    sB.Draw(uiButtonStop, uiCmd2, Color.White);
                if (uiButtonMove != null)
                    sB.Draw(uiButtonMove, uiCmd3, Color.White);
                if (uiButtonAttack != null)
                    sB.Draw(uiButtonAttack, uiCmd4, Color.White);

                if (SelectedObject.name == "MCV")
                    if (uiButtonDeploy != null)
                        sB.Draw(uiButtonDeploy, uiCmd4, Color.White);
                if (SelectedObject.name == "Harvester")
                    if (uiButtonHarvest != null)
                        sB.Draw(uiButtonHarvest, uiCmd4, Color.White);

                if (SelectedObjectType == SelectedType.Unit)
                {
                    Texture2D pict = Textures.GetPictureReference(((Unit)SelectedObject).pictureName);
                    if (pict != null)
                        sB.Draw(pict, uiPicture, Color.White);
                }
            }
            else if (SelectedObjectType == SelectedType.Building || SelectedObjectType == SelectedType.MultipleUnits)
            {
                if (uiButtonDestroy != null)
                    sB.Draw(uiButtonDestroy, uiCmd1, Color.White);
                if (uiButtonRepair != null)
                    sB.Draw(uiButtonRepair, uiCmd2, Color.White);
                
                Texture2D pict = Textures.GetPictureReference(((Building)SelectedObject).pictureName);
                if (pict != null)
                    sB.Draw(pict, uiPicture, Color.White);

                //Available constructions
                if (SelectedObjectType == SelectedType.Building)
                {
                    Building b = (Building)SelectedObject;
                    if (b.IsBeingBuilt == false)
                    {
                        int c = 0;
                        //c can NOT exceed 8... if there are too many buildings in Dune.xml => exception is being thrown :D
                        //add xml validation someday...
                        for (int i = 0; i < b.canProduceBuildings.Count; i++)
                        {
                            sB.Draw(Textures.GetPictureReference(GlobalData.gameSettings.GetBuildingByName(b.canProduceBuildings[i]).pictureName),
                                    objectsToBuild[c], Color.White);
                            c++;
                        }
                        for (int i = 0; i < b.canProduceUnits.Count; i++)
                        {
                            sB.Draw(Textures.GetPictureReference(GlobalData.gameSettings.GetUnitByName(b.canProduceUnits[i]).pictureName),
                                    objectsToBuild[c], Color.White);
                            c++;
                        }
                    }
                }
            }

            if (SelectedObjectType == SelectedType.Building || SelectedObjectType == SelectedType.Unit)
            {
                float ratio = SelectedObject.currentHealth / SelectedObject.maxHealth;
                Color clr = new Color((byte)(255 * (1 - ratio)), (byte)(255 * ratio), 0);
                Rectangle rc = new Rectangle(uiHealth.X, uiHealth.Y, (int)(uiHealth.Width * ratio), uiHealth.Height);
                sB.Draw(Textures.GetPictureReference("MiniMap_Object"), uiHealth, Color.White);
                sB.Draw(Textures.GetPictureReference("MiniMap_Object"), rc, clr);
            }


            sB.End();

            bf.DrawString(exitButton.X, exitButton.Y, Color.Black, Properties.Resources.UI_Exit);
            string spice = Properties.Resources.UI_Credits + Supplies.spice.ToString();
            string energy = Properties.Resources.UI_Energy + Supplies.energy.ToString() + Properties.Resources.UI_EnergyCreditsSeparator;
            int spiceStart = uiTop.Right - bf.MeasureString(spice) - exitButton.X;
            int energyStart = spiceStart - bf.MeasureString(energy);
            bf.DrawString(spiceStart, exitButton.Y, Color.Black, spice);

            bf.DrawString(energyStart, exitButton.Y, Color.Black, energy);

        }

        public static Command GetCommand(Point pt)
        {
            Command cmd = Command.None;

            if (IsInRect(pt, exitButton) == true)
            {
                cmd = Command.Exit;
            }
            else if (SelectedObjectType == SelectedType.None || SelectedObjectType == SelectedType.MultipleBuildings)
            {
                cmd = Command.None;
            }
            else if (SelectedObjectType == SelectedType.Building)
            {
                if (IsInRect(pt, uiCmd1))
                    cmd = Command.Destroy;
                else if (IsInRect(pt, uiCmd2))
                    cmd = Command.Repair;
                else
                {
                    if (SelectedObject.IsBeingBuilt == false)
                        for (int i = 0; i < 9 && i < objectsToBuildNames.Count; i++)
                        {
                            if (IsInRect(pt, objectsToBuild[i]))
                            {
                                objectToBuildName = objectsToBuildNames[i];
                                objectToBuildIsBuilding = objectsToBuildIsBuilding[i];
                                cmd = Command.Build;
                            }
                        }
                }
            }
            else if (SelectedObjectType == SelectedType.Unit || SelectedObjectType == SelectedType.MultipleUnits)
            {
                if (IsInRect(pt, uiCmd1))
                    cmd = Command.Destroy;
                else if (IsInRect(pt, uiCmd2))
                    cmd = Command.Stop;
                else if (IsInRect(pt, uiCmd3))
                    cmd = Command.Move;
                else if (IsInRect(pt, uiCmd4))
                {
                    if (SelectedObject.name == "MCV")
                        cmd = Command.Deploy;
                    else if (SelectedObject.name == "Harvester")
                        cmd = Command.Harvest;
                    else cmd = Command.Attack;
                }
            }

            if (cmd != Command.None)
                DuneGame.MiscSB.PlayCue("buttonClick");

            return cmd;
        }


        private static bool IsInRect(Point pt, Rectangle rc)
        {
            if (pt.X >= rc.Left && pt.X <= rc.Right && pt.Y >= rc.Top && pt.Y <= rc.Bottom)
                return true;
            return false;
        }
    }
}
