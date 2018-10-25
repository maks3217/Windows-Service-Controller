using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BibliotekaKontoler
{
    public class KontrolerProcess
    {
        private string Data;
        private string Nazwa;
        private uint Limit;
        private uint TimeRuning;
        private Timer timer = new Timer();
        Process[] procesy;

        public string getNazwa()
        {
            return this.Nazwa;
        }

        public KontrolerProcess(string nazwa, uint limit)
        {
            TimeRuning = 0;
            Nazwa = nazwa;
            Limit = limit;
            //Limit = limit * 6;
            Data = DateTime.Now.ToShortDateString();
            timer.Interval = 10000;
            timer.Elapsed += new ElapsedEventHandler(this.UpdateTimeRunning);
            timer.Start();
        }

     

        public bool IsProcesRunning(string Nazwa)
        {
            Process[] pname = Process.GetProcessesByName(Nazwa);
            if (pname.Length == 0)
                return false;
            else
                return true;
        }

        public void UpdateTimeRunning(object sender, EventArgs e)
        {
            //Loger.Write(Nazwa + " " + Limit + " TimeRuning=" + TimeRuning);
            UpdateDate();
            bool flaga = false;
            if (IsProcesRunning(Nazwa))
            {
                TimeRuning++;
                flaga = true;
            }
            if (TimeRuning > Limit && flaga)
            {
                Loger.Write("Wyczerpany limit procesu" + Nazwa);
                procesy = Process.GetProcessesByName(Nazwa);
                foreach(Process pro in procesy)
                {
                    pro.Kill();
                }
            }
        }

        public void UpdateDate()
        {
            string tmp = DateTime.Now.ToShortDateString();
            if(!tmp.Equals(Data))
            {
                Data = tmp;
                TimeRuning = 0;
            }
        }

        public void StopProcesObject()
        {
            timer.Stop();
        }

    }
}
