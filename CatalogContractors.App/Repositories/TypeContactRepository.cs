using CatalogContractors.Infra;
using CatalogContractors.Models;
using CatalogContractors.Models.Repositories;
using CatalogContractors.Database;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace CatalogContractors.App.Repositories
{
   public class TypeContactRepository : ITypeContactRepository
   {
        private readonly Context _context;

        public TypeContactRepository(Context context)
        {
            _context = context;
        }
        public void Add(TypeContact item)
        {
            _context.TypeContact.Add(item);
        }
        public bool Delete(long id)
        {
            TypeContact typeContact = GetById(id);
            if (typeContact == null)
            {
                return false;
            }

            _context.TypeContact.Remove(typeContact);
            return true;
        }
        public IQueryable<TypeContact> GetAll(QueryParameters queryParameters)
        {
            IQueryable<TypeContact> _allItems = _context.TypeContact.OrderBy(ct => ct.Id);

            return _allItems
                .Skip(queryParameters.PageSize * (queryParameters.PageNumber - 1))
                .Take(queryParameters.PageSize);
        }
        public TypeContact GetById(long id)
        {
            return _context.TypeContact.FirstOrDefault(x => x.Id == id);
        }
        public bool Update(TypeContact changedItem)
        {
            var originalItem = GetById(changedItem.Id);

            if(originalItem == null)
            {
                return false;
            }

            originalItem.Name = changedItem.Name;

            _context.TypeContact.Update(originalItem);
            return true;
        }
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
   }
}
