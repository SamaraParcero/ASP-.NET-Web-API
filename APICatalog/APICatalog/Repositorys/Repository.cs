using APICatalog.Context;
using APICatalog.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatalog.Repositorys
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            //Usamos o Set para pegar o tipo do objeto que iremos pegar do banco
            return await _context.Set<T>().AsNoTracking().ToListAsync();//O Tracking tira o gerenciamento de entidade na memória
        }
        public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate)
        {
           return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity;
        }


        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            //_context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }

        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
            return entity;
        }

       
    }
}
