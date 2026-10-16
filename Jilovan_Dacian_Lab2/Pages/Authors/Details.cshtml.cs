using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Jilovan_Dacian_Lab2.Models;

namespace Jilovan_Dacian_Lab2.Pages.AuthorsPages;

public class DetailsModel : PageModel
{
    private readonly Jilovan_Dacian_Lab2Context _context;
    public DetailsModel(Jilovan_Dacian_Lab2Context context)
    {
        _context = context;
    }

    public Author Author { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var authors = await _context.Author.FirstOrDefaultAsync(m => m.ID == id);
        if (authors is null)
        {
            return NotFound();
        }
        else
        {
            Author = authors;
        }

        return Page();
    }
}
