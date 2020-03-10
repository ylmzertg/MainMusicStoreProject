using MainMusicStore.Data;
using MainMusicStore.DataAccess.IMainRepository;
using MainMusicStore.Models.DbModels;
using System.Linq;

namespace MainMusicStore.DataAccess.MainRepository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var data = _db.Products.FirstOrDefault(x => x.Id == product.Id);
            if (data != null)
            {
                if (product.ImageUrl != null)
                {
                    data.ImageUrl = product.ImageUrl;
                }
                data.ISBN = product.ISBN;
                data.Price = product.Price;
                data.Price50 = product.Price50;
                data.Price100 = product.Price100;
                data.ListPrice = product.ListPrice;
                data.Title = product.Title;
                data.Description = product.Description;
                data.CategoryId = product.CategoryId;
                data.CoverTypeId = product.CoverTypeId;
                data.Author = product.Author;
            }
        }
    }
}
