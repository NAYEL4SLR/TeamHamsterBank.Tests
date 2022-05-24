# Critical parts that need to be tested
In the static class 'StoreAndLoad' there are three methods (LoadAccounts, LoadTransactions, LoadUsers) that import data from text-files by exporting the text lines as string-arrays to be stored in the right collections.  <br />
All three follow the exact same technique to perform their tasks. There are Also three other methods (SaveAccounts, SaveTransactions, SaveUsers) which function in a very similar way but in reverse.  <br />

If the text-files get corrupted or even switched by other files that match the same names, the app would load inaccurate details or even keep crashing, thus becoming completely ruined and unreliable. Worth to mention that if the app gets compromised and stores some invalid data in its lists, then it would ruin the data in a related text-file.
## Suggested measures
- LoadAccounts() can be tested to represent the three importing methods. And 'SaveTransactions()' to represent the three exporting methods. 
- Check if data was indeed exported to the right collection or file after calling the related method.
- Test the length and the count of rows, then compare them with the original source (Text-file / Collection). 
- Generating some fake data to mock a collection or file is helpful and important for the unit testing  <br />
- Check if the app can avoid writing invalid data to the text-files.

### DeclareUsers()
It's vitally important to test 'DeclareUsers()' since Admins and Customers get declared and initialized in this method. The constructor for the customer will sort out the accounts and they get initialized right afterwards.  <br />
'DeclareUsers()' can be applied to a fake collection that contains two admins and two customers,  to compare and check whether the method is working correctly or not. 

### EnoughBalance()
On each money transfer process,  ‘EnoughBalance()’ is used to check the balance before proceeding with the request. Therefore it should be tested since it’s one the critical parts for any banking app.


