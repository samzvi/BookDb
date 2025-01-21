using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace BookDb.Models
{
    class PublisherModel
    {
        private readonly string ConnectionString = "User=SYSDBA;Password=masterkey;Database=D:\\fbdata\\BOOKSDB.fdb;DataSource=localhost;Port=3050;Charset=UTF8;";

        public List<Publisher> Publishers { get; set; }

        public PublisherModel()
        {
            Publishers = new List<Publisher>();
            FetchPublishers();
        }

        public void FetchPublishers()
        {
            using (FbConnection connection = new FbConnection(ConnectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM Publisher;";

                FbCommand command = new FbCommand(query, connection);
                FbDataAdapter adapter = new FbDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    Publisher publisher = new()
                    {
                        Id = row["id"] as int?,
                        Name = row["name"] as string
                    };

                    Publishers.Add(publisher);
                }
            }
        }
    }
}
