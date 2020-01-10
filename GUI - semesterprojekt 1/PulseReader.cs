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
        //Erklærer de (x) atributter
        public Meassurement _meassurement;

        // kan vi gøre disse t0 atributter til lokale variabler i stedet?
        private DateTime _starttid;
        private DateTime _stoptid;

        private bool startBool = true;
        private PWM _pwm;
        private RPi _rpi;
        private Puls puls;
        private int antalPuls;

        //Opretter cunstructoren. Denne tager 2 parametre, som er RPI'en og den givne pulsbreddemodulation
        public PulseReader(RPi rpi, PWM pwm)
        {
            _pwm = pwm;
            _rpi = rpi;
            puls = new Puls(rpi);
            //antalPuls = puls.ReadPuls();
        }

        //returnerer et historik-element med starttid, stoptid og antal puls 
        public Meassurement ReadCalculatedPulse()
        {
            return _meassurement;
        }

        // returnerer den beregnede puls 
        public int getCalculatedPulse()
        {
            return _meassurement.getPulse();
        }

        // Denne metode starter/stopper pulsmåling og skal initialiseres af start/stop knappen på pulsmålerkassen. 
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
                
                _meassurement = new Meassurement(_starttid, _stoptid, antalPuls);
               
                _pwm.SetPWM(25);
            }
        }
    }
}
