using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automat
{
    public abstract class Product : IProduct
    {
        public string Name { get; }
        public string Description { get; }
        public int Price { get; }

        //Constructor
        public Product (string name, string description, int price)
        {
            Name = name;
            Description = description;
            Price = price;
        }

        public abstract void Use();
        
        public virtual void Buy()
        {
           Console.WriteLine($"   Du köpte en, {Name} för {Price} kr. Tack för att " +
                $"du köpte en produkt av mig. Ha en fin dag");
        }
    }
}