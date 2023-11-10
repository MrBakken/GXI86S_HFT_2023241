using GXI86S_HFT_2023241.Models;
using System.Linq;

namespace GXI86S_HFT_2023241.Logic.InterfaceLogic
{
    public interface ICustomerLogic
    {
        void Create(Customer item);
        void Delete(int id);
        Customer Read(int id);
        IQueryable<Customer> ReadAll();
        void Update(Customer item);
    }
}