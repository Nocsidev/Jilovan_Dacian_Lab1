using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Jilovan_Dacian_Lab2.Models;

namespace Jilovan_Dacian_Lab2.Pages.Category;

public class IndexModel : PageModel
{
    private readonly Jilovan_Dacian_Lab2Context _context;

    public IndexModel(Jilovan_Dacian_Lab2Context context)
    {
        _context = context;
    }

    public IList<Jilovan_Dacian_Lab2.Models.Category> Category { get; set; } = default!;

    public int? SelectedCategoryID { get; set; }
    public IList<Book> SelectedBooks { get; set; } = new List<Book>();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        Category = await _context.Category
            .Include(i => i.BookCategories!)
            .ThenInclude(i => i.Book)
            .ThenInclude(i => i.Author)
            .OrderBy(i => i.CategoryName)
            .ToListAsync();

        if (id != null)
        {
            Jilovan_Dacian_Lab2.Models.Category? category = Category.SingleOrDefault(i => i.ID == id.Value);

            if (category == null)
            {
                return NotFound();
            }

            SelectedCategoryID = category.ID;

            SelectedBooks = category.BookCategories?.Select(i => i.Book)
                .OrderBy(i => i.Title)
                .ToList() ?? new List<Book>();
        }


        return Page();
    }
}
