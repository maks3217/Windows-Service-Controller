using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Configuration;
using System.Windows;

namespace BibliotekaKontoler
{
    public class Konfigurator
    {
        public static string ChoseProcess()
        {
            String nazwa;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string fileName = ofd.FileName;
            Process proces = new Process();
            try
            {
                proces.StartInfo.FileName = fileName;
                proces.Start();
                nazwa = proces.ProcessName;
                proces.Kill();
                return nazwa;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Nieprawidłowy proces !");
                return "";
            }
        }

        public static bool IsProcess(string nazwa)
        {
            return true;
        }

        public static bool ProcessOnList(string nazwa)
        {
            List<Itemm> items = GetItemList();
            foreach(Itemm item in items )
            {
                if (item.Nazwa == nazwa)
                    return true;
            }
            return false;
        }

        public static bool CheckPropierties(string nazwa, string limit)
        {
            if(!IsProcess(nazwa))
            {
                MessageBox.Show("Nieprawidłowy proces !");
                return false;
            }
            if(ProcessOnList(nazwa))
            {
                MessageBox.Show("Proces znajduje się już na liscie");
                return false;
            }
            int n;
            bool isNumeric = int.TryParse(limit, out n);
            if(!isNumeric)
            {
                MessageBox.Show("Nieprawidłowa wartosc limitu (To nie jest liczba)");
                return false;
            }
            else
            {
                if(n<0)
                {
                    MessageBox.Show("Nieprawidłowa wartosc limitu (Przekroczony zakres)");
                    return false;
                }
            }
            return true;

        }

        public static List<Itemm> GetItemList()
        {
            List<Itemm> items = new List<Itemm>();
            try
            {
                string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string configFile = System.IO.Path.Combine(appPath, "AplikacjaKontroler.exe.config");
                //Loger.Write(appPath);
                ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
                configFileMap.ExeConfigFilename = configFile;

                System.Configuration.Configuration config =
                    ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                string ProcesString = config.AppSettings.Settings["ProcesList"].Value;
                //Loger.Write(ProcesString);
                string[] ProcessAray = ProcesString.Split(',');
                foreach(string singleProces in ProcessAray)
                {
                    if (singleProces!="")
                    {
                        string[] singleItems = singleProces.Split(':');                       
                        Itemm item = new Itemm();
                        item.Nazwa = singleItems[0];
                        item.Limit = (uint)Int32.Parse(singleItems[1]);
                        items.Add(item);
                    }
                }
                return items;

            }
            catch (Exception ex)
            {
                Loger.Write("Błąd odczytu parametru: " + ex.ToString(), "error");
                return items;
            }
        } 

        public static void UpdateSetings(List<Itemm> items)
        {
            string tmp;
            string result = "";
            foreach(Itemm item in items)
            {
                tmp = item.Nazwa + ":" +  item.Limit.ToString() + ",";
                result = result + tmp;
            }

            try
            {
                string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string configFile = System.IO.Path.Combine(appPath, "AplikacjaKontroler.exe.config");

                ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
                configFileMap.ExeConfigFilename = configFile;

                System.Configuration.Configuration config =
                    ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

                config.AppSettings.Settings["ProcesList"].Value = result;
                config.Save();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd odczytu parametru: " + ex.ToString(), "error");
                return;
            }
        }
    }
}
