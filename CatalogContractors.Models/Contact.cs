using System.ComponentModel.DataAnnotations;

namespace CatalogContractors.Models
{
    public class Contact
    {
        public long Id { get; set; }
        public long ContractorId { get; set; }
        //public Contractor Contractor { get; set; }
        public TypeContact TypeContact { get; set; }

        [Required]
        public string value { get; set; }
    }
}
