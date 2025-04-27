using System.Reflection;

namespace Sonata
{
    public class PathRepository
    {
        private string AppDirectory = "";
        public string DatabasePath { get; private set; } = "D:\\Projects\\Sonata\\Соната - Идеални Части.csv";
        public List<string> ReportPaths { get; private set; } = new List<string>() 
        {
            "D:\\Projects\\Sonata\\Соната - Google Forms.csv"
        };
        public string ResultsPath { get; private set; } = "D:\\Projects\\Sonata\\Соната - Results.csv";

        public PathRepository() 
        {
            AppDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
            Console.WriteLine("Добави база данни с идеални части:");
            FetchDatabasePath();
            Console.WriteLine("");

            Console.WriteLine("Добави файлове с гласове (Google Forms, Ръчно събиране). Празен ред, за да продължиш:");
            FetchReportPaths();
            Console.WriteLine("");

            Console.WriteLine("Файл за резултати:");
            FetchResultsPath();
            Console.WriteLine("");
        }

        private void FetchDatabasePath()
        {
            if (!string.IsNullOrEmpty(DatabasePath))
                return;

            string? path = null;
            while (string.IsNullOrEmpty(path))
            {
                path = FetchInputPath();
            }
            DatabasePath = path;
        }

        private void FetchReportPaths()
        {
            if (ReportPaths.Count > 0)
                return;

            string? path;
            while (true)
            {
                path = FetchInputPath();
                if (!string.IsNullOrEmpty(path))
                {
                    if (ReportPaths.Contains(path))
                    {
                        Console.WriteLine("Файлът вече е добавен.");
                    }
                    else
                    {
                        ReportPaths.Add(path);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void FetchResultsPath()
        {
            if (!string.IsNullOrEmpty(ResultsPath))
                return;

            string? path = null;
            while (string.IsNullOrEmpty(path))
            {
                path = FetchInputPath();
                if (!string.IsNullOrEmpty(path) && MatchesInputPaths(path))
                {
                    Console.WriteLine("Не може да се запише резултата в този файл.");
                    path = null;
                }
            }
            ResultsPath = path;
        }


        private string? FetchInputPath()
        {
            string? path = Console.ReadLine();

            if (!string.IsNullOrEmpty(path))
            {
                path = path.ToLower().Replace("\"", "").Trim();
                if (!Path.IsPathFullyQualified(path))
                {
                    path = Path.Combine(AppDirectory, path);
                }
            }

            return path;
        }

        private bool MatchesInputPaths(string path)
        {
            if (path.Equals(Path.GetFullPath(DatabasePath)))
                return true;

            foreach (string reportPath in ReportPaths)
            {
                if (path.Equals(Path.GetFullPath(reportPath)))
                    return true;
            }

            return false;
        }
    }
}
