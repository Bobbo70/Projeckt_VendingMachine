using System.ComponentModel;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace Automat
{
    internal class Program
    {
        static void Main()
        {            
            {
                Console.WriteLine();
                Console.WriteLine("   Välkommen till min Automat. Hoppas du hittar något du gillar :)");
                Console.WriteLine("   Tryck enter för att öppna Automaten");
                Console.ReadLine();
            }
            AutomatItem automat = new();           
            Wallet wallet = InitWallet();
            Console.WriteLine(wallet.AmountAvailable);
            automat.Menu(wallet);
        }

        //Metod för hur många valörer av varje sort som sätts in i maskinen
        static Wallet InitWallet()
        {
            Wallet money = new();
            money.PutIn(1, 10);
            money.PutIn(5, 10);
            money.PutIn(10, 10);
            money.PutIn(20, 10);
            money.PutIn(50, 10);
            money.PutIn(100, 100);
            return money;
        }
    }
}