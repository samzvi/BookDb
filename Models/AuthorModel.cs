using System.Data;
using FirebirdSql.Data.FirebirdClient;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookDb.Models
{
    class AuthorModel
    {
        private readonly string ConnectionString = "User=SYSDBA;Password=masterkey;Database=D:\\fbdata\\BOOKSDB.fdb;DataSource=localhost;Port=3050;Charset=UTF8;";

        public List<Author> Authors { get; set; }

        public AuthorModel()
        {
            Authors = new List<Author>();
            FetchAuthors();
        }

        public void FetchAuthors()
        {
            using (FbConnection connection = new FbConnection(ConnectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM Author;";

                FbCommand command = new FbCommand(query, connection);
                FbDataAdapter adapter = new FbDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    Author author = new()
                    {
                        Id = row["id"] as int?,
                        Name = row["name"] as string,
                        Surname = row["surname"] as string
                    };

                    Authors.Add(author);
                }
            }
        }
    }
}
