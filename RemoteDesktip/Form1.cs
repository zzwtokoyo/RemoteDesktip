using AxMSTSCLib;
using Microsoft.Win32;
using MSTSCLib;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace RemoteDesktip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitEvent();
            InitWebParam(11);
        }

        private void InitWebParam(int ieVersion)
        {
            SetWebBrowserFeatures(ieVersion);
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            axMsTscAxNotSafeForScripting1.OnConnecting += AxMsTscAxNotSafeForScripting1_OnConnecting;
            axMsTscAxNotSafeForScripting1.OnConnected += AxMsTscAxNotSafeForScripting1_OnConnected;
            axMsTscAxNotSafeForScripting1.OnDisconnected += AxMsTscAxNotSafeForScripting1_OnDisconnected;
            axMsTscAxNotSafeForScripting1.OnAutoReconnecting += AxMsTscAxNotSafeForScripting1_OnAutoReconnecting;
        }

        private AutoReconnectContinueState AxMsTscAxNotSafeForScripting1_OnAutoReconnecting(object sender, IMsTscAxEvents_OnAutoReconnectingEvent e)
        {
            try
            {
                string Res = "连接失败:";

                if (0 == e.disconnectReason)
                {
                    label5.Text = Res + "autoReconnectContinueAutomatic";
                    return AutoReconnectContinueState.autoReconnectContinueAutomatic;
                }
                else if (1 == e.disconnectReason)
                {
                    label5.Text = Res + "autoReconnectContinueStop";
                    return AutoReconnectContinueState.autoReconnectContinueStop;
                }
                else
                {
                    label5.Text = Res + "autoReconnectContinueManual";
                    return AutoReconnectContinueState.autoReconnectContinueManual;
                }
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        private void AxMsTscAxNotSafeForScripting1_OnConnected(object sender, EventArgs e)
        {
            try
            {
                label5.Text = "已经连接";
                this.isConnext = true;

                button1.Enabled = true;
                button1.Text = "断开连接";
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        private void AxMsTscAxNotSafeForScripting1_OnConnecting(object sender, EventArgs e)
        {
            try
            {
                label5.Text = "正在连接";
                button1.Enabled = false;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        private void AxMsTscAxNotSafeForScripting1_OnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
        {
            try
            {
                label5.Text = "断开连接";
                this.isConnext = false;

                button1.Enabled = true;
                button1.Text = "连接";
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        private bool isConnext = false;

        /// <summary>
        /// 网络级身份验证
        /// </summary>
        private bool pssNetCred = true;

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!this.groupBox1.Controls.Contains(this.axMsTscAxNotSafeForScripting1))
                //{
                //    this.groupBox1.Controls.Add(this.axMsTscAxNotSafeForScripting1);
                //    this.axMsTscAxNotSafeForScripting1.Dock = DockStyle.Fill;
                //}

                if (button1.Text.Equals("连接"))
                {
                    if (string.IsNullOrEmpty(textBox1.Text))
                    {
                        MessageBox.Show("请输入远程地址");
                        return;
                    }
                    if (string.IsNullOrEmpty(textBox2.Text))
                    {
                        MessageBox.Show("请输入登陆用户");
                        return;
                    }
                    if (string.IsNullOrEmpty(textBox3.Text))
                    {
                        MessageBox.Show("请输入密码");
                        return;
                    }

                    MessageBox.Show("RDP协议版本:" + axMsTscAxNotSafeForScripting1.Version);
                    axMsTscAxNotSafeForScripting1.Server = textBox1.Text;
                    axMsTscAxNotSafeForScripting1.UserName = textBox2.Text;
                    axMsTscAxNotSafeForScripting1.AdvancedSettings9.RDPPort = int.Parse(textBox4.Text);
                    //IMsTscNonScriptable msTscNonScriptable = (IMsTscNonScriptable)axMsTscAxNotSafeForScripting1.GetOcx();
                    //msTscNonScriptable.ClearTextPassword = textBox3.Text;
                    axMsTscAxNotSafeForScripting1.AdvancedSettings9.ClearTextPassword = textBox3.Text;
                    if (this.pssNetCred == true)
                    {
                        axMsTscAxNotSafeForScripting1.AdvancedSettings9.EnableCredSspSupport = true;
                    }
                    else
                    {
                        axMsTscAxNotSafeForScripting1.AdvancedSettings9.EnableCredSspSupport = false;
                    }
                    axMsTscAxNotSafeForScripting1.Connect();

                    Debug.WriteLine(string.Format("开始远程连接,远程地址:{0},用户名:{1},密码:{2},端口:{3}", textBox1, textBox2, textBox3, textBox4));
                }
                else if (button1.Text == "断开连接")
                {
                    axMsTscAxNotSafeForScripting1.Disconnect();
                    //if (axMsTscAxNotSafeForScripting1 != null)
                    //{
                    //    this.groupBox1.Controls.Remove(this.axMsTscAxNotSafeForScripting1);
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("与远程连接操作:{0} 错误，异常信息:{1}", button1.Text, ex.Message));
            }
        }

        private void Checked_Changed(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                int height = axMsTscAxNotSafeForScripting1.DesktopHeight;
                int width = axMsTscAxNotSafeForScripting1.DesktopWidth;
                //选中状态
                //MessageBox.Show(string.Format("选中状态,H:{0},W:{1}", height, width));
                if (button1.Text == "断开连接")
                {
                    axMsTscAxNotSafeForScripting1.FullScreen = true;
                }
            }
            else if (checkBox1.Checked == false)
            {
                //未选中状态
                //MessageBox.Show("取消选中状态");
                if (button1.Text == "断开连接")
                {
                    axMsTscAxNotSafeForScripting1.FullScreen = false;
                }
            }
        }

        private void Resize_Changed(object sender, AxMSTSCLib.IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent e)
        {
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (label5.Text == "已经连接" && this.axMsTscAxNotSafeForScripting1 != null)
                {
                    this.axMsTscAxNotSafeForScripting1.Disconnect();
                }
            }
            catch
            {
                return;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                this.pssNetCred = true;
            }
            else if (checkBox2.Checked == false)
            {
                this.pssNetCred = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strPage = textBox5.Text;
            if(strPage != "")
            {
                this.webBrowser1.ScriptErrorsSuppressed = true;
                this.webBrowser1.Navigate(strPage);
                this.webBrowser1.NewWindow += new CancelEventHandler(webBrowser1_NewWindow);
            }
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            webBrowser1.Url = new Uri(((WebBrowser)sender).StatusText);
            e.Cancel = true;
        }

        /// <summary>
        /// 修改注册表信息来兼容当前程序
        ///
        /// </summary>
        public static void SetWebBrowserFeatures(int ieVersion)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;
            //获取程序及名称
            var appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            //得到浏览器的模式的值
            UInt32 ieMode = GeoEmulationModee(ieVersion);
            string featureControlRegKey = string.Empty;
            if (Environment.Is64BitProcess)
            {
                //64
                featureControlRegKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Internet Explorer\Main\FeatureControl\";
            }
            else
            {
                //32
                featureControlRegKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\";
            }
            //设置浏览器对应用程序（appName）以什么模式（ieMode）运行
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION",
                appName, ieMode, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION",
                appName, 1, RegistryValueKind.DWord);
        }      

        /// <summary>
        /// 通过版本得到浏览器模式的值
        /// </summary>
        /// <param name="browserVersion"></param>
        /// <returns></returns>
        public static UInt32 GeoEmulationModee(int browserVersion)
        {
            UInt32 mode = 11000; // 默认状态IE11
            switch (browserVersion)
            {
                case 7:
                    mode = 7000; // IE7
                    break;

                case 8:
                    mode = 8000; // IE8
                    break;

                case 9:
                    mode = 9000; // IE9
                    break;

                case 10:
                    mode = 10000; // IE10
                    break;

                case 11:
                    mode = 11000; // IE11
                    break;
            }
            return mode;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (label5.Text == "已经连接" && this.axMsTscAxNotSafeForScripting1 != null)
            {
                if (MessageBox.Show("即将断开远程连接，是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    this.Close();
                }
            }
            else if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Close();
            }
            else
            {
                return;
            }
        }
    }
}