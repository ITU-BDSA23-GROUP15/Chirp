using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
        Cheeps = new List<CheepViewModel>();
    }
    public async Task<IActionResult> OnGet(string author, [FromQuery(Name = "page")] int pageIndex = 1)
    {
        var cheeps = await _service.GetCheepsFromAuthor(author, pageIndex);
        Cheeps = cheeps.ToList();
        return Page();
    }
}
