using System;
using System.Collections.Generic;
using System.Text;

/* FELADATLEÍRÁS
 * -------------
 * Készíts programot, ami max. 50 ember adatait bekéri
 * az elemszámot, nevet, születési dátumot és matematika évvégi jegyet,
 * majd átlagol, meghatározza van-e bukás, kik buktak, van-e ötös, kik kaptak ötöst!
 */

namespace ClassStatistics
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.Title = "Osztálystatisztika";
            
            // Bekérjük a felhasználótól a diákok számát.
            uint rekord = 0; // Rekordok száma
            bool rekFail = true; // Hiba állapota

            while (rekFail)
            {
                // Az itt megadott ciklus addig fut, ameddig hiba van a bekért adatban.

                try
                {
                    // A try{} blokk megpróbálja bekérni a diákok számát, azt konvertálni
                    // majd a hiba változót hamisra állítani (megállítani az egyébként végtelen ciklust).

                    System.Console.Write("Kérem adja meg a diákok számát (0-50): ");
                    rekord = System.Convert.ToUInt16(System.Console.ReadLine());
                    rekFail = false;
                }
                catch (System.FormatException)
                {
                    // Formátumhiba esetén a hibaváltozó visszaáll igazra (a ciklus újra le fog futni)
                    // és értesítjük a felhasználót a vétségéről.

                    rekFail = true;
                    System.Console.WriteLine("A megadott szám nem egész szám.");
                    System.Console.WriteLine();
                }
                catch (System.OverflowException)
                {
                    // Hasonló módon járunk el túlcsordulás (tartományhiba) esetén is.

                    rekFail = true;
                    System.Console.WriteLine("A megadott szám kívül esik a megengedett (0-50) tartományon.");
                    System.Console.WriteLine();
                }
                finally
                {
                    // A finally{} blokk lefut akkor is ha volt hiba és akkor is ha nem.
                    // Itt érvényesítjük a feladat szempontjából szükséges feltételt: max. 50 rekord lehetséges.

                    if (rekord < 0 || rekord > 50)
                    {
                        rekFail = true;
                        System.Console.WriteLine("A megadott szám kívül esik a megengedett (0-50) tartományon.");
                        System.Console.WriteLine();
                    }
                }
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
                bool rekordFail = true;

                while (rekordFail)
                {
                    // A már ismert struktúrát használva itt is addig
                    // kérünk be adatokat a felhasználótól, amíg azok megfelelőek.

                    try
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine(System.Convert.ToString(i+1) + ". rekord:");

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

                        rekordFail = false;

                        // Itt elvégzünk egy hibakezelést, amely a 'matekjegy' nem eleme [0; 5] Z+
                        // intervallum hibáját hivatott megelőzni.
                        // Mivel nincs eldobott hiba, nincs is mit elkapni: egy normál if blokkal fogunk dolgozni.
                        if (matekjegy <= 0 || matekjegy > 5)
                        {
                            matekjegy = 0;
                            rekordFail = true;
                            System.Console.WriteLine("A matematika osztályzat érvénytelen.");
                            System.Console.WriteLine();
                        }
                    }
                    catch (System.FormatException)
                    {
                        // Formátumhiba esetén a hibaváltozó visszaáll igazra (a ciklus újra le fog futni)
                        // és értesítjük a felhasználót a vétségéről.

                        rekordFail = true;
                        System.Console.WriteLine("A megadott szám nem egész szám.");
                        System.Console.WriteLine();
                    }
                    catch (System.OverflowException)
                    {
                        // Hasonló módon járunk el túlcsordulás (tartományhiba) esetén is.

                        rekordFail = true;
                        System.Console.WriteLine("A megadott szám kívül esik a megengedett tartományon.");
                        System.Console.WriteLine();
                    }
                }

                // A while blokkon kívül már minden adat megfelelő, tehát letároljuk a diákot.
                diakok[i] = new Student(i, nev, ev, honap, nap, matekjegy);

                System.Console.WriteLine("Rekord #" + System.Convert.ToString(i + 1) + " sikeresen tárolva.");
            }

            // Végigmegyünk (iteráljuk) a beolvasott adatokat,
            // meghatározva a feladat megoldásait.
            int jegyOsszeg = 0;
            bool bukas = false, kituno = false;
            uint bukott_szam = 0, kituno_szam = 0;
            Student[] bukottak = new Student[rekord];
            Student[] kitunok = new Student[rekord];

            foreach (Student diak in diakok)
            {
                // Hozzáadjuk a jegyek összegéhez a jelenlegi matematika jegyet.
                jegyOsszeg += System.Convert.ToInt32(diak.matekjegy);

                // Ha a diák kitűnő, vagy bukott tanuló, beállítjuk
                // a logikai változót, hogy létezik kitűnő/bukott diák
                // és a kitűnőket/bukottakat tároló tömbbe beírjuk a diákot.

                if ( diak.matekjegy == 1 )
                {
                    bukas = true;
                    bukottak[bukott_szam] = diak;
                    bukott_szam++;
                }
                else if ( diak.matekjegy == 5 )
                {
                    kituno = true;
                    kitunok[kituno_szam] = diak;
                    kituno_szam++;
                }
            }

            System.Console.WriteLine();

            double atlag = System.Convert.ToDouble(jegyOsszeg) / System.Convert.ToDouble(rekord);
            System.Console.WriteLine("A tanulók érdemjegyeinek átlaga: " + System.Convert.ToString(atlag));

            // Ha vannak bukott diákok, megjelenítjük az adataikat, újbóli iterációval.
            if (bukas)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Megbuktak matematikából (" + System.Convert.ToString(bukott_szam) + " diák):");

                foreach (Student bukott in bukottak)
                {
                    // Mivel a bukottak tömb tartalmazhat üres elemeket, kell egy védelem.
                    // Így elkerüljük az üres (null) elemek iterációjából adódó hibát.
                    if (bukott != null)
                    {
                        System.Console.Write(System.Convert.ToString(bukott.id + 1) + ". " + bukott.nev);
                        System.Console.Write(" (" + System.Convert.ToString(bukott.ev) + ". ");
                        System.Console.Write(System.Convert.ToString(bukott.honap) + ". ");
                        System.Console.WriteLine(System.Convert.ToString(bukott.nap) + ".)");
                    }
                }
            }

            // Hasonló módon eljárunk a kitűnőkkel is.
            // Ha vannak bukott diákok, megjelenítjük az adataikat, újbóli iterációval.
            if (kituno)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Kitűnő eredményt értek el (" + System.Convert.ToString(kituno_szam) + " diák):");

                foreach (Student kituno_diak in kitunok)
                {
                    if (kituno_diak != null)
                    {
                        System.Console.Write(System.Convert.ToString(kituno_diak.id + 1) + ". " + kituno_diak.nev);
                        System.Console.Write(" (" + System.Convert.ToString(kituno_diak.ev) + ". ");
                        System.Console.Write(System.Convert.ToString(kituno_diak.honap) + ". ");
                        System.Console.WriteLine(System.Convert.ToString(kituno_diak.nap) + ".)");
                    }
                }
            }
            
            // Várunk egy billentyűleütésre a program befejezése előtt.
            System.Console.WriteLine();
            System.Console.Write("A kilépéshéz nyomjon ENTER-t.");
            System.Console.ReadLine();
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
