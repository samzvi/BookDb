using System.Data;
using System.Windows;
using FirebirdSql.Data.FirebirdClient;


namespace BookDb
{
    public partial class MainWindow : Window
    {
        private readonly string connectionString = "User=SYSDBA;Password=masterkey;Database=D:\\fbdata\\BOOKSDB.fdb;DataSource=localhost;Port=3050;Charset=UTF8;";
        public MainWindow()
        {
            InitializeComponent();
            LoadBooks();
        }


        public void LoadBooks()
        {
            try
            {
                using (FbConnection connection = new FbConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT 
                                        b.id AS Book_Id,
                                        b.title AS Title,
                                        a.name || ' ' || a.surname AS Author_Name,
                                        b.release_date AS Release_Date,
                                        b.pages AS Pages,
                                        p.name AS Publisher_Name,
                                        b.keywords AS Keywords,
                                        b.description AS Description
                                    FROM 
                                        Book b
                                    JOIN 
                                        Author a ON b.author_id = a.id
                                    JOIN 
                                        Publisher p ON b.publisher_id = p.id";

                    FbCommand command = new FbCommand(query, connection);
                    FbDataAdapter adapter = new FbDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    BooksDataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání knih: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateAuthorButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is DataRowView selectedRow)
            {
                int bookId = Convert.ToInt32(selectedRow["BOOK_ID"]);
                var detailsWindow = new BookDetailsWindow(bookId, connectionString);

                bool? dialogResult = detailsWindow.ShowDialog();

                if (dialogResult == true)
                    LoadBooks();
            }
            else
            {
                MessageBox.Show("Vyber knihu pro zobrazeni informaci.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}