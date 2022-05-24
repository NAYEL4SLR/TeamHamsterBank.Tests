using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TeamHamsterBank
{
    static class StoreAndLoad
    {
        internal static List<string[]> AccountFile = new List<string[]>();
        internal static List<string[]> UsersFile = new List<string[]>();
        internal static List<string[]> TransactionsFile = new List<string[]>();
        private static List<string> _fileLines;
        public static void LoadAccounts()
        {
            try
            {
                if (File.Exists("Accounts.txt"))
                {
                    _fileLines = File.ReadAllLines
                        ("Accounts.txt", Encoding.Default).ToList();
                }
                else if (File.Exists("Accounts - Backup.txt"))
                {
                    _fileLines = File.ReadAllLines
                        ("Accounts - Backup.txt", Encoding.Default).ToList();
                }
                else
                {
                    throw new Exception("\n\n\n\t\tSystemunderhåll pågår!   " +
                        "Vi beklagar stilleståndet\n\n\t" +
                        " Tryck 'Enter för att avsluta tjänsten'");
                }
                for (int i = 0; i < _fileLines.Count; i++)
                {
                    AccountFile.Add(_fileLines[i].Split(new string[]
                    { "________" }, StringSplitOptions.None));
                }
                AccountFile = AccountFile.OrderBy(arr => int.Parse(arr[2])).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void LoadUsers()
        {
            try
            {
                if (File.Exists("Users.txt"))
                {
                    _fileLines = File.ReadAllLines
                        ("Users.txt", Encoding.Default).ToList();
                }
                else if (File.Exists("Users - Backup.txt"))
                {
                    _fileLines = File.ReadAllLines
                        ("Users - Backup.txt", Encoding.Default).ToList();
                }
                else
                {
                    throw new Exception("\n\n\n\t\tSystemunderhåll pågår!   " +
                        "Vi beklagar stilleståndet\n\n\t" +
                        " Tryck 'Enter för att avsluta tjänsten'");
                }
                for (int i = 0; i < _fileLines.Count; i++)
                {
                    UsersFile.Add(_fileLines[i].Split(new string[]
                    { "________" }, StringSplitOptions.None));
                }
                DeclareUsers();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void LoadTransactions()
        {
            try
            {
                if (File.Exists("Transactions.txt"))
                {
                    _fileLines = File.ReadAllLines
                        ("Transactions.txt", Encoding.Default).ToList();
                }
                else if (File.Exists("Transactions - Backup.txt"))
                {
                    _fileLines = File.ReadAllLines
                        ("Transactions - Backup.txt", Encoding.Default).ToList();
                }
                else
                {
                    throw new Exception("\n\n\n\t\tSystemunderhåll pågår!   " +
                        "Vi beklagar stilleståndet\n\n\t" +
                        " Tryck 'Enter för att avsluta tjänsten'");
                }
                for (int i = 0; i < _fileLines.Count; i++)
                {
                    TransactionsFile.Add(_fileLines[i].Split(new string[]
                    { "________" }, StringSplitOptions.None));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void DeclareUsers()
        {
            foreach (var user in UsersFile)
            {
                if (user[3] == "Admin")
                {
                    Bank.UsersList.Add(new Admin(user[0], user[1], user[2]));
                }
                else if (user[3] == "Customer")
                {
                    Bank.UsersList.Add(new Customer(user[0], user[1], user[2]));
                }
            }
        }

        public static void SaveAccounts()
        {
            try
            {
                string save = String.Empty;
                foreach (User user in Bank.UsersList)
                {
                    if (user is Customer)
                    {
                        Customer customer = user as Customer;
                        foreach (Account account in customer._accounts)
                        {
                            save += account.ToSave();
                        }
                    }
                }
                File.WriteAllText("Accounts.txt", save,
                    Encoding.Default);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void SaveUsers()
        {
            try
            {
                string save = String.Empty;
                foreach (User user in Bank.UsersList)
                {
                    save += user.ToSave();
                }
                File.WriteAllText("Users.txt", save,
                    Encoding.Default);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void SaveTransactions()
        {
            try
            {
                WriteTransactionsToFile();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void WriteTransactionsToFile()
        {
            if (CheckTransactionsListData(TransactionsFile))
            {
                string save = String.Empty;
                foreach (string[] transaction in TransactionsFile)
                {
                    save += $"{transaction[0]}________{transaction[1]}________{transaction[2]}" +
                        $"________{transaction[3]}________{transaction[4]}\n";
                }
                File.WriteAllText("Transactions.txt", save,
                    Encoding.Default);
            }
            else
            {
                throw new ArgumentException("Invalid data detected !");
            }
        }

        public static bool CheckTransactionsListData(List<string[]> list)
        {
            // If any row has different length
            if (list.Any(row => row.Length != list[0].Length))
            {
                return false;
            }
            // If the account number is valid (the string cannot be parsed)
            if (!list.All(row => int.TryParse(row[4], out int n)))
            {
                return false;
            }
            return true;
        }
    }
}
