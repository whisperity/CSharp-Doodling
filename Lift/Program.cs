using System;
using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Text;

namespace Lift
{
    class Program
    {
        struct igeny
        {
            public short ora;
            public short perc;
            public short masodperc;
            public short csapat;
            public short honnan;
            public short hova;
        }

        static void Main(string[] args)
        {
            // ELSŐ RÉSZFELADAT
            FileStream igeny_txt = null;
            try
            {
                igeny_txt = new FileStream("igeny.txt", FileMode.Open, FileAccess.Read);
            }
            catch (System.IO.IOException)
            {
                System.Console.WriteLine("Nem sikerült az igeny.txt megnyitása.");
                System.Console.WriteLine("Kérem ellenőrizze, hogy a forrásfájl a futtatható állománnyal egy mappában van-e!");
                System.Console.WriteLine("A kilépéshez nyomjon ENTER-t...");
                System.Console.ReadLine();
                Environment.Exit(1);
            }

            StreamReader txt = new StreamReader(igeny_txt);
            short emeletek = System.Convert.ToInt16(txt.ReadLine());
            short csapatok = System.Convert.ToInt16(txt.ReadLine());
            short igenyek_szam = System.Convert.ToInt16(txt.ReadLine());

            igeny[] igenyek = new igeny[igenyek_szam];
            for (int i = 0; i < igenyek_szam; i++)
            {
                string sor = txt.ReadLine();
                string[] elemek = sor.Split(' ');

                igenyek[i].ora = System.Convert.ToInt16(elemek[0]);
                igenyek[i].perc = System.Convert.ToInt16(elemek[1]);
                igenyek[i].masodperc = System.Convert.ToInt16(elemek[2]);
                igenyek[i].csapat = System.Convert.ToInt16(elemek[3]);
                igenyek[i].honnan = System.Convert.ToInt16(elemek[4]);
                igenyek[i].hova = System.Convert.ToInt16(elemek[5]);
            }
            
            // MÁSODIK RÉSZFELADAT
            System.Console.Write("2. feladat: Melyik szinten áll a lift az induláskor? ");
            short lift_kezdopont = System.Convert.ToInt16(System.Console.ReadLine());
            
            // HARMADIK RÉSZFELADAT
            System.Console.Write("3. feladat: A lift az utolsó igény teljesítése után a(z) ");
            System.Console.WriteLine(igenyek[igenyek.GetUpperBound(0)].hova + ". emeleten áll meg.");

            // NEGYEDIK RÉSZFELADAT
            int[] emelet_lista = new int[(int)igenyek.GetUpperBound(0) * 2];
            int j = 0;
            int em_i = 0;
            while (j < igenyek.GetUpperBound(0))
            {
                emelet_lista[em_i] = igenyek[j].honnan;
                emelet_lista[++em_i] = igenyek[j].hova;

                j++;
                em_i++;
            }
            int maximum = emelet_lista[0];
            int minimum = emelet_lista[0];

            for (int k = 0; k < emelet_lista.GetUpperBound(0); k++)
            {
                if ( emelet_lista[k] > maximum )
                {
                    maximum = emelet_lista[k];
                }

                if ( emelet_lista[k] < minimum )
                {
                    minimum = emelet_lista[k];
                }
            }
            System.Console.Write("4. feladat: A lift a(z) " + System.Convert.ToString(minimum) + ". és a(z) ");
            System.Console.WriteLine(System.Convert.ToString(maximum) + ". szintek között mozgott.");

            // ÖTÖDIK RÉSZFELADAT
            int felfele_utas = 0;
            int felfele_utas_nelkul = 0;
            for (int l = 0; l < igenyek.GetUpperBound(0); l++)
            {
                if (igenyek[l].honnan < igenyek[l].hova)
                {
                    felfele_utas++;
                }

                if (igenyek[l].hova < igenyek[l + 1].honnan)
                {
                    felfele_utas_nelkul++;
                }
            }

            System.Console.Write("5. feladat: A lift ennyiszer indult felfelé, utassal: ");
            System.Console.Write(System.Convert.ToString(felfele_utas) + ", utas nélkül: ");
            System.Console.WriteLine(System.Convert.ToString(felfele_utas_nelkul) + ".");

            // HATODIK RÉSZFELADAT
            List<int> csapatok_lista = new List<int>(csapatok);
            for (int m = 1; m <= csapatok; m++)
            {
                csapatok_lista.Add(m);
            }

            for (int n = 0; n < igenyek.GetUpperBound(0); n++)
            {
                csapatok_lista.Remove(igenyek[n].csapat);
            }

            csapatok_lista.Sort();
            System.Console.Write("6. feladat: A következő csapatok nem használták a liftet: ");
            foreach (int nem_utazott_csapat_id in csapatok_lista.ToArray())
            {
                System.Console.Write(System.Convert.ToString(nem_utazott_csapat_id) + ' ');
            }
            System.Console.WriteLine();


            
            System.Console.ReadLine();
        }
    }
}
