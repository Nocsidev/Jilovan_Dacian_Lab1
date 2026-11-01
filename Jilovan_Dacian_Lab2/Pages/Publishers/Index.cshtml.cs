using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Jilovan_Dacian_Lab2.Models;
using Jilovan_Dacian_Lab2.Models.ViewModels;

namespace Jilovan_Dacian_Lab2.Pages.Publishers;

public class IndexModel : PageModel
{
    private readonly Jilovan_Dacian_Lab2Context _context;

    public IndexModel(Jilovan_Dacian_Lab2Context context)
    {
        _context = context;
    }

    public IList<Publisher> Publisher { get; set; } = default!;

    public PublisherIndexData PublisherData { get; set; } = new();
    public int PublisherID { get; set; }
    public int BookID { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id, int? bookID)
    {
        PublisherData = new PublisherIndexData();
        PublisherData.Publishers = await _context.Publisher
            .Include(i => i.Books)
            .ThenInclude(i => i.Author)
            .OrderBy(i => i.PublisherName)
            .ToListAsync();

        if (id != null)
        {
            Publisher? publisher = PublisherData.Publishers
                .SingleOrDefault(i => i.ID == id.Value);
            if (publisher == null)
            {
                return NotFound();
            }

            PublisherID = publisher.ID;
            PublisherData.Books = publisher.Books ?? Array.Empty<Book>();
        }

        return Page();
    }
}
