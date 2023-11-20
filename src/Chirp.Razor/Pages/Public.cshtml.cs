using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

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
    public async Task<IActionResult> OnGetAsync([FromQuery(Name = "page")] int pageIndex = 1)
    {
        var cheeps = await _cheepRepository.GetCheeps(pageIndex, 32);
        Cheeps = cheeps.ToList();
        return Page();
    }

    [BindProperty]
    public string Text { get; set; }

    // post cheep
    public async Task<IActionResult> OnPostAsync()
    {
        if (!User.Identity!.IsAuthenticated || string.IsNullOrWhiteSpace(Text))
        {
            return RedirectToPage("Public");
        }

        string userName = User.Identity!.Name!;

        if (!await _authorRepository.AuthorExists(userName))
        {
            var email = User.Claims.Where(e => e.Type == "emails").Select(e => e.Value).SingleOrDefault();
            await _authorRepository.CreateAuthor(new CreateAuthorDto(userName, email!));
        }

        await _cheepRepository.CreateCheep(new CreateCheepDto(Text, userName));

        return RedirectToPage("Public");
    }
    public async Task<IActionResult> OnPostFollow(string authorName){
        if (User.Identity!.IsAuthenticated) {
            // string userName = User.Identity!.Name!;
            // var author = await _authorRepository.GetAuthorByName(userName);
            // var authorToFollow = await _authorRepository.GetAuthorByName(Text);
            // await _authorRepository.FollowAuthor(author.AuthorId, authorToFollow.AuthorId);

            Console.WriteLine($"User {User.Identity!.Name!}");
            var user = await _authorRepository.GetAuthorByName(User.Identity!.Name!);
            var author = await _authorRepository.GetAuthorByName(authorName);
            Console.WriteLine($"User: {user.AuthorId}, {user.Name}, {user.Email}");
            Console.WriteLine($"Author: {author.AuthorId}, {author.Name}, {author.Email}");

            
            await _authorRepository.FollowAuthor(user.AuthorId, author.AuthorId);
            var authorWithFollowers = await _authorRepository.GetAuthorWithFollowers(author.AuthorId);
            Console.WriteLine($"Current followers: {authorWithFollowers.Followers.Count}");
            return RedirectToPage("Public");
        }
        else {
            return RedirectToPage("Public");
        }
    }
}
