using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;
    public class AboutMeModel : PageModel
    {
        public bool IsAuthenticated { get; private set; }
        public IEnumerable<Claim> FilteredClaims { get; private set; } = new List<Claim>();
        public void OnGet()
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false;

            if (IsAuthenticated)
            {
                var desiredClaimTypes = new List<string> { "name", "emails" };
                FilteredClaims = User.Claims.Where(c => desiredClaimTypes.Contains(c.Type));
            }
        }
    }
