using NEWPROJECT.Models;
using NEWPROJECT.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging.Console;
using System.Runtime.InteropServices;



namespace NEWPROJECT.Services
{
    public class BrandBagsService : IBrandBagsService
    {
        private void SaveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(BrandBags));
        }
        private List<BrandBags> BrandBags;
        private string fileName = "Data.json";
        Random rand = new Random();

        public BrandBagsService()
        {
            this.fileName = Path.Combine("data", "data.json");

            using (var jsonFile = File.OpenText(fileName))
            {
                BrandBags = JsonSerializer.Deserialize<List<BrandBags>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            }
        }

        public List<BrandBags> GetAll(int id)
        {
            return BrandBags.Where(p => p.UserId == id).ToList();
        }

        public BrandBags Get(int id)
        {
            return BrandBags.Where(p => p.Id == id).FirstOrDefault();
        }

        public void Add(BrandBags bag)
        {
            bag.Id =  rand.Next(1000, 10001);
            BrandBags.Add(bag);
            SaveToFile();

        }

        public void Delete(int id)
        {
            var bag = Get(id);
            if (bag is null)
                return;

            BrandBags.Remove(bag);
            
            SaveToFile();


        }
        
        public void DeleteAllBooks(int userId) 
        {
            BrandBags.RemoveAll(bag => bag.UserId == userId);
            SaveToFile();
        }

        public bool Update(int id, BrandBags newUser)
        {
            var index = BrandBags.FindIndex(p => p.Id == newUser.Id);
            if (index == -1)
                return false;

            BrandBags[index] = newUser;
            SaveToFile();
            return true;

        }

        public int Count { get => BrandBags.Count(); }
    }
}