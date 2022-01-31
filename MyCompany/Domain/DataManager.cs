using MyCompany.Domain.Repositories.Abstract;

namespace MyCompany.Domain
{
    public class DataManager
    {

        public IServiceItemsRepository ServiceItems { get; set; }

        public DataManager( IServiceItemsRepository serviceItemsRepository)
        {
       
            ServiceItems = serviceItemsRepository;
        }
    }
}
