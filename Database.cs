namespace Sonata
{
    public struct Account
    {
        public string Apartment;
        public string Emails;
        public float Percentages;
    }

    public class Database : Table
    {
        public bool FirstRow = true;
        public List<Account> Accounts = [];

        public override void Process()
        {
            int apartmentIndex = GetIndexOf("apartment|обект");
            if (apartmentIndex < 0)
            {
                Console.WriteLine($"ERROR: Няма колонка за обекти в {Source}. Тя трябва да има в името си \"ownership\" или \"ич\".");
                return;
            }

            int emailsIndex = GetIndexOf("username|mail");
            if (emailsIndex < 0)
            {
                Console.WriteLine($"ERROR: Няма колонка за мейли в {Source}. Тя трябва да има в името си \"username\" или \"mail\".");
                return;
            }

            int ownershipPercentageIndex = GetIndexOf("ownership|ич");
            if (ownershipPercentageIndex < 0)
            {
                Console.WriteLine($"ERROR: Няма колонка за идеални части в {Source}. Тя трябва да има в името си \"ownership\" или \"ич\".");
                return;
            }

            foreach (string[] row in Rows)
            {
                Accounts.Add(new Account()
                {
                    Apartment = row[apartmentIndex],
                    Emails = row[emailsIndex],
                    Percentages = float.Parse(row[ownershipPercentageIndex])
                });
            }
        }
    }
}
