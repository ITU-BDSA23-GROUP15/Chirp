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
    public ActionResult OnGet([FromQuery(Name = "page")] int page = 1)
    {
        int pageRange = (page-1) * 32;
        Cheeps = _service.GetCheeps(pageRange);
        return Page();
    }
}
