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
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();


        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        public void start()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

            TimeSpan interval = new TimeSpan(0, 0, 2);
            Thread.Sleep(interval);

            ShowWindow(handle, SW_SHOW);



            string path = Path.GetPathRoot(Environment.SystemDirectory);
            Console.WriteLine(path);

        }
    }
}
