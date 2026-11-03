using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Jilovan_Dacian_Lab2.Models;

namespace Jilovan_Dacian_Lab2.Pages.BorrowingPages;

public class CreateModel : PageModel
{
    private readonly Jilovan_Dacian_Lab2Context _context;

    public CreateModel(Jilovan_Dacian_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = new();

    public bool HasMembers { get; private set; }
    public bool HasBooks { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        Borrowing.ReturnDate = DateTime.Today;
        await PopulateDropDownListsAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Borrowing.MemberID is null)
        {
            ModelState.AddModelError("Borrowing.MemberID", "Select a member.");
        }
        else if (!await _context.Member.AnyAsync(m => m.ID == Borrowing.MemberID))
        {
            ModelState.AddModelError("Borrowing.MemberID", "The selected member no longer exists. Select another member.");
        }

        if (Borrowing.BookID is null)
        {
            ModelState.AddModelError("Borrowing.BookID", "Select a book.");
        }
        else if (!await _context.Book.AnyAsync(b => b.ID == Borrowing.BookID))
        {
            ModelState.AddModelError("Borrowing.BookID", "The selected book no longer exists. Select another book.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateDropDownListsAsync();
            return Page();
        }

        // Save only the selected member, book, and return date.
        var borrowing = new Borrowing
        {
            MemberID = Borrowing.MemberID,
            BookID = Borrowing.BookID,
            ReturnDate = Borrowing.ReturnDate
        };

        _context.Borrowing.Add(borrowing);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqlException { Number: 547 })
        {
            _context.Entry(borrowing).State = EntityState.Detached;
            ModelState.AddModelError(string.Empty, "The selected member or book is no longer available. Check your selections and try again.");
            await PopulateDropDownListsAsync();
            return Page();
        }

        return RedirectToPage("./Index");
    }

    private async Task PopulateDropDownListsAsync()
    {
        var bookList = await _context.Book
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .Select(b => new
            {
                b.ID,
                BookFullName = b.Title + (b.Author != null
                    ? " - " + b.Author.LastName + " " + b.Author.FirstName
                    : "")
            })
            .ToListAsync();

        var members = await _context.Member
            .AsNoTracking()
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .ToListAsync();

        HasBooks = bookList.Count > 0;
        HasMembers = members.Count > 0;
        ViewData["BookID"] = new SelectList(bookList, "ID", "BookFullName", Borrowing.BookID);
        ViewData["MemberID"] = new SelectList(members, "ID", "FullName", Borrowing.MemberID);
    }
}
