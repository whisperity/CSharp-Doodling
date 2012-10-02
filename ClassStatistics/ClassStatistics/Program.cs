using System;
using System.Collections.Generic;
using System.Text;

// A System.IO osztály szükséges a fájlműveletekhez.
using System.IO;

/* FELADATLEÍRÁS
 * -------------
 * Készíts programot, ami max. 50 ember adatait bekéri
 * az elemszámot, nevet, születési dátumot és matematika évvégi jegyet,
 * majd átlagol, meghatározza van-e bukás, kik buktak, van-e ötös, kik kaptak ötöst!
 */

/* FELADATLEÍRÁS
 * -------------
 * Alakítsa át úgy a programot, hogy az a rekordokat fájlból
 * is képes legyen kezelni, valamint a kimenet fájlba íródjon.
 */

namespace ClassStatistics
{
    class Program
    {
        private static FileStream create_FileStream(string path, FileMode mode, FileAccess access, bool ertesitsen = true)
        {
            /* Ez a függvény a megadott fájlhoz létrehoz
             * egy fájl adatfolyam mutatót, és ezzel visszatér.
             * 
             * Menet közben elvégzünk néhány szükséges hibakezelést.
             */

            // Készítünk egy FileStream objektumot amely a hozzáférési "fogantyú" lesz.
            // Itt 'null'-ra kell inicializálni, mivel különben "Use of unassigned local variable" hibát kapunk.
            FileStream new_handle = null;

            // A későbbi hiba megjelenítéséhez egy stringbe tároljuk a hiba szöveget.
            string hiba_szoveg = "";
            try
            {
                // Megpróbáljuk létrehozni a fájlhoz való kapcsolatot.
                // Ez különböző hibákkal térhet vissza, amelyeket későbbi catch(){} blokkokban kezelünk.
                new_handle = new FileStream(path, mode, access);
            }
            catch (System.IO.FileNotFoundException)
            {
                // Nem található fájl esetén megjelenítjük a megfelelő hibát.
                
                hiba_szoveg = "A megadott fájl (" + path + ") nem található.";
            }
            catch (System.UnauthorizedAccessException)
            {
                // Ha hozzáférési hiba történik, értesítjük a felhasználót.
                hiba_szoveg = "A fájl nem kezelhető, mivel nincs megfelelő hozzáférés.";
            }
            catch (System.ArgumentException)
            {
                // ArgumentException történik, ha a paraméter nem értelmezhető.
                // Például "C:\Valami?.txt" esetén, mivel a Windows nem kezeli a "?"-t a fájlnévben.
                hiba_szoveg = "A megadott paraméter nem megfelelő.\nValószínűleg nem használható karaktereket tartalmaz.";
            }
            catch (System.IO.IOException ioex)
            {
                // Az általános IOException (I/O hiba) akkor kerül megjelenítésre,
                // ha a konkrét hibatípus nem ismert.

                // Az 'ioex' helyi változó tárolja a hiba (Exception) objektumát.
                // Így a szöveg kiolvashatóvá válik.
                hiba_szoveg = "Kezelési hiba történt: " + ioex.Message;
            }

            // Ha a meghíváskor kértük az értesítést, akkor most megjelenítjük a hiba szövegét.
            if ( ertesitsen == true )
            {
                System.Console.WriteLine(hiba_szoveg);
            }

            // Visszatérünk a létrehozott objektummal (vagy null-lal).
            return new_handle;
        }
            
        static void Main(string[] args)
        {
            // Beállítjuk a konzolablak fejlécét.
            System.Console.Title = "Osztálystatisztika";
            
            // Rekordok száma.
            uint rekord = 0;

            // Ellenőrizzük, hogy létezik-e 'diakok.txt' fájl a futtatási mappában.
            string diak_fajl = "diakok.txt";
            bool diak_fajl_letezik = System.IO.File.Exists(diak_fajl);
            bool diakok_fajlbol = false;
            FileStream olvas_handle = null;
            StreamReader stream_olvaso = null;

            // Ha létezik, kiépítünk egy kapcsolatot a fájlhoz és beolvassuk a diákokat.
            if (diak_fajl_letezik == true)
            {
                olvas_handle = create_FileStream(diak_fajl, FileMode.Open, FileAccess.Read, false);

                if (olvas_handle != null)
                {
                    stream_olvaso = new StreamReader(olvas_handle);

                    // Beolvassuk a rekordok számát az első sorból.
                    rekord = System.Convert.ToUInt32(stream_olvaso.ReadLine());

                    // A diákok most fájlból kerültek beolvasásra, úgyhogy
                    // letiltjuk a későbbi direkt beolvasást.
                    diakok_fajlbol = true;
                }
            }

            // Ha a diákok nem fájlból kerülnek beolvasásra, akkor bekérjük kézzel.
            if (diakok_fajlbol == false)
            {
                // Bekérjük a felhasználótól a diákok számát.
                bool rekord_bekeres_hiba = true; // Hiba állapota

                while (rekord_bekeres_hiba)
                {
                    // Az itt megadott ciklus addig fut, ameddig hiba van a bekért adatban.

                    try
                    {
                        // A try{} blokk megpróbálja bekérni a diákok számát, azt konvertálni
                        // majd a hiba változót hamisra állítani (megállítani az egyébként végtelen ciklust).

                        System.Console.Write("Kérem adja meg a diákok számát (0-50): ");
                        rekord = System.Convert.ToUInt16(System.Console.ReadLine());
                        rekord_bekeres_hiba = false;
                    }
                    catch (System.FormatException)
                    {
                        // Formátumhiba esetén a hibaváltozó visszaáll igazra (a ciklus újra le fog futni)
                        // és értesítjük a felhasználót a vétségéről.

                        rekord_bekeres_hiba = true;
                        System.Console.WriteLine("A megadott szám nem egész szám.");
                        System.Console.WriteLine();
                    }
                    catch (System.OverflowException)
                    {
                        // Hasonló módon járunk el túlcsordulás (tartományhiba) esetén is.

                        rekord_bekeres_hiba = true;
                        System.Console.WriteLine("A megadott szám kívül esik a megengedett (0-50) tartományon.");
                        System.Console.WriteLine();
                    }
                    finally
                    {
                        // A finally{} blokk lefut akkor is ha volt hiba és akkor is ha nem.
                        // Itt érvényesítjük a feladat szempontjából szükséges feltételt: max. 50 rekord lehetséges.

                        if (rekord < 0 || rekord > 50)
                        {
                            rekord_bekeres_hiba = true;
                            System.Console.WriteLine("A megadott szám kívül esik a megengedett (0-50) tartományon.");
                            System.Console.WriteLine();
                        }
                    }
                }
            }
            else if (diakok_fajlbol == true)
            {
                // Ha fájlból lettek belolvasva, csak kiírjuk a már beolvasott számot.
                System.Console.WriteLine("Rekordok száma: " + System.Convert.ToString(rekord));
            }

            /* A feladat megvalósításához tömbtartalmú tömböket (ún. jagged array) fogok használni
             * Ennek lényege egy tipikus adattábla leképzése használható formába:
             * A külső tömb a sorokat tartalmazza, a belső tömb pedig (minden sorkulcs egy értéke)
             * egy újabb tömb, amelyben minden kulcs az oszlopot, minden érték pedig a cellát szimbolizálja.
             */

            /* ^ Igen ám, csak sajnos C#-ban ez nem megvalósítható.
             * Helyette készítünk lentebb egy saját osztályt, amelyekből fog
             * az egész adatstruktúra felépülni.
             */

            // Lefoglaljuk az adattáblát a memóriában, felhasználva az új osztályt.
            Student[] diakok = new Student[rekord];

            for (uint i = 0; i <= (rekord-1); i++)
            {
                // Egy for ciklusban bekérjük az adatokat és eltároljuk őket az osztályba.

                string nev = "";
                uint ev = 0, honap = 0, nap = 0, matekjegy = 0;
                bool rekord_bekeres_hiba = true;

                // Ha nem fájlból történik a beolvasás, akkor kézzel kérjük be az adatokat.
                if (diakok_fajlbol == false)
                {
                    while (rekord_bekeres_hiba)
                    {
                        // A már ismert struktúrát használva itt is addig
                        // kérünk be adatokat a felhasználótól, amíg azok megfelelőek.

                        try
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine(System.Convert.ToString(i + 1) + ". rekord:");

                            // Ha már volt beolvasva név, akkor kiírjuk.
                            // (Így tehát ha később hiba történik, itt nem kell újra megadni az adatot.)
                            System.Console.Write("Név? ");
                            if (nev == "")
                            {
                                nev = System.Console.ReadLine();
                            }
                            else
                            {
                                System.Console.WriteLine(nev);
                            }

                            // Hasonló módon járunk el a többi változó esetében is.

                            System.Console.Write("Születési év? ");
                            if (ev == 0)
                            {
                                ev = System.Convert.ToUInt32(System.Console.ReadLine());
                            }
                            else
                            {
                                System.Console.WriteLine(System.Convert.ToString(ev));
                            }

                            System.Console.Write("Hónap? ");
                            if (honap == 0)
                            {
                                honap = System.Convert.ToUInt32(System.Console.ReadLine());
                            }
                            else
                            {
                                System.Console.WriteLine(System.Convert.ToString(honap));
                            }

                            System.Console.Write("Nap? ");
                            if (nap == 0)
                            {
                                nap = System.Convert.ToUInt32(System.Console.ReadLine());
                            }
                            else
                            {
                                System.Console.WriteLine(System.Convert.ToString(nap));
                            }

                            System.Console.Write("Matekmatika évvégi osztályzat? ");
                            if (matekjegy == 0)
                            {
                                matekjegy = System.Convert.ToUInt32(System.Console.ReadLine());
                            }
                            else
                            {
                                System.Console.WriteLine(System.Convert.ToString(matekjegy));
                            }

                            rekord_bekeres_hiba = false;

                            // Itt elvégzünk egy hibakezelést, amely a 'matekjegy' nem eleme [0; 5] Z+
                            // intervallum hibáját hivatott megelőzni.
                            // Mivel nincs eldobott hiba, nincs is mit elkapni: egy normál if blokkal fogunk dolgozni.
                            if (matekjegy <= 0 || matekjegy > 5)
                            {
                                matekjegy = 0;
                                rekord_bekeres_hiba = true;
                                System.Console.WriteLine("A matematika osztályzat érvénytelen.");
                                System.Console.WriteLine();
                            }
                        }
                        catch (System.FormatException)
                        {
                            // Formátumhiba esetén a hibaváltozó visszaáll igazra (a ciklus újra le fog futni)
                            // és értesítjük a felhasználót a vétségéről.

                            rekord_bekeres_hiba = true;
                            System.Console.WriteLine("A megadott szám nem egész szám.");
                            System.Console.WriteLine();
                        }
                        catch (System.OverflowException)
                        {
                            // Hasonló módon járunk el túlcsordulás (tartományhiba) esetén is.

                            rekord_bekeres_hiba = true;
                            System.Console.WriteLine("A megadott szám kívül esik a megengedett tartományon.");
                            System.Console.WriteLine();
                        }
                    }
                }
                else if (diakok_fajlbol == true)
                {
                    // Fájlból történő beolvasás során végrehatjuk azt és feltöltjük a váltózokat.
                    if (olvas_handle != null && stream_olvaso != null)
                    {
                        string adatsor = stream_olvaso.ReadLine();
                        string[] ertekek = adatsor.Split(new string[] { ";" }, StringSplitOptions.None);

                        // Az 'ertekek' tömb most tartalmazza a sor elemeit.
                        // Most rendre feltöltjük a már deklarált változókat az értékekkel.
                        i = System.Convert.ToUInt32(ertekek[0]);
                        nev = ertekek[1];
                        ev = System.Convert.ToUInt32(ertekek[2]);
                        honap = System.Convert.ToUInt32(ertekek[3]);
                        nap = System.Convert.ToUInt32(ertekek[4]);
                        matekjegy = System.Convert.ToUInt32(ertekek[5]);

                        System.Console.WriteLine("Rekord #" + System.Convert.ToString(i + 1) + " sikeresen beolvasva.");
                    }
                }

                // A while blokkon kívül már minden adat megfelelő, tehát letároljuk a diákot.
                diakok[i] = new Student(i, nev, ev, honap, nap, matekjegy);

                System.Console.WriteLine("Rekord #" + System.Convert.ToString(i + 1) + " sikeresen tárolva.");
            }
            System.Console.WriteLine(System.Convert.ToString(diakok.Length) + " rekord tárolva a memóriába.");

            // Lezárjuk a diákokat beolvasó adatfolyamot.
            if (diak_fajl_letezik == true)
            {
                if (olvas_handle != null)
                {
                    olvas_handle.Close();
                }

                if (stream_olvaso != null)
                {
                    stream_olvaso.Close();
                }
            }
            // Megtörténik a diákok fájlba kiírása (ha a felhasználó ezt kéri).
            System.Console.WriteLine();

            // Két logikai változó. Az első a beírt érték hibáját jelzi (érvénytelen bemenet),
            // a másik a tényleges kiíratási szándékot tartalmazza.
            bool iras_bemenet_hiba = true;
            bool fajlba_iras = false;
            
            // Addig próbáljuk a usert rávenni, hogy írjon be értéket, amíg a beírt érték jó nem lesz.
            while (iras_bemenet_hiba)
            {
                // Bekérjük a választ.
                System.Console.WriteLine("FIGYELEM! A meglévő fájl felül lesz írva!");
                System.Console.Write("Fájlba mentsük a diákokat és az eredményeket? (I/N) ");
                string valasz = System.Console.ReadLine();

                // Feltételezzük, hogy a bemenet jó, így a ciklust letiltjuk.
                iras_bemenet_hiba = false;

                if (valasz == "i" || valasz == "I")
                {
                    // Ha a felhasználó igazat írt, beállítjuk, hogy szeretne tároltatni.
                    fajlba_iras = true;
                }
                else if (valasz == "n" || valasz == "N")
                {
                    // Nemleges válasz esetén (biztonsági okokból még egyszer) beállítjuk
                    // a nemleges választ. (Itt használhatnánk az alap értéket, ami ugyaúgy FALSE.)
                    fajlba_iras = false;
                }
                else
                {
                    // Hibás válasz esetén visszakapcsoljuk a ciklus újra lefutását.
                    iras_bemenet_hiba = true;
                }
            }
            
            if (fajlba_iras == true)
            {
                // Létrehozzuk a diákokat tartalmazó fájlt és a hozzá tartozó folyamot.
                FileStream diaktomb_iras = create_FileStream(diak_fajl, FileMode.Create, FileAccess.Write, true);
                
                if (diaktomb_iras == null)
                {
                    // Ha hiba történt a kapcsolat létrehozása közben, akkor megjelenítjük azt.

                    System.Console.Write("A diákokat tartalmazó fájl (" + Directory.GetCurrentDirectory() + "\\" + diak_fajl);
                    System.Console.WriteLine(") nem írható.");
                }
                else
                {
                    // Ha nem történt hiba és a fájl írható, akkor beleírjuk a diákokat.

                    // A fejlécbe (első sor) a diákok száma kerül.
                    StreamWriter stream_diaktomb_iras = new StreamWriter(diaktomb_iras);
                    stream_diaktomb_iras.WriteLine(System.Convert.ToString(rekord));

                    // Kiürítjük (a merevlemezre írjuk) a memóriában található buffert.
                    stream_diaktomb_iras.Flush();
                    
                    foreach (Student aktualis_diak in diakok)
                    {
                        // Majd iterációval végiglépkedünk a diakok tömbön
                        // és minden elemet beleírunk a fájlba.

                        // A beíráskor a sorokat a sortörés karakter, az "oszlopokat" pedig ';'
                        // (pontosvessző) választja el, később ezek mentén tudunk olvasni.

                        stream_diaktomb_iras.Write(System.Convert.ToString(aktualis_diak.id) + ";");
                        stream_diaktomb_iras.Write(System.Convert.ToString(aktualis_diak.nev) + ";");
                        stream_diaktomb_iras.Write(System.Convert.ToString(aktualis_diak.ev) + ";");
                        stream_diaktomb_iras.Write(System.Convert.ToString(aktualis_diak.honap) + ";");
                        stream_diaktomb_iras.Write(System.Convert.ToString(aktualis_diak.nap) + ";");
                        stream_diaktomb_iras.Write(System.Convert.ToString(aktualis_diak.matekjegy) + ";");
                        
                        // Beírjuk a sorvége karaktert, majd a fájlrendszerbe küldjük a változtatásokat.
                        stream_diaktomb_iras.WriteLine();
                        stream_diaktomb_iras.Flush();
                    }

                    // Lezárjuk a jelenlegi írási kapcsolatot.
                    stream_diaktomb_iras.Close();
                    diaktomb_iras.Close();
                }
            }

            // Végigmegyünk (iteráljuk) a beolvasott adatokat,
            // meghatározva a feladat megoldásait.
            int jegy_osszeg = 0;
            bool volt_bukas = false, volt_kituno = false;
            uint bukottak_szama = 0, kitunok_szama = 0;
            Student[] bukott_diakok = new Student[rekord];
            Student[] kituno_diakok = new Student[rekord];

            foreach (Student aktualis_diak in diakok)
            {
                // Hozzáadjuk a jegyek összegéhez a jelenlegi matematika jegyet.
                jegy_osszeg += System.Convert.ToInt32(aktualis_diak.matekjegy);

                // Ha a diák kitűnő, vagy aktualis_bukott_diak tanuló, beállítjuk
                // a logikai változót, hogy létezik kitűnő/aktualis_bukott_diak diák
                // és a kitűnőket/bukottakat tároló tömbbe beírjuk a diákot.

                if ( aktualis_diak.matekjegy == 1 )
                {
                    volt_bukas = true;
                    bukott_diakok[bukottak_szama] = aktualis_diak;
                    bukottak_szama++;
                }
                else if ( aktualis_diak.matekjegy == 5 )
                {
                    volt_kituno = true;
                    kituno_diakok[kitunok_szama] = aktualis_diak;
                    kitunok_szama++;
                }
            }

            System.Console.WriteLine();

            // Ha az adatok kiírását kértük, készítünk egy
            // újabb FileStream és StreamWriter objektumot.
            string eredmeny_fajl = "eredmeny.txt";
            FileStream eredmeny_iras = null;
            StreamWriter stream_eredmeny_iras = null;

            if (fajlba_iras == true)
            {
                eredmeny_iras = create_FileStream(eredmeny_fajl, FileMode.Create, FileAccess.Write, true);

                if (eredmeny_iras == null)
                {
                    // Ha hiba történt a fájl létrehozása során, értesítjük a usert.
                    System.Console.Write("A diákokat tartalmazó fájl (" + Directory.GetCurrentDirectory() + "\\" + eredmeny_fajl);
                    System.Console.WriteLine(") nem írható.");

                    // És a fajlba_iras változót hamisra állítjuk, így nem történik későbbi írás.
                    fajlba_iras = false;
                }
                else
                {
                    // Ha sikerült a kapcsolat kiépítése, akkor létrehozzuk az író adatfolyamot.
                    stream_eredmeny_iras = new StreamWriter(eredmeny_iras);
                }
            }

            double atlag = System.Convert.ToDouble(jegy_osszeg) / System.Convert.ToDouble(rekord);
            System.Console.WriteLine("A tanulók érdemjegyeinek átlaga: " + System.Convert.ToString(atlag));
            
            // Ha az eredmények kiírását kértük, akkor elsőnek beírjuk az átlagot.
            if (fajlba_iras == true)
            {
                stream_eredmeny_iras.WriteLine("A tanulók érdemjegyeinek átlaga: " + System.Convert.ToString(atlag));
                stream_eredmeny_iras.Flush();
            }

            // Ha vannak aktualis_bukott_diak diákok, megjelenítjük az adataikat, újbóli iterációval.
            if (volt_bukas)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Megbuktak matematikából (" + System.Convert.ToString(bukottak_szama) + " diák):");

                if (fajlba_iras == true)
                {
                    stream_eredmeny_iras.WriteLine();
                    stream_eredmeny_iras.WriteLine("Megbuktak matematikából (" + System.Convert.ToString(bukottak_szama) + " diák):");
                    stream_eredmeny_iras.Flush();
                }

                foreach (Student aktualis_bukott_diak in bukott_diakok)
                {
                    // Mivel a bukott_diakok tömb tartalmazhat üres elemeket, kell egy védelem.
                    // Így elkerüljük az üres (null) elemek iterációjából adódó hibát.
                    if (aktualis_bukott_diak != null)
                    {
                        string diak_sor = build_diaksor(aktualis_bukott_diak.id, aktualis_bukott_diak.nev, aktualis_bukott_diak.ev, aktualis_bukott_diak.honap, aktualis_bukott_diak.nap);

                        System.Console.Write(diak_sor);

                        // A aktualis_bukott_diak diákokat is beleírjuk az eredmények fájlba.
                        if (fajlba_iras == true) 
                        {
                            stream_eredmeny_iras.Write(diak_sor);
                            stream_eredmeny_iras.Flush();
                        }
                    }
                }
            }

            // Hasonló módon eljárunk a kitűnőkkel is.
            // Ha vannak aktualis_bukott_diak diákok, megjelenítjük az adataikat, újbóli iterációval.
            if (volt_kituno)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Kitűnő eredményt értek el (" + System.Convert.ToString(kitunok_szama) + " diák):");

                if (fajlba_iras == true)
                {
                    stream_eredmeny_iras.WriteLine();
                    stream_eredmeny_iras.WriteLine("Kitűnő eredményt értek el (" + System.Convert.ToString(kitunok_szama) + " diák):");
                    stream_eredmeny_iras.Flush();
                }

                foreach (Student aktualis_kituno_diak in kituno_diakok)
                {
                    if (aktualis_kituno_diak != null)
                    {
                        string diak_sor = build_diaksor(aktualis_kituno_diak.id, aktualis_kituno_diak.nev, aktualis_kituno_diak.ev, aktualis_kituno_diak.honap, aktualis_kituno_diak.nap);

                        System.Console.Write(diak_sor);

                        if (fajlba_iras == true)
                        {
                            stream_eredmeny_iras.Write(diak_sor);
                            stream_eredmeny_iras.Flush();
                        }
                    }
                }
            }

            // Lezárjuk a kapcsolatot az eredmények fájllal.
            if (fajlba_iras == true)
            {
                stream_eredmeny_iras.Close();
                eredmeny_iras.Close();
            }

            // Várunk egy billentyűleütésre a program befejezése előtt.
            System.Console.WriteLine();
            System.Console.Write("A kilépéshéz nyomjon ENTER-t.");
            System.Console.ReadLine();
            Environment.Exit(0);
        }

        private static string build_diaksor(uint id, string nev, uint ev, uint honap, uint nap)
        {
            /* Ez a függvény egy string sort készít az adott diákról.
             * Ezzel a függvénnyel készül a aktualis_bukott_diak és kitűnő lista.
             */

            string diak_sor;

            diak_sor = System.Convert.ToString(id + 1) + ". " + nev;
            diak_sor += " (" + System.Convert.ToString(ev) + ". ";
            diak_sor += System.Convert.ToString(honap) + ". ";
            diak_sor += System.Convert.ToString(nap) + ".)";

            return diak_sor;
        }
    }

    class Student
    {
        // Ez az osztály határoz meg egy diákot.

        // Az alábbi változók standard olvas és ír (getter, setter)
        // meghívásokkal rendelkeznek, ezt használjuk a struktúra kialakítására.
        public uint id { get; set; }
        public string nev { get; set; }
        public uint ev { get; set; }
        public uint honap { get; set; }
        public uint nap { get; set; }
        public uint matekjegy { get; set; }

        // A konstruktor (azonos névű, mint az osztály maga)
        // automatikusan lefut, amikor az osztály inicializálásra kerül.
        // (Tehát a 'Student var = new Student(param1, param2, param3);' sorban.)
        public Student(uint id, string nev, uint ev, uint honap, uint nap, uint matekjegy)
        {
            this.id = id;
            this.nev = nev;
            this.ev = ev;
            this.honap = honap;
            this.nap = nap;
            this.matekjegy = matekjegy;
        }
    }
}
