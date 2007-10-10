using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Dune_2_Remade_Map_Creator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = null;
            int[,] map = null;
            Color sand = Color.FromArgb(255,255,255);
            Color rock = Color.FromArgb(0,255,0);
            Color mountain = Color.FromArgb(0,0,0);
            Color spice = Color.FromArgb(255,0,0);
            Color temp;

            OpenFileDialog od = new OpenFileDialog();
            if (od.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    bmp = new Bitmap(od.FileName);
                }
                catch
                {
                    MessageBox.Show("Invalid image");
                    return;
                }

                map = new int[bmp.Width, bmp.Height];

                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        temp = bmp.GetPixel(x, y);
                        if (temp == sand)
                                map[x, y] = 0;
                        else if (temp == rock)
                                map[x, y] = 2;
                        else if (temp == mountain)
                                map[x, y] = 1;
                        else if (temp == spice)
                                map[x, y] = 3;
                        else
                        {
                            MessageBox.Show("Invalid image - unknown tile");
                            return;
                        }
                        }
                    }
                }

                StreamWriter sw = new StreamWriter("Map1.map");

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        sw.Write(map[x,y]);
                    }
                    sw.Write(sw.NewLine);
                }

                sw.Close();
            }
        }
    }