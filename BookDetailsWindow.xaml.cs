using System;
using System.Windows;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Linq;

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
                                        b.total_pages, 
                                        b.on_page,
                                        b.times_read,
                                        b.rating,
                                        b.acquirement_date,
                                        b.publisher_id,
                                        b.keywords, 
                                        b.description, 
                                        b.notes
                                    FROM 
                                        Book b
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
                            AcquirementDateTextBox.Text = reader["acquirement_date"].ToString();
                            OnPageTextBox.Text = reader["on_page"].ToString();
                            TotalPagesTextBox.Text = reader["total_pages"].ToString();
                            TimesReadTextBox.Text = reader["times_read"].ToString();
                            RatingTextBox.Text = reader["rating"].ToString();
                            PublisherComboBox.SelectedValue = reader["publisher_id"];
                            KeywordsTextBox.Text = reader["keywords"].ToString();
                            DescriptionTextBox.Text = reader["description"].ToString();
                            NotesTextBox.Text = reader["notes"].ToString();
                        }
                    }

                    LoadComboBoxData(connection, AuthorComboBox, "SELECT id, name || ' ' || surname AS full_name FROM Author");
                    LoadComboBoxData(connection, PublisherComboBox, "SELECT id, name FROM Publisher");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání údajů o knize: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void LoadComboBoxData(FbConnection connection, System.Windows.Controls.ComboBox comboBox, string query)
        {
            var dictionary = new Dictionary<int, string>();
            using (var command = new FbCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    dictionary[id] = name;
                }
            }
            comboBox.ItemsSource = dictionary.ToList();
            comboBox.DisplayMemberPath = "Value";
            comboBox.SelectedValuePath = "Key";
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
                                        total_pages = @Total_Pages, 
                                        on_page = @OnPage,
                                        times_read = @TimesRead,
                                        rating = @Rating,
                                        acquirement_date = @AcquirementDate,
                                        publisher_id = @Publisher_Id,
                                        keywords = @Keywords, 
                                        description = @Description,
                                        notes = @Notes
                                    WHERE 
                                        id = @Id";

                    FbCommand command = new FbCommand(query, connection);
                    command.Parameters.AddWithValue("@Title", TitleTextBox.Text);
                    command.Parameters.AddWithValue("@Author_Id", AuthorComboBox.SelectedValue);
                    command.Parameters.AddWithValue("@Total_Pages", TotalPagesTextBox.Text);
                    command.Parameters.AddWithValue("@OnPage", OnPageTextBox.Text);
                    command.Parameters.AddWithValue("@TimesRead", TimesReadTextBox.Text);
                    command.Parameters.AddWithValue("@Rating", RatingTextBox.Text);
                    command.Parameters.AddWithValue("@AcquirementDate", AcquirementDateTextBox.Text);
                    command.Parameters.AddWithValue("@Publisher_Id", PublisherComboBox.SelectedValue);
                    command.Parameters.AddWithValue("@Keywords", KeywordsTextBox.Text);
                    command.Parameters.AddWithValue("@Description", DescriptionTextBox.Text);
                    command.Parameters.AddWithValue("@Notes", NotesTextBox.Text);
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
