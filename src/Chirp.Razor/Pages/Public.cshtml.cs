using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public List<CheepDto> Cheeps { get; set; }

    public PublicModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        Cheeps = new List<CheepDto>();
    }
    public async Task<IActionResult> OnGet([FromQuery(Name = "page")] int pageIndex = 1)
    {
        var cheeps = await _cheepRepository.GetCheeps(pageIndex, 32); 
        Cheeps = cheeps.ToList();
        return Page();
    }

    // post cheep
    public async Task<IActionResult> OnPost([FromForm] string message)
    {
        if (string.IsNullOrWhiteSpace(message) || !User.Identity!.IsAuthenticated)
        {
            return RedirectToPage();
        }

        var email_claim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email);
        string email = email_claim!.Value;
        string userName = User.Identity!.Name!;

        if (!await _authorRepository.AuthorExists(userName)){
            _authorRepository.CreateAuthor(new CreateAuthorDto(userName, email));
        }
        
        _cheepRepository.CreateCheep(new CreateCheepDto(message, userName));

        return RedirectToPage();
    }
}
