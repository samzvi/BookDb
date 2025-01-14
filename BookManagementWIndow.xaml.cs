using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FirebirdSql.Data.FirebirdClient;

namespace BookDb
{
    public partial class BookManagementWindow : Window
    {
        private readonly string _connectionString;
        private readonly int? _bookId;
        private readonly bool _isEditMode;

        public BookManagementWindow(string connectionString, int? bookId = null)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _bookId = bookId;
            _isEditMode = bookId.HasValue;

            LoadComboBoxOptions();

            if (_isEditMode)
            {
                LoadBookDetails();
                Title = "Upravit knihu";
                TitleLabel.Content = "Upravit knihu";
            }
            else
            {
                Title = "Přidat knihu";
                TitleLabel.Content = "Přidat knihu";
                ToggleDetailFields(false);
            }
        }

        private void ToggleDetailFields(bool isVisible)
        {
            Visibility visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            CurrentPageTextBox.Visibility = visibility;
            TotalReadsTextBox.Visibility = visibility;
            RatingTextBox.Visibility = visibility;
            CurrentPageLabel.Visibility = visibility;
            TotalReadsLabel.Visibility = visibility;
            RatingLabel.Visibility = visibility;
            TotalPagesTextBox.Visibility = Visibility.Visible;
            TotalPagesLabel.Visibility = Visibility.Visible;
        }

        private void LoadBookDetails()
        {
            try
            {
                using (FbConnection connection = new FbConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"
                                SELECT 
                                    title, 
                                    author_id, 
                                    total_pages, 
                                    current_page, 
                                    total_reads, 
                                    rating, 
                                    acquirement_date, 
                                    publisher_id, 
                                    keywords, 
                                    description, 
                                    notes
                                FROM Book 
                                WHERE id = @Id";

                    FbCommand command = new FbCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", _bookId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            TitleTextBox.Text = reader["title"].ToString();
                            AuthorComboBox.SelectedValue = reader["author_id"];
                            AcquirementDatePicker.SelectedDate = reader["acquirement_date"] != DBNull.Value
                                ? Convert.ToDateTime(reader["acquirement_date"])
                                : (DateTime?)null;
                            CurrentPageTextBox.Text = reader["current_page"].ToString();
                            TotalPagesTextBox.Text = reader["total_pages"].ToString();
                            TotalReadsTextBox.Text = reader["total_reads"].ToString();
                            RatingTextBox.Text = reader["rating"].ToString();
                            PublisherComboBox.SelectedValue = reader["publisher_id"];
                            KeywordsTextBox.Text = reader["keywords"].ToString();
                            DescriptionTextBox.Text = reader["description"].ToString();
                            NotesTextBox.Text = reader["notes"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání údajů!\n Error: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void LoadComboBoxOptions()
        {
            try
            {
                using (FbConnection connection = new FbConnection(_connectionString))
                {
                    connection.Open();
                    LoadComboBoxData(connection, AuthorComboBox, "SELECT id, name || ' ' || surname AS full_name FROM Author");
                    LoadComboBoxData(connection, PublisherComboBox, "SELECT id, name FROM Publisher");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($" Chyba při načítání možností!\n Error: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    string query = _isEditMode ? @"
                            UPDATE Book SET 
                                title = @Title, 
                                author_id = @Author_Id, 
                                total_pages = @Total_Pages, 
                                current_page = @Current_Page, 
                                total_reads = @Total_Reads, 
                                rating = @Rating, 
                                acquirement_date = @Acquirement_Date, 
                                publisher_id = @Publisher_Id, 
                                keywords = @Keywords, 
                                description = @Description, 
                                notes = @Notes
                            WHERE id = @Id"
                            : @"
                            INSERT INTO Book (title, author_id, total_pages, current_page, total_reads, rating, acquirement_date, publisher_id, keywords, description, notes)
                                VALUES (@Title, @Author_Id, @Total_Pages, @Current_Page, @Total_Reads, @Rating, @Acquirement_Date, @Publisher_Id, @Keywords, @Description, @Notes)";

                    FbCommand command = new FbCommand(query, connection);
                    command.Parameters.AddWithValue("@Title", TitleTextBox.Text);
                    command.Parameters.AddWithValue("@Author_Id", AuthorComboBox.SelectedValue);
                    command.Parameters.AddWithValue("@Total_Pages", TotalPagesTextBox.Text);
                    command.Parameters.AddWithValue("@Current_Page", CurrentPageTextBox.Text);
                    command.Parameters.AddWithValue("@Total_Reads", TotalReadsTextBox.Text);
                    command.Parameters.AddWithValue("@Rating", RatingTextBox.Text);
                    command.Parameters.AddWithValue("@Acquirement_Date", AcquirementDatePicker.SelectedDate.HasValue
                        ? (object)AcquirementDatePicker.SelectedDate.Value
                        : DBNull.Value);
                    command.Parameters.AddWithValue("@Publisher_Id", PublisherComboBox.SelectedValue);
                    command.Parameters.AddWithValue("@Keywords", KeywordsTextBox.Text);
                    command.Parameters.AddWithValue("@Description", DescriptionTextBox.Text);
                    command.Parameters.AddWithValue("@Notes", NotesTextBox.Text);

                    if (_isEditMode)
                        command.Parameters.AddWithValue("@Id", _bookId);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Úspěšně uloženo!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při ukládání!\n Error: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
