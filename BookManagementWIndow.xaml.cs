using System.Windows;
using BookDb.Models;
using BookDb.Classes;

namespace BookDb
{
    public partial class BookManagementWindow : Window
    {

        private readonly bool _isEditMode;
        private readonly AuthorModel _authorModel = new();
        private readonly PublisherModel _publisherModel = new();
        private readonly StateModel _stateModel = new();
        private Book? _temporaryBook;


        public BookManagementWindow(string connectionString, bool isEditMode, Book? originalBook = null)
        {
            InitializeComponent();

            _isEditMode = isEditMode;

            
            
            LoadComponents(originalBook);
        }


        private void LoadComponents(Book? originalBook)
        {
            if (_isEditMode)
            {
                _temporaryBook = new Book
                {
                    Id = originalBook.Id,
                    Title = originalBook.Title,
                    AuthorId = originalBook.AuthorId,
                    PublisherId = originalBook.PublisherId,
                    AcquirementDate = originalBook.AcquirementDate,
                    TotalPages = originalBook.TotalPages,
                    CurrentPage = originalBook.CurrentPage,
                    TotalReads = originalBook.TotalReads,
                    Rating = originalBook.Rating,
                    Keywords = originalBook.Keywords,
                    Notes = originalBook.Notes,
                    Description = originalBook.Description,
                    ReadingState = originalBook.ReadingState,
                    OwnershipState = originalBook.OwnershipState
                };

                DataContext = _temporaryBook;


                Title = "Upravit knihu";
                TitleLabel.Content = "Upravit knihu";
                DeleteButton.Visibility = Visibility.Visible;
            }
            else
            {
                _temporaryBook = new Book();
                DataContext = _temporaryBook;

                Title = "Přidat knihu";
                TitleLabel.Content = "Přidat knihu";
                DeleteButton.Visibility = Visibility.Collapsed;

            }

            IsRatedCheckBox.IsChecked = _temporaryBook.Rating is null ? false : true;
            IsRatedHelper();

            AuthorComboBox.ItemsSource = _authorModel.Authors;
            PublisherComboBox.ItemsSource = _publisherModel.Publishers;
            OwnershipStateComboBox.ItemsSource = _stateModel.OwnershipStates;
            ReadingStateComboBox.ItemsSource = _stateModel.ReadingStates;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (_temporaryBook is not null)
            {
                BookModel bookModel = new();
                bool saveSuccess;

                _temporaryBook.Rating = IsRatedCheckBox.IsChecked is true ? _temporaryBook.Rating : null;

                if (_isEditMode)
                    saveSuccess = bookModel.UpdateBook(_temporaryBook);
                else
                    saveSuccess = bookModel.AddNewBook(_temporaryBook);

                if (saveSuccess)
                {
                    if (_isEditMode)
                    {
                        MessageBox.Show(
                            $"Změny na knížce '{_temporaryBook.Title}' byly úspěšně uloženy!",
                            "Úspěch",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Nová kniha '{_temporaryBook.Title}' byla úspěšně uložena!",
                            "Úspěch",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }

                    DialogResult = true;
                    Close();
                }
                else
                    MessageBox.Show("Kniha nebyla uložena\nZkontroluj zda jsou vyžadována pole vyplněna", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_temporaryBook == null)
                return;

            MessageBoxResult result = MessageBox.Show(
                $"Opravdu chcete smazat knihu '{_temporaryBook.Title}'?",
                "Potvrzení smazání",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                BookModel bookModel = new BookModel();

                bool deleteSuccess = bookModel.DeleteBook((int)_temporaryBook.Id);

                if (deleteSuccess)
                {
                    MessageBox.Show(
                        $"Kniha '{_temporaryBook.Title}' byla úspěšně smazána!",
                        "Úspěch",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Kniha '{_temporaryBook.Title}' se nepodařilo smazat.",
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
            IsRatedHelper();
        }

        private void IsRatedHelper()
        {
            RatingSlider.IsEnabled = IsRatedCheckBox.IsChecked is true ? true : false;
        }

        private void NumericOnly_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
    }
}
