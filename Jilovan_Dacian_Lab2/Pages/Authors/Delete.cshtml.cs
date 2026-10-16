using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Jilovan_Dacian_Lab2.Models;

namespace Jilovan_Dacian_Lab2.Pages.AuthorsPages;

public class DeleteModel : PageModel
{
    private readonly Jilovan_Dacian_Lab2Context _context;

    public DeleteModel(Jilovan_Dacian_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var authors = await _context.Author.FindAsync(id);
        if (authors != null)
        {
            Author = authors;
            _context.Author.Remove(Author);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
