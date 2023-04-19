using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automat
{
    public class Drink : Product
    {
        public Drink(string name, string description, int price) : base(name, description, price)
        {
        }
        public override void Use()
        {
            Console.WriteLine();
            Console.WriteLine($"   Är så törstig, dricker min {Name} ");
            Console.WriteLine();
        }
    }
}