using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FileTools.StreamHashCalculator
{
    public class SHA256HashCalculator : IHashCalculator
    {
        public async Task<string> GetStreamHash(Stream stream)
        {
			using var hasher = SHA256.Create();
            var bytes = await hasher.ComputeHashAsync(stream);
            return bytes.ToBase64();
		}
    }
}
