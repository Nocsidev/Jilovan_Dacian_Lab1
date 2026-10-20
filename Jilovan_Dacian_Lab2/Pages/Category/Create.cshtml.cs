using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Jilovan_Dacian_Lab2.Models;

namespace Jilovan_Dacian_Lab2.Pages.Category;

public class CreateModel : PageModel
{
    private readonly Jilovan_Dacian_Lab2Context _context;

    public CreateModel(Jilovan_Dacian_Lab2Context context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Jilovan_Dacian_Lab2.Models.Category Category { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Category.Add(Category);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
