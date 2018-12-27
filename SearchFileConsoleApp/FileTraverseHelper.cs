using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SearchFileConsoleApp
{
    class FileTraverseHelper
    {
        public FileTraverseHelper()
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("log.txt",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true)
            .CreateLogger();
        }

        public StringCollection TraverseTree(string root)
        {
            StringCollection fileCollection = new StringCollection();
            Stack<string> dirs = new Stack<string>(20);

            if (!Directory.Exists(root))
            {
                throw new ArgumentException();
            }
            dirs.Push(root);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs;
                try
                {
                    subDirs = Directory.GetDirectories(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Log.Error(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Log.Error(e.Message);
                    continue;
                }

                string[] files = null;
                try
                {
                    files = Directory.GetFiles(currentDir);
                }

                catch (UnauthorizedAccessException e)
                {

                    Log.Error(e.Message);
                    continue;
                }

                catch (DirectoryNotFoundException e)
                {
                    Log.Error(e.Message);
                    continue;
                }
                // Perform the required action on each file here.
                // Modify this block to perform your required task.
                foreach (string file in files)
                {
                    try
                    {
                        FileInfo fi = new FileInfo(file);
                        fileCollection.Add(fi.Name);
                    }
                    catch (FileNotFoundException e)
                    {
                        Log.Error(e.Message);
                        continue;
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.Message);
                        continue;
                    }
                }

                foreach (string str in subDirs)
                {
                    dirs.Push(str);
                }
            }

            return fileCollection;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void AddWatcher(string root)
        {
            FileSystemWatcher watcher = new FileSystemWatcher()
            {
                Path = root,
                IncludeSubdirectories = true,
                Filter = "*.*"
            };

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnCreated);
            watcher.Deleted += new FileSystemEventHandler(OnDeleted);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        private static void OnCreated(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("File Created: " + e.FullPath + " " + e.ChangeType);
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("File Changed: " + e.FullPath + " " + e.ChangeType);
        }

        private static void OnDeleted(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("File Deleted: " + e.FullPath + " " + e.ChangeType);
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }
    }
}
