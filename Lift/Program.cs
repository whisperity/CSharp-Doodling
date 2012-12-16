using System;
using System.Collections.Generic;
using System.IO;
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

            txt.Close();
            igeny_txt.Close();

            System.Console.Write("2. feladat: Melyik szinten áll a lift az induláskor? ");
            short lift_kezdopont = System.Convert.ToInt16(System.Console.ReadLine());

            System.Console.Write("3. feladat: A lift az utolsó igény teljesítése után a(z) ");
            System.Console.WriteLine(igenyek[igenyek.GetUpperBound(0)].hova + ". emeleten áll meg.");

            int[] emelet_lista = new int[(int)igenyek.Length * 2];
            int j = 0;
            int em_i = 0;
            while (j < igenyek.Length)
            {
                emelet_lista[em_i] = igenyek[j].honnan;
                emelet_lista[++em_i] = igenyek[j].hova;

                j++;
                em_i++;
            }
            int maximum = emelet_lista[0];
            int minimum = emelet_lista[0];

            for (int k = 0; k < emelet_lista.Length; k++)
            {
                if (emelet_lista[k] > maximum)
                {
                    maximum = emelet_lista[k];
                }

                if (emelet_lista[k] < minimum)
                {
                    minimum = emelet_lista[k];
                }
            }
            System.Console.Write("4. feladat: A lift a(z) " + System.Convert.ToString(minimum) + ". és a(z) ");
            System.Console.WriteLine(System.Convert.ToString(maximum) + ". szintek között mozgott.");

            int felfele_utas = 0;
            int felfele_utas_nelkul = 0;
            for (int l = 0; l < igenyek.Length - 1; l++)
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

            System.Console.Write("5. feladat: A lift ennyiszer indult felfelé: utassal: ");
            System.Console.Write(System.Convert.ToString(felfele_utas) + ", utas nélkül: ");
            System.Console.WriteLine(System.Convert.ToString(felfele_utas_nelkul) + ".");

            List<int> csapatok_lista = new List<int>(csapatok);
            for (int m = 1; m <= csapatok; m++)
            {
                csapatok_lista.Add(m);
            }

            for (int n = 0; n < igenyek.Length; n++)
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

            Random veletlen_generator = new Random();
            int vizsgalt_csapat = 0;

            bool csapat_letezik = false;
            while (!csapat_letezik)
            {
                vizsgalt_csapat = veletlen_generator.Next(1, csapatok);
                if (!csapatok_lista.Contains(vizsgalt_csapat))
                {
                    csapat_letezik = true;
                }
            }
            
            System.Console.WriteLine("7. feladat: A következő csapatot vizsgáljuk: " + System.Convert.ToString(vizsgalt_csapat) + ".");

            List<igeny> vizsgalt_csapat_utjai = new List<igeny>(igenyek.Length);
            for (int o = 0; o < igenyek.Length; o++)
            {
                if (igenyek[o].csapat == vizsgalt_csapat)
                {
                    vizsgalt_csapat_utjai.Add(igenyek[o]);
                }
            }

            bool volt_szabalytalansag = false;
            int szabalytalan_honnan = -1;
            int szabalytalan_hova = -1;

            for (int p = 1; p < vizsgalt_csapat_utjai.Count; p++)
            {
                if (vizsgalt_csapat_utjai[p].honnan != vizsgalt_csapat_utjai[p-1].hova)
                {
                    volt_szabalytalansag = true;

                    szabalytalan_honnan = vizsgalt_csapat_utjai[p-1].hova;
                    szabalytalan_hova = vizsgalt_csapat_utjai[p].honnan;
                }
            }

            if (volt_szabalytalansag == true)
            {
                System.Console.WriteLine("Történt szabálytalanság.");
                System.Console.Write("A vizsgált csapat a következő két szint között gyalog közlekedett: ");
                System.Console.WriteLine(System.Convert.ToString(szabalytalan_honnan) + ". és " + System.Convert.ToString(szabalytalan_hova) + ".");
            }
            else
            {
                System.Console.WriteLine("Nem történt szabálytalanság.");
            }

            System.Console.WriteLine("8. feladat: Hiba történt a munkák regisztrálása során.");
            System.Console.WriteLine("Kérem a következő csapat munkaadatait a műszakvizsga feltöltéséhez: " + System.Convert.ToString(vizsgalt_csapat) + "!");

            FileStream blokkol_txt = null;
            try
            {
                blokkol_txt = new FileStream("blokkol.txt", FileMode.Create, FileAccess.Write);
            }
            catch (System.IO.IOException)
            {
                System.Console.WriteLine("Nem sikerült a blokkol.txt létrehozása.");
                System.Console.WriteLine("Kérem ellenőrizze, hogy van írási joga a futtatható állományt tartalmazó mappához!");
                System.Console.WriteLine("A kilépéshez nyomjon ENTER-t...");
                System.Console.ReadLine();
                Environment.Exit(1);
            }

            StreamWriter blokkol_writer = new StreamWriter(blokkol_txt);

            for (int q = 0; q < vizsgalt_csapat_utjai.Count; q++)
            {
                int munkakod = -1;
                bool sikeres = false;

                System.Console.WriteLine("-----");
                System.Console.WriteLine("Indulási emelet: " + System.Convert.ToString(vizsgalt_csapat_utjai[q].honnan));
                System.Console.WriteLine("Célemelet: " + System.Convert.ToString(vizsgalt_csapat_utjai[q].hova));

                bool feladatkod_jo = false;
                while (!feladatkod_jo)
                {
                    try
                    {

                        System.Console.Write("Kérem az elvégzett feladat kódját! (1-99) ");
                        munkakod = System.Convert.ToInt32(System.Console.ReadLine());

                        if (munkakod < 1 || munkakod > 99)
                        {
                            throw new System.Exception("A munkakód csak 1 és 99 közötti egész szám lehet.");
                        }

                        feladatkod_jo = true;
                    }
                    catch (System.Exception hiba)
                    {
                        System.Console.WriteLine("Hiba történt a munkakód beolvasása során: " + hiba.Message);
                        feladatkod_jo = false;
                    }
                }

                System.Console.Write("Befejezés ideje: ");
                System.Console.Write(System.Convert.ToString(vizsgalt_csapat_utjai[q].ora) + ":");
                System.Console.Write(System.Convert.ToString(vizsgalt_csapat_utjai[q].perc) + ":");
                System.Console.WriteLine(System.Convert.ToString(vizsgalt_csapat_utjai[q].masodperc));

                bool sikeres_jo = false;
                string sikeres_bemenet = null;
                while (!sikeres_jo)
                {
                    try
                    {

                        System.Console.Write("Kérem a munka sikerességét! (I/N) ");
                        sikeres_bemenet = System.Console.ReadLine();

                        if (sikeres_bemenet != "I" && sikeres_bemenet != "i" && sikeres_bemenet != "N" && sikeres_bemenet != "n")
                        {
                            throw new System.Exception("Kérem, használja az I (igen) vagy N (nem) a sikeresség jelzéséhez.");
                        }

                        sikeres_jo = true;
                    }
                    catch (System.Exception hiba)
                    {
                        System.Console.WriteLine("Hiba történt a sikeresség beolvasása során: " + hiba.Message);
                        sikeres_jo = false;
                    }
                }

                if (sikeres_bemenet == "i" || sikeres_bemenet == "I")
                {
                    sikeres = true;
                }
                else if (sikeres_bemenet == "n" || sikeres_bemenet == "N")
                {
                    sikeres = false;
                }

                blokkol_writer.WriteLine("Indulási emelet: " + System.Convert.ToString(vizsgalt_csapat_utjai[q].honnan));
                blokkol_writer.WriteLine("Célemelet: " + System.Convert.ToString(vizsgalt_csapat_utjai[q].hova));
                blokkol_writer.WriteLine("Feladatkód: " + System.Convert.ToString(munkakod));
                blokkol_writer.Write("Befejezés ideje: ");
                blokkol_writer.Write(System.Convert.ToString(vizsgalt_csapat_utjai[q].ora) + ":");
                blokkol_writer.Write(System.Convert.ToString(vizsgalt_csapat_utjai[q].perc) + ":");
                blokkol_writer.WriteLine(System.Convert.ToString(vizsgalt_csapat_utjai[q].masodperc));
                blokkol_writer.Write("Sikeresség: ");

                if (sikeres == true)
                {
                    blokkol_writer.WriteLine("befejezett");
                }
                else if (sikeres == false)
                {
                    blokkol_writer.WriteLine("befejezetlen");
                }

                blokkol_writer.WriteLine("-----");
                blokkol_writer.Flush();
            }

            blokkol_writer.Close();
            blokkol_txt.Close();
            System.Console.WriteLine();

            System.Console.WriteLine("A kilépéshez nyomjon ENTER-t...");
            System.Console.ReadLine();
            Environment.Exit(0);
        }
    }
}