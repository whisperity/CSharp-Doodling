using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/* FELADATLEÍRÁS
 * -------------
 * http://www.oh.gov.hu/letolt/okev/doc/2005_osz/e_info_05okt_fl.pdf
 * (a PDF fájlban, a 10. oldaltól)
 */

// GLOBAL TODO: További dokumentáció mutogatási célokból.

namespace Vigenere
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Első lépésként betöltjük a 'Vtabla.dat' fájlt
             * amely egy kódtábla a feladat megoldásához.
             */

            FileStream vtabla_dat = null;
            string vtabla_eleresiut = "vtabla.dat";

            /* TODO: Hibakezelés, ha a vtabla.dat nem található.
            if (!System.IO.File.Exists(vtabla_eleresiut))
            {
                System.Console.Write("A vtabla.dat fájl nem található a megadott helyen (");
                System.Console.WriteLine(System.IO.Directory.GetCurrentDirectory() + ")");
                System.Console.WriteLine("Kérem adja meg a vtabla.dat fájl elérési útvonalát!");

            }
            */

            try
            {
                vtabla_dat = new FileStream(vtabla_eleresiut, FileMode.Open, FileAccess.Read);
            }
            catch (System.IO.IOException ioex)
            {
                System.Console.WriteLine("Hiba történt a fájl megnyitása során:");
                System.Console.WriteLine(ioex.Message);
                System.Console.WriteLine();

                System.Console.WriteLine("A kilépéshez nyomjon ENTER-t...");
                System.Console.ReadLine();
                Environment.Exit(1);
            }

            /* ELSŐ RÉSZFELADAT
             * ----------------
             * Kérjen be a felhasználótól egy maximum 255 karakternyi,
             * nem üres szöveget! A továbbiakban ez a nyílt szöveg.
             */
            System.Console.WriteLine("Kérek egy szöveget. (Max. 255 karakter)");
            System.Console.WriteLine("A beírás után nyomjon ENTER-t az elküldéshez.");

            string nyilt_szoveg = null;
            bool nyilt_szoveg_hiba = true;

            while (nyilt_szoveg_hiba)
            {
                System.Console.Write("> ");
                nyilt_szoveg = System.Console.ReadLine();
                nyilt_szoveg_hiba = false;

                if (nyilt_szoveg.Length >= 255 || nyilt_szoveg == "")
                {
                    System.Console.WriteLine("A beírt szöveg nem felel meg a feltételeknek.");
                    nyilt_szoveg_hiba = true;
                }
            }

            

            // Várunk egy billentyűleütést a kilépés előtt.
            System.Console.WriteLine("\nA kilépéshez nyomjon ENTER-t...");
            System.Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
