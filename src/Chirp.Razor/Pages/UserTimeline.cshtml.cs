using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public List<CheepDto> Cheeps { get; set; }
    public IEnumerable<string> Following { get; set; }
    public IEnumerable<string> Followers { get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        Cheeps = new List<CheepDto>();
        Following = new List<string>();
        Followers = new List<string>();
    }
    public async Task<IActionResult> OnGetAsync(string authorName, [FromQuery(Name = "page")] int pageIndex = 1)
    {
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(authorName, pageIndex, 32);
        Cheeps = cheeps.ToList();
        Following =  _authorRepository.GetAuthorFollowing(authorName);
        Followers = _authorRepository.GetAuthorFollowers(authorName);
        return Page();
    }

    [BindProperty]
    [StringLength(160)]
    public string? Text { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!User.Identity!.IsAuthenticated || string.IsNullOrWhiteSpace(Text))
        {
            return RedirectToPage("UserTimeline");
        }

        string userName = User.Identity!.Name!;

        if (!await _authorRepository.AuthorExists(userName))
        {
            var email = User.Claims.Where(e => e.Type == "emails").Select(e => e.Value).SingleOrDefault();
            await _authorRepository.CreateAuthor(new CreateAuthorDto(userName, email!));
        }

        await _cheepRepository.CreateCheep(new CreateCheepDto(Text, userName));

        return RedirectToPage("UserTimeline");
    }

    public async Task<IActionResult> OnPostFollow(string authorName){
        string currentUrl = HttpContext.Request.Path;
        if (User.Identity!.IsAuthenticated) {
            await _authorRepository.FollowAuthor(User.Identity!.Name!, authorName);
        }
            return Redirect(currentUrl);
    }

    public async Task<IActionResult> OnPostUnfollow(string authorName){
        string currentUrl = HttpContext.Request.Path;
        if (User.Identity!.IsAuthenticated) {
            await _authorRepository.UnfollowAuthor(User.Identity!.Name!, authorName);
        }
            return Redirect(currentUrl);
        
    }
}
