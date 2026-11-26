using System.Diagnostics;

namespace FileTools.PDFHasher
{
    public class GhostScriptApi(GhostScriptLocator ghostScriptLocator) : IGhostScriptApi
    {
        private readonly string ExecutableFileName = ghostScriptLocator.GetLocation();

        public bool IsAvailable => this.ExecutableFileName != null;

        public (int ExitCode, string Output) ExtractPdfToPath(string filename, string destination)
        {
            var arguments = $"-dBATCH -dNOPAUSE -dSAFER -sDEVICE=png16m -sOutputFile=\"{destination}\\pdffile-%04d.png\" \"{filename}\"";

            using var psi = new Process();
            psi.StartInfo.FileName = ExecutableFileName;
            psi.StartInfo.Arguments = arguments;
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
