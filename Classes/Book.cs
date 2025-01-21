using System;
using System.ComponentModel;
using BookDb.Models;

namespace BookDb
{
    public class Book : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int? _id;
        public int? Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string? _title;
        public string? Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private DateTime? _acquirementDate;
        public DateTime? AcquirementDate
        {
            get { return _acquirementDate; }
            set
            {
                if (_acquirementDate != value)
                {
                    _acquirementDate = value;
                    OnPropertyChanged(nameof(AcquirementDate));
                }
            }
        }

        private int? _totalPages;
        public int? TotalPages
        {
            get { return _totalPages; }
            set
            {
                if (_totalPages != value)
                {
                    _totalPages = value;
                    OnPropertyChanged(nameof(TotalPages));
                }
            }
        }

        private int? _currentPage;
        public int? CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                }
            }
        }

        private int? _totalReads;
        public int? TotalReads
        {
            get { return _totalReads; }
            set
            {
                if (_totalReads != value)
                {
                    _totalReads = value;
                    OnPropertyChanged(nameof(TotalReads));
                }
            }
        }

        private int? _rating;
        public int? Rating
        {
            get { return _rating; }
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }

        private string? _keywords;
        public string? Keywords
        {
            get { return _keywords; }
            set
            {
                if (_keywords != value)
                {
                    _keywords = value;
                    OnPropertyChanged(nameof(Keywords));
                }
            }
        }

        private string? _notes;
        public string? Notes
        {
            get { return _notes; }
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    OnPropertyChanged(nameof(Notes));
                }
            }
        }

        private string? _description;
        public string? Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private int? _authorId;
        public int? AuthorId
        {
            get { return _authorId; }
            set
            {
                if (_authorId != value)
                {
                    _authorId = value;
                    OnPropertyChanged(nameof(AuthorId));
                    
                    if (_authorId.HasValue)
                    {
                        AuthorModel authorModel = new AuthorModel();
                        Author = authorModel.Authors.FirstOrDefault(author => author.Id == _authorId);
                    }
                }
            }
        }

        private Author? _author;
        public Author? Author
        {
            get { return _author; }
            set
            {
                if (_author != value)
                {
                    _author = value;
                    OnPropertyChanged(nameof(Author));
                }
            }
        }

        private int? _publisherId;
        public int? PublisherId
        {
            get { return _publisherId; }
            set
            {
                if (_publisherId != value)
                {
                    _publisherId = value;
                    OnPropertyChanged(nameof(PublisherId));
                    
                    if (_publisherId.HasValue)
                    {
                        PublisherModel publisherModel = new PublisherModel();
                        Publisher = publisherModel.Publishers.FirstOrDefault(publisher => publisher.Id == _publisherId);
                    }
                }
            }
        }

        private Publisher? _publisher;
        public Publisher? Publisher
        {
            get { return _publisher; }
            set
            {
                if (_publisher != value)
                {
                    _publisher = value;
                    OnPropertyChanged(nameof(Publisher));
                }
            }
        }

        private int _readingState;
        public int ReadingState
        {
            get { return _readingState; }
            set
            {
                if (_readingState != value)
                {
                    _readingState = value;
                    OnPropertyChanged(nameof(ReadingState));
                }
            }
        }

        private int _ownershipState;
        public int OwnershipState
        {
            get { return _ownershipState; }
            set
            {
                if (_ownershipState != value)
                {
                    _ownershipState = value;
                    OnPropertyChanged(nameof(OwnershipState));
                }
            }
        }
    }
}
