using SonataCore;
using System.IO;
using System.Text;

namespace Sonata
{
    public class Contacts
    {
        Database? db = null;
        public Contacts(Database database) => db = database;

        public override string ToString()
        {
            if (db == null)
            {
                Console.WriteLine("Няма заредена база данни.");
                return String.Empty;
            }

            var csv = new StringBuilder();
            csv.AppendLine("First Name,Middle Name,Last Name,Phonetic First Name,Phonetic Middle Name,Phonetic Last Name,Name Prefix,Name Suffix,Nickname,File As,Organization Name,Organization Title,Organization Department,Birthday,Notes,Photo,Labels,E-mail 1 - Label,E-mail 1 - Value,E-mail 2 - Label,E-mail 2 - Value,Phone 1 - Label,Phone 1 - Value");

            foreach (Account account in db.Accounts)
            {
                string[] fullNames = account.Names.Split(';');
                string[] emails = account.Emails.Trim().Split(';');
                Func<string, bool> IsAparment = apartment => account.Apartment.Equals(apartment, StringComparison.OrdinalIgnoreCase);
                bool isFirstNameFirm = fullNames[0].StartsWith('"');
                // HACKS
                //if (fullNames.Length == isFirstNameFirm ? 2 : 1))
                //    continue;
                //if (emails.Length <= 2)
                //    continue;

                int mailIndex = 0;
                for (int i = 0; i < fullNames.Length; i++)
                {
                    string fullName = fullNames[i];
                    if (fullName.StartsWith('"'))
                        continue;

                    if (mailIndex >= emails.Length)
                        continue;

                    string[] nameParts = fullName.Trim().Split(' ');
                    csv.Append(nameParts[0].FirstCharToUpper()); // First Name
                    int secondNameIndex = nameParts.Length > 2 ? 2 : 1;
                    csv.Append($","); // Middle Name
                    csv.Append($","); // Last Name
                    if (nameParts.Length > 1)
                    {
                        csv.Append($"{nameParts[secondNameIndex]} - {account.Apartment.Replace("-","")}"); // Last Name
                    }
                    csv.Append($","); // Phonetic First Name
                    csv.Append($","); // Phonetic Middle Name
                    csv.Append($","); // Phonetic Last Name
                    csv.Append($","); // Name Prefix
                    csv.Append($","); // Name Suffix
                    csv.Append($","); // Nickname
                    csv.Append($","); // File As
                    csv.Append($","); // Organization Name
                    csv.Append($","); // Organization Title
                    csv.Append($","); // Organization Department
                    csv.Append($","); // Birthday
                    csv.Append($","); // Notes
                    csv.Append($","); // Photo
                    csv.Append($","); // Labels
                    csv.Append($"Соната - Всички ::: Соната - вх.{account.Entrance} ::: * myContacts"); // Labels

                    csv.Append($","); // E-mail 1 - Label
                    csv.Append($",{emails[mailIndex]}"); // E-mail 1 - Value
                    mailIndex++;
                    csv.Append($","); // E-mail 2 - Label
                    csv.Append($","); // E-mail 2 - Value

                    // if the amount of names match the amount of emails match them 1-1
                    int realNamesCount = fullNames.Length - (isFirstNameFirm ? 1 : 0);
                    bool useOneMailPerName = realNamesCount == emails.Length &&
                        !(IsAparment("A-04") || IsAparment("В-39") || IsAparment("Д-02"));// exceptions; 
                    bool writeSecondMail = mailIndex < emails.Length && !useOneMailPerName; // has more mails
                    if (writeSecondMail)
                    {
                        csv.Append($"{emails[mailIndex]}");
                        mailIndex++;
                    }

                    csv.Append($","); // Phone 1 - Label
                    csv.Append($","); // Phone 1 - Value
                    csv.Append('\n');

                    // if there is only one mail just use the first name as contact
                    if (emails.Length == 1)
                        break;
                }
            }

            return csv.ToString();
        }
    }
}
