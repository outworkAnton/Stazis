using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Stazis
{
    static class Program
    {
        public static MainForm myForm;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myForm = new MainForm();
            Application.Run(myForm);
        }
    }
}
