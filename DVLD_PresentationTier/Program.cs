using DotNetEnv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DVLD_PresentationTier
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string envPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "A:\\ProjectInC#V2\\DVLD_PresentationTier\\.env"));
            if (File.Exists(envPath))
            {
                // DotNetEnv.Env.Load has overloads that accept a path
                DotNetEnv.Env.Load(envPath);
            }
            else
            {
                // fallback: try default search (or log/throw)
                DotNetEnv.Env.Load();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmTest());
           // Application.Run(new Main());
            Application.Run(new frmLogin());

            


        }
    }
}
