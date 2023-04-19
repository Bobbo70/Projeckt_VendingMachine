using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automat
{
    public class Wallet
    {
        private readonly int[] _amounts = new int[Denominations.Length];
        public static readonly int[] Denominations = { 1000, 500, 200, 100, 50, 20, 10, 5, 1 };

        public int AmountAvailable
        {
            get
            {
                var amount = 0;
                for (var i = 0; i < Denominations.Length; i++)
                    amount += _amounts[i] * Denominations[i];
                return amount;
            }
        }

        public int Count(int denomination)
        {
            var i = Array.IndexOf(Denominations, denomination);
            return i < 0 ? 0 : _amounts[i];
        }

        public void PutIn(int denomination, int count)
        {
            var i = Array.IndexOf(Denominations, denomination);
            if (i < 0 || count < 0)
                return;
            _amounts[i] += count;
        }

        public void PutIn(int amount)
        {
            foreach (var denomination in Denominations)
            {
                PutIn(denomination, amount / denomination);
                amount %= denomination;
            }
        }

        public void TakeOut(int denomination, int count)
        {
            var i = Array.IndexOf(Denominations, denomination);
            if (i < 0 || count > Count(denomination))
                throw new InvalidOperationException();
            _amounts[i] -= count;
        }
    }
}
