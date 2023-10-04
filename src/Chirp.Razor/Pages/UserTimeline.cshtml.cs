﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.CheepService;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
        Cheeps = new List<CheepViewModel>();
    }
    public ActionResult OnGet(string author, [FromQuery(Name = "page")] int page = 0)
    {
        int pageRange = page * 32;
        Cheeps = _service.GetCheepsFromAuthor(author, pageRange);
        return Page();
    }
}
