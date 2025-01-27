using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using BookDb.Converters;
using BookDb.Models;

namespace BookDb
{
    public partial class MainWindow : Window
    {
        private readonly string dbPath = @"D:/fbdata/BOOKSDB.fdb";
        private readonly string dbInitPath = @"..\..\..\db\DbInitialize.bat";
        private readonly string connectionString = "User=SYSDBA;Password=masterkey;Database=D:\\fbdata\\BOOKSDB.fdb;DataSource=localhost;Port=3050;Charset=UTF8;";
        private BookModel? bookModel;
        private StateModel? stateModel;

        public MainWindow()
        {
            InitializeDatabase();

            InitializeComponent();

            bookModel = new BookModel();
            stateModel = new StateModel();

            LoadDefaultView();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                MessageBoxResult result = MessageBox.Show(
                    "Database není vytvořena, chcete spustit pokus o vytvoření?",
                    "Chyba",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                {
                    Process.Start(dbInitPath);
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }

        public void LoadDefaultView()
        {
            bookModel.FetchBooks();
            var booksWithDetails = bookModel.Books.Select(book => new
            {
                book.Id,
                book.Title,
                AuthorName = $"{book.Author.Name} {book.Author.Surname}",
                PublisherName = book.Publisher.Name,
                AcquirementDate = book.AcquirementDate == null
                                ? "Neznámé"
                                : book.AcquirementDate?.ToString("d"),
                OnPageTotal = book.CurrentPage == null
                                ? $"0 z {book.TotalPages}"
                                : $"{book.CurrentPage} z {book.TotalPages}",
                TotalReads = book.TotalReads == null
                                ? "Nepřečteno"
                                : book.TotalReads.ToString(),
                Rating = (book.Rating == null || book.Rating == 0) ? "Nehodnoceno" : $"{book.Rating} z 10",
                ReadingColor = stateModel.ReadingStates.FirstOrDefault(state => state.Id == book.ReadingState).Color,
                ReadingName = stateModel.ReadingStates.FirstOrDefault(state => state.Id == book.ReadingState).Name,
                OwnershipColor = stateModel.OwnershipStates.FirstOrDefault(state => state.Id == book.OwnershipState).Color,
                OwnershipName = stateModel.OwnershipStates.FirstOrDefault(state => state.Id == book.OwnershipState).Name,
                book.Keywords,
                book.Description,
                book.Notes
            }).ToList(); BooksDataGrid.ItemsSource = null;

            BooksDataGrid.ItemsSource = booksWithDetails;
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            var addBookWindow = new BookManagementWindow(connectionString, isEditMode: false);
            bool? dialogResult = addBookWindow.ShowDialog();

            if (dialogResult == true)
            {
                LoadDefaultView();
            }
        }

        private void AuthorsPublishersButton_Click(object sender, RoutedEventArgs e)
        {
            var autPubWindow = new AuthorsPublishersWindow();
            bool? dialogResult = autPubWindow.ShowDialog();

            if (dialogResult == true)
            {
                LoadDefaultView();
            }
        }

        private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = BooksDataGrid.SelectedItem;

            int bookId = (int)selectedRow.GetType().GetProperty("Id").GetValue(selectedRow);

            var detailsWindow = new BookManagementWindow(connectionString, isEditMode: true, bookModel.Books.FirstOrDefault(book => book.Id == bookId));
            bool? dialogResult = detailsWindow.ShowDialog();

            if (dialogResult == true)
                LoadDefaultView();
        }
    }
}
