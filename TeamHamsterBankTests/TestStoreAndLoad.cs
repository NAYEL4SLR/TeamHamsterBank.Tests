namespace TeamHamsterBankTests;

[TestClass]
public class TestStoreAndLoad
{
    [TestMethod]
    public void DeclareUsers_InitializeUsersSuccessfully()
    {
        /* DeclareUsers() is a static void method which loops through the data in
        'UsersFile' to use for declaring users and adding them to 'UsersList' */
        StoreAndLoad.UsersFile = GetSampleUsers();
        StoreAndLoad.DeclareUsers();
        var actualAdmins = Bank.UsersList.Count(user => user is Admin);
        var actualCustomers = Bank.UsersList.Count(user => user is Customer);
        Assert.IsTrue(2 == actualAdmins && 2 == actualCustomers);

        // Matching test for the full name
        var expected = GetSampleUsers();
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.AreEqual(expected[i][0], Bank.UsersList[i].UserID);
            Assert.AreEqual(expected[i][1], Bank.UsersList[i].FullName);
        }
    }

    public List<string[]> GetSampleUsers()
    {
        return new List<string[]>
            {
                new string[] { "111111", "Peter Griffen", "password", "Admin"},
                new string[] { "222222", "Alex Sawnson", "password", "Admin"},
                new string[] { "333333", "Morty Smith", "password", "Customer"},
                new string[] { "666666", "Rick Sanchez", "password", "Customer" }
            };
    }

    [TestMethod]
    public void SaveTransactions_WriteToTextFileCorrectly()
    {
        /* SaveTransactions() takes in data from 'TransactionsFile' and writes it to text-file*/
        StoreAndLoad.TransactionsFile = GetSampleTransactions();
        StoreAndLoad.SaveTransactions();

        // Testing the lines count in the text-file after writing to it.
        var actualCount = File.ReadAllLines("Transactions.txt").ToList().Count();
        Assert.AreEqual(4, actualCount);

        // Testing the length for a loaded string-array from the saved lines
        var fileContent = File.ReadAllLines("Transactions.txt").ToList();
        var actualLength = fileContent.First().Split("________").Length;
        Assert.AreEqual(5, actualLength);
    }

    public List<string[]> GetSampleTransactions()
    {
        return new List<string[]>
            {
                new string[] { "12/10/2021 8:42:21 PM", "Överföring", "-300", "EUR", "100000007"},
                new string[] { "12/10/2021 8:42:33 PM", "Insättning", "150", "GBP", "100000008"},
                new string[] { "12/10/2021 8:44:49 PM", "Uttag", "-1000", "EUR", "100000002"},
                new string[] { "12/10/2021 9:46:24 PM", "Lån", "90000", "SEK", "100000005"}
            };
    }

    [TestMethod]
    public void WriteTransactionsToFile_ShouldFail()
    {
        var invalidList = new List<string[]>
            {
                new string[] { "Invalid Data", "fafasfsda", "0x4112200"},
                new string[] { "corrupted", "Data"},
                new string[] { ""}
            };
        StoreAndLoad.TransactionsFile = invalidList;

        // Applying the being tested method:
        Assert.ThrowsException<ArgumentException>(() => StoreAndLoad.WriteTransactionsToFile());
    }

    //Integration test
    [TestMethod]
    public void LoadAccounts_LoadDataCorrectly()
    {
        // LoadAccounts() writes data to 'AccountFile'
        StoreAndLoad.LoadAccounts();

        // Testing if data was successfully added by the method
        StoreAndLoad.AccountFile.Should().NotBeEmpty();

        // Testing the length for an added string-array by the method
        var actualLength = StoreAndLoad.AccountFile[0].Length;
        // Privatkonto, Allkonto, 100000009, 35000, SEK, 777777  (Length = 6) 
        Assert.AreEqual(6, actualLength);

        // Comparing the lines counts between the text-file and 'AccountFile'
        var expectedCount = File.ReadAllLines("Accounts - Backup.txt").ToList().Count();
        var actualCount = StoreAndLoad.AccountFile.Count;
        Assert.AreEqual(expectedCount, actualCount);
    }
}
