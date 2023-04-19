using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Automat
{
    public class Food : Product
    {
        public Food (string name, string description, int price) : base(name, description, price)
        {            
        }        
        public override void Use()
        {
            Console.WriteLine();
            Console.WriteLine($"   Äter den goda {Name} nå ");
            Console.WriteLine();
        }        
    }
}