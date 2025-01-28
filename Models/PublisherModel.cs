// PublisherModel.cs
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows;

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

                string query = @"SELECT * FROM Publisher;";
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

        public bool Add(Publisher? publisher)
        {
            try
            {
                using (var connection = new FbConnection(ConnectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO Publisher (name) VALUES (@Name);";
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

        public bool Update(Publisher? publisher)
        {
            try
            {
                using (var connection = new FbConnection(ConnectionString))
                {
                    connection.Open();

                    string query = @"UPDATE Publisher SET name = @Name WHERE id = @Id;";
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

        public bool? Delete(int? publisherId)
        {
            try
            {
                using (FbConnection connection = new FbConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT title FROM Book WHERE publisher_id = @PublisherId";

                    FbCommand command = new(query, connection);
                    command.Parameters.AddWithValue("@PublisherId", publisherId);

                    using (FbDataReader reader = command.ExecuteReader())
                    {
                        StringBuilder titles = new StringBuilder();

                        while (reader.Read())
                        {
                            titles.AppendLine($"- '{reader.GetString(0)}'");
                        }


                        if (titles.Length > 0)
                        {
                            MessageBoxResult result =
                            MessageBox.Show($"Vydavatel je přiřazen k některým knihám!\nJeho smazáním dojde ke smazání těchto knih:\n\n" +
                                            $"{titles}\nPřejete si pokračovat?",
                                            "Vydavatel s referencí",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Warning);
                            if (result == MessageBoxResult.No)
                                return null;
                        }

                        query = "DELETE FROM Publisher WHERE id = @PublisherId";
                        command = new FbCommand(query, connection);
                        command.Parameters.AddWithValue("@PublisherId", publisherId);
                        command.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch { return false; }
        }
    }
}
