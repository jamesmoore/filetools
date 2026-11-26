using System.IO.Abstractions;

namespace FileTools.Dupecheck
{
	public record DuplicateFileItem(IFileInfo FileItem, string Hash, string HashMethod);
}
