using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/* A lifthasználati igényeket az igeny.txt állomány tartalmazza.
 * 
 * A programhoz mellékelésre került az adatokat
 * tartalmazó igeny.txt forrásfájl, amely fordításkor
 * automatikusan a kimeneti mappába kerül.
 */

namespace Lift
{
    class Program
    {
        // Az igeny adatsor tartalmazza egy darab igénynek a tulajdonságait,
        // később ezt olvassuk be a fájlból. A változónevek egyértelműek.
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
            igeny[] igenyek;
            short csapatok;
            IgenyekOlvas(out igenyek, out csapatok);

            // MÁSODIK RÉSZFELADAT
            short lift_kezdopont;
            LiftKezdopont(out lift_kezdopont);

            // HARMADIK RÉSZFELADAT
            UtolsoEmelet(ref igenyek);

            // NEGYEDIK RÉSZFELADAT
            SzintIntervallum(ref igenyek);
            
            // ÖTÖDIK RÉSZFELADAT
            FelfeleIndulasok(ref igenyek);

            // HATODIK RÉSZFELADAT
            List<int> csapatok_lista;
            NemUtazott(ref igenyek, csapatok, out csapatok_lista);
            
            // HETEDIK RÉSZFELADAT
            List<igeny> vizsgalt_csapat_utjai;
            int vizsgalt_csapat;
            Szabalytalan(ref igenyek, csapatok, ref csapatok_lista, out vizsgalt_csapat_utjai, out vizsgalt_csapat);
            
            // NYOLCADIK RÉSZFELADAT
            BlokkoloKartya(vizsgalt_csapat, ref vizsgalt_csapat_utjai);
            
            System.Console.WriteLine("A kilépéshez nyomjon ENTER-t...");
            System.Console.ReadLine();
            Environment.Exit(0);
        }

        static FileStream IgenyFajlMegnyit()
        {
            // Beolvassuk a fájlt (hiba esetén megállunk és kilépünk) és
            // a létrehozott változókba eltároljuk az értékeket.
            FileStream igeny_txt = null;
            try
            {
                // Megpróbáljuk létrehozni a fájlkapcsolatot a forrásfájlhoz.
                igeny_txt = new FileStream("igeny.txt", FileMode.Open, FileAccess.Read);
            }
            catch (System.IO.IOException)
            {
                // Ha a fenti, try{} blokkban található kód az itt megjelölt hibát adja, akkor
                // ezt "elkapjuk" (catch{}) és lefuttatjuk az alábbi kódot: értesítjük
                // a felhasználót a hibáról, majd, mivel a program nem folytatható, megszakítjuk azt.
                System.Console.WriteLine("Nem sikerült az igeny.txt megnyitása.");
                System.Console.WriteLine("Kérem ellenőrizze, hogy a forrásfájl a futtatható állománnyal egy mappában van-e!");
                System.Console.WriteLine("A kilépéshez nyomjon ENTER-t...");
                System.Console.ReadLine();
                Environment.Exit(1);
            }

            return igeny_txt;
        }

        static void IgenyekOlvas(out igeny[] igenyek, out short csapatok)
        {
            /* ELSŐ RÉSZFELADAT
             * ----------------
             * Olvassa be az igeny.txt állományban talált adatokat, s azok
             * felhasználásával oldja meg a következő feladatokat!
             */

            FileStream igeny_txt = IgenyFajlMegnyit();

            // A StreamReader objektum olvasni fogja a létrejött fájlkapcsolatot.
            StreamReader txt = new StreamReader(igeny_txt);

            // A feladatnak megfelelően beolvassuk a fejléc adatokat:
            // Első sorában a szintek száma (legfeljebb 100), a második
            // sorban a csapatok száma (legfeljebb 50), a harmadik sorban
            // pedig az igények száma (legfeljebb 100) olvasható.
            short emeletek = System.Convert.ToInt16(txt.ReadLine());
            csapatok = System.Convert.ToInt16(txt.ReadLine());
            short igenyek_szam = System.Convert.ToInt16(txt.ReadLine());

            // Létrehozunk egy, igeny típusú adatsorokat tartalmazó tömböt.
            igenyek = new igeny[igenyek_szam];

            // Majd egyesével végiglépkedve beolvasunk minden további sort a fájl végéig.
            // (Jelen pillanatban a StreamReader "mutatója" a negyedik sor elején áll.)
            // A negyedik sortól kezdve soronként egy-egy igény szerepel a jelzés sorrendjében.
            for (int i = 0; i < igenyek_szam; i++)
            {
                // Egy igény hat számból áll: az első három szám az időt adja
                // meg (óra, perc, másodpercszám sorrendben), a negyedik a csapat
                // sorszáma, az ötödik az induló-, a hatodik a célszint sorszáma.
                // Az egyes számokat pontosan egy szóköz választja el egymástól.

                // Minden egész sort beolvassuk és utána a szóközök mentén felbontva
                // feltöltjük az igényeket tartalmazó tömböt.
                string sor = txt.ReadLine();
                string[] elemek = sor.Split(' ');

                igenyek[i].ora = System.Convert.ToInt16(elemek[0]);
                igenyek[i].perc = System.Convert.ToInt16(elemek[1]);
                igenyek[i].masodperc = System.Convert.ToInt16(elemek[2]);
                igenyek[i].csapat = System.Convert.ToInt16(elemek[3]);
                igenyek[i].honnan = System.Convert.ToInt16(elemek[4]);
                igenyek[i].hova = System.Convert.ToInt16(elemek[5]);
            }

            // Lezárjuk az olvasó kapcsolatot az igeny.txt fájlhoz.
            txt.Close();
            igeny_txt.Close();
        }

        static void LiftKezdopont(out short lift_kezdopont)
        {
            /* MÁSODIK RÉSZFELADAT
             * -------------------
             * Tudjuk, hogy a megfigyelés kezdetén a lift éppen áll.
             * Kérje be a felhasználótól, hogy melyik szinten áll a lift, és
             * a további részfeladatok megoldásánál ezt vegye figyelembe!
             */
            System.Console.Write("2. feladat: Melyik szinten áll a lift az induláskor? ");
            lift_kezdopont = System.Convert.ToInt16(System.Console.ReadLine());
        }

        static void UtolsoEmelet(ref igeny[] igenyek)
        {
            /* HARMADIK RÉSZFELADAT
             * --------------------
             * Határozza meg, hogy melyik szinten áll majd a lift az utolsó kérés teljesítését követően!
             */
            // A lift értelemszerűen az utolsó igény érkezési pontján fog állni.
            // A igenyek.GetUpperBound(0) megadja a legutolsó igény azonosítószámát a tömbben,
            // majd mi kiolvassuk az igények tömb ezen elemének a "hova" változójából a választ.
            System.Console.Write("3. feladat: A lift az utolsó igény teljesítése után a(z) ");
            System.Console.WriteLine(igenyek[igenyek.GetUpperBound(0)].hova + ". emeleten áll meg.");
        }

        static void SzintIntervallum(ref igeny[] igenyek)
        {
            /* NEGYEDIK RÉSZFELADAT
             * --------------------
             * Írja a képernyőre, hogy a megfigyelés kezdete és az
             * utolsó igény teljesítése között melyik volt a legalacsonyabb
             * és melyik a legmagasabb sorszámú szint, amelyet a lift érintett!
             */
            // Létrehozunk egy üres tömböt, amely kétszer akkora, mint amennyi igény van.
            int[] emelet_lista = new int[(int)igenyek.Length * 2];
            int i = 0;
            int em_i = 0;
            while (i < igenyek.Length)
            {
                // Amíg vannak igények, minden indulási és érkezési értéket átemelünk ebbe a tömbbe.

                // A j minden elem után nő egyet (ez az index az igényekhez),
                // az em_i pedig az emelet_lista tömbben való index, amely j-nként kettőt nő
                // (hiszen minden igény két emelet értéket ad meg).

                // A második sorban mindenképpen ++em_i -t használunk, hogy
                // az em_i előbb nőjjön 1-gyel, minthogy azt kulcsként
                // használnánk. Tehát ha az em_i 2 volt, akkor a
                // "honnan" érték a 2. helyre íródik, az em_i 1-gyel nő, és így
                // a 3. helyre írja a "hova" értéket.
                // (Vesd össze: em_i++ esetén a kettő lenne az index és csak a sor UTÁN nőne 1-gyel.)
                emelet_lista[em_i] = igenyek[i].honnan;
                emelet_lista[++em_i] = igenyek[i].hova;

                i++;
                em_i++;
            }

            // Végigmegyünk az összes emelet listáján, közben maximum- és minimumkeresést végezve.
            int maximum = emelet_lista[0];
            int minimum = emelet_lista[0];

            for (int j = 0; j < emelet_lista.Length; j++)
            {
                if (emelet_lista[j] > maximum)
                {
                    maximum = emelet_lista[j];
                }

                if (emelet_lista[j] < minimum)
                {
                    minimum = emelet_lista[j];
                }
            }

            System.Console.Write("4. feladat: A lift a(z) " + System.Convert.ToString(minimum) + ". és a(z) ");
            System.Console.WriteLine(System.Convert.ToString(maximum) + ". szintek között mozgott.");
        }

        static void FelfeleIndulasok(ref igeny[] igenyek)
        {
            /* ÖTÖDIK RÉSZFELADAT
             * ------------------
             * Határozza meg, hogy hányszor kellett a liftnek felfelé indulnia
             * utassal és hányszor utas nélkül! Az eredményt jelenítse meg a képernyőn!
             */
            // Ha a lift célállomása az adott igényen belül magasabban van,
            // mint az induló állomás, akkor az egy út felfelé utassal.
            // Azonban, ha a jelenlegi célállomás a következő igény
            // induló állomása alatt van, akkor a liftnek felelé kell mozognia,
            // tehát az egy felfelé út lesz utas nélkül.
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
        }

        static void NemUtazott(ref igeny[] igenyek, short csapatok, out List<int> csapatok_lista)
        {
            /* HATODIK RÉSZFELADAT
             * -------------------
             * Határozza meg, hogy mely szerelőcsapatok nem vették igénybe
             * a liftet a vizsgált intervallumban! A szerelőcsapatok sorszámát
             * egymástól egy-egy szóközzel elválasztva írja a képernyőre!
             */

            // Készítünk egy listát, amely csapatok darabszámú elemet tartalmazhat.
            csapatok_lista = new List<int>(csapatok);

            // Ezt a listát feltöltjük az összes, forrásfájl szerint létező
            // csapat számával (1-től csapatszámig).
            for (int i = 1; i <= csapatok; i++)
            {
                csapatok_lista.Add(i);
            }

            // Iterálva (végiglépkedve) az igényeket tartalmazó tömböt,
            // kitöröljük azokat a csapatokat, amelyekhez találtunk igényt.
            for (int j = 0; j < igenyek.Length; j++)
            {
                csapatok_lista.Remove(igenyek[j].csapat);
            }

            // A listát rendezzük, majd a képernyőre írjuk a megoldást.
            // (A List.ToArray() metódus egy, a lista elemével azonos
            // típusú tömböt (tehát List<int> esetén int[]-t) készít,
            // amely már iterálható a foreach{} blokkal.)
            csapatok_lista.Sort();
            System.Console.Write("6. feladat: A következő csapatok nem használták a liftet: ");
            foreach (int nem_utazott_csapat_id in csapatok_lista.ToArray())
            {
                System.Console.Write(System.Convert.ToString(nem_utazott_csapat_id) + ' ');
            }
            System.Console.WriteLine();
        }

        static void Szabalytalan(ref igeny[] igenyek, short csapatok, ref List<int> csapatok_lista, out List<igeny> vizsgalt_csapat_utjai, out int vizsgalt_csapat)
        {
            /* HETEDIK RÉSZFELADAT
             * -------------------
             * Előfordul, hogy egyik vagy másik szerelőcsapat áthágja
             * a szabályokat, és egyik szintről gyalog megy a másikra.
             * (Ezt onnan tudhatjuk, hogy más emeleten igényli a liftet,
             * mint ahova korábban érkezett.) Generáljon véletlenszerűen egy
             * létező csapatsorszámot! (Ha nem jár sikerrel, dolgozzon a 3. csapattal!)
             * Határozza meg, hogy a vizsgált időszak igényei alapján lehet-e egyértelműen
             * bizonyítani, hogy ez a csapat vétett a szabályok ellen! Ha igen, akkor adja
             * meg, hogy melyik két szint közötti utat tették meg gyalog, ellenkező
             * esetben írja ki a "Nem bizonyítható szabálytalanság" szöveget!
             */
            // Készítünk egy véletlengenerátor objektumot, ez fogja majd elkészíteni a számunkat.
            Random veletlen_generator = new Random();
            vizsgalt_csapat = 0;

            // A hatodik feladatban elkészült egy lista, amely tartalmazza
            // azokat a csapatokat, amelyek NEM vették igénybe a liftet (csapatok_lista).
            // Az alábbi while ciklusban generálunk egy véletlenszerű csapat sorszámot.
            // Ha ez a generált szám egy olyan csapatra mutat, amelyik nem vette igénybe
            // a liftet, akkor újra generálunk egy másikat, és így tovább, amíg létező
            // csapatot azonosító sorszámot kapunk.
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

            // Készítünk egy tömböt, amelyet feltöltünk a kiválasztott csapat összes igényével.
            vizsgalt_csapat_utjai = new List<igeny>(igenyek.Length);
            for (int i = 0; i < igenyek.Length; i++)
            {
                if (igenyek[i].csapat == vizsgalt_csapat)
                {
                    vizsgalt_csapat_utjai.Add(igenyek[i]);
                }
            }

            // Megvizsgáljuk, hogy volt-e szabálytalanság:
            // egyesével végiglépkedünk a feltöltött listán.
            // A feladat szövege szerint akkor történt szabálytalanság, ha: 
            // "[a csapat] más emeleten igényli a liftet, mint ahova korábban érkezett."
            // Tehát ha bármely igény 'honnan'-ja eltér az előző 'hova'-jától,
            // akkor szabálytalanok voltak, és ezt regisztráljuk.
            bool volt_szabalytalansag = false;
            int szabalytalan_honnan = -1;
            int szabalytalan_hova = -1;

            for (int j = 1; j < vizsgalt_csapat_utjai.Count; j++)
            {
                if (vizsgalt_csapat_utjai[j].honnan != vizsgalt_csapat_utjai[j - 1].hova)
                {
                    volt_szabalytalansag = true;

                    szabalytalan_honnan = vizsgalt_csapat_utjai[j - 1].hova;
                    szabalytalan_hova = vizsgalt_csapat_utjai[j].honnan;
                }
            }

            // A vizsgálat eredményét kiírjuk a képernyőre.
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
        }

        static FileStream BlokkolFajlMegnyit()
        {
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

            return blokkol_txt;
        }
        
        static void BlokkoloKartya(int vizsgalt_csapat, ref List<igeny> vizsgalt_csapat_utjai)
        {
            /* NYOLCADIK RÉSZFELADAT
             * ---------------------
             * A munkák elvégzésének adminisztrálásához minden csapatnak
             * egy blokkoló kártyát kell használnia. A kártyára a liftben elhelyezett
             * blokkolóóra rögzíti az emeletet, az időpontot. Ennek a készüléknek a
             * segítségével kell megadni a munka kódszámát és az adott munkafolyamat
             * sikerességét. A munka kódja 1 és 99 közötti egész szám lehet. A sikerességet a
             * „befejezett” és a „befejezetlen” szavakkal lehet jelezni.
             * Egy műszaki hiba folytán az előző feladatban vizsgált csapat kártyájára az általunk nyomon
             * követett időszakban nem került bejegyzés. Ezért a csapatfőnöknek a műszak végén
             * pótolnia kell a hiányzó adatokat. Az igeny.txt állomány adatait felhasználva írja a képernyőre
             * időrendben, hogy a vizsgált időszakban milyen kérdéseket tett fel az óra, és kérje
             * be az adott válaszokat a felhasználótól! A pótlólag feljegyzett adatokat írja a blokkol.txt állományba!
             */
            System.Console.WriteLine("8. feladat: Hiba történt a munkák regisztrálása során.");
            System.Console.WriteLine("Kérem a következő csapat munkaadatait a műszakvizsga feltöltéséhez: " + System.Convert.ToString(vizsgalt_csapat) + "!");

            // Létrehozzuk a blokkol.txt fájlt. Ha hiba történik, akkor kilépünk a programból.
            // Továbbá létrehozunk egy, a fájlba adatfolyamot író objektumot is.
            FileStream blokkol_txt = BlokkolFajlMegnyit();

            StreamWriter blokkol_writer = new StreamWriter(blokkol_txt);

            for (int q = 0; q < vizsgalt_csapat_utjai.Count; q++)
            {
                // Egyesével végiglépkedve a csapat útjain elvégezzük a feladatot:
                // Megjelenítjük az út adatait és bekérjük a válaszokat.
                int munkakod = -1;
                bool sikeres = false;

                System.Console.WriteLine("-----");
                System.Console.WriteLine("Indulási emelet: " + System.Convert.ToString(vizsgalt_csapat_utjai[q].honnan));
                System.Console.WriteLine("Célemelet: " + System.Convert.ToString(vizsgalt_csapat_utjai[q].hova));

                // Ez a while ciklus addig kéri a felhasználótól a választ, amíg az
                // a feladatnak (1-99 közé eső) és a formátumnak (egész szám) megfelelő választ ad.
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

                // Hasonló módon itt is addig ismételjük a bekérést, amíg jó választ nem kapunk.
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

                // A fájlba is kiírjuk az igény adatait, valamint a bekért válaszokat.
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

            // Zárjuk az adatfolyamot, a fájlt, majd kilépünk a programból.
            blokkol_writer.Close();
            blokkol_txt.Close();
            System.Console.WriteLine();
        }
    }
}