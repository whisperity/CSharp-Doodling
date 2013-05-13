using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

// Feladatleírás: http://dload.oktatas.educatio.hu/erettsegi/feladatok_emelt_2012osz/e_inf_12okt_fl.pdf

namespace szinkep
{
    class Program
    {
        public struct Keppont
        {
            public byte R;
            public byte G;
            public byte B;

            public override bool Equals(object obj)
            {
                if (obj == null || !(obj is Keppont))
                {
                    return false;
                }

                Keppont b = (Keppont)obj;

                if (this.R == b.R && this.G == b.G && this.B == b.B)
                    return true;
                else
                    return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public Keppont(byte r, byte g, byte b)
            {
                R = r;
                G = g;
                B = b;
            }

            static public bool operator ==(Keppont a, Keppont b)
            {
                if (a.R == b.R &&
                    a.G == b.G &&
                    a.B == b.B)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            static public bool operator !=(Keppont a, Keppont b)
            {
                if (a.R != b.R ||
                    a.G != b.G ||
                    a.B != b.B)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        static Keppont[,] Kep = new Keppont[50, 50];

        static void Main(string[] args)
        {
            Console.Title = "szinkep";

            Elso();
            Console.WriteLine(); Masodik();
            Console.WriteLine(); Harmadik();
            Console.WriteLine(); Negyedik();
            Console.WriteLine(); Otodik();
            Console.WriteLine(); Hatodik();
            Console.WriteLine(); Hetedik();

            Console.WriteLine();
            Console.WriteLine("Futtatás vége. ENTER kilép a programból!");
            Console.ReadLine();
            Environment.Exit(0);
        }

        static void Elso()
        {
            // Első feladat
            using (FileStream keptxt = new FileStream("kep.txt", FileMode.Open, FileAccess.Read))
            using (StreamReader read = new StreamReader(keptxt))
            {
                int sor = 0;
                int oszlop = 0;

                while (!read.EndOfStream)
                {
                    string olvas = read.ReadLine();
                    string[] elemek = olvas.Split(' ');

                    Kep[sor, oszlop] = new Keppont(Convert.ToByte(elemek[0]), Convert.ToByte(elemek[1]), Convert.ToByte(elemek[2]));

                    oszlop++;
                    if (oszlop == 50)
                    {
                        sor++;
                        oszlop = 0;
                    }
                }
            }
        }

        static void Masodik()
        {
            // Második feladat

            // Beolvasás
            Console.WriteLine("2. feladat: Kérek egy színt!");
            Console.Write("Vörös? ");
            string Sr = Console.ReadLine();

            Console.Write("Zöld? ");
            string Sg = Console.ReadLine();

            Console.Write("Kék? ");
            string Sb = Console.ReadLine();

            byte r = 0;
            byte g = 0;
            byte b = 0;
            try
            {
                r = Convert.ToByte(Sr);
                g = Convert.ToByte(Sg);
                b = Convert.ToByte(Sb);
            }
            catch (Exception)
            {
                Console.WriteLine("Érvénytelen a beírt érték! Számnak kell lennie, 0 és 255 között!");
                Console.WriteLine("A program futtatása megszakad. ENTER leütése után kilép...");
                Console.ReadLine();
                Environment.Exit(1);
            }

            // Keresés
            Keppont keresett_szin = new Keppont(r, g, b);
            bool talalt = false;
            for (int i = 0; i <= Kep.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= Kep.GetUpperBound(1); j++)
                {
                    if (Kep[i, j] == keresett_szin)
                        talalt = true;
                }
            }

            if (talalt)
                Console.WriteLine("Ilyen létezik a képen.");
            else
                Console.WriteLine("Ilyen nem létezik a képen.");
        }

        static void Harmadik()
        {
            // Harmadik feladat
            // (0-tól indexelünk ezért a 35. sor 8. képpontjának koordinátái 34 és 7)
            Keppont kp_35_8 = Kep[34, 7];

            // Sor átnézése
            int sorban = 0;
            for (int i = 0; i <= Kep.GetUpperBound(1); i++)
            {
                if (Kep[34, i] == kp_35_8)
                    sorban++;
            }

            // Oszlop átnézése
            int oszlopban = 0;
            for (int j = 0; j <= Kep.GetUpperBound(0); j++)
            {
                if (Kep[j, 7] == kp_35_8)
                    oszlopban++;
            }

            Console.WriteLine("3. feladat: Sorban: " + Convert.ToString(sorban) +
                " Oszlopban: " + Convert.ToString(oszlopban));
        }

        static void Negyedik()
        {
            // Negyedik feladat
            Keppont voros = new Keppont(255, 0, 0);
            Keppont zold = new Keppont(0, 255, 0);
            Keppont kek = new Keppont(0, 0, 255);

            int cVoros = 0;
            int cZold = 0;
            int cKek = 0;

            for (int i = 0; i <= Kep.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= Kep.GetUpperBound(1); j++)
                {
                    Keppont k = Kep[i, j];
                    if (Kep[i, j] == voros)
                        cVoros++;

                    if (Kep[i, j] == zold)
                        cZold++;

                    if (Kep[i, j] == kek)
                        cKek++;
                }
            }

            // Kiírás
            Console.Write("4. feladat: A legtöbbször előforduló szín a ");
            if (cVoros > cZold && cVoros > cKek)
                Console.WriteLine("vörös.");

            if (cZold > cVoros && cZold > cKek)
                Console.WriteLine("zöld.");

            if (cKek > cVoros && cKek > cZold)
                Console.WriteLine("kék.");
        }

        static void Otodik()
        {
            // Ötödik feladat
            Keppont fekete = new Keppont(0, 0, 0);

            // Három képpont széles
            // ergó
            // balsó három oszlop, felső három sor
            // alsó három sor és jobbsó három oszlop
            // (meg persze a metszéspontok, de azt felülírjuk kétszer)

            for (int i = 0; i <= Kep.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= Kep.GetUpperBound(1); j++)
                {
                    if (// Felső három sor
                        i == 0 || i == 1 || i == 2 ||
                        // Alsó három sor
                        i == Kep.GetUpperBound(0) - 2 ||
                        i == Kep.GetUpperBound(0) - 1 ||
                        i == Kep.GetUpperBound(0) ||
                        // Balsó három oszlop
                        j == 0 || j == 1 || j == 2 ||
                        // Jobbsó három oszlop
                        j == Kep.GetUpperBound(1) - 2 ||
                        j == Kep.GetUpperBound(1) - 1 ||
                        j == Kep.GetUpperBound(1)
                        )
                    {
                        Kep[i, j] = fekete;
                    }
                }
            }
        }

        static void Hatodik()
        {
            // Hatodik feladat
            using (FileStream fajl = new FileStream("keretes.txt", FileMode.Create, FileAccess.Write))
            using (StreamWriter write = new StreamWriter(fajl))
            {
                Keppont aktualis;

                for (int i = 0; i <= Kep.GetUpperBound(0); i++)
                {
                    for (int j = 0; j <= Kep.GetUpperBound(1); j++)
                    {
                        aktualis = Kep[i, j];
                        string sor = String.Join(" ", aktualis.R, aktualis.G, aktualis.B);

                        write.WriteLine(sor);
                    }
                }
            }
        }

        static void Hetedik()
        {
            Keppont sarga = new Keppont(255, 255, 0);

            int bal = 0;
            int felso = 0;
            int jobb = 0;
            int also = 0;

            // Keresés
            for (int i = 0; i <= Kep.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= Kep.GetUpperBound(1); j++)
                {
                    if (Kep[i, j] == sarga)
                    {
                        // Az első megtalált képpont a téglalap bal felső sarka
                        // (Persze kihasználjuk, hogy tudjuk, téglalapról van szó.)
                        if (bal == 0)
                            bal = i;
                        if (felso == 0)
                            felso = j;

                        // Maximumkeresés a jobb (X) és alsó (Y) meghatározására
                        if (i > jobb)
                            jobb = i;
                        if (j > also)
                            also = j;
                    }
                }
            }

            // Az iteráció végére megvannak a koordináták.
            int sorok = also - felso;
            int oszlopok = jobb - bal;
            int szam = sorok * oszlopok;

            // Kiírás
            Console.WriteLine("7. feladat:");
            Console.WriteLine("Kezd: " + felso + ", " + bal);
            Console.WriteLine("Vége: " + also + ", " + jobb);
            Console.WriteLine("Képpontok száma: " + szam);
        }
    }
}