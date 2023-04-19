using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automat
{
    public interface IProduct
    {
        string Name { get; }
        string Description { get; }
        int Price { get; }
        void Use();
        void Buy();
    }
}