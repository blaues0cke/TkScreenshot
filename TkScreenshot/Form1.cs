using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace TkScreenshot
{
    public partial class Form1 : Form
    {
        public Bitmap bmp;
        public String CurrentPath;
        public String version = "0.7";
        public XmlDocument XmlFile = new XmlDocument();
        public String XmlFileName = "config.xml";
        private void AppendScreenshot()
        {
            if (BackgroundImage == null)
            {
                return;
            }

            Bitmap bmp2 = new Bitmap(Width - 16, Height - 36);
            Graphics g = Graphics.FromImage(bmp2);
            HideWindow();
            g.CopyFromScreen(Location.X + 8, Location.Y + 28, 0, 0, new Size(Width - 16, Height - 36));
            ShowWindow();
            g.Dispose();
            Bitmap bmp1 = bmp;


            int newwidth = 0;
            if (bmp1.Width > bmp2.Width)
            {
                newwidth = bmp1.Width;
            }
            else
            {
                newwidth = bmp2.Width;
            }

            bmp = new Bitmap(newwidth, bmp1.Height + bmp2.Height);
            g = Graphics.FromImage(bmp);
            g.DrawImageUnscaled(bmp1, 0, 0);
            g.DrawImageUnscaled(bmp2, 0, bmp1.Height);






            BackgroundImage = bmp;
            XmlNode Temp = XmlFile.SelectSingleNode("TkScreenshot/Options/Screenshots");
            Temp.InnerText = (Convert.ToInt16(XmlFile.SelectSingleNode("TkScreenshot/Options/Screenshots").InnerText) + 1).ToString();
            XmlFile.Save(XmlFileName);


            SetTitle();
        }
        private void CreateScreenshot()
        {

            bmp = new Bitmap(Width - 16, Height - 36);
            Graphics g = Graphics.FromImage(bmp);
            HideWindow();
            g.CopyFromScreen(Location.X + 8, Location.Y + 28, 0, 0, new Size(Width - 16, Height - 36));
            ShowWindow();
            g.Dispose();
            BackgroundImage = bmp;
            XmlNode Temp = XmlFile.SelectSingleNode("TkScreenshot/Options/Screenshots");
            Temp.InnerText = (Convert.ToInt16(XmlFile.SelectSingleNode("TkScreenshot/Options/Screenshots").InnerText) + 1).ToString();
            XmlFile.Save(XmlFileName);

            SetTitle();
        }
        public void SetTitle()
        {
            Text = "TkScreenshot " + version;
            if (BackgroundImage != null)
            {
                Text += " - " + BackgroundImage.Width + "x" + BackgroundImage.Height + "px";
            }



        }
        public void FillForms()
        {
            textBox1.Text = XmlFile.SelectSingleNode("TkScreenshot/Options/FtpUser").InnerText;
            textBox2.Text = XmlFile.SelectSingleNode("TkScreenshot/Options/FtpPassword").InnerText;
            textBox3.Text = XmlFile.SelectSingleNode("TkScreenshot/Options/FtpPath").InnerText;
            textBox4.Text = XmlFile.SelectSingleNode("TkScreenshot/Options/HttpPath").InnerText;
            trackBar1.Value = Convert.ToInt16(XmlFile.SelectSingleNode("TkScreenshot/Options/BgRed").InnerText);
            trackBar2.Value = Convert.ToInt16(XmlFile.SelectSingleNode("TkScreenshot/Options/BgBlue").InnerText);
            trackBar3.Value = Convert.ToInt16(XmlFile.SelectSingleNode("TkScreenshot/Options/BgGreen").InnerText);
            trackBar4.Value = Convert.ToInt16(Convert.ToDouble(XmlFile.SelectSingleNode("TkScreenshot/Options/Opacity").InnerText));
            comboBox1.SelectedIndex = Convert.ToInt16(XmlFile.SelectSingleNode("TkScreenshot/Options/FileFormat").InnerText);
            textBox5.Text = XmlFile.SelectSingleNode("TkScreenshot/Options/FileName").InnerText;
            listView1.Items.Add(new ListViewItem(new string[] { "%dd", "Tag (01-31)" }));
            listView1.Items.Add(new ListViewItem(new string[] { "%dH", "Stunde (00-23)" }));
            listView1.Items.Add(new ListViewItem(new string[] { "%di", "Minute (00-59)" }));
            listView1.Items.Add(new ListViewItem(new string[] { "%dm", "Monat (01-12)"} ));
            listView1.Items.Add(new ListViewItem(new string[] { "%ds", "Sekunde (00-59)" }));
            listView1.Items.Add(new ListViewItem(new string[] { "%dy", "Jahr (00-99)" }));
            listView1.Items.Add(new ListViewItem(new string[] { "%dY", "Jahr (0000-9999)" }));
            listView1.Items.Add(new ListViewItem(new string[] { "%h",  "Screenshot-Höhe"} ));
            listView1.Items.Add(new ListViewItem(new string[] { "%s",  "Screenshot-Nummer)"} ));
            listView1.Items.Add(new ListViewItem(new string[] { "%w",  "Screenshot-Breite"} ));
        }
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }




        public String ParseFileName (string FileName)
        {
            DateTime Date = DateTime.Now;
            FileName = FileName.Replace("%dd", Date.ToString("dd"));
            FileName = FileName.Replace("%dH", Date.ToString("HH"));
            FileName = FileName.Replace("%di", Date.ToString("mm"));
            FileName = FileName.Replace("%dm", Date.ToString("MM"));
            FileName = FileName.Replace("%ds", Date.ToString("ss"));
            FileName = FileName.Replace("%dy", Date.ToString("yy"));
            FileName = FileName.Replace("%dY", Date.ToString("yyyy"));
            FileName = FileName.Replace("%w", this.Size.Width.ToString());
            FileName = FileName.Replace("%h", this.Size.Height.ToString());
            FileName = FileName.Replace("%s", (Convert.ToInt16(XmlFile.SelectSingleNode("TkScreenshot/Options/Screenshots").InnerText) + 1).ToString());
            FileName = FileName + "." + comboBox1.SelectedItem.ToString().ToLower();
            return FileName;
        }







        public void HideWindow()
        {
            this.Opacity = 0;
        }
        public void LoadConfigFile()
        {
            if (!File.Exists(XmlFileName))
            {
                XmlNode Root, Options, History, Temp;
                Root = XmlFile.CreateElement("TkScreenshot");
                Options = XmlFile.CreateElement("Options");
                Temp = XmlFile.CreateElement("FtpUser");
                Temp.InnerText = "";
                Options.AppendChild(Temp);
                Temp = XmlFile.CreateElement("FtpPassword");
                Temp.InnerText = "";
                Options.AppendChild(Temp);
                Temp = XmlFile.CreateElement("FtpPath");
                Temp.InnerText = "";
                Options.AppendChild(Temp);
                Temp = XmlFile.CreateElement("HttpPath");
                Temp.InnerText = "";
                Options.AppendChild(Temp);
                Temp = XmlFile.CreateElement("Opacity");
                Temp.InnerText = "80";
                Options.AppendChild(Temp);
                Temp = XmlFile.CreateElement("BgRed");
                Temp.InnerText = "255";
                Options.AppendChild(Temp);
                Temp = XmlFile.CreateElement("BgGreen");
                Temp.InnerText = "255";
                Options.AppendChild(Temp);
                Temp = XmlFile.CreateElement("BgBlue");
                Temp.InnerText = "255";
                Options.AppendChild(Temp);
                Temp = XmlFile.CreateElement("FileFormat");
                Temp.InnerText = "6";
                Options.AppendChild(Temp);
                Temp = XmlFile.CreateElement("FileName");
                Temp.InnerText = "%dd_%dm_%dY_%s_%w_%h";
                Options.AppendChild(Temp);
                Temp = XmlFile.CreateElement("Screenshots");
                Temp.InnerText = "0";
                Options.AppendChild(Temp);
                Root.AppendChild(Options);
                History = XmlFile.CreateElement("History");
                Root.AppendChild(History);
                XmlFile.AppendChild(Root);
                XmlFile.Save(XmlFileName);
            }
            else
            {
                XmlFile.Load(XmlFileName);
            }
            FillForms();
            SetBackgroundColor();
        }
        public void ShowWindow()
        {
            Opacity = (double)(Convert.ToDouble(trackBar4.Value) / 100);
        }


        private void UploadImage()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(UploadImage));
                return;
            }
            

            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(textBox3.Text + CurrentPath);
            req.Credentials = new NetworkCredential(textBox1.Text, textBox2.Text);
            MemoryStream IS = new MemoryStream();
            bmp.Save(IS, System.Drawing.Imaging.ImageFormat.Png);
            IS.Flush();
            UploadFile(req, IS.ToArray(), -1);
        }



        public void UploadFile(FtpWebRequest req, byte[] localFilePath, Int32 bufferSize)
        {


            button1.Enabled = false;
            int defaultBufferSize = 1024;
            if (bufferSize < 1)
            {
                bufferSize = defaultBufferSize;
            }
            MemoryStream tempStream = new MemoryStream(localFilePath);
            byte[] buffer = new byte[bufferSize];
            Int64 noOfBuffers = tempStream.Length / Convert.ToInt64(bufferSize);//rundet automatisch ab
            Int32 lastBufferSize = Convert.ToInt32(tempStream.Length - noOfBuffers * bufferSize);
            req.Method = WebRequestMethods.Ftp.UploadFile;
            Stream ftpStream = req.GetRequestStream();
            for (int i = 0; i < noOfBuffers; i++)
            {
                tempStream.Read(buffer, 0, buffer.Length);
                ftpStream.Write(buffer, 0, buffer.Length);
                progressBar1.Value = (int)(i / (noOfBuffers - 1) * 100);

            }
            if (lastBufferSize > 0)
            {
                tempStream.Read(buffer, 0, lastBufferSize);
                ftpStream.Write(buffer, 0, lastBufferSize);

            }
            ftpStream.Close();
            tempStream.Close();
            button1.Enabled = true;
            DateTime Date = DateTime.Now;
            XmlNode Temp = XmlFile.SelectSingleNode("TkScreenshot/History");
            XmlNode Temp1 = XmlFile.CreateElement("Url");
            Temp1.InnerText = CurrentPath;
            XmlAttribute Temp2 = XmlFile.CreateAttribute("time");
            Temp2.InnerText = Date.ToString("HH:mm:ss");
            Temp1.Attributes.Append(Temp2);
            listView2.Items.Add(new ListViewItem(new string[] { Temp2.InnerText, Date.ToString("dd.MM.yy"), textBox4.Text + CurrentPath }));
            Temp2 = XmlFile.CreateAttribute("date");
            Temp2.InnerText = Date.ToString("dd.MM.yy");
            Temp1.Attributes.Append(Temp2);
            Temp.AppendChild(Temp1);
            XmlFile.Save(XmlFileName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CurrentPath = ParseFileName(textBox5.Text);

            System.Windows.Forms.Clipboard.SetDataObject(textBox4.Text + CurrentPath, true);


            Thread bla = new Thread(new ThreadStart(UploadImage));
            bla.Start();
        }
        private void panel2_Click(object sender, EventArgs e)
        {
            CreateScreenshot();
        }
        private void panel1_Click(object sender, EventArgs e)
        {
            CreateScreenshot();
        }
        public void ResetButtonPosition()
        {
            button1.Location = new Point(Width - button1.Width - 26, progressBar1.Location.Y - button1.Height - button2.Height - 12);
            button2.Location = new Point(Width - button2.Width - 26, progressBar1.Location.Y - button2.Height - 6);
            button3.Location = new Point(button2.Location.X - 96, button2.Location.Y);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadConfigFile();
            Text += " " + version;
            Height -= panel1.Height - 10;
            SetTitle();
            ShowGui();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == true)
            {
                panel1.Visible = false;
                button3.Visible = false;
                this.MinimumSize = new Size(0, 0);
                Height -= panel1.Height;
                BackgroundImage = bmp;
            }
            else
            {
                panel1.Visible = true;
                Height += panel1.Height;
                button3.Visible = true;
                this.MinimumSize = new Size(500, 500);
                BackgroundImage = null;
            }
        }
        public void SetBackgroundColor()
        {
            BackColor = Color.FromArgb(trackBar1.Value, trackBar3.Value, trackBar2.Value);
        }
        public void SetPreviewBackgroundColor()
        {
            panel2.BackColor = Color.FromArgb(trackBar1.Value, trackBar3.Value, trackBar2.Value);
        }
        private void ColorTrackBarValueChanged(object sender, EventArgs e)
        {
            groupBox3.Text = "Rot (" + trackBar1.Value.ToString() + ")";
            groupBox4.Text = "Grün (" + trackBar3.Value.ToString() + ")";
            groupBox5.Text = "Blau (" + trackBar2.Value.ToString() + ")";
            SetPreviewBackgroundColor();
        }
        private void OpacityTrackBarValueChanged(object sender, EventArgs e)
        {
            groupBox2.Text = "Transparenz (" + trackBar4.Value.ToString() + "%)";
            Opacity = (double)(Convert.ToDouble(trackBar4.Value) / 100);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SaveOptions();
        }
        public void SaveOptions()
        {
            XmlNode Temp;
            Temp = XmlFile.SelectSingleNode("TkScreenshot/Options/FtpUser");
            Temp.InnerText = textBox1.Text;
            Temp = XmlFile.SelectSingleNode("TkScreenshot/Options/FtpPassword");
            Temp.InnerText = textBox2.Text;
            Temp = XmlFile.SelectSingleNode("TkScreenshot/Options/FtpPath");
            Temp.InnerText = textBox3.Text;
            Temp = XmlFile.SelectSingleNode("TkScreenshot/Options/HttpPath");
            Temp.InnerText = textBox4.Text;
            Temp = XmlFile.SelectSingleNode("TkScreenshot/Options/Opacity");
            Temp.InnerText = trackBar4.Value.ToString();
            Temp = XmlFile.SelectSingleNode("TkScreenshot/Options/BgRed");
            Temp.InnerText = trackBar1.Value.ToString();
            Temp = XmlFile.SelectSingleNode("TkScreenshot/Options/BgGreen");
            Temp.InnerText = trackBar3.Value.ToString();
            Temp = XmlFile.SelectSingleNode("TkScreenshot/Options/BgBlue");
            Temp.InnerText = trackBar2.Value.ToString();
            Temp = XmlFile.SelectSingleNode("TkScreenshot/Options/FileName");
            Temp.InnerText = textBox5.Text;
            Temp = XmlFile.SelectSingleNode("TkScreenshot/Options/FileFormat");
            Temp.InnerText = comboBox1.SelectedIndex.ToString();
            XmlFile.Save(XmlFileName);
            SetBackgroundColor();
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox6.Text = ParseFileName(textBox5.Text);
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            textBox6.Text = ParseFileName(textBox5.Text);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox6.Text = ParseFileName(textBox5.Text);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Thread bla = new Thread(new ThreadStart(FillHistory));
            bla.Start();


        }


        public void FillHistory()
        {
            XmlNodeList XmlRef = XmlFile.SelectNodes("/TkScreenshot/History/Url");
            foreach (XmlNode Url in XmlRef)
            {
                listView2.Items.Add(new ListViewItem(new string[] { Url.Attributes["date"].InnerText, Url.Attributes["time"].InnerText, Url.InnerText }));
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                CreateScreenshot();
            }
            else if (e.Button == MouseButtons.Right)
            {
                AppendScreenshot();
            }
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            ShowGui();
        }

        private void ShowGui() {
            label11.Visible = false;
            button2.Visible = true;
            button1.Visible = true;
            progressBar1.Visible = true;
            BackgroundImage = bmp;
            ResetButtonPosition();
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (label11.Visible == false)
            {
                label11.Visible = true;
            }
            if (button1.Visible == true)
            {
                button1.Visible = false;
            }
            if (button2.Visible == true)
            {
                button2.Visible = false;
            }
            if (progressBar1.Visible == true)
            {
                progressBar1.Visible = false;
            }
            BackgroundImage = null;
        }


        
    }
}
