using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using FileTools.Common;
using FileTools.FileHashCalculator;
using FileTools.StreamHashCalculator;
using FileToolsUI.SFV;

namespace FileTools
{
    class SFVFileProcessor(IFileSystem fileSystem) : IRecursionHandler
    {
        private List<SFVFileInfo> list = new List<SFVFileInfo>();

		public List<SFVFileInfo> List
        {
            get { return list; }
            set { list = value; }
        }

        public void Load(string strFilename)
        {
            var calculator = new DefaultFileHashCalculator(new CRCHashCalculator());
            var fi = fileSystem.FileInfo.New(strFilename);
			using var sr = new StreamReader(fi.FullName);
			string sfvLine;
			while ((sfvLine = sr.ReadLine()) != null)
			{
				if (sfvLine.Length < 9 || sfvLine.StartsWith(";"))
					continue;

				string sfvValue = sfvLine.Trim().Substring(sfvLine.Trim().Length - 8, 8);
				string sfvName = sfvLine.Trim().Substring(0, sfvLine.Trim().Length - 8).Trim();
				string absolutepath = fi.DirectoryName + "\\" + sfvName;
				//this.lstLog.Items.Add(absolutepath + "(" + sfvValue + ")");

				SFVFileInfo sfvfile = new SFVFileInfo();
				sfvfile.CRCInSFVFile = sfvValue;
				sfvfile.relativename = sfvName;
				sfvfile.file = fileSystem.FileInfo.New(absolutepath);

				this.List.Add(sfvfile);
			}
		}

        public void Save(string sfvFilename)
        {
			using var sw = new StreamWriter(sfvFilename);
			foreach (SFVFileInfo sfvfile in this.List)
			{
				sw.WriteLine(sfvfile.relativename + " " + sfvfile.CRCFromFile);
			}
		}

        public void ProcessDirectory(IDirectoryInfo di)
        {
            // no action
        }

        public void ProcessFile(IDirectoryInfo root, IFileInfo fi)
        {
            SFVFileInfo sf = new SFVFileInfo();
            sf.file = fi;
            //sf.startingdirectory = diRoot.FullName;
            sf.relativename = fi.FullName.Replace(root.FullName + "\\", "");
            this.List.Add(sf);
        }
    }

}
