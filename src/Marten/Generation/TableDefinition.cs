using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Marten.Generation
{
    public class TableDefinition
    {
        public readonly IList<TableColumn> Columns = new List<TableColumn>();

        public TableColumn PrimaryKey;

        public TableDefinition(string name, TableColumn primaryKey)
        {
            Name = name;
            PrimaryKey = primaryKey;
        }

        public string Name { get; set; }

        public void Write(StringWriter writer)
        {
            writer.WriteLine("DROP TABLE IF EXISTS {0} CASCADE;", Name);
            writer.WriteLine("CREATE TABLE {0} (", Name);

            var length = Columns.Select(x => x.Name.Length).Max() + 4;

            writer.WriteLine("    {0}{1} CONSTRAINT pk_{2} PRIMARY KEY,", PrimaryKey.Name.PadRight(length),
                PrimaryKey.Type, Name);

            Columns.Each(col =>
            {
                writer.Write("    {0}{1}", col.Name.PadRight(length), col.Type);
                if (col == Columns.Last())
                {
                    writer.WriteLine();
                }
                else
                {
                    writer.WriteLine(",");
                }
            });

            writer.WriteLine(");");
        }
    }
}