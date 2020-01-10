using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI___semesterprojekt_1
{
    class PulsTilBCD
    {
        // Erklærer attributten
        private string BCD;

        //Opretter cunstructoren. Denne tager en parameter, som er den beregnede puls
        public PulsTilBCD(int pulsen)
        {
            // bruger de givne metoder til at omregne indputtet til hhv enere og tiere til binære tal
            string biEnere = Convert.ToString((pulsen % 10), 2).PadLeft(4, '0');
            string biTiere = Convert.ToString(((pulsen / 10) % 10), 2).PadLeft(4, '0');

            // kun for forståelse og visning i vinduet
            Console.WriteLine("Enere: " + biEnere);
            Console.WriteLine("Tiere: " + biTiere + "\n");

            // bruger den givne metode til at omregne fra binært tal til BCD
            BCD = Convert.ToInt16(biTiere + biEnere, 2).ToString();
        }

        // opretter en getmetode så vi kan få fat i pulsen på BCD format som skal bruges i SevenSeg klassen
        public short getPulsTilBCD()
        {
            return Convert.ToInt16(BCD);
        }
    }
}
