using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace webapi
{
    public class MyEF
    {
        private readonly MyApplicationContext _dbContext;

        public MyEF(MyApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddEntityAsync(Entity entity)
        {
            await _dbContext.Entities.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Entity>> GetEntitiesByTypeAsync(string type)
        {
            return await _dbContext.Entities
                .Where(e => e.Type == type)
                .ToListAsync();
        }

        public async Task<List<Entity>> GetEntitiesByTextAsync(string text)
        {
            return await _dbContext.Entities
                .Where(e => e.Text.Contains(text))
                .ToListAsync();
        }
    }
}
