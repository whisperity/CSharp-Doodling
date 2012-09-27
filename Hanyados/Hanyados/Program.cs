using System;
using System.Collections.Generic;
using System.Text;

/* FELADATLEÍRÁS
 * -------------
 * Írjon programot, amely bekér két egész számot,
 * majd megjeleníti a hányadosukat és maradékos osztási eredményüket.
*/

namespace Hanyados
{
    class Program
    {
        static void Main(string[] args)
        {
            // Változók deklarálása.
            int a = 0;
            int b = 0;
            int hanyados = 0;
            int maradek = 0;
            bool aFail = true;
            bool bFail = true;

            // A while(){} blokk addig fog futni, amíg
            // az a változó bekért értéke megfelelő.
            while (aFail)
            {
                System.Console.Write("(int) a:= ");
                try
                {
                    // Megpróbáljuk beolvasni és konvertálni a változót.

                    a = System.Convert.ToInt16(System.Console.ReadLine());
                    aFail = false;
                }
                catch (FormatException)
                {
                    // Ha hiba történik, megjelenítjük a hibát,
                    // majd a felhasználót újbóli adatmegadásra szólítjuk:
                    // a while blokk még egyszer lefut.

                    System.Console.WriteLine("?FORMAT ERROR");
                    System.Console.WriteLine("READY.");
                    aFail = true;
                }
            }
            
            while (bFail)
            {
                System.Console.Write("(int) b:= ");
                try
                {
                    b = System.Convert.ToInt16(System.Console.ReadLine());
                    bFail = false;
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("?FORMAT ERROR");
                    System.Console.WriteLine("READY.");
                    bFail = true;
                }
            }
            
            // Kiszámoljuk és megjelenítjük az értékeket.
            hanyados = a / b;
            maradek = a % b;
            System.Console.Write(System.Convert.ToString(a) + " osztva " + System.Convert.ToString(b) + "-vel: ");
            System.Console.Write(System.Convert.ToString(hanyados));
            System.Console.Write(", maradék: ");
            System.Console.WriteLine(System.Convert.ToString(maradek));

            double tizedestort = System.Convert.ToDouble(a) / System.Convert.ToDouble(b);
            System.Console.WriteLine("Tizedestört alak: " + System.Convert.ToString(tizedestort));

            System.Console.ReadLine();
        }
    }
}