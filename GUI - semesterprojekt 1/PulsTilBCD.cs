using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI___semesterprojekt_1
{
    class PulsTilBCD
    {
        // Erklærer de 6 attributter
        private int tiere;
        private int enere;
        private string BCD;
        private string biEnere;
        private string biTiere;
        private DateTime starttid = DateTime.Now;

        //Opretter cunstructoren. Denne tager en parameter
        public PulsTilBCD(int pulsen)
        {

            tiere = (pulsen / 10) % 10;
            enere = (pulsen % 10);

            // bruger de givne metoder til at omregne indputtet til hhv enere og tiere til binære tal
            string biTal = Convert.ToString(pulsen, 2).PadLeft(4, '0');
            biEnere = Convert.ToString(enere, 2).PadLeft(4, '0');
            biTiere = Convert.ToString(tiere, 2).PadLeft(4, '0');

            // kun for forståelse og visning i vinduet
            Console.WriteLine("Enere: " + biEnere);
            Console.WriteLine("Tiere: " + biTiere + "\n");

            // bruger den givne metode til at omregne fra binært tal til BCDs
            BCD = Convert.ToInt16(biTiere + biEnere, 2).ToString();

            // kun for forståelse og visning i vinduet
            Console.WriteLine("Bruger tal til binær tal: \t" + biTal);
            Console.WriteLine("BCD på binær form: \t\t" + biTiere + biEnere);
            Console.WriteLine("BCD på decimaltal: \t\t" + BCD + "\n");
        }

        // opretter en getmetode så vi kan få fat i pulsen på BCD format som skal bruges i SevenSeg klassen
        public short getPulsTilBCD()
        {
            return Convert.ToInt16(BCD);
        }

        // returnerer en tekststreng med informationerne
        public override string ToString()
        {
            return "Tid:" + starttid + "\t\tPulsen er: " + BCD + "\n";
        }
    }
}
