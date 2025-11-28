using System.Linq.Expressions;

namespace APICatalog.Repositorys
{
    public interface IRepository<T>
    {
        //Cuidado para não violar principio ISP -> Não deve ser forçado a depende de interface que não utiliza
        IEnumerable<T> GetAll();
        //Recebe como argumento uma expressão do tipo lambda e retorna um valor booleano 
        T? GetById(Expression<Func<T,bool>> predicate);
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);


    }
}
