using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using FirebirdSql.Data.FirebirdClient;


namespace BookDb
{
    public partial class MainWindow : Window
    {
        private readonly string dbPath = "D:/fbdata/BOOKSDB.fdb";
        private readonly string dbInitPath = "..\\..\\..\\db\\DbInitialize.bat";
        private readonly string connectionString = "User=SYSDBA;Password=masterkey;Database=D:\\fbdata\\BOOKSDB.fdb;DataSource=localhost;Port=3050;Charset=UTF8;";
        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists(dbPath))
            {
                MessageBoxResult result = MessageBox.Show($" Database není vytvořena, chcete spustit pokus o vytvoření?",
                "Chyba", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                    Process.Start(dbInitPath);
                else
                    Application.Current.Shutdown();
            }

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
                                        b.current_page,
                                        b.total_reads,
                                        b.rating || ' z ' || 10 as RATING,
                                        b.keywords,
                                        b.description,
                                        b.notes,
                                        rs.color AS reading_state_color,
                                        os.color AS ownership_state_color,
                                        b.current_page || ' z ' || b.total_pages AS ON_PAGE_TOTAL
                                    FROM 
                                        Book b
                                    JOIN 
                                        Author a ON b.author_id = a.id
                                    JOIN 
                                        Publisher p ON b.publisher_id = p.id
                                    LEFT JOIN 
                                        State rs ON b.reading_state = rs.id
                                    LEFT JOIN 
                                        State os ON b.ownership_state = os.id";

                    FbCommand command = new FbCommand(query, connection);
                    FbDataAdapter adapter = new FbDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    BooksDataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBoxResult result = MessageBox.Show($" Chyba při připojování k databázi, chcete opakovat pokus?\n Error: {ex.Message}",
                    "Chyba", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                    LoadBooks();
                else if (result == MessageBoxResult.No)
                    Application.Current.Shutdown();
            }
        }


        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            var addBookWindow = new BookManagementWindow(connectionString);
            bool? dialogResult = addBookWindow.ShowDialog();

            if (dialogResult == true)
            {
                LoadBooks();
            }
        }

        private void CreateAuthorButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is DataRowView selectedRow)
            {
                int bookId = Convert.ToInt32(selectedRow["BOOK_ID"]);
                var detailsWindow = new BookManagementWindow(connectionString, bookId);

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
