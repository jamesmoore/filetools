using System;
using System.IO;
using System.IO.Abstractions;

namespace FileTools.Common
{

	public class DirectoryRecurser(IRecursionHandler handler)
	{
		public event EventHandler<DirectoryRecurserEvent> DirectoryRecurserAction;

        protected void OnDirectoryRecurserAction(DirectoryRecurserEventType type, IDirectoryInfo info)
        {
            if (this.DirectoryRecurserAction != null)
            {
                var de = new DirectoryRecurserEvent(type);
                de.DirectoryInfo = info;
                DirectoryRecurserAction(this, de);
            }
        }

        protected void OnDirectoryRecurserAction(DirectoryRecurserEventType type, IFileInfo info)
        {
            if (this.DirectoryRecurserAction != null)
            {
                var de = new DirectoryRecurserEvent(type);
                de.FileInfo = info;
                DirectoryRecurserAction(this, de);
            }
        }

		#region Constructors

		#endregion

		public void Recurse(IDirectoryInfo root)
        {
            RecurseInternal(root, root);
        }

        protected void RecurseInternal(IDirectoryInfo root, IDirectoryInfo di)
        {
            OnDirectoryRecurserAction(DirectoryRecurserEventType.ScanDirectory, di);
            handler.ProcessDirectory(di);

            try
            {
                foreach (var fi in di.GetFiles())
                {
                    OnDirectoryRecurserAction(DirectoryRecurserEventType.ScanFile, fi);
                    handler.ProcessFile(root, fi);
                }
            }
            catch (UnauthorizedAccessException)
            {
                //TODO
            }
            catch (DirectoryNotFoundException) { }

            try
            {
                foreach (var di2 in di.GetDirectories())
                {
                    try
                    {
                        RecurseInternal(root, di2);
                    }
                    catch (UnauthorizedAccessException) { }
                }
            }
            catch (UnauthorizedAccessException)
            {
                //TODO
            }
            catch (DirectoryNotFoundException) { }
        }




    }
}
