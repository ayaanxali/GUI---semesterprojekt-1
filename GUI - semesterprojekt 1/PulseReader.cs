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
    class PulseReader
    {
        public Meassurement M1;
        private DateTime _starttid;
        private DateTime _stoptid;
        private PWM _pwm;
        bool startBool = true;

        private RPi _rpi;
        private Puls puls;
        private int antalPuls;

        public PulseReader(RPi rpi, PWM pwm)
        {
            _pwm = pwm;
            _rpi = rpi;
            puls = new Puls(rpi);
            //antalPuls = puls.ReadPuls();
        }

        public Meassurement ReadCalculatedPulse() //spørg lene om typen af denne metode (messurement/int?- ift. klassediagrammet)
        {
            return M1;
        }

        public int getCalculatedPulse()
        {
            return M1.getPulse();
        }

        // Denne metode skal initialiseres af start/stop knappen på pulsmålerkassen. 
        public void StartReading()
        {
            if (startBool == true)
            {
                _rpi.wait(250);
                _starttid = DateTime.Now;
                puls.StartPuls();
                startBool = false;
                _pwm.SetPWM(99);
            }
            else if (startBool == false)
            {
                _rpi.wait(250);
                _stoptid = DateTime.Now;
                startBool = true;
                antalPuls = puls.ReadPuls();
                
                M1 = new Meassurement(_starttid, _stoptid, antalPuls);
               
                _pwm.SetPWM(25);
            }
        }
    }
}
