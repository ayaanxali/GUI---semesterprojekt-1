using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPI;
using RPI.Display;
using RPI.Controller;
using RPI.Heart_Rate_Monitor;

namespace GUI___semesterprojekt_1
{
    class Meassurement
    {
        // Erklærer de 4 attributter
        private int calculatedPulse;
        private int numberOfPulse;
        private DateTime starttime;
        private DateTime stoptime;

        // Laver constructoren - denne tager tre af attributterne som parametre
        public Meassurement(DateTime Starttid, DateTime Stoptid, int antalPuls)
        {
            double tid;
            starttime = Starttid;
            stoptime = Stoptid;
            numberOfPulse = antalPuls;
            TimeSpan måleTid = (stoptime - starttime);
            
            // Sørger for, at vi ikke får en nulreference når pulsobjektet oprettes
            if (måleTid.TotalMinutes > 0.01)
            {
                tid = Convert.ToDouble(måleTid.TotalMinutes);
            }
            else
            {
                numberOfPulse = 70;
                tid = 1;
            }

            calculatedPulse = Convert.ToInt32(numberOfPulse / tid);
        }
        // Returnerer den beregnede puls 
        public int getPulse()
        {
            return calculatedPulse;
        }
        // Udskriver teksten til historikken
        public override string ToString()
        {
            return $"{starttime.ToString("dd/MM/yyyy HH:mm")}\t\t {calculatedPulse} ";
        }
    }
}
