using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public List<CheepDto> Cheeps { get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        Cheeps = new List<CheepDto>();
    }
    public async Task<IActionResult> OnGet(string author, [FromQuery(Name = "page")] int pageIndex = 1)
    {
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(author, pageIndex, 32);
        Cheeps = cheeps.ToList();
        return Page();
    }

    [BindProperty]
    public string Text {get;set;}

    public async Task<IActionResult> OnPost(string Text)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToPage("UserTimeline");
        }

        var email = User.Claims.Where(e => e.Type == "emails").Select(e => e.Value).SingleOrDefault();
        string userName = User.Identity!.Name!;

        if (!await _authorRepository.AuthorExists(userName)){
            await _authorRepository.CreateAuthor(new CreateAuthorDto(userName, email!));
        }
        
        await _cheepRepository.CreateCheep(new CreateCheepDto(Text, userName));

        return RedirectToPage("UserTimeline");
    }
}
