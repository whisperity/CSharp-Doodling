using System;
using System.Collections.Generic;
// Az IO osztályt be kell ágyazni (using szó), hogy kezelhessük a fájlrendszert.
using System.IO;
// A Security.AccessControl osztály szükséges a jogkörökkel való interakcióhoz.
using System.Security.AccessControl;
using System.Text;

/* MEGJEGYZÉS
 * ----------
 * Példa program a fájlkezelés bemutatására.
 */

/* TOVÁBBI FELADAT
 * ---------------
 * A jelenlegi Solution-ben található másik projekt
 * (ClassStatistics) módosítása, hogy fájlokat kezeljen.
 */

// Fájlkezelés jegyzet: http://www.functionx.com/vcsharp2003/fileprocessing/

namespace IOExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ez csak egy címsor beállítása a konzolablakban.
            System.Console.Title = "IO Példa";

            // Bekérjük a fájl elérési útját a felhasználótól.
            System.Console.WriteLine("Kérem a kezelendő fájl elérési útját.");
            System.Console.Write("> ");
            string fajl = System.Console.ReadLine();

            // Az iohiba változó alapból hamis. Igaz lesz, ha bármilyen hiba történik.
            bool iohiba = false;
            // Deklaráljuk a fájlhoz tartozó osztály objektumát.
            FileStream handle;

            try
            {
                // Megpróbáljuk létrehozni a fájlhoz való kapcsolatot.
                // Ez különböző hibákkal térhet vissza, amelyeket későbbi catch(){} blokkokban kezelünk.
                handle = new FileStream(fajl, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
            catch (System.IO.IOException ioex)
            {
                // Az 'ioex' helyi változó tárolja a hiba (Exception) objektumát.
                // Így a szöveg kiolvashatóvá válik.
                System.Console.WriteLine("Kezelési hiba történt: " + ioex.Message);
                iohiba = true;
            }
            catch (System.UnauthorizedAccessException)
            {
                // Ha hozzáférési hiba történik, értesítjük a felhasználót.
                System.Console.WriteLine("A fájl nem kezelhető, mivel nincs megfelelő hozzáférés.");
                iohiba = true;
            }
            catch (System.ArgumentException)
            {
                // ArgumentException történik, ha a paraméter nem értelmezhető.
                // Például "C:\Valami?.txt" esetén, mivel a Windows nem kezeli a "?"-t a fájlnévben.
                System.Console.WriteLine("A megadott paraméter nem megfelelő.");
                System.Console.WriteLine("Valószínűleg nem használható karaktereket tartalmaz.");
                iohiba = true;
            }
            finally
            {
                // A finally{} blokk lefut minden esetben,
                // akkor is, ha volt elkapott (catch) hiba,
                // és akkor is, ha nem.

                // Ha az iohiba korábban true-ra állt, kilépünk a programból.
                if (iohiba == true)
                {
                    System.Console.WriteLine("A kilépéshez nyomjon meg egy gombot...");
                    System.Console.ReadKey();
                    System.Environment.Exit(1);
                }
            }

            // Létrehozunk egy, a fájl attribútumait olvasni tudó objektumot is.
            FileInfo info = new FileInfo(fajl);
            
            System.Console.WriteLine("Fájl megnyitása sikeres.");
            System.Console.WriteLine("A fájl mérete: " + System.Convert.ToString(info.Length) + " bájt.");
            
            // Billentyűlenyomásra várunk a kilépés előtt.
            System.Console.WriteLine("A kilépéshez nyomjon meg egy gombot...");
            System.Console.ReadKey();
        }
    }
}
