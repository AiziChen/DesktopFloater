using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// Create by quanyeChen
/// License on Apache License 2.0
/// 2018-8-1
/// </summary>
namespace 灵动小桌
{
    public partial class smartBaby : Form
    {
        private static readonly string DEFAULT_IMG = "empty";
        private static readonly string CURRENT_IMG_PATH = "current_image_path";
        private Point ptMouseCurrrnetPos, ptMouseNewPos, ptFormPos, ptFormNewPos;
        private bool blnMouseDown = false;
        private ResizeForm resizeForm;

        public int picWidth = 0;
        public int picHeight = 0;
        public int cursor = 0;
        public int defaultPicWidth = 0;
        public int defaultPicHeight = 0;

        private bool lose = false;

        public smartBaby()
        {
            InitializeComponent();
            resizeForm = new ResizeForm(this);
        }
        
        /// <summary>
        /// 窗口加载时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        // 加载窗口时获取图片高、宽
        private void smartBaby_Load(object sender, EventArgs e)
        {
            // 初始化配置文件
            if (!File.Exists(Tools.WinProfile.CONFIG_FILE_PATH))
            {
                // 创建文件
                new FileStream(Tools.WinProfile.CONFIG_FILE_PATH, FileMode.OpenOrCreate);
            }

            String imagePath = Tools.WinProfile.ProfileReadValue("Settings", CURRENT_IMG_PATH, Tools.WinProfile.CONFIG_FILE_PATH);
            if (!(imagePath.Equals(DEFAULT_IMG) || imagePath.Equals("")))
            {
                try
                {
                    SetImage(imagePath);
                    lose = false;
                } catch (Exception)
                {
                    ResetImage();
                    MessageBox.Show("设置的图片可能丢失了");
                    lose = true;
                }
            }

            // 读取启动配置文件中的高度和宽度，并设置
            string width = Tools.WinProfile.ProfileReadValue("WidthAndHeight", "Width", Tools.WinProfile.CONFIG_FILE_PATH);
            string height = Tools.WinProfile.ProfileReadValue("WidthAndHeight", "Height", Tools.WinProfile.CONFIG_FILE_PATH);
            if (!width.Equals("") && !height.Equals("") && !lose)
            {
                picWidth = Int32.Parse(width);
                picHeight = Int32.Parse(height);

                pictureBox1.Width = picWidth;
                pictureBox1.Height = picHeight;
                
                String cursor = Tools.WinProfile.ProfileReadValue("WidthAndHeight", "Cursor", Tools.WinProfile.CONFIG_FILE_PATH);
                this.cursor = Int32.Parse(cursor);
            }
            else
            {
                picHeight = pictureBox1.Height;
                picWidth = pictureBox1.Width;
                int maxValue = (picWidth > picHeight) ? picWidth : picHeight;
                this.cursor = maxValue / 2;
            }
            
            this.Width = picWidth;
            this.Height = picHeight;
        }

        /// <summary>
        /// 窗口关闭时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void smartBaby_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 当该窗口关闭后，保存当前的高、宽度到启动配置文件
            Tools.WinProfile.ProfileWriteValue("WidthAndHeight", "Width", picWidth.ToString(), Tools.WinProfile.CONFIG_FILE_PATH);
            Tools.WinProfile.ProfileWriteValue("WidthAndHeight", "Height", picHeight.ToString(), Tools.WinProfile.CONFIG_FILE_PATH);
            Tools.WinProfile.ProfileWriteValue("WidthAndHeight", "Cursor", cursor.ToString(), Tools.WinProfile.CONFIG_FILE_PATH);
        }

        private void 换图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "普通图/动态图|*.gif;*.jpg;*.png;*.jpeg";
            try
            {
                DialogResult result = openFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    String fileName = openFileDialog.FileName;
                    SetImage(fileName);
                }
            }
            catch (Exception)
            {
                ResetImage();
                MessageBox.Show("抱歉，未能更换图片。");
            }
        
        }

        private void 大小变更ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resizeForm.ShowDialog();
        }
        
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            // 完全退出程序
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// 实现窗口移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FloatForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (blnMouseDown)
            {
                ptMouseNewPos = Control.MousePosition;
                ptFormNewPos.X = ptMouseNewPos.X - ptMouseCurrrnetPos.X + ptFormPos.X;
                ptFormNewPos.Y = ptMouseNewPos.Y - ptMouseCurrrnetPos.Y + ptFormPos.Y;
                Location = ptFormNewPos;
                ptFormPos = ptFormNewPos;
                ptMouseCurrrnetPos = ptMouseNewPos;
            }
        }

        /// <summary>
        /// 鼠标左键和右键按下的动作监听方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FloatForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                blnMouseDown = true;
                ptMouseCurrrnetPos = Control.MousePosition;
                ptFormPos = Location;
            }

            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this, new Point(e.X, e.Y));
            }
        }

        /// <summary>
        /// 鼠标按下时的监听方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FloatForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                blnMouseDown = false;
            }
        }
        
        // 重置成默认的图片
        private void ResetImage()
        {
            Bitmap bitmap = Properties.Resources.littlemen;
            
            pictureBox1.Image = bitmap;
            int width = bitmap.Width;
            int height = bitmap.Height;

            pictureBox1.Image = bitmap;
            pictureBox1.Width = width;
            pictureBox1.Height = height;
            this.picWidth = this.defaultPicWidth = width;
            this.picHeight = this.defaultPicHeight = height;
            this.Width = width;
            this.Height = height;
            int maxValue = (width > height) ? width : height;
            // 保存当前图片路径到配置文件
            Tools.WinProfile.ProfileWriteValue("Settings", CURRENT_IMG_PATH, DEFAULT_IMG, Tools.WinProfile.CONFIG_FILE_PATH);
            cursor = maxValue / 2;
        }

        private void SetImage(String filePath)
        {
            Image image = Image.FromFile(filePath);
            int width = image.Width;
            int height = image.Height;

            pictureBox1.Image = image;
            pictureBox1.Width = width;
            pictureBox1.Height = height;
            this.picWidth = this.defaultPicWidth = width;
            this.picHeight = this.defaultPicHeight = height;
            this.Width = width;
            this.Height = height;
            int maxValue = (width > height) ? width : height;
            // 保存当前图片路径到配置文件
            Tools.WinProfile.ProfileWriteValue("Settings", CURRENT_IMG_PATH, filePath, Tools.WinProfile.CONFIG_FILE_PATH);
            cursor = maxValue / 2;
        }

    }
}
