﻿using System;
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

        // Laver cunstructoren - denne tager tre af attributterne som parametre
        public Meassurement(DateTime Starttid, DateTime Stoptid, int antalPuls)
        {
            int tid;
            starttime = Starttid;
            stoptime = Stoptid;
            numberOfPulse = antalPuls;
            TimeSpan måleTid = (stoptime - starttime);
            
            //Sørger for, at vi ikke får en nulreference når pulsobjektet oprettes
            if (måleTid.TotalMinutes > 0.15)
            {
                tid = Convert.ToInt32(Math.Round(numberOfPulse / Convert.ToDouble(måleTid.TotalMinutes)));
            }
            else
            {
                numberOfPulse = 70;
                tid = 1;
            }

            calculatedPulse = numberOfPulse / tid;
        }
        //returnerer den beregnede puls 
        public int getPulse()
        {
            return calculatedPulse;
        }
        //udskriver teksten til historikken
        public override string ToString()
        {
            return $"{starttime.ToString("dd/MM/yyyy HH:mm")}\t\t {calculatedPulse} ";
        }
    }
}