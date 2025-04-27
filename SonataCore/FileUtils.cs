using System.Text;

namespace Sonata
{
    public static class FileUtils
    {
        public static TTable ReadTable<TTable>(string path, int skipLines = 0)
            where TTable : class, ITable, new()
        {
            TTable table = new()
            {
                Source = path
            };
            int skippedLines = 0;
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                        continue;

                    if (skipLines > skippedLines)
                    {
                        skippedLines++;
                        continue;
                    }

                    line = line.Trim('"').Trim();
                    var values = line.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        string value = values[i];
                        if (string.IsNullOrEmpty(value))
                            continue;

                        if (value.StartsWith('"') && value.EndsWith('"'))
                            values[i] = value[1..^1];
                        values[i] = values[i].Replace("\"\"","\"");
                    }
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
