﻿using CodePulse_API.Data;
using CodePulse_API.Models.Domain;
using CodePulse_API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse_API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {

            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;

        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();    
        }
    }
}
