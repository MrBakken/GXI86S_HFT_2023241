using GXI86S_HFT_2023241.Models;
using System.Linq;

namespace GXI86S_HFT_2023241.Logic.InterfaceLogic
{
    public interface ITransactionLogic
    {
        void Create(Transaction item);
        void Delete(int id);
        Transaction Read(int id);
        IQueryable<Transaction> ReadAll();
        void Update(Transaction item);
    }
}