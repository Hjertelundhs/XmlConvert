using System.Xml.Linq;

class Program
{
    static void Main()
    {

        var path = "input.txt";

        if (!File.Exists(path))
        {
            Console.WriteLine($"Filen hittades inte: {Path.GetFullPath(path)}");
            return;
        }
        var lines = File.ReadAllLines("input.txt");

        var people = new XElement("people");

        XElement currentPerson = null;
        XElement currentFamily = null;

        foreach (var line in lines)
        {
            var parts = line.Split('|');
            var type = parts[0];

            switch (type)
            {
                case "P":
                    currentPerson = new XElement("person",
                        new XElement("firstname", parts[1]),
                        new XElement("lastname", parts[2])
                    );
                    people.Add(currentPerson);
                    currentFamily = null;
                    break;

                case "T":
                    var phone = new XElement("phone",
                        new XElement("mobile", parts[1]),
                        new XElement("landline", parts.Length > 2 ? parts[2] : "")
                    );

                    if (currentFamily != null)
                        currentFamily.Add(phone);
                    else
                        currentPerson.Add(phone);
                    break;

                case "A":
                    var address = new XElement("address",
                        new XElement("street", parts[1]),
                        new XElement("city", parts[2]),
                        new XElement("postalcode", parts.Length > 3 ? parts[3] : "")
                    );

                    if (currentFamily != null)
                        currentFamily.Add(address);
                    else
                        currentPerson.Add(address);
                    break;

                case "F":
                    currentFamily = new XElement("family",
                        new XElement("name", parts[1]),
                        new XElement("born", parts[2])
                    );

                    currentPerson.Add(currentFamily);
                    break;
            }
        }

        var doc = new XDocument(people);

        doc.Save("output.xml");

        Console.WriteLine("XML skapad!");
    }
}