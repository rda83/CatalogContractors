using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CatalogContractors.API.ViewModel
{
    public class ResponseViewModel
    {
        public bool success { get; private set; }
        public long id { get; private set; }

        private ResponseViewModel(){}

        public static ResponseViewModel GetResponseViewModel(bool success, long id)
        {
            ResponseViewModel result = new ResponseViewModel()
            {
                success = success,
                id = id
            };

            return result;
        }

    }
}
