using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogContractors.Models
{
    public class Contractor
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Contact> Contacts { get; set; }

    }
}
