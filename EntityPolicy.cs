using System.Threading.Tasks;

namespace Bcc.EntityApi
{
    public class EntityPolicy<TEntity> where TEntity : BaseEntity
    {
        public virtual Task<bool> CanViewAny()
        {
            return Task.FromResult(true);
        }

        public virtual Task<bool> CanView(TEntity entity)
        {
            return Task.FromResult(true);
        }

        public virtual Task<bool> CanCreate(TEntity entity = null)
        {
            return Task.FromResult(true);
        }

        public virtual Task<bool> CanUpdate(TEntity entity)
        {
            return Task.FromResult(true);
        }

        public virtual Task<bool> CanDelete(TEntity entity = null)
        {
            return Task.FromResult(true);
        }
    }
}