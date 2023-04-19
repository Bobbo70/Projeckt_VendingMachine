using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Linq;

namespace Automat
{   
    public class AutomatItem
    {
        private Wallet? _wallet;

        public List<Product> Products { get; set; } = new();

        //Produkt meny
        public AutomatItem()
        {
            Products.AddRange(new Product[]
            {
                new Food(name: "Baguette", description: "Räkor och ägg", 55),
                new Food(name: "Sallad", description: "Kyckling", 85),
                new Food(name: "Fralla", description: "Ost och skinka", 18),
                new Food(name: "Sandwich", description: "Kyckling och bacon", 49),
            
                new Treats(name: "Kechoklad", description: "Kex överdragen med choklad", 10),
                new Treats(name: "Chips", description: "Salt", 10),
                new Treats(name: "Gott o Blandat", description: "Vingummi i olika smaker", 10),
                new Treats(name: "Mjölkchoklad", description: "Klassisk chokladkaka", 10),

                new Drink(name: "Coca Cola", description: "Haypad dryck, 50 cl", 20),
                new Drink(name: "Energidryck", description: "Red Bull, för dem som behöver en extra kick..", 20),
                new Drink(name: "Mineralvatten", description: "Bubbelvatten med citron 50 cl", 15),
                new Drink(name: "Juice", description: "Mer med päronsmak 50 cl", 15),
            });
            return;
        }               

        //Automat meny
        public void Menu(Wallet money)
        {
            _wallet = money;
            while (true)
            {
                Console.Clear();
                ShowProductsMenu();

                int yourchoice = ReadChoice("   Ditt Val: ");

                if (yourchoice == Products.Count + 1)
                    return;

                if (yourchoice <= 0 || yourchoice > Products.Count)
                {
                    Console.WriteLine("   Ett ogiltigt val! Tryck på valfri tagent för att göra ett nytt försök");
                    Console.ReadKey(true);
                    continue;
                }
                Product product = Products[yourchoice -1];
                Process(product);
            }
        }
        //Visar automatmeny
        void ShowProductsMenu()
        {
            Console.WriteLine("-----------------------");
            Console.WriteLine("     AUTOMATMENY");
            Console.WriteLine("-----------------------");
            for (int i = 0; i < Products.Count; i++)
                Console.WriteLine($"   {i + 1}. {Products[i].Name}");
            Console.WriteLine("########################");
            Console.WriteLine($"   {Products.Count + 1}. Avsluta");
            Console.WriteLine();
        }

        //Behållare för meddelande i menyn
        static int ReadChoice(string message)
        {
            Console.Write(message);
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int number))
                return -1;
            return number;
        }

        //Metod som visar vilken produkt som är köpt
        void Process(Product item)
        {
            ShowProductPage(item);
            if (!ConfirmBuy())
                return;

            Buy(item);

            item.Use();
            item.Buy();

            Console.WriteLine("   Tryck på valfri tagent för att fortsätta!");            
            Console.ReadKey(true);
        }
        
        //Metod för innehåll av produkten
        static void ShowProductPage(Product item)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"   {item.Name.ToUpper()}");
            Console.WriteLine("#########################");
            Console.WriteLine($"   Innehåll: {item.Description}");
            Console.WriteLine($"   Pris: {item.Price}kr");
            Console.WriteLine();
        }

        //Kassa för att köpa eller gå ändra produkt
        static bool ConfirmBuy()
        {
            Console.WriteLine("   1. Köpa");
            Console.WriteLine("   2. Tillbaks till butik");
            Console.WriteLine();
            int choice = ReadChoice("   Ditt val: ");
            if (choice == 1)
                return true;
            if (choice != 2)
            {
                Console.WriteLine("   Ett ogiltigt val! Tryck på valfri tagent för att göra ett nytt försök!");
                Console.ReadKey(true);
            }
            return false;
        }

        //Betala för sin produkt
        void Buy(Product item)
        {
            int userPaid = SetInMoney();
            int amount = userPaid - item.Price;

            while (item.Price > userPaid)
            {
                Console.WriteLine($"   Du har för lite pengar {-amount} kr");
                if (!ConfirmContinuePaying())
                {
                    _wallet?.PutIn(userPaid);
                    return;
                }
                userPaid += SetInMoney(userPaid);
            }
            ReturnMoney(amount);
            ShowMessage("   Köpet gick igenom. Tack");
            Console.Clear();
        }

        //Insättning i ATM 
        int SetInMoney(int amount = 0)
        {
            while (true)
            {
                ShowMoneyMenu(amount);

                int denomination = ReadChoice("   Valörer: ");
                if (denomination == -1)                    
                    break;

                if (Array.IndexOf(Wallet.Denominations, denomination) < 0)
                {
                    ShowMessage("   Ogiltig valör");
                    continue;
                }

                int count = ReadChoice("   Hur många valörer? ");

                if (count < 0)
                {
                    ShowMessage("   Ogiltigt belopp");
                    continue;
                }

                if (count > _wallet?.Count(denomination))
                {
                    ShowMessage("   Du har inte tillräckligt");
                    continue;
                }                

                _wallet?.TakeOut(denomination, count);
                amount += denomination * count;
            }
            return amount;
        }       

        //Växel återbetalas till kund
        void ReturnMoney(int amount)
        {
            if (amount > 0)
                Console.WriteLine($"   Du fick {amount}kr tillbaks");            

            foreach (int denomination in Wallet.Denominations)
            {
                _wallet?.PutIn(denomination, amount / denomination);
                amount %= denomination;
            }
        }

        //Viasr din plånbok
        static void ShowMoneyMenu(int amount)
        {
            Console.Clear();
            Console.WriteLine("   Sätta in pengar");
            Console.WriteLine();
            Console.WriteLine($"   Total: {amount}kr");            
        }

        //Metod för att insättnig ja eller nej
        static bool ConfirmContinuePaying()
        {
            Console.Write("   Vill du sätta in mera? [y/n] ");
            while (true)
            {
                var answer = Console.ReadKey(true).KeyChar;
                if (!"yn".Contains(answer))
                    continue;

                Console.WriteLine(answer);
                return answer == 'y';
            }
        }

        //Meddelande när köpet är klart och kunden använder sin produkt
        private static void ShowMessage(string message)
        {
            Console.WriteLine($"{message}.  Tryck på valfri tagent för att fortsätta");
            Console.ReadKey(true);
        }
    }
}