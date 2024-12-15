using NEWPROJECT.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NEWPROJECT.Interfaces;

namespace NEWPROJECT.Services
{
    public class usersService : IUserService
    {
        private List<User> Users;
        private string fileName = "Users.json";
        int nextId = 1444;
        readonly IBrandBagsService ifUserDeleted;

        private void SaveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(Users));
        }

        public usersService(IBrandBagsService ifUserDeleted)
        {
            this.ifUserDeleted = ifUserDeleted;
            this.fileName = Path.Combine("data", "users.json");

            using (var jsonFile = File.OpenText(fileName))
            {
                Users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            }
        }

        public List<User> GetAll()
        {
            return Users;
        }

        public User Get(int id)
        {
            return Users.Where(p => p.Id == id).FirstOrDefault();
        }

        public void Add(User user)
        {
            user.Id = nextId++;
            Users.Add(user);
            SaveToFile();

        }

        public bool Delete(int id)
        {
            var user = Get(id);
            if (user is null)
                return false;

            Users.Remove(user);
            ifUserDeleted.DeleteAllBooks(user.Id);
            SaveToFile();
            return true;


        }

        public bool Update(int id, User newUser)
        {
            var index = Users.FindIndex(p => p.Id == newUser.Id);
            if (index == -1)
                return false;

            Users[index] = newUser;
            SaveToFile();
            return true;

        }

        public int ExistUser(string name, int password)
        {
            User p = Get(password);
            if (p == null)
                return -1;
            return 0;
        }

        public User GetById(int id) => Users.FirstOrDefault(p => p.Id == id);

        public int Count { get => Users.Count(); }
    }
}
