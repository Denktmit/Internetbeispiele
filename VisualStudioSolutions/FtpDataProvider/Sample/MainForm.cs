using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FtpDataProvider;

namespace Sample
{
    public partial class MainForm : Form
    {
        FtpProvider _ftp;
        string _currentDir = string.Empty;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _ftp = new FtpProvider(tbServer.Text, tbLogin.Text, tbPassword.Text);
            _ftp.FtpProgressChanged += new FtpProgressEventHandler(_ftp_FtpProgressChanged);
            ChangeDirectory("");
        }

        private void lvContent_DoubleClick(object sender, EventArgs e)
        {
            if (lvContent.SelectedItems.Count == 1)
            {
                FtpContent item = (FtpContent)(lvContent.SelectedItems[0]).Tag;

                if (item.IsDirectory)
                {
                    ChangeDirectory(item.Path);
                }
                else
                {
                    DownloadFile(item);
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string source = openFileDialog.FileName;

                backgroundWorkerUpload.RunWorkerAsync((object)source);
            }
        }

        private void ChangeDirectory(string folder)
        {
            List<FtpContent> ftpContent = _ftp.GetDirectoryDetailList(folder);
            ftpContent = ftpContent.OrderByDescending(p => p.IsDirectory).ThenBy(p => p.Name).ToList();
            lvContent.Items.Clear();
            _currentDir = folder;

            foreach (FtpContent content in ftpContent)
            {
                ListViewItem item = new ListViewItem(new[] { content.Name, (content.IsDirectory ? "DIR" : "FILE") });
                item.Tag = content;
                lvContent.Items.Add(item);
            }
        }

        private void DownloadFile(FtpContent item)
        {
            saveFileDialog.FileName = item.Name;

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string target = saveFileDialog.FileName;

                backgroundWorkerDownload.RunWorkerAsync(new object[] { item, target });
            }
        }

        private void backgroundWorkerDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            FtpContent item = (FtpContent)((object[])e.Argument)[0];
            string target = (string)((object[])e.Argument)[1];

            if (System.IO.Path.GetFileName(target) == item.Name)
            {
                _ftp.DownloadFile(System.IO.Path.GetDirectoryName(item.Path), item.Name, System.IO.Path.GetDirectoryName(target));
            }
            else
            {
                _ftp.DownloadFile(System.IO.Path.GetDirectoryName(item.Path), item.Name, System.IO.Path.GetDirectoryName(target), System.IO.Path.GetFileName(target));
            }
        }

        private void backgroundWorkerDownload_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tsProgressBar.Value = e.ProgressPercentage;
            tsStatusLabel.Text = (string)e.UserState;
        }

        private void backgroundWorkerDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tsProgressBar.Value = 0;
            tsStatusLabel.Text = string.Empty;
        }

        private void backgroundWorkerUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            _ftp.UploadFile((string)e.Argument, _currentDir);
        }

        private void backgroundWorkerUpload_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tsProgressBar.Value = e.ProgressPercentage;
            tsStatusLabel.Text = (string)e.UserState;
        }

        private void backgroundWorkerUpload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tsProgressBar.Value = 0;
            tsStatusLabel.Text = string.Empty;
            ChangeDirectory(_currentDir);
        }

        void _ftp_FtpProgressChanged(object sender, FtpProgressEventArgs e)
        {
            string statusMsg = e.Remaning.ToString() + " @ " + e.KbPSec.ToString("N").Substring(0, e.KbPSec.ToString("N").Length - 3) + " KB/sec";

            if (backgroundWorkerDownload.IsBusy)
            {
                backgroundWorkerDownload.ReportProgress(Convert.ToInt32(e.Percent), (object)statusMsg);
            }
            else if (backgroundWorkerUpload.IsBusy)
            {
                backgroundWorkerUpload.ReportProgress(Convert.ToInt32(e.Percent), (object)statusMsg);
            }
        }
    }
}
