using CodePulse_API.Models.Domain;

namespace CodePulse_API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);

        Task<IEnumerable<BlogPost>>GetAllBlogPostAsync();

        Task<BlogPost?>DeleteAsync(Guid id);

        Task<BlogPost?> GetbyUrlHandleAsync(string urlHandle);
        Task<BlogPost?> GetbyIdAsync(Guid Id);
    }
}
