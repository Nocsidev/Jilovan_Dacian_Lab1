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

    public async Task OnGetAsync(int? AuthorID)
    {
        ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "LastName", AuthorID);

        Book = await _context.Book
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .Where(b => AuthorID == null || b.AuthorID == AuthorID)
            .ToListAsync();
    }
}
