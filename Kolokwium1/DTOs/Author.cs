using System.Collections.Generic;

namespace Kolokwium1.DTOs;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public List<Book> Books { get; set; }
}