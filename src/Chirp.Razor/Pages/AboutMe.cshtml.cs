using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;
public class AboutMeModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public IEnumerable<Claim> FilteredClaims { get; private set; } = new List<Claim>();
    public List<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<string> Following { get; set; } = new List<string>();
    public IEnumerable<string> Followers { get; set; } = new List<string>();

    public AboutMeModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository)
    {
        _authorRepository = authorRepository;
        _cheepRepository = cheepRepository;
    }

    public bool IsAuthenticated()
    {
        return User.Identity!.IsAuthenticated;
    }

    public bool IsCurrentAuthor(string authorName)
    {
        return User.Identity!.IsAuthenticated && authorName == User.Identity!.Name;
    }

    public async Task<IActionResult> OnGetAsync([FromQuery(Name = "page")] int pageIndex = 1)
    {
        if (IsAuthenticated())
        {
            string userName = User.Identity!.Name!;

            if (!await _authorRepository.AuthorExists(userName))
            {
                var email = User.Claims.Where(e => e.Type == "emails").Select(e => e.Value).SingleOrDefault();
                await _authorRepository.CreateAuthor(new CreateAuthorDto(userName, email!));
            }
            var cheeps = await _cheepRepository.GetCheepsFromAuthor(userName, pageIndex, int.MaxValue);
            Cheeps = cheeps.ToList();
            var desiredClaimTypes = new List<string> { "name", "emails" };
            FilteredClaims = User.Claims.Where(c => desiredClaimTypes.Contains(c.Type));
            Following = _authorRepository.GetAuthorFollowing(userName).ToList();
            Followers = _authorRepository.GetAuthorFollowers(userName).ToList();
            return Page();
        }
        return RedirectToPage("Public");
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        await _authorRepository.DeleteAuthor(User.Identity!.Name!);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("Public");
    }

     public async Task<IActionResult> OnPostFollowAsync(string authorName){
        if (IsAuthenticated()) {
            await _authorRepository.FollowAuthor(User.Identity!.Name!, authorName);
        }
        return RedirectToPage("AboutMe");
    }

    public async Task<IActionResult> OnPostUnfollowAsync(string authorName){
        if (IsAuthenticated()) {
            await _authorRepository.UnfollowAuthor(User.Identity!.Name!, authorName);
        }   
        return RedirectToPage("AboutMe");
    }
}
