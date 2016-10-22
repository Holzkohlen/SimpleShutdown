using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShutDown4U_Project
{
    public partial class Form1 : Form
    {
        Backend back;

        public Form1()
        {
            InitializeComponent();
            back = new Backend();
            setSettings(back.loadSettings());
        }

        private void somethingChanged(object sender, EventArgs e)
        {
            if(saveBox.Checked == true)
            {
                back.saveSettings(hBox.Text, mBox.Text, zwangBox.Checked, saveBox.Checked, rbShutdown.Checked);
            }
        }

        private void setSettings(string[] sRueck)
        {
            if (sRueck[0] == "true")
            {
                saveBox.Checked = sRueck[0] == "true" ? true : false;
                zwangBox.Checked = sRueck[1] == "true" ? true : false;
                hBox.Text = sRueck[2];
                mBox.Text = sRueck[3];
                rbShutdown.Checked = sRueck[4] == "true" ? true : false;
                rbReboot.Checked = rbShutdown.Checked == false ? true : false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int iReturn = back.shutdownActivate(hBox.Text, mBox.Text, rbShutdown.Checked, zwangBox.Checked);
            if (iReturn == -1)
            {
                error("Bei der Eingabe handelt es sich nicht um eine Zahl", "Upps");
            }
            else
            {
                statusBox.BackColor = Color.LightGreen;
                statusBox.Text = "aktiviert";
            }
        }

        private void error(string Meldung, string Header)
        {
            MessageBox.Show(Meldung, Header);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            back.shutdownDeactivate();
            statusBox.BackColor = Color.LightPink;
            statusBox.Text = "deaktiviert";
        }

        private void überToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Simple Shutdown 1.0\r\nVon Lukas Hempel", "Über");
        }

        private void zurücksetzenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hBox.Text = "0";
            mBox.Text = "0";
            zwangBox.Checked = false;
            saveBox.Checked = false;
            rbShutdown.Checked = true;
            if (saveBox.Checked == true)
            {
                back.saveSettings(hBox.Text, mBox.Text, zwangBox.Checked, saveBox.Checked, rbShutdown.Checked);
            }
        }

        private void saveBox_CheckedChanged(object sender, EventArgs e)
        {
            back.saveSettings(hBox.Text, mBox.Text, zwangBox.Checked, saveBox.Checked, rbShutdown.Checked);
        }
    }
}
