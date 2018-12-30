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

        public List<FileInformation> TraverseTree(string root)
        {
            List<FileInformation> list = new List<FileInformation>();
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
                        list.Add(new FileInformation { Name = fi.Name, Path = fi.FullName });
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

            return list;
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
            watcher.Changed += new FileSystemEventHandler(Program.OnChanged);
            watcher.Created += new FileSystemEventHandler(Program.OnCreated);
            watcher.Deleted += new FileSystemEventHandler(Program.OnDeleted);
            watcher.Renamed += new RenamedEventHandler(Program.OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        
    }
}
