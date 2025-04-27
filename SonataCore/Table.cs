using System.Text.RegularExpressions;

namespace Sonata
{
    public interface ITable
    {
        public string Source { get; set; }
        public void AddRow(string[] rowValues);
        public virtual void Process() { }
    }

    public class Table : ITable
    {
        public static int INVALID_INDEX = -1;
        public string Source { get; set; } = "";

        protected List<string> Headers = [];
        protected List<string[]> Rows = [];

        protected int GetIndexOf(string regexString)
        {
            Regex rx = new Regex(regexString, RegexOptions.IgnoreCase);
            string? header = Headers.FirstOrDefault(h => rx.IsMatch(h));
            return !string.IsNullOrEmpty(header) ? Headers.IndexOf(header) : INVALID_INDEX;
        }

        public void AddRow(string[] rowValues)
        {
            if (Headers.Count == 0)
            {
                Headers.AddRange(rowValues);
            }
            else
            {
                Rows.Add(rowValues);
            }
        }

        public virtual void Process() { }
    }
}
