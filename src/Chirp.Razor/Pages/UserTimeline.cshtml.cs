﻿using System.ComponentModel.DataAnnotations;
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
    
    [BindProperty]
    [StringLength(160)]
    public string? Text { get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        Cheeps = new List<CheepDto>();
        Following = new List<string>();
        Followers = new List<string>();
    }

    public bool IsAuthenticated() {
        return User.Identity!.IsAuthenticated;
    }

    public bool IsCurrentAuthor(string authorName) {
        return User.Identity!.IsAuthenticated && authorName == User.Identity!.Name;
    }
    public async Task<IActionResult> OnGetAsync(string authorName, [FromQuery(Name = "page")] int pageIndex = 1)
    {
        if (IsAuthenticated() && IsCurrentAuthor(authorName))
        {
            Cheeps = (await _cheepRepository.GetPersonalCheeps(authorName, pageIndex, 32)).ToList();
        }
        else
        {
            Cheeps = (await _cheepRepository.GetCheepsFromAuthor(authorName, pageIndex, 32)).ToList();
        }

        Following =  _authorRepository.GetAuthorFollowing(User.Identity!.Name!);
        Followers = _authorRepository.GetAuthorFollowers(User.Identity!.Name!);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!IsAuthenticated() || string.IsNullOrWhiteSpace(Text))
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

    public async Task<IActionResult> OnPostFollowAsync(string authorName){
        if (IsAuthenticated()) {
            await _authorRepository.FollowAuthor(User.Identity!.Name!, authorName);
        }
        return RedirectToPage("UserTimeline");
    }

    public async Task<IActionResult> OnPostUnfollowAsync(string authorName){
        if (IsAuthenticated()) {
            await _authorRepository.UnfollowAuthor(User.Identity!.Name!, authorName);
        }   
        return RedirectToPage("UserTimeline");
    }
}
