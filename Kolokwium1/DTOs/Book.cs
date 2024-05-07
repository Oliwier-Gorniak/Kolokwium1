using System;
using System.Collections.Generic;

namespace Kolokwium1.DTOs;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<Author> Authors { get; set; }
}