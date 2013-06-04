using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// 2013. május emelt érettségi programozás feladat megoldása.
// http://dload.oktatas.educatio.hu/erettsegi/feladatok_2013tavasz_emelt/e_inf_13maj_fl.pdf
// 
// A forrásfájl (szavazatok.txt) a programhoz csatolva.

namespace valasztas
{
    class valasztas
    {
        /// <summary>
        /// Egy képviselőjelölt adatait tartalmazza
        /// </summary>
        struct Szavazat
        {
            /// <summary>
            /// A választókerület sorszáma
            /// </summary>
            public uint Valasztokerulet { get; private set; }

            /// <summary>
            /// A leadott szavazatok száma
            /// </summary>
            public uint SzavazatSzam { get; private set; }
            
            /// <summary>
            /// A képviselő vezetékneve
            /// </summary>
            public string Vezeteknev { get; private set; }
            
            /// <summary>
            /// A képviselő utóneve
            /// </summary>
            public string Utonev { get; private set; }

            /// <summary>
            /// A képviselő pártja
            /// </summary>
            public string Part { get; private set; }

            /// <summary>
            /// Konstruktőr egy képviselő adatainak létrehozásához
            /// </summary>
            /// <param name="kerulet">a választókerület száma</param>
            /// <param name="szavazatSzam">a leadott szavazatok száma</param>
            /// <param name="vezeteknev">a képviselő vezetékneve</param>
            /// <param name="utonev">a képviselő keresztneve</param>
            /// <param name="part">a képviselő pártja</param>
            public Szavazat(uint kerulet, uint szavazatSzam, string vezeteknev, string utonev, string part) : this()
            {
                this.Valasztokerulet = kerulet;
                this.SzavazatSzam = szavazatSzam;
                this.Vezeteknev = vezeteknev;
                this.Utonev = utonev;
                this.Part = part;
            }
        }

        /// <summary>
        /// A szavazásra jogosultak száma
        /// </summary>
        const uint JogosultakSzama = 12345;
        
        /// <summary>
        /// A szavazati adatokat tároló lista
        /// </summary>
        static List<Szavazat> Szavazatok = new List<Szavazat>(100);

        /// <summary>
        /// A pártok adatait tároló lista
        /// Használatos a rövidítés->teljes név konverzióhoz
        /// </summary>
        static Dictionary<string, string> Partok = new Dictionary<string, string>(5);
            
        static void Main(string[] args)
        {
            Console.Title = "valasztas";

            // Betöltjük a pártok adatait a feladatlapon megadottak szerint
            Partok.Add("GYEP", "Gyümölcsevők Pártja");
            Partok.Add("HEP", "Húsevők Pártja");
            Partok.Add("TISZ", "Tejivók Szövetsége");
            Partok.Add("ZEP", "Zöldségevők Pártja");
            Partok.Add("-", "Független jelöltek");

            // Végigfutnak a feladatok
            Elso();
            Console.WriteLine(); Masodik();
            Console.WriteLine(); Harmadik();
            Console.WriteLine(); Negyedik();
            Console.WriteLine(); Otodik();
            Console.WriteLine(); Hatodik();
            Console.WriteLine(); Hetedik();

            Console.WriteLine();
            Console.WriteLine("Futtatás vége. ENTER kilép.");
            Console.ReadLine();
            Environment.Exit(0);
        }

        static void Elso()
        {
            // Fájl beolvasása
            try
            {
                using (FileStream szavazatokFajl = new FileStream("szavazatok.txt", FileMode.Open, FileAccess.Read))
                using (StreamReader sr = new StreamReader(szavazatokFajl))
                {
                    while (!sr.EndOfStream)
                    {
                        // A bemeneti fájl szöveges sorokat tartalmaz. Ezt olvassuk, daraboljuk...
                        string sor = sr.ReadLine();
                        string[] elemek = sor.Split(' ');

                        try
                        {
                            // ...és tároljuk.
                            Szavazat egy_szavazat = new Szavazat(
                                Convert.ToUInt32(elemek[0]),
                                Convert.ToUInt32(elemek[1]),
                                elemek[2],
                                elemek[3],
                                elemek[4]);

                            Szavazatok.Add(egy_szavazat);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Nem sikerült a fájl beolvasása: " + e.Message);
                            continue;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("A fájl beolvasása során hiba történt.");
                Console.WriteLine(e.Message);

                Console.WriteLine("A program nem futhat tovább! ENTER kilép.");
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        static void Masodik()
        {
            Console.WriteLine("2. feladat");
            Console.WriteLine("A helyhatósági választáson " + Convert.ToString(Szavazatok.Count) + " képviselőjelölt indult.");
        }

        static void Harmadik()
        {
            Console.WriteLine("3. feladat");
            Console.Write("A képviselő vezetékneve? ");
            string vezetek = Console.ReadLine();

            Console.Write("Utóneve? ");
            string uto = Console.ReadLine();

            // Megkeressük ahol a két megadott érték egyezik
            IEnumerable<Szavazat> talalt_kepviselo = Szavazatok.Where(
                keres => (keres.Vezeteknev == vezetek && keres.Utonev == uto));

            if (talalt_kepviselo.Count() == 0)
                Console.WriteLine("Ilyen nevű képviselőjelölt nem szerepel a nyilvántartásban!");
            else
                Console.WriteLine("A képviselőjelölt " + Convert.ToString(talalt_kepviselo.First().SzavazatSzam) +
                    " szavazatot kapott.");
        }

        static void Negyedik()
        {
            Console.WriteLine("4. feladat");

            // Ez egy kicsit érdekesebb.
            // Lényege, hogy az aggregátor végigszalad az összes elemen a Szavazatok listában
            // majd minden elemnél a korábban létező "osszeg" ideiglenes változóhoz
            // hozzáadja az aktuális elem struct{} megadott tulajdonságát (SzavazatSzam)
            //
            // Az eredményt meg kiadja uint32-ként.
            uint szavazok_szama = Szavazatok.Aggregate<Szavazat, uint>(0, (osszeg, aktualis) => osszeg + aktualis.SzavazatSzam);

            double reszveteli_arany = ((double)szavazok_szama / (double)JogosultakSzama) * 100;

            Console.WriteLine("A választáson " + Convert.ToString(szavazok_szama) + " állampolgár," +
                " a jogosultak " + Convert.ToString(Math.Round(reszveteli_arany, 2)) + "%-a vett részt.");
        }

        static void Otodik()
        {
            Console.WriteLine("5. feladat");
            
            Dictionary<string, uint> PartokSzavazatai = new Dictionary<string, uint>(5);

            // Végigmegyünk a Szavazatok tömbön, összeszedve a pártok szavazatait, pártok szerint csoportosítva
            IEnumerable<IGrouping<string, uint>> kigyujt = Szavazatok.GroupBy<Szavazat, string, uint>(
                keySelector: part => part.Part, 
                elementSelector: szavazat => szavazat.SzavazatSzam);

            // Ezt az IEnumerable<>-t enumeráljuk/iteráljuk
            foreach (IGrouping<string, uint> elem in kigyujt)
            {
                // Megkeressük (Where()) az összes szavazatot ahol a párt az éppen keresett párt
                // Majd kiválasztjuk csak a szavazatszámot és minden ilyen elem értékét hozzáadjuk az összeghez
                uint part_szavazat = Szavazatok.Where(partszavazat => partszavazat.Part == elem.Key).
                    Select(szam => szam.SzavazatSzam).
                    Aggregate<uint, uint>(0, (osszeg, aktualis) => osszeg + aktualis);

                PartokSzavazatai.Add(Partok[elem.Key], part_szavazat);
            }

            // A 4. feladattal megegyező aggregáció
            uint szavazok_szama = Szavazatok.Aggregate<Szavazat, uint>(0, (osszeg, aktualis) => osszeg + aktualis.SzavazatSzam);

            foreach (KeyValuePair<string, uint> egyPart in PartokSzavazatai)
            {
                double szavazat_arany = ((double)egyPart.Value / (double)szavazok_szama) * 100;
                Console.WriteLine(egyPart.Key + "= " + Convert.ToString(Math.Round(szavazat_arany, 2)) + "%");
            }
        }

        static void Hatodik()
        {
            Console.WriteLine("6. feladat");

            // Kiválasztjuk a legtöbb szavazatot elérőket (azon rekordok, ahol a szám megegyezik az összes rekord közül
            // legnagyobb számmal).
            IEnumerable<Szavazat> legtobb_szavazatot_elerok = Szavazatok.
                Where(szav => szav.SzavazatSzam == Szavazatok.Max(szavazat => szavazat.SzavazatSzam));

            foreach (Szavazat egy_jelolt in legtobb_szavazatot_elerok)
            {
                Console.WriteLine(egy_jelolt.Vezeteknev + " " + egy_jelolt.Utonev + " (" +
                    (egy_jelolt.Part == "-" ? "független" : egy_jelolt.Part) + ")");
            }
        }

        static void Hetedik()
        {
            // Fájl írása
            Console.WriteLine("7. feladat");
            try
            {
                using (FileStream fs = new FileStream("kepviselok.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    // Összeszedjük a kerületek szerint a szavazatokat, majd rendezzük a gyűjteményt a kerület száma szerint
                    IOrderedEnumerable<IGrouping<uint, Szavazat>> valasztokeruletek_szavazatai =
                        Szavazatok.GroupBy<Szavazat, uint, Szavazat>(
                            keySelector: csoport => csoport.Valasztokerulet,
                            elementSelector: szav => szav).
                        OrderBy(order => order.Key);

                    foreach (IGrouping<uint, Szavazat> egy_valasztokerulet in valasztokeruletek_szavazatai)
                    {
                        // Meghatározzuk az adott kerületben (ahol a két érték egyezik) elért maximális szavazatot
                        uint max_szavazat_a_keruletben = Szavazatok.
                            Where(kerulet => kerulet.Valasztokerulet == egy_valasztokerulet.Key).
                                Max(mi_a_max => mi_a_max.SzavazatSzam);

                        // Megkeressük a nyertes jelöltet
                        // (a .First() a végén visszaadja az első elemet az IEnumerable<>-ből.)
                        // (Feltételezzük, hogy csak egy nyertes van.)
                        Szavazat nyertes = Szavazatok.
                            Where<Szavazat>(kerulet => kerulet.Valasztokerulet == egy_valasztokerulet.Key).
                            Where<Szavazat>(szavazatszam => szavazatszam.SzavazatSzam == max_szavazat_a_keruletben).First();

                        // Kiírjuk az adatokat a fájlba
                        sw.WriteLine(Convert.ToString(egy_valasztokerulet.Key) + ". " +
                            nyertes.Vezeteknev + " " + nyertes.Utonev + " (" +
                            (nyertes.Part == "-" ? "független" : nyertes.Part) + ")");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Nem sikerült a 'kepviselok.txt' állomány írása.");
                Console.WriteLine(e.Message);

                Console.WriteLine("ENTER kilép.");
                Environment.Exit(0);
            }

            Console.WriteLine("Az állomány (kepviselok.txt) elkészült.");
        }
    }
}
