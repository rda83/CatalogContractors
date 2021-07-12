using CatalogContractors.Infra;
using CatalogContractors.Models;
using CatalogContractors.Models.Repositories;
using CatalogContractors.Database;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace CatalogContractors.App.Repositories
{
   public class ContractorRepository : IContractorRepository
    {
        private readonly Context _context;

        public ContractorRepository(Context context)
        {
            _context = context;
        }
        public void Add(Contractor item)
        {
            _context.Contractor.Add(item);
        }
        public bool Delete(long id)
        {
            Contractor contractor = GetById(id);
            if (contractor == null)
            {
                return false;
            }

            _context.Contractor.Remove(contractor);
            return true;
        }
        public IQueryable<Contractor> GetAll(QueryParameters queryParameters)
        {
            IQueryable<Contractor> _allItems = _context.Contractor.Include(c => c.Contacts).OrderBy(ct => ct.Id);

            return _allItems
                .Skip(queryParameters.PageSize * (queryParameters.PageNumber - 1))
                .Take(queryParameters.PageSize);
        }
        public Contractor GetById(long id)
        {
            //return _context.Contractor.FirstOrDefault(x => x.Id == id);
            return _context.Contractor.Include(c => c.Contacts).FirstOrDefault(x => x.Id == id);
        }
        public bool Update(Contractor changedItem)
        {
            var originalItem = GetById(changedItem.Id);

            if(originalItem == null)
            {
                return false;
            }

            originalItem.Name = changedItem.Name;
            originalItem.Description = changedItem.Description;
            
            _context.Contractor.Update(originalItem);
            return true;
        }
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
   }
}
