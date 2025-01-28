// EditAuthorPublisherWindow.xaml.cs
using System.Windows;
using BookDb.Models;
using BookDb.Classes;

namespace BookDb
{
    public partial class EditAuthorPublisherWindow : Window
    {
        private Author? _temporaryAuthor;
        private Publisher? _temporaryPublisher;
        private readonly bool _isEditMode;
        private bool _isAuthor;

        public EditAuthorPublisherWindow(bool isEditMode, Author? originalAuthor = null, Publisher? originalPublisher = null)
        {
            InitializeComponent();
            _isEditMode = isEditMode;
            _isAuthor = originalAuthor is not null;

            if (_isAuthor)
            {
                _temporaryAuthor = new Author
                {
                    Id = originalAuthor.Id,
                    Name = originalAuthor.Name,
                    Surname = originalAuthor.Surname
                };
                DataContext = _temporaryAuthor;
            }

            else
            {
                _temporaryPublisher = new Publisher
                {
                    Id = originalPublisher.Id,
                    Name = originalPublisher.Name
                };
                DataContext = _temporaryPublisher;
            }

            SetEditElements();
        }

        private void SetEditElements()
        {
            DeleteButton.Visibility = _isEditMode ? Visibility.Visible : Visibility.Collapsed;

            SurnameLabel.Visibility = _isAuthor ? Visibility.Visible : Visibility.Collapsed;
            SurnameTextBox.Visibility = _isAuthor ? Visibility.Visible : Visibility.Collapsed;

            if (_isAuthor)
            {
                Title = _isEditMode ? "Upravit autora" : "Nový autor";
                TitleLabel.Content = _isEditMode ? "Upravit autora" : "Nový autor";
            }
            else
            {
                Title = _isEditMode ? "Upravit vydavatele" : "Nový vydavatel";
                TitleLabel.Content = _isEditMode ? "Upravit vydavatele" : "Nový vydavatel";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool isSuccessful;

            if (_isAuthor)
            {
                var model = new AuthorModel();
                isSuccessful = _isEditMode
                    ? model.Update(_temporaryAuthor)
                    : model.Add(_temporaryAuthor);
            }
            else
            {
                var model = new PublisherModel();
                isSuccessful = _isEditMode
                    ? model.Update(_temporaryPublisher)
                    : model.Add(_temporaryPublisher);
            }

            if (isSuccessful)
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
                        $"Nový {(_isAuthor ? "autor" : "vydavatel")} byl úspěšně vytvořen",
                        "Úspěch",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            else
                MessageBox.Show($"{(_isAuthor ? "Autor" : "Vydavatel")} nebyl uložen\nNeznámá chyba", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);

            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =
                MessageBox.Show($"Opravdu si přejete smazat tohto {(_isAuthor ? "autora" : "vydavatele")}?",
                                "Potvrzení smazání",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                bool? isSuccessful;
                dynamic model;

                model = _isAuthor
                    ? new AuthorModel()
                    : new PublisherModel();

                int? objectId = 0;

                objectId = _isAuthor
                    ? _temporaryAuthor.Id
                    : _temporaryPublisher.Id;

                isSuccessful = model.Delete(objectId);

                if (isSuccessful is true)
                {
                    MessageBox.Show($"{(_isAuthor ? "Autor" : "Vydavatel")} byl úspěšně smazán", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else if (isSuccessful is false)
                    MessageBox.Show($"{(_isAuthor ? "Autor" : "Vydavatel")} nebyl smazán\nNeznámá chyba", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
