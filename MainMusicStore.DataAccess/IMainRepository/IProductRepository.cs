using MainMusicStore.Models.DbModels;

namespace MainMusicStore.DataAccess.IMainRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
