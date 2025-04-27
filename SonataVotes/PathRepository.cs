using System.Reflection;

namespace Sonata
{
    public class PathRepository
    {
        private string AppDirectory = "";
        public string DatabasePath { get; private set; } = "";
        public List<string> ReportPaths { get; private set; } = new List<string>();
        public string ResultsPath { get; private set; } = "";

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
            string? path = null;
            while (string.IsNullOrEmpty(path))
            {
                path = FetchInputPath();
            }
            DatabasePath = path;
        }

        private void FetchReportPaths()
        {
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
