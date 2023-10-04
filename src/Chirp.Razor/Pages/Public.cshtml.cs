using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.CheepService;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }
    public ActionResult OnGet([FromQuery(Name = "page")] int page = 0)
    {
        int pageRange = page * 32;
        Cheeps = _service.GetCheeps(pageRange);
        return Page();
    }
}
