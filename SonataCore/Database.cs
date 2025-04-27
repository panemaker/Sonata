namespace Sonata
{
    public struct Account
    {
        public string Names;
        public string Apartment;
        public string Entrance;
        public string Emails;
        public float Percentages;
    }

    public class Database : Table
    {
        public bool FirstRow = true;
        public List<Account> Accounts = [];

        public override void Process()
        {
            int nameIndex = GetIndexOf("собственик");
            int apartmentIndex = GetIndexOf("apartment|обект");
            int entranceIndex = GetIndexOf("№");
            int emailsIndex = GetIndexOf("username|mail|e-mail");
            int ownershipPercentageIndex = GetIndexOf("ownership|ич");

            foreach (string[] row in Rows)
            {
                Account account = new Account();
                account.Names = nameIndex != INVALID_INDEX ? row[nameIndex] : string.Empty;
                account.Apartment = apartmentIndex != INVALID_INDEX ? row[apartmentIndex] : string.Empty;
                account.Entrance = entranceIndex != INVALID_INDEX ? row[entranceIndex] : string.Empty;
                account.Emails = emailsIndex != INVALID_INDEX ? row[emailsIndex] : string.Empty;
                account.Percentages = ownershipPercentageIndex != INVALID_INDEX ? float.Parse(row[ownershipPercentageIndex]) : 0.0f;
                Accounts.Add(account);
            }
        }
    }
}
