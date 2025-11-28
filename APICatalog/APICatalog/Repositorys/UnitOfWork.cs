using APICatalog.Context;

namespace APICatalog.Repositorys
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProductRepository? _productRepository;

        private ICategoryRepository? _categoryRepository;

        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProductRepository ProductRepository
        {
            get
            {
                //return _productRepository =_productRepository?? new ProductRepository(_context);
                if(_productRepository == null)
                {
                    _productRepository  =  new ProductRepository(_context);
                }
                return _productRepository;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();//lIBERA RECURSOS ALOCADOS PARA CONTEXTO
        }
    }
}
