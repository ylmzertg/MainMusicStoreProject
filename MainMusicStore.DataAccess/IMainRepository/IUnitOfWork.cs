using System;

namespace MainMusicStore.DataAccess.IMainRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        ICompanyRepository Company { get; }
        IProductRepository  Product { get; }
        ICoverTypeRepository CoverType { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }
        IApplicationUserRepository ApplicationUser{ get; }
        ISPCallRepository sp_call { get; }
        void Save();
    }
}
