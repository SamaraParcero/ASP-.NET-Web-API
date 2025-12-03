namespace APICatalog.Repositorys
{
    public interface IUnitOfWork
    {
        //IRepository<Product> ProdutoRepository { get; }
        //IRepository<Category> CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        void Commit();
    }
}
