using System.Text;
using System.Windows;
using BookDb.Classes;
using FirebirdSql.Data.FirebirdClient;

namespace BookDb.Models
{
    public class AuthorModel
    {
        private readonly string? ConnectionString = ConfigHelper.GetConnectionString();

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

                string query = @"SELECT * FROM Author;";
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

        public bool Add(Author author)
        {
            try
            {
                using (var connection = new FbConnection(ConnectionString))
                {

                    string query = @"INSERT INTO Author (name, surname) VALUES (@Name, @Surname);";
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

        public bool Update(Author author)
        {
            try
            {
                using (var connection = new FbConnection(ConnectionString))
                {
                    connection.Open();

                    string query = @"UPDATE Author SET 
                                        name = @Name, 
                                        surname = @Surname
                                    WHERE id = @Id;";
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

        public bool? Delete(int? authorId)
        {
            try
            {
                using (FbConnection connection = new FbConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT title FROM Book WHERE author_id = @AuthorId";
                    FbCommand command = new(query, connection);
                    command.Parameters.AddWithValue("@AuthorId", authorId);

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
                            MessageBox.Show($"Autor je přiřazen k některým knihám!\nJeho smazáním dojde ke smazání těchto knih:\n\n" +
                                            $"{titles}\nPřejete si pokračovat?",
                                            "Autor s referencí",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Warning);
                            if (result == MessageBoxResult.No)
                                return null;
                        }

                        query = "DELETE FROM Author WHERE id = @AuthorId";
                        command = new FbCommand(query, connection);
                        command.Parameters.AddWithValue("@AuthorId", authorId);
                        command.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch { return false; }
        }
    }
}
