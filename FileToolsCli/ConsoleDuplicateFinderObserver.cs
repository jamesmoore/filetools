using FileTools.Dupecheck;

sealed class ConsoleDuplicateFinderObserver : IDuplicateFinderObserver, IDisposable
{
    private readonly bool isErrorRedirected = Console.IsErrorRedirected;
    private int lastPercent = -1;
    private bool hasOpenLine;

    public void SetProgressText(string file) { }

    public void SetProgressPercent(int percent)
    {
        // The finder reports 100% after hashing, before it groups the results.
        if (percent >= 100 || percent <= lastPercent)
            return;

        if (isErrorRedirected && lastPercent >= 0 && percent / 10 <= lastPercent / 10)
            return;

        var message = $"Duplicate scan: {percent,3}%";
        if (isErrorRedirected)
        {
            Console.Error.WriteLine(message);
        }
        else
        {
            Console.Error.Write($"\r{message}");
            hasOpenLine = true;
        }

        lastPercent = percent;
    }

    public void Complete()
    {
        if (!isErrorRedirected)
            Console.Error.Write('\r');

        Console.Error.WriteLine("Duplicate scan: 100%");
        hasOpenLine = false;
    }

    public void Dispose()
    {
        if (hasOpenLine)
            Console.Error.WriteLine();
    }
}
