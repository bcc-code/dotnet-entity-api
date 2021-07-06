using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bcc.EntityApi
{
    [ApiController]
    public class EntityController<TEntity> : ControllerBase where TEntity : BaseEntity
    {
        protected readonly DbContext _context;

        protected readonly EntityPolicy<TEntity> _policy;
        protected DbSet<TEntity> _dbSet { get; set; }
        public EntityController(DbContext context, EntityPolicy<TEntity> policy)
        {
            _context = context;
            _policy = policy;
            _dbSet = _context.Set<TEntity>();
        }

        /// <summary>
        /// Fetch all Entities.
        /// </summary>
        // GET: api/Entity
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
        {
            if (! await _policy.CanViewAny())
            {
                return Forbid();
            }

            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Fetch a specific Entity.
        /// </summary>
        // GET: api/Entity/bada4c31-fa63-4e5e-9a0f-af6ddca15092
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEntity>> Get(Guid id)
        {
            TEntity entity = await _dbSet.Where(e => e.Id == id).FirstOrDefaultAsync();

            if (! await _policy.CanView(entity))
            {
                return Forbid();
            }

            return new ActionResult<TEntity>(entity);
        }

        /// <summary>
        /// Create a new Entity.
        /// </summary>
        // POST: api/Entity
        [HttpPost]
        public virtual async Task<ActionResult<TEntity>> Create(TEntity entity)
        {
            if (! await _policy.CanCreate(entity))
            {
                return Forbid();
            }
            var testEntity = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        /// <summary>
        /// Update an Entity.
        /// </summary>
        // PUT: api/Entity
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public virtual async Task<ActionResult<TEntity>> Update(TEntity entity)
        {
            if (! await _policy.CanUpdate(entity))
            {
                return Forbid();
            }

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return new ActionResult<TEntity>(entity);
        }

        /// <summary>
        /// Delete a specific Entity.
        /// </summary>
        // DELETE: api/Entity/bada4c31-fa63-4e5e-9a0f-af6ddca15092
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(Guid id)
        {
            var entity = await _dbSet.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (! await _policy.CanDelete(entity))
            {
                return Forbid();
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}