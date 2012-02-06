using System;
using System.Windows.Forms;
using Artwork_Stack.GUI;

namespace Artwork_Stack
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Setup());
        }
    }
}
