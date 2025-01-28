// AuthorsPublishersWindow.xaml.cs
using System.Windows;
using BookDb.Models;
using BookDb.Classes;

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
            _authorModel.FetchAuthors();
            _publisherModel.FetchPublishers();

            AuthorsDataGrid.ItemsSource = null;
            PublishersDataGrid.ItemsSource = null;

            AuthorsDataGrid.ItemsSource = _authorModel.Authors;
            PublishersDataGrid.ItemsSource = _publisherModel.Publishers;
        }

        private void EditAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsDataGrid.SelectedItem is Author selectedAuthor)
            {
                var editWindow = new EditAuthorPublisherWindow(originalAuthor: selectedAuthor, isEditMode: true);
                editWindow.ShowDialog();
            }
            LoadData();
        }


        private void EditPublisherButton_Click(object sender, RoutedEventArgs e)
        {
            if (PublishersDataGrid.SelectedItem is Publisher selectedPublisher)
            {
                var editWindow = new EditAuthorPublisherWindow(originalPublisher: selectedPublisher, isEditMode: true);
                editWindow.ShowDialog();
            }
            LoadData();
        }


        private void AddAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditAuthorPublisherWindow(originalAuthor: new Author(),isEditMode: false);
            editWindow.ShowDialog();

            LoadData();
        }


        private void AddPublisherButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditAuthorPublisherWindow(originalPublisher: new Publisher(), isEditMode: false);
            editWindow.ShowDialog();

            LoadData();

        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
