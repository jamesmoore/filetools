namespace FileTools.Dupecheck
{
    class DuplicateFileFinderObserver(frmDuplicateFileFinder form) : IDuplicateFinderObserver
    {
		public void SetProgressText(string file)
        {
            form.ProgressMessage = file;
            //form.SetProgressMessage(file);
        }

        public void SetProgressPercent(int i)
        {
            form.ProgressPercent = i;
        }
    }
}
