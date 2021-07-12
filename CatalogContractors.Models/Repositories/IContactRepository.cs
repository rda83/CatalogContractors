using System.Linq;
using CatalogContractors.Infra;

namespace CatalogContractors.Models.Repositories
{
    public interface IContactRepository
    {
        Contact GetById(long contactId);
        void Add(long contractorId, Contact item);
        bool Delete(long id);
        bool Update(Contact changedItem);
        IQueryable<Contact> GetAll(long contractorId, QueryParameters queryParameters);
        bool Save();
    }
}
