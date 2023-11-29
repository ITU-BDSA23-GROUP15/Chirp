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

        public void OnGet()
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false;

            if (IsAuthenticated)
            {
                var desiredClaimTypes = new List<string> { "name", "emails" };
                FilteredClaims = User.Claims.Where(c => desiredClaimTypes.Contains(c.Type));
                Following = _authorRepository.GetAuthorFollowing(User.Identity!.Name!);
                Followers = _authorRepository.GetAuthorFollowers(User.Identity!.Name!);
                // Cheeps = (await _cheepRepository.GetCheepsFromAuthor(User.Identity!.Name!, pageIndex, 32)).ToList();
            }
        }
    }
