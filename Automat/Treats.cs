using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automat
{
    public class Treats : Product
    {
        public Treats(string name, string description, int price) : base(name, description, price)
        {            
        }
        public override void Use()
        {
            Console.WriteLine();
            Console.WriteLine($"   Sååå gott det är att äta {Name} mmmmm ");
            Console.WriteLine();
        }
    }
}