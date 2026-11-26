using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace FileTools.Compressed
{
    public class _7zwrapper(string _7zPath) : ICompressedFileAPI
    {

		// to add
		// 7za.exe a -mpass=4 -mfb=255 -r -y -tzip "C:\Users\jamesm\Desktop\ZIPMAX~1\_ext_001\DPS-MLB4.ZIP" *

        public async Task<bool> Test(string filename)
        {
            string s;
            return Execute7zipCommand("t " + Enquote(filename), out s);
        }

        public bool Extract(string filename, string directory)
        {
            string s;
            return Execute7zipCommand("x " + Enquote(filename) + " -o" + Enquote(directory), out s);
        }

        public bool CompressDirectory(string filename, string directory, string format)
        {
            string s;
            //return Execute7zipCommand("a -mpass=4 -mfb=255 -r -y -tzip " + Enquote(filename) + " " + Enquote(directory + "\\*"));
            return Execute7zipCommand("a -mx=9 -r -y -t" + format + " " + Enquote(filename) + " " + Enquote(directory + "\\*"), out s);
        }

        public CompressedFile List(string filename)
        {
            string s;
            Execute7zipCommand("l -slt " + Enquote(filename), out s);

            string[] lines = s.Split('\r');

            CompressedFile files = new CompressedFile();
            CompressedFileEntry compressedFile = null;
            foreach (string line in lines)
            {
                if (line.Contains(" = "))
                {
                    int position = line.IndexOf(" = ");
                    string name = line.Substring(0, position).Trim();
                    string value = line.Substring(position + 3).Trim();

                    if (name == "Path")
                    {
                        compressedFile = new CompressedFileEntry();
                        files.Add(compressedFile);
                        compressedFile.Path = value;
                    }

                    if (name == "Folder")
                    {
                        if (value == "+") files.Remove(compressedFile);
                        compressedFile.Folder = value;
                    }

                    if (name == "Size")
                    {
                        long l;
                        long.TryParse(value, out l);
                        compressedFile.Size = l;
                    }

                    if (name == "Packed Size")
                    {
                        long l;
                        long.TryParse(value, out l);
                        compressedFile.PackedSize = l;
                    }

                    if (name == "Modified")
                    {
                        if (string.IsNullOrEmpty(value) == false)
                            compressedFile.Modified = DateTime.Parse(value.Replace(" ", "T"));

                    }

                    if (name == "Attributes")
                    {
                        compressedFile.Attributes = value;
                    }

                    if (name == "CRC")
                    {
                        try
                        {
                            if (value != string.Empty)
                            {
                                compressedFile.Crc = long.Parse(value, NumberStyles.AllowHexSpecifier);
                            }
                            else
                            {
                                files.Remove(compressedFile);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.ToString());
                        }
                    }

                }
            }

            return files;
        }

        private bool Execute7zipCommand(string arguments, out string output)
        {
            Process psi = new Process();
            psi.StartInfo.FileName = $"{_7zPath}\\7z.exe";
            psi.StartInfo.Arguments = arguments;
            psi.StartInfo.CreateNoWindow = true;
            psi.StartInfo.UseShellExecute = false;
            psi.StartInfo.RedirectStandardOutput = true;

            psi.Start();

            output = psi.StandardOutput.ReadToEnd();

            psi.WaitForExit();

            if (psi.ExitCode != 0)
            {
                throw new Exception(output);
            }
            else
            {
                return true;
            }


        }

        private string Enquote(string s)
        {
            return "\"" + s + "\"";
        }


    }
}
