using System;

namespace MainMusicStore.DataAccess.IMainRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository category { get; }
        ICoverTypeRepository CoverType { get; }
        ISPCallRepository sp_call { get; }
        void Save();
    }
}
