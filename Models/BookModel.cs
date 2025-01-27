using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Windows;

namespace BookDb.Models
{
    class BookModel
    {
        private readonly string ConnectionString = "User=SYSDBA;Password=masterkey;Database=D:\\fbdata\\BOOKSDB.fdb;DataSource=localhost;Port=3050;Charset=UTF8;";

        public List<Book> Books { get; set; }

        public BookModel()
        {
            Books = new List<Book>();

            FetchBooks();
        }

        public void FetchBooks()
        {
            Books.Clear();
            using (FbConnection connection = new FbConnection(ConnectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM Book;";

                FbCommand command = new FbCommand(query, connection);
                FbDataAdapter adapter = new FbDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    Book book = new()
                    {
                        Id = row["id"] as int?,
                        Title = row["Title"] as string,
                        AcquirementDate = row["acquirement_date"] as DateTime?,
                        TotalPages = row["total_pages"] as int?,
                        CurrentPage = row["current_page"] as int?,
                        TotalReads = row["total_reads"] as int?,
                        Rating = row["rating"] as int?,
                        Keywords = row["keywords"] as string,
                        Notes = row["notes"] as string,
                        Description = row["description"] as string,
                        AuthorId = (int)row["author_id"],
                        PublisherId = (int)row["publisher_id"],
                        OwnershipState = (int)row["ownership_state"],
                        ReadingState = (int)row["reading_state"]
                    };
                    Books.Add(book);
                }
            }
        }

        public bool AddNewBook(Book newBook)
        {
            try
            {
                using (FbConnection connection = new FbConnection(ConnectionString))
                {
                    string query = @"INSERT INTO Book (title, author_id, total_pages, current_page, total_reads, rating, acquirement_date, publisher_id, keywords, description, notes, reading_state, ownership_state)
                                    VALUES (@Title, @Author_Id, @Total_Pages, @Current_Page, @Total_Reads, @Rating, @Acquirement_Date, @Publisher_Id, @Keywords, @Description, @Notes, @Reading_State, @Ownership_State)";

                    using (FbCommand command = new FbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", newBook.Title);
                        command.Parameters.AddWithValue("@Author_Id", newBook.AuthorId);
                        command.Parameters.AddWithValue("@Total_Pages", newBook.TotalPages);
                        command.Parameters.AddWithValue("@Current_Page", newBook.CurrentPage);
                        command.Parameters.AddWithValue("@Total_Reads", newBook.TotalReads);
                        command.Parameters.AddWithValue("@Rating", newBook.Rating);
                        command.Parameters.AddWithValue("@Acquirement_Date", newBook.AcquirementDate);
                        command.Parameters.AddWithValue("@Publisher_Id", newBook.PublisherId);
                        command.Parameters.AddWithValue("@Keywords", newBook.Keywords ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Description", newBook.Description ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Notes", newBook.Notes ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Reading_State", newBook.ReadingState);
                        command.Parameters.AddWithValue("@Ownership_State", newBook.OwnershipState);

                        connection.Open();
                        command.ExecuteNonQuery();

                    }
                    return true;
                }
            }
            catch { return false; }
        }

        public bool UpdateBook(Book updatedBook)
        {
            try
            {
                using (FbConnection connection = new FbConnection(ConnectionString))
                {
                    connection.Open();

                    string query = @"UPDATE Book SET 
                                        title = @Title, 
                                        author_id = @Author_Id, 
                                        total_pages = @Total_Pages, 
                                        current_page = @Current_Page, 
                                        total_reads = @Total_Reads, 
                                        rating = @Rating, 
                                        acquirement_date = @Acquirement_Date, 
                                        publisher_id = @Publisher_Id, 
                                        keywords = @Keywords, 
                                        description = @Description, 
                                        notes = @Notes,
                                        reading_state = @Reading_State,
                                        ownership_state = @Ownership_State
                                    WHERE id = @Id";

                    using (FbCommand command = new FbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", updatedBook.Id); // Ensure the Book object has an Id property
                        command.Parameters.AddWithValue("@Title", updatedBook.Title);
                        command.Parameters.AddWithValue("@Author_Id", updatedBook.AuthorId);
                        command.Parameters.AddWithValue("@Total_Pages", updatedBook.TotalPages);
                        command.Parameters.AddWithValue("@Current_Page", updatedBook.CurrentPage);
                        command.Parameters.AddWithValue("@Total_Reads", updatedBook.TotalReads ?? (object)DBNull.Value); // Allow null
                        command.Parameters.AddWithValue("@Rating", updatedBook.Rating ?? (object)DBNull.Value); // Allow null
                        command.Parameters.AddWithValue("@Acquirement_Date", updatedBook.AcquirementDate);
                        command.Parameters.AddWithValue("@Publisher_Id", updatedBook.PublisherId);
                        command.Parameters.AddWithValue("@Keywords", updatedBook.Keywords ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Description", updatedBook.Description ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Notes", updatedBook.Notes ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Reading_State", updatedBook.ReadingState);
                        command.Parameters.AddWithValue("@Ownership_State", updatedBook.OwnershipState);

                        command.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch { return false; }
        }

        public bool DeleteBook(int bookId)
        {
            try
            {
                using (FbConnection connection = new FbConnection(ConnectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Book WHERE id = @BookId";
                    FbCommand command = new FbCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    command.ExecuteNonQuery();
                    
                    return true;
                }
            }
            catch { return false; }
        }

    }
}
