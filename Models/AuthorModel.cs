// AuthorModel.cs
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;

namespace BookDb.Models
{
    public class AuthorModel
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
            Authors.Clear();
            using (var connection = new FbConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Author;";
                using (var command = new FbCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Authors.Add(new Author
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Surname = reader.GetString(reader.GetOrdinal("surname"))
                            });
                        }
                    }
                }
            }
        }

        public bool AddAuthor(Author author)
        {
            try
            {
                using (var connection = new FbConnection(ConnectionString))
                {

                    string query = "INSERT INTO Author (name, surname) VALUES (@Name, @Surname);";
                    using (var command = new FbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", author.Name);
                        command.Parameters.AddWithValue("@Surname", author.Surname);
                        
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch { return false; }
        }

        public bool UpdateAuthor(Author author)
        {
            try
            {
                using (var connection = new FbConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "UPDATE Author SET name = @Name, surname = @Surname WHERE id = @Id;";
                    using (var command = new FbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", author.Name);
                        command.Parameters.AddWithValue("@Surname", author.Surname);
                        command.Parameters.AddWithValue("@Id", author.Id);
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch { return false; }
        }
    }
}
