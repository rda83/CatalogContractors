using System.Linq;
using CatalogContractors.Infra;

namespace CatalogContractors.Models.Repositories
{
    public interface ITypeContactRepository
    {
        TypeContact GetById(long id);
        void Add(TypeContact item);
        bool Delete(long id);
        bool Update(TypeContact item);
        IQueryable<TypeContact> GetAll(QueryParameters queryParameters);
        bool Save();
    }
}
