namespace FileTools.PDFHasher
{
    public interface IGhostScriptApi
    {
        bool IsAvailable { get; }

        (int ExitCode, string Output) ExtractPdfToPath(string filename, string tempPath);
    }
}