using FileTools.HashStore;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileTools
{
    public static class ExtensionMethods
    {
        public static string ToBase64(this byte[] array)
        {
            return Convert.ToBase64String(array);
        }

        public static byte[] FromBase64(this string hex)
        {
            return Convert.FromBase64String(hex);
        }

		public static void ClearAttributes(this IFileInfo fileInfo)
		{
			fileInfo.Attributes = FileAttributes.Normal;
		}

		public static void ClearAttributes(this IDirectoryInfo directoryInfo)
		{
			directoryInfo.Attributes = FileAttributes.Normal;
		}

        public static async Task<string> GetOrAddAsync(this IHashStore hashStore, string key, Func<Task<string>> f)
        {
            var hash = hashStore.GetHash(key);

            if (hash != null)
            {
                return hash;
            }
            else
            {
                hash = await f();
                await hashStore.StoreHash(key, hash);
                return hash;
            }
        }

        public static void DeleteFile(this IFileInfo fi)
		{
			var parent = fi.Directory;
			try
			{
				fi.Delete();
			}
			catch (UnauthorizedAccessException)
			{
				fi.Refresh();
				if (!fi.Exists || (fi.Attributes & FileAttributes.ReadOnly) == 0) throw;

				// Attribute writes can be denied on a share even when deletion is allowed.
				// Only change the read-only flag when a failed deletion requires it.
				fi.Attributes &= ~FileAttributes.ReadOnly;
				fi.Delete();
			}
			parent.DeleteDirectoryIfEmpty();
		}

		public static bool HasContents(this IDirectoryInfo di)
		{
			return di.GetDirectories().Length != 0 || di.GetFiles().Length != 0;
		}

		public static void DeleteDirectoryIfEmpty(this IDirectoryInfo di)
		{
			if (di.IsRoot() || di.HasContents()) return;

			var parent = di.Parent;
			di.Delete(false);
			if (parent.HasContents() == false && parent.IsRoot() == false)
			{
				DeleteDirectoryIfEmpty(parent);
			}
		}

		private static bool IsRoot(this IDirectoryInfo di)
		{
			return di.Parent == null;
		}
	}
}
