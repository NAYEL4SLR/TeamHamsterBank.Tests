namespace TeamHamsterBankTests;

[TestClass]
public class TestEnoughBalance
{
    [DataTestMethod]
    [DataRow("5000", "1300", true)]   // Succeeds
    [DataRow("30000", "1700", true)]  // Succeeds
    [DataRow("1400", "2500", false)]  // Fails
    [DataRow("600", "1100", false)]   // Fails
    public void EnoughBalance_SucceedsIfBalanceIsEqualOrGreater(string balance, string withdrawal, bool enough)
    {
        Account testAccount = new Account(decimal.Parse(balance));
        Assert.AreEqual(testAccount.EnoughBalance(decimal.Parse(withdrawal)), enough);
    }
}
