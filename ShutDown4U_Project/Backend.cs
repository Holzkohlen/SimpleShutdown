using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShutDown4U_Project
{
    class Backend
    {
        string sLocalFolder;
        string sDir;
        string sPath;

        public Backend()
        {
            sLocalFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            sDir = Path.Combine(sLocalFolder, "SimpleShutdown");
            sPath = Path.Combine(sDir, "Settings.bin");
        }

        public int shutdownActivate(string std, string min, bool bShutdown, bool bZwang)
        {
            int iStd, iMin;
            bool bStd = Int32.TryParse(std, out iStd);
            bool bMin = Int32.TryParse(min, out iMin);
            if ((bStd == false) || (bMin == false))
            {
                return -1;
            }
            else
            {
                int iSek = ((iStd * 60) * 60) + (iMin * 60);
                if (bZwang == false)
                {
                    if (bShutdown == false)
                    {
                        Process.Start("shutdown", "-r -t " + iSek);
                    }
                    else
                    {
                        Process.Start("shutdown", "-s -t " + iSek);
                    }
                }
                else
                {
                    if (bShutdown == false)
                    {
                        Process.Start("shutdown", "-f -r -t " + iSek);
                    }
                    else
                    {
                        Process.Start("shutdown", "-f -s -t " + iSek);
                    }
                }
                return 0;
            }
        }

        public void shutdownDeactivate()
        {
            Process.Start("shutdown", "-a");
        }

        public void saveSettings(string sStd, string sMin, bool bZwang, bool bSave, bool bShutdown)
        {
            string sWrite = "";          
            if (File.Exists(sPath))
            {
                using (FileStream fsDelete = new FileStream(sPath, FileMode.Truncate))//Remove content of Settings.bin file
                {
                    fsDelete.Flush();
                }
            }
            else
            {
                Directory.CreateDirectory(sDir);
            }
            using (FileStream fs = new FileStream(sPath, FileMode.OpenOrCreate))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    //Save Settings, Force Close, Hours, Minutes
                    sWrite = bSave == true ? "true" : "false";
                    bw.Write(sWrite);
                    sWrite = bZwang == true ? "true" : "false";
                    bw.Write(sWrite);
                    bw.Write(sStd);
                    bw.Write(sMin);
                    sWrite = bShutdown == true ? "true" : "false";
                    bw.Write(sWrite);

                    fs.Flush();
                }
            }
        }

        public string[] loadSettings()
        {
            string[] sRueck = new string[5];
            if (File.Exists(sPath))
            {
                using (FileStream fs = new FileStream(sPath, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        for (int i = 0; i < sRueck.Length; i++)
                        {
                            sRueck[i] = br.ReadString();
                        }
                    }
                }
            }
            return sRueck;
        }
    }
}
