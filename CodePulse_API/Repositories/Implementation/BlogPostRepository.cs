using CodePulse_API.Data;
using CodePulse_API.Models.Domain;
using CodePulse_API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse_API.Repositories.Implementation
{
    public class BlogPostRepository :IBlogPostRepository
    {
        public readonly ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogpost) {
            await dbContext.BlogPosts.AddAsync(blogpost);
            await dbContext.SaveChangesAsync();
            return blogpost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await dbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (existingBlogPost != null)
            {
                dbContext.BlogPosts.Remove(existingBlogPost);
                await dbContext.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogPostAsync()
        {
            return await dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetbyUrlHandleAsync(string urlHandle)
        {
            return await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> GetbyIdAsync(Guid id)
        {
            return await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }
    }

}
