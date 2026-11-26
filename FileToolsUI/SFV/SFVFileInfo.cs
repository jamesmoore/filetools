using FileTools.FileHashCalculator;
using FileTools.StreamHashCalculator;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileToolsUI.SFV
{
    public enum SFVStatus
    {
        OK,
        Bad,
        Missing,
        IOException
    }

    public class SFVFileInfo
    {
        public IFileInfo file;

        public string relativename;
        //public string startingdirectory;

        public string CRCInSFVFile;
        public string CRCFromFile;

        public async Task Process()
        {
            try
            {
                var crcCalculator = new DefaultFileHashCalculator(new CRCHashCalculator());
                CRCFromFile = await crcCalculator.GetFileHash(file);
                if (CRCInSFVFile == null) CRCInSFVFile = CRCFromFile;

                if (CRCInSFVFile.ToLower() != CRCFromFile.ToLower())
                {
                    SFVFileScannedEvent(SFVStatus.Bad);
                }
                else
                {
                    SFVFileScannedEvent(SFVStatus.OK);
                }
                //this.EnsureVisible();
            }
            catch (System.IO.FileNotFoundException)
            {
                SFVFileScannedEvent(SFVStatus.Missing);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                SFVFileScannedEvent(SFVStatus.Missing);
            }
            catch (System.IO.IOException)
            {
                SFVFileScannedEvent(SFVStatus.IOException);
            }
        }

        public delegate void SFVFileScanned(SFVStatus status);

        public event SFVFileScanned SFVFileScannedEvent;
    }
}
