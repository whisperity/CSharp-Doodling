using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/* FELADATLEÍRÁS
 * -------------
 * http://www.oh.gov.hu/letolt/okev/doc/erettsegi_2010/e_info_10maj_fl.pdf
 * (a PDF fájlban, a 10. oldaltól)
 */

/* A projekthez adott eladott.txt állomány a bemeneti fájl,
 * ez automatikusan másolásra kerül a kimeneti mappába.
 */

namespace Helyjegy
{
    class Program
    {
        static void Main(string[] args)
        {
            /* ELSŐ RÉSZFELADAT
             * ----------------
             * Olvassa be az eladott.txt állományban talált adatokat, s 
             * azok felhasználásával oldja meg a következő feladatokat!
             */
            // Létrehozzuk a fájlt olvasó objektumokat.
            FileStream eladott_txt = new FileStream("eladott.txt", FileMode.Open, FileAccess.Read);
            StreamReader txt = new StreamReader(eladott_txt);

            // Beolvassuk az első sort, majd a feladatnak megfelelően feldolgozzuk.
            string elso_sor = txt.ReadLine();
            string[] elso_harom_ertek = elso_sor.Split(' ');
            int eladott_jegyek_szama = System.Convert.ToInt32(elso_harom_ertek[0]);
            int vonal_hossza = System.Convert.ToInt32(elso_harom_ertek[1]);
            int fizetendo_per_tizkm = System.Convert.ToInt32(elso_harom_ertek[2]);

            // Majd beolvassuk az összes többi sort és szintén feldolgozzuk.
            Jegy[] vasarlasok = new Jegy[eladott_jegyek_szama];

            for (int i = 0; i < eladott_jegyek_szama; i++)
            {
                string aktualis_sor = txt.ReadLine();
                string[] sor_bontva = aktualis_sor.Split(' ');

                int ules_szam = System.Convert.ToInt32(sor_bontva[0]);
                int felszall = System.Convert.ToInt32(sor_bontva[1]);
                int leszall = System.Convert.ToInt32(sor_bontva[2]);

                vasarlasok[i] = new Jegy(ules_szam, felszall, leszall);
            }

            // Innentől a feladat megoldásához a vasarlasok tömböt fogjuk használni.

            /* MÁSODIK RÉSZFELADAT
             * -------------------
             * Adja meg a legutolsó jegyvásárló ülésének sorszámát
             * és az általa beutazott távolságot!
             * A kívánt adatokat a képernyőn jelenítse meg!
             */
            // A vasarlasok.GetUpperBound(0) sorszámú utas az utolsó vásárlót jelenti.
            System.Console.Write("2. feladat: ");
            System.Console.Write("A legutolsó jegyvásárló a(z) ");
            System.Console.Write(System.Convert.ToString(vasarlasok[vasarlasok.GetUpperBound(0)].ules_szam));
            System.Console.Write(". számú ülésen utazott és ");
            int utolso_tavolsag = vasarlasok[vasarlasok.GetUpperBound(0)].leszall - vasarlasok[vasarlasok.GetUpperBound(0)].felszall;
            System.Console.WriteLine(System.Convert.ToString(utolso_tavolsag) + " km utat tett meg.");
            
            /* HARMADIK RÉSZFELADAT
             * --------------------
             * Listázza ki, kik utazták végig a teljes utat!
             * Az utasok sorszámát egy-egy szóközzel elválasztva írja a képernyőre!
             */
            System.Console.WriteLine();
            System.Console.Write("3. feladat: A teljes utat végigutazták a következő sorszámú utasok: ");

            // Itt használható lenne a foreach() iteráció
            // de mivel szükségünk van az utas sorszámára,
            // (és a Jegy osztály nem tartalmaz egy belső "sorszám" értéket)
            // ezért inkább egy for ciklust használunk.

            // A GetUpperBound(0) megmondja a tömb 0. (és egyetlen)
            // dimenziójában a legnagyobb index értékét, tehát most
            // a for ciklus annyiszor fog lefutni, ahány elem van.
            for (int i = 0; i < vasarlasok.GetUpperBound(0); i++)
			{
                // Hogy elkerüljük az esetleges hibákat, mindig megnézzük,
                // hogy az aktuálisan iterált tömb elem egy Jegy típusú objektum-e.
                if ( vasarlasok[i] is Jegy )
                {

                    // Azok utazták végig a teljes utat, akik 0-nál szálltak fel, és
                    // az utolsó állomáson szálltak le. Ezt megjelenítjük a feladat szerint.
                    if ( vasarlasok[i].felszall == 0 && vasarlasok[i].leszall == vonal_hossza )
                    {
                        System.Console.Write(System.Convert.ToString(i) + " ");
                    }
                }
            }
            System.Console.WriteLine();

            /* NEGYEDIK RÉSZFELADAT
             * --------------------
             * Határozza meg, hogy a jegyekből mennyi bevétele származott
             * a társaságnak! Az eredményt írja a képernyőre!
             */
            int bevetel = 0;

            // Itt egy újabb iteráció következik.
            for (int i = 0; i < vasarlasok.GetUpperBound(0); i++)
            {
                if (vasarlasok[i] is Jegy)
                {
                    // Minden utasnál megvizsgáljuk az utazott távolságot,
                    // amelyet minden megkezdett 10 km-ként felszorzunk
                    // a tarifával (fizetendo_per_tizkm).

                    int utazott_tavolsag = vasarlasok[i].leszall - vasarlasok[i].felszall;
                    
                    // Feldaraboljuk az egészet minden megkezdett tíz kilométerre.
                    // A (double) a kifejezés előtt automatikusan double-lé konvertál
                    // (vigyázzunk, mivel az ilyen nem könnyen hibakezelhető!)
                    // így nem lesz hibaüzenet a fordítás során.
                    // Számítás után visszakonvertáljuk egész számmá az értéket.
                    double tizkm_darabok_szama = System.Math.Ceiling((double) utazott_tavolsag / 10);
                    
                    // A jegy árának meghatározásakor az értéket öttel
                    // osztható számra kell kerekítenie. (1, 2, 6 és 7
                    // esetén lefelé, 3, 4, 8 és 9 esetén
                    // pedig felfelé kell kerekítenie.)
                    // 
                    // Az 5-tel való osztás maradéktáblázata:
                    // 0: 0     5: 0
                    // 1: 1     6: 1
                    // 2: 2     7: 2
                    // 3: 3     8: 3
                    // 4: 4     9: 4
                    int maradek = ((int)tizkm_darabok_szama * fizetendo_per_tizkm) % 5;
                    int jegy_ara = 0;
                    if (maradek <= 2)
                    {
                        // 0, 1, 2, 5, 6 és 7 végződés esetén a fizetendő összeg lefelé
                        // kerekítődik, tehát a fizetendő összegből kivonjuk a maradékot.
                        jegy_ara = ((int)tizkm_darabok_szama * fizetendo_per_tizkm) - maradek;
                    }
                    else if (maradek >= 3)
                    {
                        // 3, 4, 8 és 9 esetén pedig felelé kerekítünk, tehát a maradékot hozzáadjuk.
                        jegy_ara = ((int)tizkm_darabok_szama * fizetendo_per_tizkm) + maradek;
                    }

                    // A meghatározott jegyárat hozzáadjuk a bevetel változóhoz.
                    bevetel += jegy_ara;
                }
            }

            System.Console.WriteLine();
            System.Console.WriteLine("4. feladat: Az autóbusztársaság összes bevétele: " + System.Convert.ToString(bevetel) + " Ft.");

            /* ÖTÖDIK RÉSZFELADAT
             * ------------------
             * Írja a képernyőre, hogy a busz végállomást megelőző
             * utolsó megállásánál hányan szálltak fel és le!
             */
            // Ennek a megvalósításához az úgynevezett HashSet-et fogjuk használni.
            // Ez a Set olyan, mint egy tömb, viszont nem tud egy elemet kétszer tartalmazni.
            HashSet<int> allomasok = new HashSet<int>{};
            for (int i = 0; i < vasarlasok.GetUpperBound(0); i++)
            {
                // Ha a hozzáadáskor az adott elem már a Set eleme lenne,
                // egyszerűen nem kerül hozzáadásra.
                allomasok.Add(vasarlasok[i].felszall);
                allomasok.Add(vasarlasok[i].leszall);
            }

            // Sorba rendezzük ezt a Set-et, majd tömböt készítünk.
            IEnumerable<int> allomasok_sorbateve = allomasok.OrderBy(elem => elem);
            int[] allomasok_int_tomb = allomasok_sorbateve.ToArray();
                        
            // Az utolsó előtti állomás km-ben számolt távolságát végül megkapjuk
            // úgy, hogy az allomasok_sorbateve elemeinek számából kivonunk 1-et.
            // 
            // Most végigmegyünk még egyszer a vasarlasok tömbön, megszámolva, 
            // hogy az adott megállónál hanyan szálltak fel és le.
            int felszallok = 0;
            int leszallok = 0;
            for (int i = 0; i < vasarlasok.GetUpperBound(0); i++)
            {
                if (vasarlasok[i] is Jegy)
                {
                    if (vasarlasok[i].felszall == allomasok_int_tomb[allomasok_int_tomb.GetUpperBound(0) - 1])
                    {
                        felszallok++;
                    }

                    if (vasarlasok[i].leszall == allomasok_int_tomb[allomasok_int_tomb.GetUpperBound(0) - 1])
                    {
                        leszallok++;
                    }
                }
            }

            System.Console.WriteLine();
            System.Console.Write("5. feladat: A végállomás előtti utolsó megállóban " + System.Convert.ToString(felszallok));
            System.Console.WriteLine(" utas szállt fel és " + System.Convert.ToString(leszallok) + " utas szállt le.");

            /* HATODIK RÉSZFELADAT
             * -------------------
             * Adja meg, hogy hány helyen állt meg a busz a kiinduló állomás
             * és a célállomás között! Az eredményt írja a képernyőre!
             */
            // Itt most nincs sok dolgunk, mivel a korábban
            // felépített tömb már tartalmazza a megállók számát.

            // Ebből azonban ki kell vonnunk kettőt, mivel a 0. elem a kezdőállomás
            // az utolsó elem pedig a végállomást jelöli.
            System.Console.WriteLine();
            System.Console.WriteLine("6. feladat: A busz " + System.Convert.ToString(allomasok_int_tomb.GetUpperBound(0) - 2) + " megállóban állt meg.");


            // A kilépés előtt várunk egy ENTER leütést
            System.Console.WriteLine();
            System.Console.WriteLine("A kilépéshez nyomjon ENTER-t...");
            System.Console.ReadLine();
            Environment.Exit(0);
        }
    }

    class Jegy
    {
        // A Jegy osztály tartalmazza egy darab jegyvásárlás adatait
        // a forrásfájl sorai egy ilyen osztályt tartalmazó tömbként
        // kerülnek leképezésre.
        public int ules_szam { get; set; }
        public int felszall { get; set; }
        public int leszall { get; set; }

        public Jegy(int ules_szam, int felszall, int leszall)
        {
            // Standard konstruktőr amely a megadott paraméterek alapján beállítja az osztály elemeit.
            this.ules_szam = ules_szam;
            this.felszall = felszall;
            this.leszall = leszall;
        }
    }
}