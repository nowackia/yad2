using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dune_2_Remade
{
    public partial class OptionsMenu : Form
    {
        public OptionsMenu()
        {
            InitializeComponent();

            this.txtNick.Text = Properties.Resources.OM_DefaultPlayerName;
            this.lblPlayer.Text = Properties.Resources.OM_PlayerName;
            this.btnCancel.Text = Properties.Resources.OM_Cancel;
            this.btnOK.Text = Properties.Resources.OM_SaveSettings;
        }

        public string PlayerNick
        {
            get
            {
                return txtNick.Text;
            }
            set
            {
                txtNick.Text = value;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

    }
}