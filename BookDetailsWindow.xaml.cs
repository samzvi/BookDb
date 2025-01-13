using System;
using System.Windows;
using FirebirdSql.Data.FirebirdClient;

namespace BookDb
{
    public partial class BookDetailsWindow : Window
    {
        private readonly int _bookId;
        private readonly string _connectionString;

        public BookDetailsWindow(int bookId, string connectionString)
        {
            InitializeComponent();
            _bookId = bookId;
            _connectionString = connectionString;
            LoadBookDetails();
        }

        private void LoadBookDetails()
        {
            try
            {
                using (FbConnection connection = new FbConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"SELECT 
                                        b.title, 
                                        b.author_id, 
                                        b.release_date, 
                                        b.pages, 
                                        p.name AS publisher, 
                                        b.keywords, 
                                        b.description 
                                    FROM 
                                        Book b
                                    JOIN 
                                        Publisher p ON b.publisher_id = p.id
                                    WHERE 
                                        b.id = @Id";
                    FbCommand bookCommand = new FbCommand(query, connection);
                    bookCommand.Parameters.AddWithValue("@Id", _bookId);

                    using (var reader = bookCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            TitleTextBox.Text = reader["title"].ToString();
                            AuthorComboBox.SelectedValue = reader["author_id"];
                            ReleaseDateTextBox.Text = reader["release_date"].ToString();
                            PagesTextBox.Text = reader["pages"].ToString();
                            PublisherTextBox.Text = reader["publisher"].ToString();
                            KeywordsTextBox.Text = reader["keywords"].ToString();
                            DescriptionTextBox.Text = reader["description"].ToString();
                        }
                    }
                    // Load authors
                    string authorsQuery = "SELECT id, name || ' ' || surname AS full_name FROM Author";
                    FbCommand authorsCommand = new FbCommand(authorsQuery, connection);

                    var authorsDictionary = new Dictionary<int, string>();
                    using (var authorsReader = authorsCommand.ExecuteReader())
                    {
                        while (authorsReader.Read())
                        {
                            int authorId = authorsReader.GetInt32(0);
                            string fullName = authorsReader.GetString(1);
                            authorsDictionary[authorId] = fullName;
                        }
                        AuthorComboBox.ItemsSource = authorsDictionary.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání údajů o knize: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (FbConnection connection = new FbConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE Book 
                                    SET 
                                        title = @Title,
                                        author_id = @Author_Id,
                                        release_date = @Release_Date, 
                                        pages = @Pages, 
                                        keywords = @Keywords, 
                                        description = @Description
                                    WHERE 
                                        id = @Id";

                    FbCommand command = new FbCommand(query, connection);
                    command.Parameters.AddWithValue("@Title", TitleTextBox.Text);
                    command.Parameters.AddWithValue("@Author_Id", AuthorComboBox.SelectedValue);
                    command.Parameters.AddWithValue("@Release_Date", ReleaseDateTextBox.Text);
                    command.Parameters.AddWithValue("@Pages", PagesTextBox.Text);
                    command.Parameters.AddWithValue("@Keywords", KeywordsTextBox.Text);
                    command.Parameters.AddWithValue("@Description", DescriptionTextBox.Text);
                    command.Parameters.AddWithValue("@Id", _bookId);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Údaje o knize byly úspěšně uloženy.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při ukládání údajů o knize: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
