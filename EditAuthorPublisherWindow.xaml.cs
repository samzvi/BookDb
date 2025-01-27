// EditAuthorPublisherWindow.xaml.cs
using System.Windows;
using BookDb.Models;

namespace BookDb
{
    public partial class EditAuthorPublisherWindow : Window
    {
        private Author? _temporaryAuthor;
        private Publisher? _temporaryPublisher;
        private readonly bool _isEditMode;

        public EditAuthorPublisherWindow(bool isEditMode, Author? originalAuthor = null, Publisher? originalPublisher = null)
        {
            InitializeComponent();
            _isEditMode = isEditMode;

            if (originalAuthor != null)
            {
                _temporaryAuthor = new Author
                {
                    Id = originalAuthor.Id,
                    Name = originalAuthor.Name,
                    Surname = originalAuthor.Surname
                };
                DataContext = _temporaryAuthor;
                Title = isEditMode ? "Nový autor" : "Upravit autora";

                SurnameLabel.Visibility = Visibility.Visible;
                SurnameTextBox.Visibility = Visibility.Visible;
            }
            else if (originalPublisher != null)
            {
                _temporaryPublisher = new Publisher
                {
                    Id = originalPublisher.Id,
                    Name = originalPublisher.Name
                };
                DataContext = _temporaryPublisher;
                Title = isEditMode ? "Nový vydavatel" : "Upravit vydavatele";

                SurnameLabel.Visibility = Visibility.Collapsed;
                SurnameTextBox.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool saveSuccess;

            if (_temporaryAuthor is not null)
            {
                AuthorModel authorModel = new();
        
                if (_isEditMode)
                    saveSuccess = authorModel.AddAuthor(_temporaryAuthor);
                else
                    saveSuccess = authorModel.UpdateAuthor(_temporaryAuthor);

                if (saveSuccess)
                {
                    if (_isEditMode)
                    {
                        MessageBox.Show(
                            $"Změny byly úspěšně uloženy!",
                            "Úspěch",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Nový autor byl úspěšně vytvořen",
                            "Úspěch",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }
                else
                    MessageBox.Show("Autor nebyl uložen\nNeznámá chyba", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            if (_temporaryPublisher is not null)
            {
                PublisherModel PublisherModel = new();

                if (_isEditMode)
                    saveSuccess = PublisherModel.AddPublisher(_temporaryPublisher);
                else
                    saveSuccess = PublisherModel.UpdatePublisher(_temporaryPublisher);

                if (saveSuccess)
                {
                    if (_isEditMode)
                    {
                        MessageBox.Show(
                            $"Změny byly úspěšně uloženy!",
                            "Úspěch",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Nový autor byl úspěšně vytvořen",
                            "Úspěch",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    DialogResult = true;
                }
                else
                {
                    DialogResult = false;
                    MessageBox.Show("Autor nebyl uložen\nNeznámá chyba", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
