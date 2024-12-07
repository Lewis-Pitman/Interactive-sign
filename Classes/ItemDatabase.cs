using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SQLite;

namespace Interactive_sign.Classes
{
    public class ItemDatabase
    {
        private const string databaseName = "InteractiveSignLocalDatabase.db";
        private static string databasePath = Path.Combine(AppContext.BaseDirectory, databaseName);
        public const SQLite.SQLiteOpenFlags flags = SQLite.SQLiteOpenFlags.ReadOnly;
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(databasePath, flags);
        }

        public async Task<CentreItem> GetItem(int ItemID)
        {
            await Init();
            return await database.Table<CentreItem>().Where(i => i.ItemID == ItemID).FirstOrDefaultAsync();
        }

        public async Task<Dictionary<string, int>> GetCategories()
        {
            //Returns data needed to display the categories: The names and an itemID in that category used to fetch an image

            await Init();
            //Get a list of every item's category
            var allCategories = await database.QueryAsync<CentreItem>("SELECT ItemCategory FROM CentreItem"); 

            //Ignore duplicates and get a list of unique categories
            var uniqueCategories = new HashSet<string>();

            foreach(CentreItem item in allCategories)
            {
                uniqueCategories.Add(item.ItemCategory);
            }

            //Get the first item in each category and store its ID, we can use this to get an image for the category
            var itemInEachCategory = new Dictionary<string, int>();

            foreach(string category in uniqueCategories)
            {
                var categoryItem = await database.Table<CentreItem>().Where(i => i.ItemCategory == category).FirstOrDefaultAsync();
                int categoryItemID = categoryItem.ItemID;
                itemInEachCategory.Add(category, categoryItemID);
            }

            return itemInEachCategory;
        }

        public async Task<List<CentreItem>> GetItemsInCategory(string category)
        {
            await Init();

            var ItemsInCategory = await database.QueryAsync<CentreItem>("SELECT * FROM CentreItem WHERE ItemCategory = ?", category);
            return ItemsInCategory;
        }
    }
}
