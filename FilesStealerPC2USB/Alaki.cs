using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FilesStealerPC2USB
{
    class Alaki
    {
        //        [DllImport("kernel32.dll")]
        //      static extern IntPtr GetConsoleWindow();


        //        [DllImport("user32.dll")]
        //static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //const int SW_HIDE = 0;
        //const int SW_SHOW = 5;

        public void Start()
        {
            string[] str = { "nice","ok"};

            string sth = string.Join(" ",str);
            Console.WriteLine(string.Join(" ", sth));
        }

    }

}
