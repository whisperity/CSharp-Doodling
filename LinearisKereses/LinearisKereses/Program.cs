using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearisKereses
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] szamok = new int[5];

            for (int i = 0; i < szamok.GetLength(0); i++)
            {
                int szam = 0;
                try
                {
                    System.Console.WriteLine("Kérem a(z) " + i + ". számot: ");
                    szam = System.Convert.ToInt32(System.Console.ReadLine());
                }
                catch ( System.FormatException )
                {
                    System.Console.WriteLine("VADBAROM...");
                    System.Console.WriteLine("Ez nem egy szám.");
                    
                    System.Console.ReadLine();
                    System.Environment.Exit(1);
                }

                szamok[i] = szam;
            }

            System.Console.WriteLine(LinKer(szamok));
            
            System.Console.ReadLine();
        }

        static bool Feltetel(int szam)
        {
            return (szam % 3 == 1 ? true : false);
        }

        static string LinKer(int[] szamok)
        {
            int i = 0;

            while (i < szamok.GetLength(0) && Feltetel(szamok[i]) == false)
            {
                i++;
            }

            bool talalt = (i < szamok.GetLength(0));

            if (talalt == true)
            {
                return "Van ilyen elem: a(z) " + i + ".";
            }
            else
            {
                return "Nincs ilyen elem.";
            }
        }
    }
}
