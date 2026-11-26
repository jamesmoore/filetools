using FileTools.HashStore;
using System;
using System.IO.Abstractions;
using System.Windows.Forms;

namespace FileTools
{
	class Program
	{
		[STAThread]
		static void Main()
        {
            var fileSystem = new FileSystem();

            var hashStoreFactory = new HashStoreFactory(fileSystem);

            Application.SetHighDpiMode(HighDpiMode.DpiUnaware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using var hashStore = hashStoreFactory.GetHashStore();
            Application.Run(new frmParent(hashStore, fileSystem));
        }
    }
}