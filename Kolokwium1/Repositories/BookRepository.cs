
using Microsoft.Data.SqlClient;
using Kolokwium1.DTOs;

namespace Kolokwium1.Repositories;

public class BookRepository : IBookRepository
{
    private readonly IConfiguration _configuration;
    private int _nextId = 1;

    public BookRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<bool> DoesBookExist(int id)
    {
        var query = "SELECT 1 FROM books WHERE PK = @PK";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }
    
    public async Task<Book> GetBookById(int id)
    {
	    var query = @"SELECT books.PK AS bookPK,
						books.title AS bookTITLE,
						authors.PK AS authorPK,
						authors.first_name AS authorNAME,
						authors.last_name AS authorLASTNAME
						FROM books
						JOIN books_authors ON books.PK = books_author.FK_book
						JOIN authors ON books_authors.FK_author = authors.PK
						WHERE books.PK = @PK";

	    await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
	    await using SqlCommand command = new SqlCommand();

	    command.Connection = connection;
	    command.CommandText = query;
	    command.Parameters.AddWithValue("@PK", id);
	    
	    await connection.OpenAsync();

	    var reader = await command.ExecuteReaderAsync();

	    var bookPKOrdinal = reader.GetOrdinal("bookPK");
	    var bookTITLEOrdinal = reader.GetOrdinal("bookTITLE");
	    var authorPKOrdinal = reader.GetOrdinal("authorPK");
	    var authorNAMEOrdinal = reader.GetOrdinal("authorNAME");
	    var authorLASTNAMEOrdinal = reader.GetOrdinal("authorLASTNAME");

	    Book book = null;

	    while (await reader.ReadAsync())
	    {
		    if (book is not null)
		    {
			    book.Authors.Add(new Author()
			    {
				    Id = reader.GetInt32(authorPKOrdinal),
				    Name = reader.GetString(authorNAMEOrdinal),
				    LastName = reader.GetString(authorLASTNAMEOrdinal)
			    });
		    }
		    else
		    {
			    book = new Book()
			    {
				    Id = reader.GetInt32(bookPKOrdinal),
				    Title = reader.GetString(bookTITLEOrdinal),
				    Authors = new List<Author>()
				    {
					    new Author()
					    {
						    Id = reader.GetInt32(authorPKOrdinal),
						    Name = reader.GetString(authorNAMEOrdinal),
						    LastName = reader.GetString(authorLASTNAMEOrdinal)
					    }
				    }
			    };
		    }
	    }

	    if (book is null) throw new Exception();
        
        return book;
    }

}