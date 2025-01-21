using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using BookDb.Models;

namespace BookDb
{
    public partial class BookManagementWindow : Window
    {

        private readonly bool _isEditMode;
        private readonly AuthorModel _authorModel = new();
        private readonly PublisherModel _publisherModel = new();
        private readonly StateModel _stateModel = new();
        private Book? _book;


        public BookManagementWindow(string connectionString, bool isEditMode, Book? book = null)
        {
            InitializeComponent();

            _isEditMode = isEditMode;
            SaveButton.IsEnabled = CanSave();

            if (_isEditMode && book != null)
            {
                _book = book;
                DataContext = _book;

                Title = "Upravit knihu";
                TitleLabel.Content = "Upravit knihu";
                DeleteButton.Visibility = Visibility.Visible;
            }
            else
            {
                _book = new();
                DataContext = _book;

                Title = "Přidat knihu";
                TitleLabel.Content = "Přidat knihu";
                DeleteButton.Visibility = Visibility.Collapsed;

            }

            LoadComboBoxes();
        }


        private void LoadComboBoxes()
        {
            AuthorComboBox.ItemsSource = _authorModel.Authors;
            PublisherComboBox.ItemsSource = _publisherModel.Publishers;
            OwnershipStateComboBox.ItemsSource = _stateModel.OwnershipStates;
            ReadingStateComboBox.ItemsSource = _stateModel.ReadingStates;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Book? book = this.DataContext as Book;

            if (book is not null)
            {
                BookModel bookModel = new BookModel();
                bool saveSuccess;

                if (_isEditMode)
                    saveSuccess = bookModel.UpdateBook(book);
                else
                    saveSuccess = bookModel.AddNewBook(book);

                if (saveSuccess)
                {
                    if (_isEditMode)
                    {
                        MessageBox.Show(
                            $"Změny na knížce '{book.Title}' byly úspěšně uloženy!",
                            "Úspěch",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Nová kniha '{book.Title}' byla úspěšně uložena!",
                            "Úspěch",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }

                    DialogResult = true;
                    Close();
                }
                else
                    MessageBox.Show("Kniha nebyla uložena", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ValidateInput(object sender, RoutedEventArgs e)
        {
            SaveButton.IsEnabled = CanSave();
        }

        private bool CanSave() // todo
        {   
            return true;
        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_book == null)
                return;

            MessageBoxResult result = MessageBox.Show(
                $"Opravdu chcete smazat knihu '{_book.Title}'?",
                "Potvrzení smazání",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                BookModel bookModel = new BookModel();

                bool deleteSuccess = bookModel.DeleteBook((int)_book.Id);

                if (deleteSuccess)
                {
                    MessageBox.Show(
                        $"Kniha '{_book.Title}' byla úspěšně smazána!",
                        "Úspěch",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Kniha '{_book.Title}' se nepodařilo smazat.",
                        "Chyba",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

                DialogResult = true;
                Close();

            }
        }

        private void IsRatedCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (IsRatedCheckBox.IsChecked == true)
            {
                RatingSlider.IsEnabled = true;
            }
            else
            {
                RatingSlider.IsEnabled = false;
                _book.Rating = null;
            }
        }
    }
}
