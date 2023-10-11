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
    public ActionResult OnGet([FromQuery(Name = "page")] int pageIndex = 1)
    {
        Cheeps = _service.GetCheeps(pageIndex);
        return Page();
    }
}
