using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Shortuts
{
    public partial class FrontWindow : Form
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern System.IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern int SetWindowText(IntPtr hWnd, String text);
        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);
        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
             IntPtr hWnd,                 //要定义热键的窗口的句柄  
             int id,                      //定义热键ID（不能与其它ID重复）            
             KeyModifiers fsModifiers,    //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效  
             Keys vk                      //定义热键的内容               
            );
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd,                 //要取消热键的窗口的句柄  
            int id                       //要取消热键的ID  
            );
        //定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）  
        [Flags()]
        public enum KeyModifiers
        {
            KeyNone = 0,
            KeyAlt = 1,
            KeyCtrl = 2,
            KeyShift = 4,
            KeyWindows = 8
        }
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        public FrontWindow()
        {
            /* 查询是否打开过本程序 */
            IntPtr findWindow = FindWindow(null, "TopWindow");

            /* 如果打开过本窗口，则退出 */
            if (findWindow != IntPtr.Zero)
            {

                MessageBox.Show("This program has been opened!");
                this.Close();
            }
            InitializeComponent();

            /* 设定窗口状态 */
            SetWindowPos(Handle, -1, 0, 0, 0, 0, 1 | 2);

            /* 注册快捷键 */
            RegisterHotKey(Handle, 254, KeyModifiers.KeyWindows, Keys.F2);
        }
        /**
      *  hwnd 在z序中的位于被置位的窗口前的窗口句柄。该参数必须为一个窗口句柄
      *  hWn dlnsertAfter 用于标识在z-顺序的此 CWnd 对象之前的 CWnd 对象。如果uFlags参数中设置了SWP_NOZORDER标记则本参数将被忽略。可为下列值之一：
             HWND_BOTTOM：值为1，将窗口置于Z序的底部。如果参数hWnd标识了一个顶层窗口，则窗口失去顶级位置，并且被置在其他窗口的底部。
             HWND_NOTOPMOST：值为-2，将窗口置于所有非顶层窗口之上（即在所有顶层窗口之后）。如果窗口已经是非顶层窗口则该标志不起作用。
             HWND_TOP：值为0，将窗口置于Z序的顶部。
             HWND_TOPMOST：值为-1，将窗口置于所有非顶层窗口之上。即使窗口未被激活窗口也将保持顶级位置。
      *  x   以客户坐标指定窗口新位置的左边界。
      *  Y   以客户坐标指定窗口新位置的顶边界。
      *  cx  以像素指定窗口的新的宽度。
      *  cy  以像素指定窗口的新的高度。
      *  uFlags 窗口尺寸和定位的标志。该参数可以是下列值的组合：
             SWP_ASYNCWINDOWPOS： 如果调用进程不拥有窗口，系统会向拥有窗口的线程发出需求。这就防止调用线程在其他线程处理需求的时候发生死锁。
             SWP_DEFERERASE：     防止产生WM_SYNCPAINT消息。
             SWP_DRAWFRAME：      在窗口周围画一个边框（定义在窗口类描述中）。
         
             SWP_NOSIZE：        0x0001 维持当前尺寸（忽略cx和Cy参数）。
             SWP_NOMOVE：        0x0002 维持当前位置（忽略X和Y参数）。
             SWP_NOZORDER：      0x0004 维持当前Z序（忽略hWndlnsertAfter参数）。
             SWP_NOREDRAW:       0x0008 不重画改变的内容。如果设置了这个标志，则不发生任何重画动作。适用于客户区和非客户区（包括标题栏和滚动条）和任何由于窗回移动而露出的父窗口的所有部分。如果设置了这个标志，应用程序必须明确地使窗口无效并区重画窗口的任何部分和父窗口需要重画的部分。

             SWP_NOACTIVATE：    0x0010 不激活窗口。如果未设置标志，则窗口被激活，并被设置到其他最高级窗口或非最高级组的顶部（根据参数hWndlnsertAfter设置）。    
             SWP_FRAMECHANGED：  0x0020 给窗口发送WM_NCCALCSIZE消息，即使窗口尺寸没有改变也会发送该消息。如果未指定这个标志，只有在改变了窗口尺寸时才发送WM_NCCALCSIZE。
             SWP_SHOWWINDOW：    0x0040 显示窗口。
             SWP_HIDEWINDOW;     0x0080 隐藏窗口。
         
             SWP_NOCOPYBITS：    0x0100 清除客户区的所有内容。如果未设置该标志，客户区的有效内容被保存并且在窗口尺寸更新和重定位后拷贝回客户区。
             SWP_NOOWNERZORDER： 0x0200 不改变z序中的所有者窗口的位置。
             SWP_NOREPOSITION：  0x0200（SWP_NOOWNERZORDER） 与SWP_NOOWNERZORDER标志相同。
             SWP_NOSENDCHANGING：0x0400 防止窗口接收WM_WINDOWPOSCHANGING消息。
      */
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;

            switch (m.Msg)
            {
                /* 0x0312 是windows消息WM_HOTKEY, 表示用户按下了热键,可用于定义热键 */
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 254:
                            StringBuilder Name = new StringBuilder(256);
                            /* 获取当前的前置窗口 */
                            IntPtr hwnd = GetForegroundWindow();
                            if (hwnd == null) return;

                            /* 获取当前的前置窗口名 */
                            GetWindowText(hwnd, Name, 256);

                            if (Name.ToString().Substring(Name.Length - "↑".Length, "↑".Length) == "↑")
                            {
                                /* 设置 窗口置顶 */
                                SetWindowPos(hwnd, -2, 0, 0, 0, 0, 1 | 2);
                                SetWindowText(hwnd, Name.ToString().Substring(0, Name.Length - "↑".Length));
                                label3.Text = Name.ToString().Substring(0, Name.Length - "↑".Length);
                            }
                            else
                            {
                                /* 取消设置 窗口置顶 */
                                SetWindowPos(hwnd, -1, 0, 0, 0, 0, 1 | 2);
                                SetWindowText(hwnd, Name.ToString() + "↑");
                                label3.Text = Name.ToString() + "↑";
                            }

                            break;
                    }
                    break;
            }

            base.WndProc(ref m);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x201)
            {
                StringBuilder Name = new StringBuilder(256);
                IntPtr hwnd = GetForegroundWindow();

                if (hwnd == null) return false;

                GetWindowText(hwnd, Name, 256);
                label3.Text = Name.ToString();
            }
            return false;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /* 双击时显示本窗口 */
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /* 退出本窗口 */
            UnregisterHotKey(Handle, 254);
            this.notifyIcon1.Visible = false;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* 隐藏本窗口 */
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
