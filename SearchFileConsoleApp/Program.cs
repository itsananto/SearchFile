using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SearchFileConsoleApp
{
    class Program
    {
        static DBHelper db = new DBHelper();
        static FileTraverseHelper fh = new FileTraverseHelper();
        static void Main(string[] args)
        {

            // Start with drives if you have to search the entire computer.
            /*string[] drives = Environment.GetLogicalDrives();

            foreach (string dr in drives)
            {
                DriveInfo di = new DriveInfo(dr);

                if (!di.IsReady)
                {
                    Console.WriteLine("The drive {0} could not be read", di.Name);
                    continue;
                }
                DirectoryInfo rootDir = di.RootDirectory;
                if (!db.IfHistoryExists())
                {
                    var sc = fh.TraverseTree(rootDir.Name);
                    db.PopulateFileArchiveTable(sc);
                }

                fh.AddWatcher(rootDir.Name);
            }

            if (!db.IfHistoryExists())
            {
                db.InsertSearchHistoryTable();
            }*/

            fh.AddWatcher("D:\\TV");
            // Wait for the user to quit the program.
            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        // Define the event handlers.
        public static void OnCreated(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("File Created: " + e.FullPath + " " + e.ChangeType);
        }

        public static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("File Changed: " + e.FullPath + " " + e.ChangeType);
        }

        public static void OnDeleted(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("File Deleted: " + e.FullPath + " " + e.ChangeType);
        }

        public static void OnRenamed(object source, RenamedEventArgs e)
        {
            if (File.Exists(e.FullPath))
            {
                db.RenameFile(new FileInfo(e.OldFullPath).Name, new FileInfo(e.FullPath).Name, e.OldFullPath, e.FullPath);
            }
            else if (Directory.Exists(e.FullPath))
            {
                // update those path which starts with the fullpath + \\
            }

            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }
    }
}
