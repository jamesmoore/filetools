namespace FileTools.Dupecheck
{
    public interface IDuplicateFinderObserver
    {
        void SetProgressText(string file);
        void SetProgressPercent(int i);
    }
}
