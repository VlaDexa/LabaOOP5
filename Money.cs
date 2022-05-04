using System;
using System.Windows;

namespace LabaOOP5
{
    public enum CurrencyType
    {
        RUR,
        UZS,
        USD
    }

    internal class Money : IComparable<Money>
    {
        public decimal Amount { get; private set; }
        public CurrencyType Type { get; private set; }
        const decimal Comission = 0.01m;

        public Money(decimal amount, CurrencyType type)
        {
            Amount = amount;
            Type = type;
        }

        public Money TransitionToAnotherType(CurrencyType newType)
        {

            var amount = Amount * Type.GetExchangeRate();
            amount /= newType.GetExchangeRate();
            return new Money(amount, newType);
        }

        public void GetComission()
        {
            Amount *= 1 - Comission;
        }

        public int CompareTo(Money? other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            return Amount.CompareTo(other.TransitionToAnotherType(Type).Amount);
        }

        public static bool operator >(Money money1, Money money2)
        {
            return money1.CompareTo(money2) > 0;
        }

        public static bool operator <(Money money1, Money money2)
        {
            return money1.CompareTo(money2) < 0;
        }

        public static Money operator +(Money a, Money b)
        {
            if (a.Type != b.Type)
            {
                var newB = b.TransitionToAnotherType(a.Type);
                return a + newB;
            }
            else
                return new Money(a.Amount + b.Amount, a.Type);
        }

        public static Money operator -(Money a, Money b)
        {
            if (a.Type != b.Type)
            {
                var newB = b.TransitionToAnotherType(a.Type);
                return a - newB;
            }
            return new Money(a.Amount - b.Amount, a.Type);
        }
    }

    internal class Account
    {
        Money Banking = new Money(100, CurrencyType.RUR);

        public void TransitionToAnotherAccount(Money amount, Account anotherAccount)
        {
            uint count = (Banking.Type != amount.Type ? 1u : 0u) + (amount.Type != anotherAccount.Banking.Type ? 1u : 0u);
            if (count != 0)
            {
                var commision = count == 1 ? "" : "дважды ";
                MessageBox.Show($"С вас {commision}возьмут комиссию 1%");
            }
            // Get the money from this account
            GetSomeMoney(amount);
            anotherAccount.AddSomeMoney(amount);
        }

        private void GetSomeMoney(Money amount)
        {
            // If Account has less money than amount
            // then throw
            if (Banking < amount)
                throw new AccountingException("Недостаточно денег");
            // If the types don't match get the commision
            if (amount.Type != Banking.Type)
                amount.GetComission();
            Banking -= amount;
        }

        private void AddSomeMoney(Money amount)
        {
            // If the types don't match get the commision
            if (amount.Type != Banking.Type)
                amount.GetComission();
            Banking += amount;
        }

        public void TransitionToAnotherType(CurrencyType newType)
        {
            // if the types don't match get the commision
            if (Banking.Type != newType)
                Banking.GetComission();
            Banking = Banking.TransitionToAnotherType(newType);
        }

        public string GetBalance()
        {
            return Banking.Amount.ToString();
        }

        public CurrencyType GetCurrencyType()
        {
            return Banking.Type;
        }
    }

    internal class AccountingException : Exception
    {
        public AccountingException(string message) : base(message)
        {
        }
    }

    public static partial class Extensions
    {
        public static CurrencyType GetDefault(this CurrencyType _) => CurrencyType.RUR;

        public static decimal GetExchangeRate(this CurrencyType type) => type switch
        {
            CurrencyType.RUR => 1,
            CurrencyType.UZS => 0.00694393m,
            CurrencyType.USD => 73.2636m,
            _ => throw new NotImplementedException()
        };
    }
}
