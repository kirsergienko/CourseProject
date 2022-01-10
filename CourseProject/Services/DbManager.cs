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
            List<int> ids = new List<int>();
            List<int> collection_ids = new List<int>();
            FindCollectionIds(ref collection_ids, search);
            FindItemsIds(ref ids, search);
            return GetSearchResult(ids, collection_ids);
        }

        private List<Item> GetSearchResult(List<int> ids, List<int> collection_ids)
        {
            var items = new List<Item>();
            foreach (var id in collection_ids)
            {
                ids.AddRange(context.Items.Where(x => x.CollectionId == id).Select(x => x.Id));
            }
            ids = ids.Distinct().ToList();
            foreach (var id in ids)
            {
                var item = GetItem(id);
                if(item != null)
                items.Add(item);
            }
            return items;
        }

        private void FindCollectionIds(ref List<int> collection_ids, string search)
        {
            collection_ids.AddRange(context.Collections.Where(x => x.Description.Contains(search)).Select(x => x.Id));
            collection_ids.AddRange(context.Collections.Where(x => x.Title.Contains(search)).Select(x => x.Id));
        }

        private void FindItemsIds(ref List<int> ids, string search)
        {
            ids.AddRange(context.IntValues.Where(x => x.Name == search).Select(x => x.ItemId));
            ids.AddRange(context.BoolValues.Where(x=>x.Name == search).Select(x=>x.ItemId));
            ids.AddRange(context.DateValues.Where(x => x.Name == search).Select(x => x.ItemId));
            ids.AddRange(context.StringValues.Where(x => x.Name == search || x.Value == search).Select(x => x.ItemId));
            ids.AddRange(context.Comments.Where(x => x.Text.Contains(search)).Select(x => x.ItemId));
            ids.AddRange(context.Tags.Where(x => x.TagName.Contains(search)).Select(x => x.ItemId));
        }

        public List<Item> GetItems(int collectionId)
        {
            var items = new List<Item>();
            var temp = context.Items.Where(x => x.CollectionId == collectionId).ToList();
            foreach (var item in temp)
            {
                items.Add(InitialItem(item));
            }
            return items;
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

        private Item InitialItem(Item item)
        {
            var newItem = new Item();
            newItem.Id = item.Id;
            var tags = context.Tags.Where(x => x.ItemId == item.Id).Select(x => x.TagName);
            newItem.Tags = String.Join(" ", tags.ToArray());
            newItem.LastChanged = item.LastChanged;
            newItem.CollectionId = item.CollectionId;
            newItem.Comments = context.Comments.Where(x => x.ItemId == item.Id).ToList();
            newItem.Likes = context.Likes.Where(x => x.ItemId == item.Id).ToList();
            return InititalValues(newItem, item);
        }

        private Item InititalValues(Item newItem, Item item)
        {
            newItem.IntValues = new List<IntValue>();
            newItem.BoolValues = new List<BoolValue>();
            newItem.StringValues = new List<StringValue>();
            newItem.DateValues = new List<DateValue>();
            newItem.IntValues.AddRange(context.IntValues.Where(x => x.ItemId == item.Id).ToList());
            newItem.BoolValues.AddRange(context.BoolValues.Where(x => x.ItemId == item.Id).ToList());
            newItem.StringValues.AddRange(context.StringValues.Where(x => x.ItemId == item.Id).ToList());
            newItem.DateValues.AddRange(context.DateValues.Where(x => x.ItemId == item.Id).ToList());
            return newItem;
        }

        public Item GetItem(int id)
        {
            var i = context.Items.FirstOrDefault(x => x.Id == id);
            return i == null ? null : InitialItem(i);
        }

        public void RemoveItem(int id)
        {
            var c = context.Items.First(x => x.Id == id);
            context.Items.Remove(c);
            context.SaveChanges();
        }

        public int AddItem(Item item)
        {
            item.LastChanged = DateTime.Now;
            context.Items.Add(item);
            context.SaveChanges();
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
            RemoveTags(item.Id);
            AddTags(item.Tags, item.Id);
            var i = context.Items.FirstOrDefault(x => x.Id == item.Id);
            i.LastChanged = DateTime.Now;
            context.SaveChanges();
        }

        private void RemoveTags(int id)
        {
            var c = context.Tags.Where(x => x.ItemId == id);
            context.Tags.RemoveRange(c);
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