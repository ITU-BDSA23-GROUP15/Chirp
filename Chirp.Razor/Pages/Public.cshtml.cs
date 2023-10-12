using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    public List<CheepDto> Cheeps { get; set; }

    public PublicModel(ICheepRepository cheepRepository)
    {
        _cheepRepository = cheepRepository;
        Cheeps = new List<CheepDto>();
    }
    public async Task<IActionResult> OnGet([FromQuery(Name = "page")] int pageIndex = 1)
    {
        var cheeps = await _cheepRepository.GetCheeps(pageIndex, 32); 
        Cheeps = cheeps.ToList();
        return Page();
    }
}
