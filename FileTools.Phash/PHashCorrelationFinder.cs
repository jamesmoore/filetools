using NLog;
using Shipwreck.Phash;

namespace FileTools.PHash
{
    public class PHashCorrelationFinder
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public IEnumerable<Match> GetMatches(IEnumerable<(FilePhash, FilePhash)> list)
        {
            return list
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .Select(tuple =>
                {
                    try
                    {
                        var score = ImagePhash.GetCrossCorrelation(tuple.Item1.PHash.FromBase64(), tuple.Item2.PHash.FromBase64());
                        return new Match(tuple.Item1.FileName, tuple.Item2.FileName, score);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"{ex.Message} Error getting match for {tuple.Item1.FileName} vs {tuple.Item2.FileName}");
                        return null;
                    }
                })
                .Where(m => m != null);
        }

        public IEnumerable<(FilePhash, FilePhash)> BuildCombinations(IEnumerable<FilePhash> dict, string leftHandFilter, string rightHandFilter)
        {
            var leftHandList = dict.Where(p => string.IsNullOrWhiteSpace(leftHandFilter) || p.FileName.Contains(leftHandFilter)).ToList();
            var rightHandList = dict.Where(p => string.IsNullOrWhiteSpace(rightHandFilter) || p.FileName.Contains(rightHandFilter)).ToList();

            var leftHandOnly = leftHandList.Except(rightHandList).ToList();
            var both = leftHandList.Intersect(rightHandList).ToList();
            var rightHandOnly = rightHandList.Except(leftHandList).ToList();

            var list = new List<(FilePhash, FilePhash)>();

            foreach (var item in both)
            {
                foreach (var item2 in both.Where(p => p.FileName.CompareTo(item.FileName) == 1))
                {
                    list.Add(new(item, item2));
                }
            }

            foreach (var leftOnlyItem in leftHandOnly)
            {
                foreach (var bothItem in both)
                {
                    list.Add(new(leftOnlyItem, bothItem));
                }
            }

            foreach (var rightOnlyItem in rightHandOnly)
            {
                foreach (var bothItem in both)
                {
                    list.Add(new(rightOnlyItem, bothItem));
                }
            }

            return list;
        }

        public record FilePhash(string FileName, string PHash);
    }
}
