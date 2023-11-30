using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;
    public class AboutMeModel : PageModel
    {
        private readonly ICheepRepository _cheepRepository;
        private readonly IAuthorRepository _authorRepository;
        public bool IsAuthenticated { get; private set; }
        public IEnumerable<Claim> FilteredClaims { get; private set; } = new List<Claim>();
        public List<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
        public IEnumerable<string> Following { get; set; } = new List<string>();
        public IEnumerable<string> Followers { get; set; } = new List<string>();

        public AboutMeModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository)
        {
            _authorRepository = authorRepository;
            _cheepRepository = cheepRepository;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery(Name = "page")] int pageIndex = 1)
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false;
            if (IsAuthenticated) {
                string userName = User.Identity!.Name!;
                
                if (!await _authorRepository.AuthorExists(userName))
                {
                    var email = User.Claims.Where(e => e.Type == "emails").Select(e => e.Value).SingleOrDefault();
                    await _authorRepository.CreateAuthor(new CreateAuthorDto(userName, email!));
                }
                var cheeps = await _cheepRepository.GetCheepsFromAuthor(userName,pageIndex, int.MaxValue);
                Cheeps = cheeps.ToList();
                var desiredClaimTypes = new List<string> { "name", "emails" };
                FilteredClaims = User.Claims.Where(c => desiredClaimTypes.Contains(c.Type));
                Following =  _authorRepository.GetAuthorFollowing(userName);
                Followers = _authorRepository.GetAuthorFollowers(userName);
                return Page();
            }
            return RedirectToPage("Public");   
        }
    }
