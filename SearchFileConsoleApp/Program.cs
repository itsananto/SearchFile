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
        static void Main(string[] args)
        {
            DBHelper db = new DBHelper();
            FileTraverseHelper fh = new FileTraverseHelper();

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
                    StringCollection sc = fh.TraverseTree(rootDir.Name);
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
    }
}
