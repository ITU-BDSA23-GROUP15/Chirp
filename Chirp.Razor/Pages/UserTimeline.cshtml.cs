﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    public List<CheepDto> Cheeps { get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository)
    {
        _cheepRepository = cheepRepository;
        Cheeps = new List<CheepDto>();
    }
    public async Task<IActionResult> OnGet(string author, [FromQuery(Name = "page")] int pageIndex = 1)
    {
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(author, pageIndex, 32);
        Cheeps = cheeps.ToList();
        return Page();
    }
}
