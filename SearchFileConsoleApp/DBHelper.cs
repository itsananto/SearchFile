using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchFileConsoleApp
{
    class DBHelper
    {
        private SQLiteConnection db;

        public DBHelper()
        {
            //TODO: Replace path with fillowing: Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SearchFile", "SearchFile.db3")
            db = new SQLiteConnection(@"Data Source=Test.db3;Pooling=true;FailIfMissing=false;Version=3");
            InitializeDatabase();
        }

        public void InitializeDatabase()
        {
            db.Open();

            String tableCommand = "CREATE TABLE IF NOT " +
                "EXISTS File_Archive (Primary_Key INTEGER PRIMARY KEY, " +
                "Text_Entry NVARCHAR(2048) NULL)";

            SQLiteCommand createTable = new SQLiteCommand(tableCommand, db);

            createTable.ExecuteReader();


            tableCommand = "CREATE TABLE IF NOT " +
                "EXISTS Search_History (Primary_Key INTEGER PRIMARY KEY, " +
                "Time INTEGER NOT NULL)";

            createTable = new SQLiteCommand(tableCommand, db);

            createTable.ExecuteReader();
        }

        public void PopulateFileArchiveTable(StringCollection files)
        {
            using (SQLiteConnection db = new SQLiteConnection(@"Data Source=Test.db3;Pooling=true;FailIfMissing=false;Version=3"))
            {
                db.Open();

                SQLiteCommand insertCommand = new SQLiteCommand();
                insertCommand.Connection = db;

                using (var transaction = db.BeginTransaction())
                {

                    foreach (var file in files)
                    {
                        insertCommand.CommandText = "INSERT INTO File_Archive VALUES (NULL, @Entry);";
                        insertCommand.Parameters.AddWithValue("@Entry", file);

                        insertCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();

                }

                db.Close();
            }

        }
    }
}
