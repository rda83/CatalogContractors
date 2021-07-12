using System.Linq;
using CatalogContractors.Infra;

namespace CatalogContractors.Models.Repositories
{
    public interface IContractorRepository
    {
        Contractor GetById(long id);
        void Add(Contractor item);
        bool Delete(long id);
        bool Update(Contractor item);
        IQueryable<Contractor> GetAll(QueryParameters queryParameters);
        bool Save();
    }
}
