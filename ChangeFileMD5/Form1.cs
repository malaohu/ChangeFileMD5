using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Security.Cryptography;

namespace ChangeFileMD5
{
    public partial class Form1 : Form
    {

        List<FilesInfo> lstFiles;//文件集合
        Thread tLoadFileMd5;//加载文件md5线程
        Thread tUpdateFileMd5;//修改文件MD5线程
        private static string DirectoryName = "NewFile";
        private static string TempTextName = "temp";
        public Form1()
        {
            lstFiles = new List<FilesInfo>();
            InitializeComponent();
            btnStart.Enabled = false;
        }
        #region 修改文件MD5
        private void button1_Click(object sender, EventArgs e)
        {
            progressBar.Value = 0;
            tUpdateFileMd5 = new Thread(UpdateFilesMD5);
            tUpdateFileMd5.Start();
        }
        #endregion

        #region 载入文件
        private void btnLoadFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
           
            ofd.Multiselect = true;
            ofd.Filter = "视频文件(*.avi,*.mkv,*.rmvb,*.mp4,*.wmv)|*.avi;*.mkv;*.rmvb;*.mp4;*.wmv";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                progressBar.Value = 0;
                btnStart.Enabled = false;
                lblTip.Text = "正在载入文件,请稍后...";
                lstFiles = new List<FilesInfo>();
                string[] files = ofd.FileNames;
                foreach (var item in files)
                {
                    FilesInfo fi = new FilesInfo();
                    var fileName = Path.GetFileName(item);
                    fi.FileName = fileName;
                    fi.FileOldMd5 = "";
                    fi.FileNewMd5 = "";
                    fi.DirectoryName = Path.GetDirectoryName(item);
                    fi.FilePath = item;
                    lstFiles.Add(fi);
                }
                BindDgv();
                btnLoadFiles.Enabled = false;

                tLoadFileMd5 = new Thread(CalcFilesMD5);
                tLoadFileMd5.Start();
            }

        }
        #endregion

        #region 计算文件MD5
        public void CalcFilesMD5()
        {
            if (lstFiles.Count == 0)
            {
                MessageBox.Show("请先载入文件!", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLoadFiles.Enabled = true;
            }
            else
            {
                //获取文件总数量
                int filesCount = lstFiles.Count;
                lblTip.BeginInvoke(new Action(delegate
                {
                    lblTip.Text = "文件载入成功,正在计算文件MD5...";
                }));
                int finishedCount = 0;
                foreach (var item in lstFiles)
                {
                    item.FileOldMd5 = GetFileMd5(item.FilePath);
                    finishedCount++;
                    dgvFilesInfo.BeginInvoke(new Action(delegate()
                    {
                        BindDgv();
                        progressBar.Value = (int)(finishedCount * 100 / filesCount * 100) / 100;
                    }));
                }

                lblTip.BeginInvoke(new Action(delegate
                {
                    lblTip.Text = "所有文件原MD5获取成功";
                    btnStart.Enabled = true;
                    btnLoadFiles.Enabled = true;
                }));
                tLoadFileMd5.Abort();
            }
        }
        #endregion

        #region 获取文件MD5值
        private string GetFileMd5(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] retVal = md5.ComputeHash(fs);
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取文件MD5失败," + ex.Message, "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sb.ToString();
        }
        #endregion

        #region 修改MD5值
        private void UpdateFilesMD5()
        {
            btnLoadFiles.BeginInvoke(new Action(delegate
            {
                btnLoadFiles.Enabled = false;
                btnStart.Enabled = false;
                lblTip.Text = "正在修改文件MD5,请稍后...";
            }));
            int finishedCount = 0;
            foreach (var item in lstFiles)
            {
                if (string.IsNullOrEmpty(item.FileOldMd5))
                {
                    MessageBox.Show("改行文件的原MD5还未计算出!");
                    continue;
                }
                else
                {
                    finishedCount++;
                    var isUpdateSuccess = ExecCmd(item.FilePath, item.DirectoryName, item, lstFiles.Count, finishedCount);
                    if (!isUpdateSuccess)
                    {
                        lblTip.BeginInvoke(new Action(delegate
                        {
                            lblTip.Text = "请确保所有的文件夹中或文件名不含有中文或空格";
                            btnLoadFiles.Enabled = true;
                            btnStart.Enabled = false;
                        }));
                        return;
                    }
                }
            }
            btnStart.BeginInvoke(new Action(delegate
            {
                btnStart.Enabled = false;
                btnLoadFiles.Enabled = true;
                lblTip.Text = "所有文件的MD5修改完成";
            }));
            MessageBox.Show("文件全部修改完成", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }
        #endregion

        #region 执行cmd命令
        private bool ExecCmd(string filePath, string fileDirectory, FilesInfo fi, int filesCount, int finishedCount)
        {
            var process = new Process();//创建一个进程
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";//设定执行cmd命令
            startInfo.UseShellExecute = false;//不使用系统外壳程序启动  
            startInfo.RedirectStandardInput = true;//不重定向输入  
            startInfo.RedirectStandardOutput = true; //重定向输出  
            startInfo.CreateNoWindow = true;//不创建窗口  
            process.StartInfo = startInfo;
            try
            {
                if (process.Start())//开始进程  
                {
                    var fileNewDirectory = Path.Combine(fileDirectory, DirectoryName);
                    var newFileDirectory = Path.Combine(fileNewDirectory, Path.GetFileName(filePath));
                    if (!Directory.Exists(fileNewDirectory))
                        Directory.CreateDirectory(fileNewDirectory);
                    if (File.Exists(Path.Combine(fileNewDirectory, TempTextName + ".txt")))
                        File.Delete(Path.Combine(fileNewDirectory, TempTextName + ".txt"));
                    if (File.Exists(newFileDirectory))
                        File.Delete(newFileDirectory);
                    process.StandardInput.WriteLine("copy nul+nul " + Path.Combine(fileNewDirectory, TempTextName + ".txt"));
                    process.StandardInput.WriteLine("copy /b " + filePath + "+" + fileNewDirectory + "\\" + TempTextName + ".txt " + newFileDirectory);
                    process.StandardInput.WriteLine("exit");
                    process.WaitForExit();//这里无限等待进程结束 
                    var output = process.StandardOutput.ReadToEnd();//读取进程的输出  
                    if (output.IndexOf("命令语法不正确") != -1)
                    {
                        MessageBox.Show("请确保所有的文件夹中或文件名不含有中文或空格!", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Directory.Delete(fileNewDirectory);
                        return false;
                    }
                    else
                    {
                        //MessageBox.Show(Path.Combine(fileNewDirectory, Path.GetFileName(filePath)));
                        fi.FileNewMd5 = GetFileMd5(Path.Combine(fileNewDirectory, Path.GetFileName(filePath)));
                        if (File.Exists(fileNewDirectory + TempTextName + ".txt"))
                            File.Delete(fileNewDirectory + TempTextName + ".txt");
                        dgvFilesInfo.BeginInvoke(new Action(delegate
                        {
                            BindDgv();
                            progressBar.Value = (int)(finishedCount * 100 / filesCount * 100) / 100;
                        }));
                    }
                }
            }
            catch
            {
                MessageBox.Show("修改MD5出错,请重试!");
                process.Close();
                return false;
            }
            finally
            {
                if (process != null)
                    process.Close();
            }
            return true;
        }
        #endregion

        #region 绑定数据
        private void BindDgv()
        {
            dgvFilesInfo.DataSource = null;
            dgvFilesInfo.DataSource = lstFiles;
            dgvFilesInfo.Columns["FileName"].HeaderText = "文件名称";
            dgvFilesInfo.Columns["FileOldMd5"].HeaderText = "文件原MD5值";
            dgvFilesInfo.Columns["FileNewMd5"].HeaderText = "文件新MD5值";
            dgvFilesInfo.Columns["FilePath"].HeaderText = "文件路径";
            dgvFilesInfo.Columns["FilePath"].Visible = false;
            dgvFilesInfo.Columns["DirectoryName"].HeaderText = "文件目录";
        }
        #endregion

    }

    public class FilesInfo
    {
        public string FileName { get; set; }//文件名称
        public string FileOldMd5 { get; set; }//文件原MD5
        public string FileNewMd5 { get; set; }//文件新MD5
        public string FilePath { get; set; }//文件路径
        public string DirectoryName { get; set; }//文件目录名称
    }
}
