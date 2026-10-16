using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Jilovan_Dacian_Lab2.Models;

namespace Jilovan_Dacian_Lab2.Pages.AuthorsPages;

public class EditModel : PageModel
{
    private readonly Jilovan_Dacian_Lab2Context _context;

    public EditModel(Jilovan_Dacian_Lab2Context context)
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
        Author = authors;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Author).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuthorExists(Author.ID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool AuthorExists(int id)
    {
        return _context.Author.Any(e => e.ID == id);
    }
}
