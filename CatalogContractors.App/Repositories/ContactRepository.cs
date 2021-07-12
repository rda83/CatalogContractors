using CatalogContractors.Infra;
using CatalogContractors.Models;
using CatalogContractors.Models.Repositories;
using CatalogContractors.Database;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace CatalogContractors.App.Repositories
{
   public class ContactRepository : IContactRepository
    {
        private readonly Context _context;
        private readonly IContractorRepository _contractorRepository;

        public ContactRepository(Context context, IContractorRepository contractorRepository)
        {
            _context = context;
            _contractorRepository = contractorRepository;
        }

        public void Add(long contractorId, Contact item)
        {
            var contractor = _contractorRepository.GetById(contractorId);

            if(contractor != null)
            {
                item.ContractorId = contractorId;
                _context.Contact.Add(item);
            }
        }
        public bool Delete(long id)
        {
            var contact = GetById(id);
            if (contact == null)
            {
                return false;
            }

            _context.Contact.Remove(contact);
            return true;
        }
        public IQueryable<Contact> GetAll(long contractorId, QueryParameters queryParameters)
        {
            IQueryable<Contact> _allItems = _context.Contact.Where(ct => ct.ContractorId == contractorId).OrderBy(ct => ct.Id);

            return _allItems
                .Skip(queryParameters.PageSize * (queryParameters.PageNumber - 1))
                .Take(queryParameters.PageSize);
        }
        public Contact GetById(long contactId)
        {
            return _context.Contact.FirstOrDefault(x => x.Id == contactId);
        }
        public bool Update(Contact changedItem)
        {
            var originalItem = GetById(changedItem.Id);

            if(originalItem == null)
            {
                return false;
            }

            originalItem.ContractorId = changedItem.ContractorId;
            originalItem.TypeContact = changedItem.TypeContact;
            originalItem.value = changedItem.value;
            
            _context.Contact.Update(originalItem);
            return true;
        }
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
   }
}
