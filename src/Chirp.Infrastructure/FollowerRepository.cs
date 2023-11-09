namespace Chirp.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Chirp.Core;

public class FollowerRepository : IFollowerRepository
{
    private readonly ChirpContext _context;

    public FollowerRepository(ChirpContext context)
    {
        _context = context;
    }

    public async Task Follow(Guid followingId)
    {
        var newFollower = new Follower
        {
            FollowingId = followingId
        };

        await _context.Followers.AddAsync(newFollower);
        await _context.SaveChangesAsync();
    }

    public async Task SetFollower(Guid followerId)
    {
        var newFollower = new Follower
        {
            FollowerId = followerId
        };

        await _context.Followers.AddAsync(newFollower);
        await _context.SaveChangesAsync();
    }
}