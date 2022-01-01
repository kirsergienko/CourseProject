using CourseProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseProject.Services
{
    public class DbManager
    {
        ApplicationContext context = new ApplicationContext();

        public List<UserModel> ReturnUsers()
        {
            return context.Users.ToList();
        }

        public int AddItem(Item item)
        {
            context.Items.Add(item);
            context.SaveChanges();
            return item.Id;
        }

        public void AddValues(AddItemModel item)
        {
            context.IntValues.AddRange(item.IntValues);
            context.BoolValues.AddRange(item.BoolValues);
            context.StringValues.AddRange(item.StringValues);
            context.DateValues.AddRange(item.DateValues);
            context.SaveChanges();
        }

        public void AddCollection(Collection collection)
        {
            context.Collections.Add(collection);
            context.SaveChanges();
        }

        public void AddUser(UserModel user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public UserModel Find(string userName)
        {
            return context.Users.FirstOrDefault(x => x.UserName == userName);
        }

        public void RemoveUser(int id)
        {
            context.Users.Remove(context.Users.FirstOrDefault(x => x.Id == id));
            context.SaveChanges();
        }

        public void Unblock(int id)
        {
            context.Users.FirstOrDefault(x => x.Id == id).IsBlocked = false;
            context.SaveChanges();
        }

        public void Block(int id)
        {
            context.Users.FirstOrDefault(x => x.Id == id).IsBlocked = true;
            context.SaveChanges();
        }

        public UserModel Find(int? id)
        {
            return context.Users.FirstOrDefault(x => x.Id == id);
        }

        public void ChangeRole(int id)
        {
            context.Users.FirstOrDefault(x => x.Id == id).IsAdmin = !context.Users.FirstOrDefault(x => x.Id == id).IsAdmin;
            context.SaveChanges();
        }

        public Collection GetCollection(Collection collection)
        {
            return context.Collections.FirstOrDefault(x => x.Title == collection.Title);
        }

        public Collection GetCollection(int id)
        {
            return context.Collections.FirstOrDefault(x => x.Id == id);
        }
    }
}