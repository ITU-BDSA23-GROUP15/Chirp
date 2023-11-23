﻿using Chirp.Infrastructure.Migrations;
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
    public IEnumerable<string> Following { get; set; }

    public PublicModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        Cheeps = new List<CheepDto>();
        Following = new List<string>();
    }
    public async Task<IActionResult> OnGetAsync([FromQuery(Name = "page")] int pageIndex = 1)
    {
        string userName = User.Identity!.Name!;

        if (!await _authorRepository.AuthorExists(userName))
        {
            var email = User.Claims.Where(e => e.Type == "emails").Select(e => e.Value).SingleOrDefault();
            await _authorRepository.CreateAuthor(new CreateAuthorDto(userName, email!));
        }
        var cheeps = await _cheepRepository.GetCheeps(pageIndex, 32);
        Cheeps = cheeps.ToList();
        Following =  _authorRepository.GetAuthorFollowing(User.Identity!.Name!);
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

        await _cheepRepository.CreateCheep(new CreateCheepDto(Text, userName));

        return RedirectToPage("Public");
    }
    public async Task<IActionResult> OnPostFollow(string authorName){
        if (User.Identity!.IsAuthenticated) {
            Console.WriteLine($"User {User.Identity!.Name!}");
            var user = await _authorRepository.GetAuthorByName(User.Identity!.Name!);
            var author = await _authorRepository.GetAuthorByName(authorName);
            Console.WriteLine($"User: {user.AuthorId}, {user.Name}, {user.Email}");
            Console.WriteLine($"Author: {author.AuthorId}, {author.Name}, {author.Email}");

            
            await _authorRepository.FollowAuthor(user.AuthorId, author.AuthorId);
            var authorWithFollowers = await _authorRepository.GetAuthorWithFollowers(user.AuthorId);
            Console.WriteLine($"{user.Name} is Currently following: {authorWithFollowers.Following.Count} authors");
            Console.WriteLine($"{user.Name} currently has followers: {authorWithFollowers.Followers.Count} authors");

            return RedirectToPage("Public");
        }
        else {
            return RedirectToPage("Public");
        }
    }

    public async Task<IActionResult> OnPostUnfollow(string authorName){
        if (User.Identity!.IsAuthenticated) {
            Console.WriteLine($"User {User.Identity!.Name!}");
            var user = await _authorRepository.GetAuthorByName(User.Identity!.Name!);
            var author = await _authorRepository.GetAuthorByName(authorName);
            Console.WriteLine($"User: {user.AuthorId}, {user.Name}, {user.Email}");
            Console.WriteLine($"Author: {author.AuthorId}, {author.Name}, {author.Email}");

            
            await _authorRepository.UnfollowAuthor(user.AuthorId, author.AuthorId);
            var authorWithFollowers = await _authorRepository.GetAuthorWithFollowers(user.AuthorId);
            Console.WriteLine($"{user.Name} is Currently following: {authorWithFollowers.Following.Count} authors");
            Console.WriteLine($"{user.Name} currently has followers: {authorWithFollowers.Followers.Count} authors");

            return RedirectToPage("Public");
        }
        else {
            return RedirectToPage("Public");
        }
    }
}
