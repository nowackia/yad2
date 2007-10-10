using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Dune_2_Remade
{
    struct Technology
    {        
        public string name;
        public int cost;
        public bool researched;
        public Texture2D picture;
        public Technology(string name, int cost, bool researched, Texture2D picture)
        {
            this.name=name;
            this.cost=cost;
            this.researched=researched;
            this.picture = picture;
        }        
    }
    public static class Supplies
    {
        public static int spice = 5000;
        public static int energy = 0;
        static Technology []technology=new Technology[10];
        static public void Initialize(ContentManager content)
        {
            for (int i = 0; i < technology.Length; i++)
            {
                technology[i].researched = false;
                technology[i].name = string.Empty;
                technology[i].cost = 0;
                technology[i].picture = null;
            }
            technology[0].name = "Build Sonic Tanks";             
            technology[0].cost = 300;
            technology[0].picture = content.Load<Texture2D>(@"Content\Textures\Picture_SonicTank");
            technology[1].name = "Build Siege Tanks";
            technology[1].cost = 250;
            technology[1].picture = content.Load<Texture2D>(@"Content\Textures\Picture_SiegeTank");
        }
        static bool IsResearched(string name)
        {
            for (int i = 0; i < technology.Length; i++)
            {
                if (technology[i].name == name)
                    return technology[i].researched;
            }
            return false;
        }        
    }
}
