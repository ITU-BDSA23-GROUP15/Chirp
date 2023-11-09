namespace Chirp.Core
{
    public interface IFollowerRepository
    {
        Task Follow(Guid followingId);
        Task SetFollower(Guid followerId);
    }
}