using Jilovan_Dacian_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Jilovan_Dacian_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Jilovan_Dacian_Lab2Context _context;

    public IndexModel(Jilovan_Dacian_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = default!;
    public BookData BookD { get; set; }
    public int BookID { get; set; }
    public int CategoryID { get; set; }

    public string TitleSort { get; set; }
    public string AuthorSort { get; set; }
    public string CurrentFilter { get; set; }

    public async Task OnGetAsync(int? AuthorID, int? id, int? categoryID, string sortOrder, string searchString)
    {
        BookD = new BookData();

        TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
        AuthorSort = sortOrder == "Author" ? "author_desc" : "Author";

        CurrentFilter = searchString;

        ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "LastName", AuthorID);

        BookD.Books = await _context.Book
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
            .ThenInclude(b => b.Category)
            .AsNoTracking()
            .Where(b => AuthorID == null || b.AuthorID == AuthorID)
            .OrderBy(b => b.Title)
            .ToListAsync();

        if (!string.IsNullOrEmpty(searchString))
        {
            BookD.Books = BookD.Books.Where(s => s.Author.FirstName.Contains(searchString)
                                                || s.Author.LastName.Contains(searchString)
                                                || s.Title.Contains(searchString));
        }

        if (id != null)
        {
            BookID = id.Value;
            Book book = BookD.Books
                .Where(b => b.ID == id.Value)
                .Single();
            BookD.Categories = book.BookCategories.Select(s => s.Category);
        }

        switch (sortOrder)
        {
            case "title_desc":
                BookD.Books = BookD.Books.OrderByDescending(b => b.Title);
                break;
            case "Author":
                BookD.Books = BookD.Books.OrderBy(b => b.Author.FullName);
                break;
            case "author_desc":
                BookD.Books = BookD.Books.OrderByDescending(b => b.Author.LastName);
                break;
            default:
                BookD.Books = BookD.Books.OrderBy(b => b.Title);
                break;
        }
    }
}
