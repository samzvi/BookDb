using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookDb.Models;

namespace BookDb
{
    public class Book : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly Dictionary<string, Func<string?>> _validationRules;
        public Book()
        {
            _validationRules = new Dictionary<string, Func<string?>>
            {
                { nameof(Title), () => string.IsNullOrWhiteSpace(Title)
                                    ? "Název nesmí být prázdný"
                                    : (Title.Length > 255
                                        ? "Název je moc dlouhý"
                                        : null)},
                { nameof(AuthorId), () => AuthorId is null ? "Vyber autora" : null },
                { nameof(ReadingState), () => ReadingState is null ? "Vyber stav čtení" : null },
                { nameof(OwnershipState), () => OwnershipState is null ? "Vyber stav vlastnictví" : null },
                { nameof(TotalPages), () => TotalPages is null ? "Zadej kolik má kniha stran" : null },
                { nameof(PublisherId), () => PublisherId is null ? "Vyber vydavatele" : null }
            };
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public string? this[string columnName]
        {
            get => _validationRules.ContainsKey(columnName) ? _validationRules[columnName]() : null;
        }

        public string Error => string.Join(Environment.NewLine, _validationRules.Values.Select(validate => validate()).Where(error => !string.IsNullOrEmpty(error)));

        public bool CanSave => string.IsNullOrEmpty(Error);

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
        [Required]
        public string? Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                    OnPropertyChanged(nameof(CanSave));
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
                    OnPropertyChanged(nameof(CanSave));
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
                    OnPropertyChanged(nameof(CanSave));
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
                    OnPropertyChanged(nameof(CanSave));
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
                    OnPropertyChanged(nameof(CanSave));
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
                    OnPropertyChanged(nameof(CanSave));
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
                    OnPropertyChanged(nameof(CanSave));
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
                    OnPropertyChanged(nameof(CanSave));
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
                    OnPropertyChanged(nameof(CanSave));
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
                    OnPropertyChanged(nameof(CanSave));
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
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        private int? _readingState;
        public int? ReadingState
        {
            get { return _readingState; }
            set
            {
                if (_readingState != value)
                {
                    _readingState = value;
                    OnPropertyChanged(nameof(ReadingState));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        private int? _ownershipState;
        public int? OwnershipState
        {
            get { return _ownershipState; }
            set
            {
                if (_ownershipState != value)
                {
                    _ownershipState = value;
                    OnPropertyChanged(nameof(OwnershipState));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }
    }
}
