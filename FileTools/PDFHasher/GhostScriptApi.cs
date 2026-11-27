using System.Diagnostics;
using System.IO;

namespace FileTools.PDFHasher
{
    public class GhostScriptApi(GhostScriptLocator ghostScriptLocator) : IGhostScriptApi
    {
        private readonly string ExecutableFileName = ghostScriptLocator.GetLocation();

        public bool IsAvailable => this.ExecutableFileName != null;

        public (int ExitCode, string Output) ExtractPdfToPath(string filename, string destination)
        {
            using var psi = new Process();
            psi.StartInfo.FileName = ExecutableFileName;
            psi.StartInfo.ArgumentList.Add("-dBATCH");
            psi.StartInfo.ArgumentList.Add("-dNOPAUSE");
            psi.StartInfo.ArgumentList.Add("-dSAFER");
            psi.StartInfo.ArgumentList.Add("-sDEVICE=png16m");
            psi.StartInfo.ArgumentList.Add($"-sOutputFile={Path.Combine(destination, "pdffile-%04d.png")}");
            psi.StartInfo.ArgumentList.Add("-f");
            psi.StartInfo.ArgumentList.Add(Path.GetFullPath(filename));
            psi.StartInfo.CreateNoWindow = true;
            psi.StartInfo.UseShellExecute = false;
            psi.StartInfo.RedirectStandardOutput = true;

            psi.Start();

            var output = psi.StandardOutput.ReadToEnd();
            psi.WaitForExit();

            return (psi.ExitCode, output);
        }
    }
}
