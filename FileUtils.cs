using System.Text;

namespace Sonata
{
    public static class FileUtils
    {
        public static TTable ReadTable<TTable>(string path)
            where TTable : class, ITable, new()
        {
            TTable table = new()
            {
                Source = path
            };
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                        continue;

                    line = line.ToLower().Replace("\"", "").Trim();
                    var values = line.Split(',');
                    table.AddRow(values);
                }
            }

            table.Process();

            return table;
        }
        public static void WriteTable(string path, string text)
        {
            File.WriteAllText(path, text, Encoding.UTF8);
        }
    }
}
