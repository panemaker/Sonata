namespace Sonata
{
    public class Program
    {
        private static void Main()
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string databasePath = "D:\\Projects\\Sonata\\SonataContacts\\FMG - База данни комплекс Соната - Собственици.csv";
            string contactsPath = "D:\\Projects\\Sonata\\SonataContacts\\contacts.csv";

            Database database = FileUtils.ReadTable<Database>(databasePath, skipLines: 1);

            Contacts contacts = new Contacts(database);
            FileUtils.WriteTable(contactsPath, contacts.ToString());
        }
    }
}