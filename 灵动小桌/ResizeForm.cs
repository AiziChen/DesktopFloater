using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public partial class ResizeForm : Form
    {
        public smartBaby mainWindow;
        private static int picWidth = 0;
        private static int picHeight = 0;

        public ResizeForm()
        {
            InitializeComponent();
        }

        public ResizeForm(smartBaby mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void ResizeForm_Load(object sender, EventArgs e)
        {
            picWidth = mainWindow.defaultPicWidth;
            picHeight = mainWindow.defaultPicHeight;
            int max = (picWidth > picHeight) ? picWidth : picHeight;
            trackBar1.Maximum = max;
            trackBar1.Value = mainWindow.cursor;
        }

        // 滚动条滚动时触发的事件
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int value = trackBar1.Value - trackBar1.Maximum / 2;

            mainWindow.cursor = trackBar1.Value;
            mainWindow.picWidth = picWidth + value;
            mainWindow.picHeight = picHeight + value;
            mainWindow.Width = picWidth + value;
            mainWindow.Height = picHeight + value;
            mainWindow.pictureBox1.SetBounds(0, 0, picWidth + value, picHeight + value);
        }

        private void ResizeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
