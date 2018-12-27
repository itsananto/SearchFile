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
                "File_Name NVARCHAR(2048) NULL)";

            SQLiteCommand createTable = new SQLiteCommand(tableCommand, db);

            createTable.ExecuteReader();


            tableCommand = "CREATE TABLE IF NOT " +
                "EXISTS Search_History (Primary_Key INTEGER PRIMARY KEY, " +
                "Time INTEGER NOT NULL)";

            createTable = new SQLiteCommand(tableCommand, db);

            createTable.ExecuteReader();
            db.Close();
        }

        public void PopulateFileArchiveTable(StringCollection files)
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

        public void InsertSearchHistoryTable()
        {
            db.Open();

            SQLiteCommand insertCommand = new SQLiteCommand();
            insertCommand.Connection = db;

            using (var transaction = db.BeginTransaction())
            {
                insertCommand.CommandText = "INSERT INTO Search_History VALUES (NULL, @Entry);";
                insertCommand.Parameters.AddWithValue("@Entry", DateTime.Now.Ticks);

                insertCommand.ExecuteNonQuery();
                transaction.Commit();
            }

            db.Close();
        }

        public bool IfHistoryExists()
        {
            db.Open();

            String tableCommand = "SELECT Time FROM Search_History";

            SQLiteCommand createTable = new SQLiteCommand(tableCommand, db);

            var reader = createTable.ExecuteReader();
            bool hasRows = reader.HasRows;

            db.Close();

            return hasRows;
        }
    }
}
