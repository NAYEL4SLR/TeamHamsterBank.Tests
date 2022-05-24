using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace TeamHamsterBank
{
    class Account
    {
        private List<string[]> _transaction = new List<string[]>();

        private string _accountType = "New Account";
        private string _accountName = "Konto";
        private decimal _balance;
        internal decimal Balance
        {
            set
            {
                _balance = value;
            }
            get => _balance;
        }
        private string _currency;
        private static int _staticAccountNum = 100000000;
        private int _accountNum;
        internal string Currency { get => _currency;}
        internal string AccountType { get => _accountType; }
        internal int AccountNumber { get  => _accountNum;  }
        private string _customerID;
        internal string CustomerID { get => _customerID; }

        /*This constructor will only be called to declare the accounts that already
         registered when the app starts runs */
        public Account(string accountName, string accountType, string accountNum,
                        decimal balance, string currency, string customerID)
        {
            _staticAccountNum++;
            _accountName = accountName;
            _accountType = accountType;
            _accountNum = int.Parse(accountNum);
            _balance = balance;
            _currency = currency;
            _customerID = customerID;
            SortOutDetails(StoreAndLoad.TransactionsFile);
        }
        // This constructer will be called when opening a new accounnt
        public Account(string accountName, string accountType, string currency, string customerID)
        {
            _staticAccountNum++;
            _accountName = accountName;
            _accountType = accountType;
            _accountNum = _staticAccountNum;
            _balance = 0.00m;
            _currency=currency;
            _customerID = customerID;
        }
        // Constructor for testing
        public Account(decimal balance)
        {
            _balance = balance;
        }
        public void SortOutDetails(List<string[]> transactionsFile)
        {
            foreach (string[] transaction in transactionsFile)
            {
                if (_accountNum.ToString() == transaction[4])
                {
                    _transaction.Add(new string[] {transaction[0], transaction[1],
                                   transaction[2], transaction[3], transaction[4]});
                }
            }
        }
        // List of available currencies in the bank
        internal static List<string[]> CurrencyList = new List<string[]>
        {
            new string[] { "SEK", "1.0" },
            new string[] { "EUR", "10.23" },
            new string[] { "USD", "9.03" },
            new string[] { "GBP", "12.03" },
            new string[] { "CAD", "7.08" },
            new string[] { "JPY", "0.080" },
            new string[] { "CHF", "9.82" },
            new string[] { "AUD", "6.46" },
        };

        public string ToSave()
        {

            return $"{_accountName}________{_accountType.Trim()}________{_accountNum}" +
                $"________{_balance}________{_currency}________{_customerID}\n";
        }
        public override string ToString()
        {
            return $"{_accountName}    Kontotyp: [{_accountType}]    KontoNr: [{_accountNum}]" +
                   $"    Saldo: {_balance:0.00}    [{_currency}]\n\n";
        }
        public static string PrintAccounts(Customer customer)
        {
            StringBuilder output = new StringBuilder();
            string accountName = string.Empty;
            string accountType = string.Empty;
            
            output.Append("\t╔═══╤═══════════╤══════════════╤════════════╤════════════════════╤══════╗\n" +
                          "\t║Val│Kontonummer│    Namn      │  Kontotyp  │        Saldo       │Valuta║\n" +
                          "\t╠═══╪═══════════╪══════════════╪════════════╪════════════════════╪══════╣\n");
            for (int i = 0; i < customer._accounts.Count; i++)
            {
                accountName = customer._accounts[i]._accountName;
                accountType = customer._accounts[i]._accountType;
                string balanceOutput = $"{customer._accounts[i]._balance:0.00}";
                if ((accountName = customer._accounts[i]._accountName).Length > 14)
                    accountName = accountName.Remove(14);
                if ((accountType = customer._accounts[i]._accountType).Length > 12)
                    accountType = accountType.Remove(12);
                output.Append(String.Format("\t║{0,-3}│{1,-11}│{2,-14}│{3,-12}│{4,20}│{5,-6}║\n",
                    i + 1, customer._accounts[i]._accountNum, accountName, accountType,
                    balanceOutput, customer._accounts[i]._currency));
                output.Append("\t╟───┼───────────┼──────────────┼────────────┼────────────────────┼──────╢\n");
            }
            output.Append("\t╚═══╧═══════════╧══════════════╧════════════╧════════════════════╧══════╝");
            return $"\n\n{output.ToString()}";
        }
        public static string PrintAccounts(params Account[] accounts)
        {
            StringBuilder output = new StringBuilder();
            string accountName = string.Empty;
            string accountType = string.Empty;

            output.Append("\t╔═══════════╤══════════════╤════════════╤════════════════════╤══════╗\n" +
                          "\t║Kontonummer│    Namn      │  Kontotyp  │        Saldo       │Valuta║\n" +
                          "\t╠═══════════╪══════════════╪════════════╪════════════════════╪══════╣\n");
            for (int i = 0; i < accounts.Length; i++)
            {
                accountName = accounts[i]._accountName;
                accountType = accounts[i]._accountType;
                string balanceOutput = $"{accounts[i]._balance:0.00}";
                if ((accountName = accounts[i]._accountName).Length > 14)
                    accountName = accountName.Remove(14);
                if ((accountType = accounts[i]._accountType).Length > 12)
                    accountType = accountType.Remove(12);
                output.Append(String.Format("\t║{0,-11}│{1,-14}│{2,-12}│{3,20}│{4,-6}║\n",
                     accounts[i]._accountNum, accountName, accountType,
                    balanceOutput, accounts[i]._currency));
                output.Append("\t╟───────────┼──────────────┼────────────┼────────────────────┼──────╢\n");
            }
            output.Append("\t╚═══════════╧══════════════╧════════════╧════════════════════╧══════╝");
            return $"\n\n{output.ToString()}";
        }
        public static void SelectAccount(Customer customer, int numberOfAccounts)
        {
            if (customer._accounts.Count == 0)
            {
                Console.WriteLine("\n\n\t\tDu har inget registrerat konto." +
                                        "  Var god och öppna ett nytt konto");
                return;
            }
            Console.Write("\n\tVälj ett konto och tryck 'Enter' eller tryck 'Enter' två gångar: ");
            Int32.TryParse(Console.ReadLine(), out int accountIndex);
            if ( accountIndex <= numberOfAccounts && accountIndex > 0 )
            {
                Console.Clear();
                Console.WriteLine("\n\n\n\t"+customer._accounts[accountIndex -1].PrintAccountHistory());
            }
        }
        public string PrintAccountHistory()
        {
            StringBuilder output = new StringBuilder();
            output.Append("\t╔═══════════════════════╤═════════════════╤════════════════╤════════╗\n" +
                          "\t║         Datum         │   Transaktion   │     Belopp     │ Valuta ║\n" +
                          "\t╠═══════════════════════╪═════════════════╪════════════════╪════════╣\n");

            foreach (string[] transaction in _transaction)
            {
                string balanceOutput = $"{decimal.Parse(transaction[2], CultureInfo.InvariantCulture):0.00}";
                output.Append(String.Format("\t║ {0,-22}│ {1,-16}│  {2,-14}│   {3,-5}║\n",
                    transaction[0], transaction[1], balanceOutput, transaction[3]));
                output.Append("\t╟───────────────────────┼─────────────────┼────────────────┼────────╢\n");
            }
            output.Append("\t╚═══════════════════════╧═════════════════╧════════════════╧════════╝");
            return $"\n\n{output}";
        }
        public static void SubmitTransaction(string transactionName, Customer customer, 
            int accountIndex, decimal value, string currency)
        {
            customer._accounts[accountIndex]._transaction.Add(
                new string[] { DateTime.Now.ToString(), transactionName, value.ToString(), currency});
            StoreAndLoad.TransactionsFile.Add(
                new string[] {DateTime.Now.ToString(), transactionName, value.ToString(), currency,
                    customer._accounts[accountIndex]._accountNum.ToString() });
        }
        public bool EnoughBalance(decimal checkSum)
        {
            return checkSum <= _balance;
        }
        public void MakeTransfer(decimal transferSum, Account toAccount)
        {

            this._balance -= transferSum;
            Bank.ExchangeCurrency( ref transferSum, ref _currency);
            Bank.ExchangeBack(ref transferSum, ref toAccount._currency);
            toAccount._balance += transferSum;
        }
        public void MakeExternalTransfer(decimal transferSum, Account toAccount)
        {

            this._balance -= transferSum;
            Bank.ExchangeCurrency(ref transferSum, ref _currency);
            Bank.ExchangeBack(ref transferSum, ref toAccount._currency);
            Bank.UpcomingTransactions.Add(new Task(() =>
            {
                toAccount._balance += transferSum;

            }));

        }
        public static void PrintCurrencies() // Prints available currencies in the bank
        {
            Console.WriteLine("  * Tillgängliga valutor *\n");
            foreach (string[] currency in CurrencyList)
            {
                Console.WriteLine($"    [{currency[0]}]\n");
            }
        }
        public static void BankLoan(Customer customer)
        {
            Console.WriteLine("  * Banklån * \n");

            // Checks the total balance the customer has 
            decimal totalBalance = 0;
            for (int i = 0; i < customer._accounts.Count; i++)
            {
                if (customer._accounts[i].Currency != "SEK")
                {
                    string currency = customer._accounts[i].Currency;
                    decimal balance = customer._accounts[i].Balance;
                    Bank.ExchangeCurrency(ref balance, ref currency);
                }
                totalBalance = totalBalance + customer._accounts[i].Balance;
            }

            if (totalBalance >= 1000m)
            {
                decimal maxLoanAmount = totalBalance * 2;
                string input;
                decimal maxLoanAmountEven = maxLoanAmount - maxLoanAmount % 1000; // Rounds to the nearest and lowest thousands

                // Customer can borrow double the current total balance
                Bank.ReturnInstruction(4);
                Console.WriteLine($"  Minimumsumman för lån är 1000,00 [SEK]. " +
                    $"Du kan låna mellan 1000,00 - {maxLoanAmountEven.ToString("F")} [SEK] baserat på ditt nuvarande kapital.\n");
                Console.WriteLine($"  Vi har en engångsavgift på 10% som dras direkt från lånet och månadsräntan är 5% av lånsumman\n");

                // Input loan amount
                Console.Write("  Vänligen ange hur mycket du vill låna: ");
                decimal loanAmount = 0;
                do
                {
                    Decimal.TryParse(input = Console.ReadLine(), out loanAmount);
                    if (input.Trim().ToUpper() == "R")
                    {
                        return;
                    }

                    if (loanAmount > maxLoanAmountEven || loanAmount < 1000m)
                    {
                        Console.WriteLine($"  Ogiltligt val. Vänligen ange en summa mellan 1000,00 - {maxLoanAmountEven.ToString("F")} [SEK]");
                    }
                } while (loanAmount > maxLoanAmountEven || loanAmount < 1000m);

                decimal fee = loanAmount * 0.1m;

                Console.Clear();
                Console.WriteLine($"  Du har valt att låna {loanAmount.ToString("F")} [SEK]\n");
                decimal loanAmountLeft = loanAmount - fee;
                Console.WriteLine($"  Engångsavgift {fee.ToString("F")} [SEK] har dragits av från ditt lån och kvarvarande summa av lånet är {loanAmountLeft.ToString("F")} [SEK]\n");
                decimal interestCost = 0;
                int months = 0;
                while (loanAmountLeft > 0m)
                {
                    interestCost = interestCost + (loanAmountLeft * 0.05m);
                    loanAmountLeft = loanAmountLeft - 100;
                    months++;
                }

                Console.WriteLine($"  Du måste minst betala av 100 [SEK] per månad och om du betalar av minimumsumman: \n" +
                    $"  - Har du betalat av lånet efter {months} månader \n" +
                    $"  - Kostnaden för räntan blir {interestCost.ToString("F")} [SEK]");

                // Account selection and transfer
                Console.WriteLine(PrintAccounts(customer));
                int index = 0;
                while (index < 1 || index > customer._accounts.Count)
                {
                    Console.Write("\n\n   Välj vilket konto du vill överföra pengarna till:   ");
                    Int32.TryParse(Console.ReadLine(), out index);
                }
                index += -1;
                string bankCurrency = customer._accounts[index].Currency; // Exchange currency if the to account is not SEK
                Bank.ExchangeBack(ref loanAmount, ref bankCurrency);

                customer._accounts[index].Balance += loanAmount;

                // Print summary
                Console.Clear();
                Console.Write($"\n\n   '{loanAmount.ToString("F")}' har lagts till [{customer._accounts[index].AccountNumber}]");
                Account.SubmitTransaction("Lån", customer, index, loanAmount, "SEK");
                Console.WriteLine(Account.PrintAccounts(customer));
            }
            // If the customer doesn't have enough money for a loan
            else { Console.WriteLine("  Du har inte tillräckligt med pengar för att låna från vår bank.\n\n" +
                "  Vänligen tryck 'Enter' för att fortsätta"); }
            Bank.Redirecting();
        }
    }
}
