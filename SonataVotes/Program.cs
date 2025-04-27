namespace Sonata
{
    public class Program
    {
        private static void Main()
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            PathRepository pathRepository = new();

            Database database = FileUtils.ReadTable<Database>(pathRepository.DatabasePath);
            List<Report> reports = [];
            foreach (string reportPath in pathRepository.ReportPaths)
            {
                reports.Add(FileUtils.ReadTable<Report>(reportPath));
            }

            VotesCounter votesCounter = new(database);
            Console.WriteLine("Броене:");
            foreach (Report report in reports)
            {
                votesCounter.CountVotes(report);
            }

            Console.WriteLine("\nРезултати:");
            string results = votesCounter.ToString();
            Console.WriteLine(results);
            FileUtils.WriteTable(pathRepository.ResultsPath, results);

            Console.ReadKey();
        }
    }
}