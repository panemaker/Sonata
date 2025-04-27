using System.Text;

namespace Sonata
{
    public struct QuestionResult
    {
        public string Question;
        public Dictionary<string, float> AnswerTotalVotes;
        public List<Account> Voters;
    }

    public class VotesCounter
    {
        Database? db = null;
        List<QuestionResult> QuestionResults = [];

        public VotesCounter(Database database) => db = database;

        public override string ToString()
        {
            var csv = new StringBuilder();
            foreach (var question in QuestionResults)
            {
                csv.AppendLine(question.Question);
                List<string> voteResults = [];
                foreach (var answerVotePair in question.AnswerTotalVotes)
                {
                    voteResults.Add($"{answerVotePair.Key},{answerVotePair.Value}");
                }
                voteResults.Sort();
                foreach (var voteResult in voteResults)
                {
                    csv.AppendLine(voteResult);
                }
                csv.AppendLine("");
            }
            return csv.ToString();
        }

        public void CountVotes(Report report)
        {
            if (report.Questions == null || db == null)
                return;

            foreach (var question in report.Questions)
            {
                TryCreateQuestionResult(question);
            }

            foreach (var vote in report.Votes)
            {
                foreach (var account in report.GetAssociatedAccounts(vote, db))
                {
                    CountVotesForAccount(account, vote.Answers, report.Questions);
                }
            }

        }

        private void CountVotesForAccount(Account account, List<string> answers, List<string> allQuestions)
        {
            foreach (var (answer, questionIndex) in answers.Select((v, i) => (v, i)))
            {
                if (string.IsNullOrEmpty(answer))
                    continue;

                var questionResult = QuestionResults.First(r => r.Question.Equals(allQuestions[questionIndex]));

                // Only vote once
                if (questionResult.Voters.Contains(account))
                {
                    Console.WriteLine($"{account.Apartment} вече е гласувал за \"{allQuestions[questionIndex]}\".");
                    continue;
                }

                questionResult.Voters.Add(account);

                float currentVote = 0.0f;
                questionResult.AnswerTotalVotes.TryGetValue(answer, out currentVote);
                questionResult.AnswerTotalVotes[answer] = currentVote + account.Percentages;
            }
        }

        private void TryCreateQuestionResult(string question)
        {
            if (!QuestionResults.Exists(r => r.Question.Equals(question, StringComparison.InvariantCultureIgnoreCase)))
            {
                QuestionResults.Add(new QuestionResult()
                {
                    Question = question,
                    AnswerTotalVotes = [],
                    Voters = []
                });
            }

        }

    }
}
