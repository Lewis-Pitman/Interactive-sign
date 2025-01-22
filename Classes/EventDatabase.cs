using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SQLite;
using Microsoft.Extensions.Logging;

namespace Interactive_sign.Classes
{
    public class EventDatabase
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

        public async Task<EventItem> GetEvent(int EventID)
        {
            await Init();
            return await database.Table<EventItem>().Where(i => i.EventID == EventID).FirstOrDefaultAsync();
        }

        public async Task<List<EventItem>> GetTodaysEvents()
        {

            await Init();

            string today = DateTime.Today.ToString("yyyy-MM-dd");
            var eventList = await database.Table<EventItem>().Where(i => i.Date == today).ToListAsync();
            //return eventList;

            // !! Changed for demo purposes. The above code works for getting today's events only,
            //    however all items are being returned below in order to demonstarte the event
            //    functionality without having to manually edit the date in the database each time.
            return await database.Table<EventItem>().ToListAsync();
        }
    }
}
