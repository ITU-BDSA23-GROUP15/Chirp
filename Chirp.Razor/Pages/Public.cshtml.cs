using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
        Cheeps = new List<CheepViewModel>();
    }
    public async Task<IActionResult> OnGet([FromQuery(Name = "page")] int pageIndex = 1)
    {
        var cheeps = await _service.GetCheeps(pageIndex); 
        Cheeps = cheeps.ToList();
        return Page();
    }
}
