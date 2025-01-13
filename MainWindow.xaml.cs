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
                                        b.id AS book_id,
                                        b.title,
                                        a.name || ' ' || a.surname AS author_name,
                                        p.name AS publisher_name,
                                        b.acquirement_date,
                                        b.total_pages,
                                        b.on_page,
                                        b.times_read,
                                        b.rating,
                                        b.keywords,
                                        b.description,
                                        b.on_page || ' z ' || b.total_pages AS ON_PAGE_TOTAL
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
                MessageBoxResult result = MessageBox.Show($" Chyba při připojování k databázi, chcete opakovat pokus?\n Error: {ex.Message}", "CHYBA", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    LoadBooks();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    Application.Current.Shutdown();
                }
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
