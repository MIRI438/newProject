using NEWPROJECT.Models;
using System.Collections.Generic;

namespace NEWPROJECT.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();
        User GetById(int id);
        void Add(User newUser);
        bool Update(int id, User newUser);
        bool Delete(int id);
        int ExistUser(string name, int password);
    }
}
