using System;
using System.Windows.Forms;
using System.Drawing;
using FileToolsUI.SFV;

namespace FileTools
{
    public class SFVListViewItem : ListViewItem
    {
        public SFVFileInfo sfvfile;
        //public ListViewItem lvi;

        public SFVListViewItem(SFVFileInfo sfv)
        {
            sfv.SFVFileScannedEvent += SetStatus;
            this.sfvfile = sfv;           
            this.Text = sfv.relativename;
            this.SubItems.Add("Status");

            try
            {
                this.SubItems.Add(sfv.file.Length.ToString());
            }
            catch (System.IO.FileNotFoundException)
            {
                this.SubItems.Add("");
            }
            this.SubItems.Add(sfv.CRCInSFVFile);
            this.UseItemStyleForSubItems = false;
        }



        private void SetStatus(SFVStatus status)
        {
            string s="UNKNOWN";
            Color c = Color.Black;
            switch (status)
            {
                case SFVStatus.OK:
                    s = "OK";
                    c = Color.Black;
                    break;
                case SFVStatus.Bad:
                    s = "BAD";
                    c = Color.Red;
                    break;
                case SFVStatus.Missing:
                    s = "MISSING";
                    c = Color.Red;
                    break;
                case SFVStatus.IOException:
                    s = "I/O Exception";
                    c = Color.Red;
                    break;
            }

            this.ListView.Invoke(new SetStatusCallback(SetStatus), new Object[] { s, c });
            //SetStatus(s, c);
        }

        delegate void SetStatusCallback(string text, Color c);

        public void SetStatus(string s,Color c)
        {
            this.SubItems[1].Text = s;
            this.ForeColor = c;
        }

    }
}
