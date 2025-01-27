// PublisherModel.cs
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;

namespace BookDb.Models
{
    public class PublisherModel
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
            Publishers.Clear();
            using (var connection = new FbConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Publisher;";
                using (var command = new FbCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Publishers.Add(new Publisher
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name"))
                            });
                        }
                    }
                }
            }
        }

        public bool AddPublisher(Publisher publisher)
        {
            try
            {
                using (var connection = new FbConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Publisher (name) VALUES (@Name);";
                    using (var command = new FbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", publisher.Name);
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch { return false; }
        }

        public bool UpdatePublisher(Publisher publisher)
        {
            try
            {
                using (var connection = new FbConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE Publisher SET name = @Name WHERE id = @Id;";
                    using (var command = new FbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", publisher.Name);
                        command.Parameters.AddWithValue("@Id", publisher.Id);
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch { return false; }
        }
    }
}
