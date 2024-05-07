using Kolokwium1.DTOs;

namespace Kolokwium1.Repositories;

public interface IBookRepository
{
    Task<Book> GetBookById(int id);
    Task<bool> DoesBookExist(int id);
}