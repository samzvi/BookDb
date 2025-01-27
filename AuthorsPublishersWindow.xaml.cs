// AuthorsPublishersWindow.xaml.cs
using System.Windows;
using BookDb.Models;

namespace BookDb
{
    public partial class AuthorsPublishersWindow : Window
    {
        private readonly AuthorModel _authorModel = new();
        private readonly PublisherModel _publisherModel = new();

        public AuthorsPublishersWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            AuthorsDataGrid.ItemsSource = null;
            AuthorsDataGrid.ItemsSource = _authorModel.Authors;

            PublishersDataGrid.ItemsSource = null;
            PublishersDataGrid.ItemsSource = _publisherModel.Publishers;
        }

        private void EditAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsDataGrid.SelectedItem is Author selectedAuthor)
            {
                var editWindow = new EditAuthorPublisherWindow(originalAuthor: selectedAuthor, isEditMode: true);
                if (editWindow.ShowDialog() == true)
                {
                    _authorModel.FetchAuthors();
                    LoadData();
                }
            }
        }

        private void EditPublisherButton_Click(object sender, RoutedEventArgs e)
        {
            if (PublishersDataGrid.SelectedItem is Publisher selectedPublisher)
            {
                var editWindow = new EditAuthorPublisherWindow(originalPublisher: selectedPublisher, isEditMode: true);
                if (editWindow.ShowDialog() == true)
                {
                    _publisherModel.FetchPublishers();
                    LoadData();
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
