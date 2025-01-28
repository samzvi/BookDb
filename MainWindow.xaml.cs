using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using BookDb.Classes;
using BookDb.Converters;
using BookDb.Models;

namespace BookDb
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly string connectionString = ConfigHelper.GetConnectionString();
        private readonly string dbPath = ConfigHelper.GetDbPath();
        private readonly string dbInitPath = Path.GetFullPath(ConfigHelper.GetDbInitPath()); 
        
        private BookModel bookModel;
        private StateModel stateModel;

        public readonly int PageSize = 10;

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                    OnPropertyChanged(nameof(IsPreviousButtonEnabled));
                    OnPropertyChanged(nameof(IsNextButtonEnabled));
                    OnPropertyChanged(nameof(PaginationVisibility));
                }
            }
        }

        public int TotalPages
        {
            get => (int)Math.Ceiling((double)bookModel.Books.Count / PageSize);
            private set
            {
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(IsPreviousButtonEnabled));
                OnPropertyChanged(nameof(IsNextButtonEnabled));
                OnPropertyChanged(nameof(PaginationVisibility));
            }
        }

        public bool IsPreviousButtonEnabled => CurrentPage > 1;

        public bool IsNextButtonEnabled => CurrentPage < TotalPages;

        public Visibility PaginationVisibility
            => bookModel.Books.Count > PageSize ? Visibility.Visible : Visibility.Collapsed;


        public MainWindow()
        {
            InitializeDatabase();

            InitializeComponent();

            bookModel = new BookModel();
            stateModel = new StateModel();

            DataContext = this;

            LoadDefaultView();
        }


        private void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Database není vytvořena, chcete spustit pokus o vytvoření?",
                    "Chyba",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var processInfo = new ProcessStartInfo
                    {
                        FileName = dbInitPath,
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Minimized,
                        Verb = "runas" 
                    };

                    Process.Start(processInfo).WaitForExit();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }


        public void LoadDefaultView()
        {
            bookModel.FetchBooks();

            int totalPages = (int)Math.Ceiling((double)bookModel.Books.Count / PageSize);

            if (CurrentPage > totalPages && totalPages > 0)
                CurrentPage = totalPages;

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
            })  .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            OnPropertyChanged(nameof(TotalPages));
            OnPropertyChanged(nameof(PaginationVisibility));
            OnPropertyChanged(nameof(IsNextButtonEnabled));
            OnPropertyChanged(nameof(IsPreviousButtonEnabled));

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
            autPubWindow.ShowDialog();

            LoadDefaultView();
        }


        private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            dynamic? selectedRow = (sender as Button)?.CommandParameter;

            if (selectedRow is not null)
            {
                int bookId = selectedRow.Id;
                Book? book = bookModel.Books.FirstOrDefault(book => book.Id == bookId);

                var detailsWindow = new BookManagementWindow(connectionString, isEditMode: true, book);
                detailsWindow.ShowDialog();

                LoadDefaultView();
            }
        }


        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadDefaultView();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadDefaultView();
            }
        }

        private void CellFocused(object sender, SelectedCellsChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;

            if (dataGrid?.SelectedCells.Count > 0)
            {
                var cellInfo = dataGrid.SelectedCells[0];

                if (cellInfo.Column.Header?.ToString() == "Popis" || cellInfo.Column.Header?.ToString() == "Poznámky")
                {
                    if (cellInfo.Column.GetCellContent(cellInfo.Item) is TextBlock textBlock)
                    {
                        textBlock.TextWrapping = textBlock.TextWrapping == TextWrapping.WrapWithOverflow
                            ? TextWrapping.NoWrap
                            : TextWrapping.WrapWithOverflow;

                        dataGrid.SelectedCells.Clear();
                    }
                }
            }
        }

    }
}
