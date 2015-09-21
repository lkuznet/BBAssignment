using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatalogManager.Domain
{
    public class Money
    {
        public decimal Amount { get; private set; }
        public Currency Currency { get; private set; }

        public Money(decimal amount, Currency currency)
        {
            if (amount < 0 || amount.ToString().Length > 10)
                throw new ArgumentException("Money Amount must be positive number and cannot have more than 10 digits.");

            this.Amount = Math.Round(amount, 2);
            this.Currency = currency;
        }
        public override string ToString()
        {
            return Amount.ToString("c") + " " + Currency.ToString();
        }
    }
}
