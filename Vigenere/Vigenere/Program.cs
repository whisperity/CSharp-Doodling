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

/*
 * Feladat szerinti példa kimenet
 * ------------------------------
 * Példa:
 * Nyílt szöveg: Ez a próba szöveg, amit kódolunk!
 * Szöveg átalakítása: EZAPROBASZOVEGAMITKODOLUNK
 * Kulcsszó: auto
 * Kulcsszó nagybetűssé alakítása: AUTO
 * Nyílt szöveg és kulcsszöveg együtt:
 * E Z A P R O B A S Z O V E G A M I T K O D O L U N K
 * A U T O A U T O A U T O A U T O A U T O A U T O A U
 * Kódolt szöveg:
 * E T T D R I U O S T H J E A T A I N D C D I E I N E
 */

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
            try
            {
                // Megpróbáljuk létrehozni a vtabla.dat fájlhoz való olvasási kapcsolatot.
                vtabla_dat = new FileStream("vtabla.dat", FileMode.Open, FileAccess.Read);
            }
            catch (System.IO.IOException ioex)
            {
                // Ha bármilyen hiba történik (a fájl nem elérhető, nincs jog olvasni)
                // erről értesítjük a felhasználót.
                System.Console.WriteLine("Hiba történt a fájl megnyitása során:");
                System.Console.WriteLine(ioex.Message);
                System.Console.WriteLine();

                // Majd megszakítjuk a program futtatását.
                // A vtabla.dat nélkül a feladat nem futtatható.
                System.Console.WriteLine("Ha a fájl nem olvasható, a programot nem lehet futtatni.");
                System.Console.WriteLine("A kilépéshez nyomjon ENTER-t...");
                System.Console.ReadLine();

                // 1-es visszatérési értékkel kilépünk a programból,
                // jelezve, hogy valami hiba történt (az érték nem 0).
                // 
                // Ez a visszatérési érték lehetőség jól jön
                // más, ezt a programot meghívó programok számára.
                // (Jelen esetben csak bemutató célzattal áll it.) 
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

            // Nagybetűssé alakítjuk a szöveget.
            kulcsszo = kulcsszo.ToUpperInvariant();

            /* ÖTÖDIK RÉSZFELADAT
             * ------------------
             * A kódolás első lépéseként fűzze össze a kulcsszót
             * egymás után annyiszor, hogy az így kapott karaktersorozat
             * (továbbiakban kulcsszöveg) hossza legyen egyenlő a kódolandó
             * szöveg hosszával! Írja ki a képernyőre az így kapott kulcsszöveget!
             */
            string kulcsszoveg = null;
            for (int i = 0; i < nyilt_szoveg.Length; i++)
			{
                kulcsszoveg += kulcsszo[i % kulcsszo.Length];
			}

            System.Console.WriteLine();
            System.Console.WriteLine("A kulcsszóból a következő kulcsszöveg lett generálva:");
            System.Console.WriteLine(kulcsszoveg);

            System.Console.WriteLine();
            System.Console.WriteLine("A nyílt szöveg és a kulcsszöveg együtt:");
            System.Console.WriteLine(nyilt_szoveg);
            System.Console.WriteLine(kulcsszoveg);

            /* HATODIK RÉSZFELADAT
             * -------------------
             * A kódolás második lépéseként a következőket hajtsa végre!
             * Vegye az átalakított nyílt szöveg első karakterét,
             * és keresse meg a vtabla.dat fájlból beolvasott táblázat első oszlopában!
             * 
             * Ezután vegye a kulcsszöveg első karakterét,
             * és keresse meg a táblázat első sorában!
             * Az így kiválasztott sor és oszlop metszéspontjában
             * lévő karakter lesz a kódolt szöveg első karaktere.
             * 
             * Ezt ismételje a kódolandó szöveg többi karakterével is!
             */
            StreamReader vtabla_olvaso = null;

            // Első körben beolvassuk a sorok számát a fájlból.
            int vt_sorok = 0;
            vtabla_olvaso = new StreamReader(vtabla_dat);
            while (vtabla_olvaso.ReadLine() != null)
            {
                vt_sorok++;
            }
            vtabla_olvaso.Close();
            
            // Majd beolvassuk az oszlopok számát (az első sorból).
            // Itt nem szükséges újból hibát kezelünk a fájlhoz,
            // feltételezzük, hogy az még mindig létezik.
            vtabla_dat = new FileStream("vtabla.dat", FileMode.Open, FileAccess.Read);
            vtabla_olvaso = new StreamReader(vtabla_dat);
            string sor = vtabla_olvaso.ReadLine();
            int vt_oszlopok = sor.Length;
            vtabla_olvaso.Close();

            // Felépítünk egy karaktermátrixot a fájlból.
            char[,] vtabla = new char[vt_oszlopok, vt_sorok];
            vtabla_dat = new FileStream("vtabla.dat", FileMode.Open, FileAccess.Read);
            vtabla_olvaso = new StreamReader(vtabla_dat);

            // A külső for ciklus a sorokat fogja olvasni.
            for (int i = 0; i < vt_sorok; i++)
            {
                // Elsőként beolvassuk az aktuális sort a fájlból.
                string aktualis_sor = vtabla_olvaso.ReadLine();

                // A belső for ciklus pedig az oszlopokat fogja feltölteni.
                for (int j = 0; j < vt_oszlopok; j++)
                {
                    vtabla[i, j] = System.Convert.ToChar(aktualis_sor[j]);
                }
            }
            vtabla_olvaso.Close();
            
            // Kívülre kimentjük a vtabla első sorát és
            // oszlopát, amely később a keresési alap lesz.
            char[] elso_sor = new char[vtabla.GetLength(0)];
            char[] elso_oszlop = new char[vtabla.GetLength(1)];
            for (int i = 0; i < vtabla.GetLength(0); i++)
            {
                elso_sor[i] = vtabla[0, i];
                elso_oszlop[i] = vtabla[i, 0];
            }

            // Majd iteráljuk (egyesével bejárjuk) a beírt nyílt szöveget.
            string kodolt_szoveg = "";
            for (int i = 0; i < nyilt_szoveg.Length; i++)
            {
                // Vesszük a nyílt szöveg és a kulcsszöveg i. karakterét
                // majd ezt oszlopként és sorként keresve "kimetsszük" a kódolt karaktert.
                int karakter_oszlop = Array.IndexOf(elso_oszlop, System.Convert.ToChar(nyilt_szoveg[i]));
                int karakter_sor = Array.IndexOf(elso_sor, System.Convert.ToChar(kulcsszoveg[i]));
                
                // A kodolt_szoveg-hez hozzáírjuk a kapott karaktert a metszéspontból.
                kodolt_szoveg += System.Convert.ToString(vtabla[karakter_sor, karakter_oszlop]);
            }

            /* HETEDIK RÉSZFELADAT
             * -------------------
             * Írja ki a képernyőre és a kodolt.dat fájlba a kapott kódolt szöveget!
             */
            System.Console.WriteLine();
            System.Console.WriteLine("A kódolt szöveg:");
            System.Console.WriteLine(kodolt_szoveg);

            // Megtörténik a fájlba írás.
            FileStream kodolt_dat = null;
            bool kodolt_dat_irhato = false;
            try
            {
                // Megpróbáljuk felépíteni a kapcsolatot írási módba.
                // A korábbi fájl automatikusan felülíródik (ha van hozzáférés).
                kodolt_dat = new FileStream("kodolt.dat", FileMode.Create, FileAccess.Write);

                // Feltételezzük, hogy a kapcsolat létrejött,
                // így a logikai változó értékét igazra állítjuk.
                kodolt_dat_irhato = true;
            }
            catch (System.IO.IOException ioex)
            {
                // Ha hiba történik, értesítjük a felhasználót.
                System.Console.WriteLine("Hiba történt a fájl megnyitása során:");
                System.Console.WriteLine(ioex.Message);
                System.Console.WriteLine();

                // Mivel a kapcsolat mégsem jöhetett létre,
                // a logikai változót "visszakapcsoljuk" hamis értékre.
                kodolt_dat_irhato = false;
            }

            if (kodolt_dat_irhato == true)
            {
                // Ha írható a fájl (korábban igazra állt a változó),
                // akkor megírjuk a feladatnak megfelelően.
                StreamWriter kodolt_writer = new StreamWriter(kodolt_dat);
                kodolt_writer.WriteLine(kodolt_szoveg);
                kodolt_writer.Flush();
                kodolt_writer.Close();
            }
            else if (kodolt_dat_irhato == false)
            {
                // Ha nem sikerült a kapcsolatot kiépíteni, csak
                // értesítjük a felhasználót a sikertelenségről.
                System.Console.WriteLine("A kódolt szöveg fájlba írása nem történt meg.");
            }

            // Várunk egy billentyűleütést a kilépés előtt.
            System.Console.WriteLine();
            System.Console.WriteLine("\nA kilépéshez nyomjon ENTER-t...");
            System.Console.ReadLine();

            // 0 (minden OK) visszatéréssi értékkel kilépünk a programból.
            Environment.Exit(0);
        }
    }
}
