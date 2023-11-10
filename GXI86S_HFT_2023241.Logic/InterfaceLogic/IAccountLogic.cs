using GXI86S_HFT_2023241.Models;
using System.Linq;

namespace GXI86S_HFT_2023241.Logic.InterfaceLogic
{
    public interface IAccountLogic
    {
        void Create(Account item);
        void Delete(int id);
        Account Read(int id);
        IQueryable<Account> ReadAll();
        void Update(Account item);
    }
}