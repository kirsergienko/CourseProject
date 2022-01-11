using CourseProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace CourseProject.Services
{
    public class DbManager
    {
        ApplicationContext context = new ApplicationContext();

        public void AddTags(string tagString, int itemId)
        {
            if (!String.IsNullOrEmpty(tagString))
            {
                List<string> temp = tagString.Split(' ').ToList();
                List<Tag> tags = new List<Tag>();
                foreach (var t in temp)
                {
                    tags.Add(new Tag { TagName = t, ItemId = itemId });
                }
                context.Tags.AddRange(tags);
                context.SaveChanges();
            }
        }

        public List<Tag> GetTagList()
        {
            return context.Tags.GroupBy(x => x.TagName).Select(x => x.FirstOrDefault()).ToList();
        }

        public List<UserModel> ReturnUsers()
        {
            return context.Users.ToList();
        }

        public List<Item> GetItems(string search)
        {
            List<Item> items = new List<Item>();
            items.AddRange(context.Items.Where(x => 
            x.Comments.Any(c => c.Text == search) ||
            x.BoolValues.Any(c => c.Name == search) ||
            x.StringValues.Any(c => c.Name == search) ||
            x.StringValues.Any(c => c.Value == search) ||
            x.IntValues.Any(c => c.Name == search) ||
            x.DateValues.Any(c => c.Name == search) ||
            x.Tags.Contains(search)).ToList());
            context.Collections.Where(x => x.Title.Contains(search) ||
            x.Description.Contains(search)).Select(x => x.Id).ToList()
            .ForEach(x => items.AddRange(context.Items.Where(i => i.CollectionId == x).ToList()));
            return items.Distinct().ToList();
        }


        public List<Item> GetItems(int collectionId)
        {
            return context.Items.Where(x => x.CollectionId == collectionId).ToList();
        }

        public List<Item> GetItemsByTag(string search)
        {
            return context.Items.Where(x => x.Tags.Contains(search)).ToList();
        }

        public List<Item> GetLastAddedItems()
        {
            return context.Items.OrderByDescending(x => x.LastChanged).Take(10).ToList();
        }

        public List<TagCloud> GetTagCloud()
        {
            return context.Tags.GroupBy(x => x.TagName).Select(x => new TagCloud { Tag = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Take(10).ToList();
        }

        public List<Collection> GetBiggestCollections()
        {
            return context.Collections.OrderByDescending(x=>x.ItemsCount).Take(5).ToList();
        }

        public void AddLike(Like like)
        {
            if (context.Likes.Any(x => x.ItemId == like.ItemId && x.UserId == like.UserId))
            {
                RemoveLike(like);
            }
            else
            {
                context.Likes.Add(like);
            }
            context.SaveChanges();
        }

        public void RemoveLike(Like like)
        {
            var _like = context.Likes.FirstOrDefault(x => x.UserId == like.UserId && x.ItemId == like.ItemId);
            context.Likes.Remove(_like);
            context.SaveChanges();
        }
        public void AddComment(Comment comment)
        {
            context.Comments.Add(comment);
            context.SaveChanges();
        }

        public Item GetItem(int id)
        {
            return context.Items.FirstOrDefault(x => x.Id == id);
        }

        public void RemoveItem(int id)
        {
            var i = context.Items.First(x => x.Id == id);
            context.Items.Remove(i);
            var c = context.Collections.Where(x => x.Id == i.CollectionId).FirstOrDefault();
            c.ItemsCount--;
            context.Configuration.ValidateOnSaveEnabled = false;
            context.SaveChanges();
            context.Configuration.ValidateOnSaveEnabled = true;
        }

        public int AddItem(Item item)
        {
            item.Tags = "";
            item.LastChanged = DateTime.Now;
            context.Items.Add(item);
            var c = context.Collections.Where(x => x.Id == item.CollectionId).FirstOrDefault();
            c.ItemsCount++;
            context.Configuration.ValidateOnSaveEnabled = false;
            context.SaveChanges();
            context.Configuration.ValidateOnSaveEnabled = true;
            return item.Id;
        }

        public void AddValues(Item item)
        {
            if (item.IntValues != null) context.IntValues.AddRange(item.IntValues);
            if (item.BoolValues != null) context.BoolValues.AddRange(item.BoolValues);
            if (item.StringValues != null) context.StringValues.AddRange(item.StringValues);
            if (item.DateValues != null) context.DateValues.AddRange(item.DateValues);
            context.SaveChanges();
        }

        private void RemoveValues(int id)
        {
            context.IntValues.RemoveRange(context.IntValues.Where(x => x.ItemId == id));
            context.BoolValues.RemoveRange(context.BoolValues.Where(x => x.ItemId == id));
            context.StringValues.RemoveRange(context.StringValues.Where(x => x.ItemId == id));
            context.DateValues.RemoveRange(context.DateValues.Where(x => x.ItemId == id));
            context.SaveChanges();
        }

        public void EditItem(Item item)
        {
            RemoveValues(item.Id);
            AddValues(item);
            var i = context.Items.FirstOrDefault(x => x.Id == item.Id);
            i.LastChanged = DateTime.Now;
            i.Tags = item.Tags;
            context.SaveChanges();
        }

        public void AddCollection(Collection collection)
        {
            context.Collections.Add(collection);
            context.SaveChanges();
        }

        public void EditCollection(Collection collection)
        {
            var c = context.Collections.FirstOrDefault(x => x.Title == collection.Title);
            context.Entry(c).CurrentValues.SetValues(collection);
            context.Configuration.ValidateOnSaveEnabled = false;
            context.SaveChanges();
            context.Configuration.ValidateOnSaveEnabled = true;
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

        public List<Collection> GetCollections(int userid)
        {
            return context.Collections.Where(x=>x.UserId == userid).ToList();
        }

        public Collection GetCollection(int id)
        {
            return context.Collections.FirstOrDefault(x => x.Id == id);
        }

        public void RemoveCollection(int id)
        {
            var c = context.Collections.First(x => x.Id == id);
            context.Collections.Remove(c);
            context.SaveChanges();
        }
    }
}