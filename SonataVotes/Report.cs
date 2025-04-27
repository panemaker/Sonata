using System.Text.RegularExpressions;

namespace Sonata
{
    public struct Vote
    {
        public string Apartment;
        public string Email;
        public List<string> Answers;
    }

    public class Report : Table
    {
        public List<string> Questions { get; set; } = [];
        public List<Vote> Votes { get; set; } = [];

        public override void Process()
        {
            int emailsIndex = GetIndexOf("username|mail");
            int apartmentIndex = GetIndexOf("apartment|обект");

            if (emailsIndex < 0 && apartmentIndex < 0)
            {
                Console.WriteLine($"ERROR: Няма колонка за обект или мейл в {Source}.");
                return;
            }

            List<int> questionIndices = new List<int>();
            foreach (string header in Headers)
            {
                Regex reverseRx = new Regex("username|mail|apartment|обект|timestamp", RegexOptions.IgnoreCase);
                if (reverseRx.IsMatch(header))
                {
                    continue;
                }
                Questions.Add(header);
                questionIndices.Add(Headers.IndexOf(header));
            }
            if (emailsIndex < 0 && apartmentIndex < 0)
            {
                Console.WriteLine($"ERROR: Няма колонка за въпроси в {Source}.");
                return;
            }


            foreach (string[] row in Rows)
            {
                Vote vote = new Vote();
                if (emailsIndex >= 0)
                {
                    vote.Email = row[emailsIndex];
                }
                if (apartmentIndex >= 0)
                {
                    vote.Apartment = row[apartmentIndex];
                }
                vote.Answers = new List<string>();
                foreach (int questionIndex in questionIndices)
                {
                    vote.Answers.Add(row[questionIndex]);
                }

                Votes.Add(vote);
            }
        }

        public IEnumerable<Account> GetAssociatedAccounts(Vote vote, Database database)
        {
            return database.Accounts.Where(a =>
            {
                if (!string.IsNullOrEmpty(vote.Email))
                    return a.Emails.Contains(vote.Email);
                else if (!string.IsNullOrEmpty(vote.Apartment))
                    return a.Apartment.Contains(vote.Apartment);
                else
                    return false;
            });
        }
    }
}
