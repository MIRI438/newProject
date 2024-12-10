using NEWPROJECT.Models;
using System.Collections.Generic;
using System.Linq;

namespace NEWPROJECT.Interfaces
{
    public interface IBrandBagsService
    {
        List <BrandBags> GetAll(int id);

        BrandBags Get(int id);

        void Add(BrandBags bags);

        void Delete(int id);

        bool Update(int id, BrandBags bags);

        int Count { get;}
    }
}