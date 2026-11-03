using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Jilovan_Dacian_Lab2.Models;

namespace Jilovan_Dacian_Lab2.Pages.BorrowingPages;

public class IndexModel : PageModel
{
    private readonly Jilovan_Dacian_Lab2Context _context;

    public IndexModel(Jilovan_Dacian_Lab2Context context)
    {
        _context = context;
    }

    public IList<Borrowing> Borrowing { get; set; } = new List<Borrowing>();

    public async Task OnGetAsync()
    {
        Borrowing = await _context.Borrowing
            .Include(b => b.Book)
                .ThenInclude(b => b!.Author)
            .Include(b => b.Member)
            .AsNoTracking()
            .ToListAsync();
    }
}
