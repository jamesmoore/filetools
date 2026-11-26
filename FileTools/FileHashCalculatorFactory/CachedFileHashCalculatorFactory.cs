using FileTools.FileHashCalculator;
using FileTools.HashStore;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace FileTools.FileHashCalculatorFactory
{
	class CachedFileHashCalculatorFactory(IFileHashCalculatorFactory inner, IHashStore hashStore) : IFileHashCalculatorFactory
    {
        private readonly Dictionary<IFileHashCalculator, IFileHashCalculator> cache = [];

		public IFileHashCalculator GetFileHashCalculator(IFileInfo info)
        {
            var innerCalculator = inner.GetFileHashCalculator(info);

            if (cache.TryGetValue(innerCalculator, out IFileHashCalculator value))
            {
                return value;
            }
            else
            {
                var item = new CachedFileHashCalculator(innerCalculator, hashStore);
                cache.Add(innerCalculator, item);
                return item;
            }
        }
    }
}
