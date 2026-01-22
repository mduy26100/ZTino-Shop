using Application.Common.Abstractions.Persistence;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public abstract class Repository<T, TKey> : IRepository<T, TKey> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly ISpecificationEvaluator _specificationEvaluator;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _specificationEvaluator = SpecificationEvaluator.Default;
        }

        // -----------------------
        // Helpers (Specification)
        // -----------------------
        protected virtual IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            if (specification is null)
                return _dbSet.AsQueryable();

            return _specificationEvaluator.GetQuery(_dbSet.AsQueryable(), specification);
        }

        protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
        {
            if (specification is null)
                throw new ArgumentNullException(nameof(specification));

            return _specificationEvaluator.GetQuery<T, TResult>(_dbSet.AsQueryable(), specification);
        }

        // -----------------------
        // Query - Expression based
        // -----------------------
        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (filter is not null)
                query = query.Where(filter);

            if (asNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id), "Id cannot be null when retrieving an entity.");

            var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
            return entity;
        }

        public virtual async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.Where(predicate);
            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> FindOneAsync(
            Expression<Func<T, bool>> predicate,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.Where(predicate);
            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        // -----------------------
        // Query - Specification based
        // -----------------------
        public virtual async Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<T?> SingleOrDefaultAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification((ISpecification<T>)specification);
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification);
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification).AsNoTracking();
            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification);
            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification);
            return await query.CountAsync(cancellationToken);
        }

        public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public virtual async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification);
            return await query.AnyAsync(cancellationToken);
        }

        public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(cancellationToken);
        }

        public virtual IAsyncEnumerable<T> AsAsyncEnumerable(ISpecification<T> specification)
        {
            var query = ApplySpecification(specification);
            return query.AsAsyncEnumerable();
        }

        // Generic GetById (support different id types)
        public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id), "Id cannot be null when retrieving an entity.");

            var entity = await _dbSet.FindAsync(new object[] { id! }, cancellationToken);
            return entity;
        }

        // -----------------------
        // Add / Update / Delete
        // -----------------------
        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        // -----------------------
        // Existence check (Expression based)
        // -----------------------
        public virtual async Task<bool> AnyAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }
    }
}
