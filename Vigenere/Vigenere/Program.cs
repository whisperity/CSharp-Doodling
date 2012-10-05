using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/* FELADATLEÍRÁS
 * -------------
 * http://www.oh.gov.hu/letolt/okev/doc/2005_osz/e_info_05okt_fl.pdf
 * (a PDF fájlban, a 10. oldaltól)
 */

/* A projekt sikeres futtatásához tartozik egy Vtabla.dat
 * fájl, egy ún. Vigenére tábla, amely egy titkosítási eszköz.
 * A fájl első sorai alább olvashatóak:
 * 
 * ABCDEFGHIJKLMNOPQRSTUVWXYZ
 * BCDEFGHIJKLMNOPQRSTUVWXYZA
 * stb.
 */


// GLOBAL TODO: További dokumentáció mutogatási célokból.

namespace Vigenere
{
    class Program
    {
        static void Main(string[] args)
        {
            // Beállítjuk a konzolablak címét.
            System.Console.Title = "Vigenére tábla";
            
            /* (NULLADIK RÉSZFELADAT)
             * ----------------------
             * Első lépésként betöltjük a 'Vtabla.dat' fájlt
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

            // A nyilt_szoveg_hiba addig marad igaz, amíg a beírt érték
            // megfelel a feltételeknek.
            // Ha nem, akkor a ciklus újra lefut, és új érték kerül bekérésre.
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

            /* MÁSODIK RÉSZFELADAT
             * -------------------
             * Alakítsa át a nyílt szöveget, hogy a későbbi kódolás feltételeinek megfeleljen!
             * 
             * A kódolás feltételei:
             * A magyar ékezetes karakterek helyett ékezetmenteseket kell használni.
             * (Például á helyett a; ő helyett o stb.)
             * A nyílt szövegben az átalakítás után csak az angol ábécé betűi szerepelhetnek.
             * A nyílt szöveg az átalakítás után legyen csupa nagybetűs.
             */
            // Első részben nagybetűssé alakítjuk a szöveget.
            // (Így később nem kell külön átalakítani a kis 'á' és nagy 'Á' betűket.)
            nyilt_szoveg = nyilt_szoveg.ToUpperInvariant();

            // Kivágjuk az összes írásjelet és számot.
            char[] irasjelek = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ', '!', '?', '@', '.', ',', '\'', '"' };
            foreach (char irasjel in irasjelek)
            {
                nyilt_szoveg = nyilt_szoveg.Replace(System.Convert.ToString(irasjel), "");
            }

            // Az összes magyar ékezetes betűt átalakítjuk az ékezet nélküli párjára.
            nyilt_szoveg = nyilt_szoveg.Replace("Á", "A");
            nyilt_szoveg = nyilt_szoveg.Replace("É", "E");
            nyilt_szoveg = nyilt_szoveg.Replace("Í", "I");
            nyilt_szoveg = nyilt_szoveg.Replace("Ó", "O");
            nyilt_szoveg = nyilt_szoveg.Replace("Ö", "O");
            nyilt_szoveg = nyilt_szoveg.Replace("Ő", "O");
            nyilt_szoveg = nyilt_szoveg.Replace("Ú", "U");
            nyilt_szoveg = nyilt_szoveg.Replace("Ü", "U");
            nyilt_szoveg = nyilt_szoveg.Replace("Ű", "U");
            
            /* HARMADIK RÉSZFELADAT
             * --------------------
             * Írja ki a képernyőre az átalakított nyílt szöveget!
             */
            System.Console.WriteLine();
            System.Console.WriteLine("A szöveg át lett alakítva a feladatnak megfelelő formátumba:");
            System.Console.WriteLine(nyilt_szoveg);

            /* NEGYEDIK RÉSZFELADAT
             * --------------------
             * Kérjen be a felhasználótól egy maximum 5 karakteres, nem üres kulcsszót!
             * A kulcsszó a kódolás feltételeinek megfelelő legyen!
             * (Sem átalakítás, sem ellenőrzés nem kell!)
             * Alakítsa át a kulcsszót csupa nagybetűssé!
             */
            // Bekérjük a megadott kulcsszót.
            System.Console.WriteLine();
            System.Console.WriteLine("Kérek egy max. 5 karakteres kulcsszót.");
            System.Console.WriteLine("FIGYELEM! A kulcsszó csak nem ékezetes, az angol ABC-ből ismert karaktereket tartalmazhat!");
            System.Console.Write("> ");
            string kulcsszo = System.Console.ReadLine();
            // Mivel a feladat megtiltja az ellenőrzést,
            // ezért elhisszük, hogy a user jól írta be.

            
            // Várunk egy billentyűleütést a kilépés előtt.
            System.Console.WriteLine("\nA kilépéshez nyomjon ENTER-t...");
            System.Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
