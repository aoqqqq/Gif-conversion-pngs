using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//bilibili W傲奇W
namespace Tr
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //try
            //{
            //    throw new Exception();
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine( "Run");

            //}
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());   
        }

        static void Main4(string[] args)  
        {
            ManualResetEvent manual = new ManualResetEvent(false);
            var th = new Thread(() => 
            {
                Console.WriteLine("open");
                manual.WaitOne();
                Console.WriteLine("线程");
            });
            th.Start(); 
            Thread.Sleep(10000);
            manual.Reset();
        }

    }
}
