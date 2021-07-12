using System.ComponentModel.DataAnnotations;

namespace CatalogContractors.Models
{
    public class TypeContact
    {
        public long Id { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}
