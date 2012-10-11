using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/* FELADATLEÍRÁS
 * -------------
 * http://www.oh.gov.hu/letolt/okev/doc/erettsegi_2010/e_info_10maj_fl.pdf
 * (a PDF fájlban, a 10. oldaltól)
 */

namespace Helyjegy
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream eladott_txt = new FileStream("eladott.txt", FileMode.Open, FileAccess.Read);
            StreamReader txt = new StreamReader(eladott_txt);

            string elso_sor = txt.ReadLine();
            string[] elso_harom_ertek = elso_sor.Split(' ');
            int eladott_jegyek_szama = System.Convert.ToInt32(elso_harom_ertek[0]);
            int vonal_hossza = System.Convert.ToInt32(elso_harom_ertek[1]);
            int fizetendo_per_tizkm = System.Convert.ToInt32(elso_harom_ertek[2]);

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

            /* MÁSODIK RÉSZFELADAT
             * -------------------
             * Adja meg a legutolsó jegyvásárló ülésének sorszámát
             * és az általa beutazott távolságot!
             * A kívánt adatokat a képernyőn jelenítse meg!
             */

            System.Console.Write("2. feladat: ");
            System.Console.Write("A legutolsó jegyvásárló a(z) ");
            System.Console.Write(System.Convert.ToString(vasarlasok[vasarlasok.GetUpperBound(0)].ules_szam));
            System.Console.Write(". számú ülésen utazott és ");
            int utolso_tavolsag = vasarlasok[vasarlasok.GetUpperBound(0)].leszall - vasarlasok[vasarlasok.GetUpperBound(0)].felszall;
            System.Console.WriteLine(System.Convert.ToString(utolso_tavolsag) + " km utat tett meg.");
            
            

            // A kilépés előtt várunk egy ENTER leütést
            System.Console.WriteLine();
            System.Console.WriteLine("A kilépéshez nyomjon ENTER-t...");
            System.Console.ReadLine();
            Environment.Exit(0);
        }
    }

    class Jegy
    {
        public int ules_szam { get; set; }
        public int felszall { get; set; }
        public int leszall { get; set; }

        public Jegy(int ules_szam, int felszall, int leszall)
        {
            this.ules_szam = ules_szam;
            this.felszall = felszall;
            this.leszall = leszall;
        }
    }
}