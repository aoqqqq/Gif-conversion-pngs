using System;
using System.Windows.Forms;
using System.Threading;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Security.AccessControl;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
//bilibili W傲奇W
namespace Tr
{
    public partial class Form1 : Form
    {
        //2023年6月29日16:14:45
        public static MyProjectEvent mevent = MyProjectEvent.NULL;
        Size a_mapsize;
        Size all_Mapsize;
        //bool dj = false;
        AutoResetEvent reset = new AutoResetEvent(false);
        AutoResetEvent reset2 = new AutoResetEvent(false);

        System.Threading.Thread thread_gif;
        System.Threading.Thread thread_gifs;
        delegate void delmap(string gifs, byte value, bool isTransparen);
        delegate void delmap2(string NULL, string file, byte value, bool isTransparen);
        private delmap GetDelmap;
        private delmap2 GetDelmap2;
        static RichTextBox log;
        static CheckBox transparentCH;
        static byte threshold = 0;
        RectangleMap map;
        static TextBox spacepixel;
        public static int Getspacepixel()
        {
            int a = 2;
            spacepixel.Invoke(new Action(() =>
            {
                a = int.Parse(spacepixel.Text);
            }));

            return a;
        }
        public static bool istransparent(out byte value)
        {
            bool a = false;
            value = threshold;
            transparentCH.Invoke(new Action(() =>
            {

                a = transparentCH.Checked;
            }));
            return a;
        }
        public static  void cwlog(string text)
        {
             log.Invoke(new Action( () =>
            {
                 log.AppendText("\n" + text);
                 log.ScrollToCaret();

            }));  

        }
        public static void cwlog(string text, IntPtr? Form1s)
        {
            if (Form1s == null)
            {
                log.AppendText("\n" + text);
                log.ScrollToCaret();
            }

        }

        public static string GetText(Control text, IntPtr? Form1s)
        {
            return text.Text;
        }
        public static string GetText(Control text)
        {
            string ret = null;
            text.Invoke((MethodInvoker)delegate
            {
                ret = text.Text;
            });
            return ret;
        }
        //2023年6月29日16:14:56

        private void Sortevent()
        {
            try
            {
                cwlog(Getinputtext(null));
                map.are.Set();
                mevent = MyProjectEvent.NULL;
                buttoninputYes.Enabled = false;
                //2023年6月30日15:51:53

            }
            catch (Exception)
            {
                buttoninputYes.Enabled = true;
                MessageBox.Show("输入非法");
            }
        }
        private void SortMode()
        {
            try
            {
                int tmp = int.Parse(Getinputtext());
                cwlog(Getinputtext());
                map.sortModes = tmp == 1 ? MySortModes.Int : tmp == 2 ? MySortModes.String : MySortModes.NULL;
                mevent = MyProjectEvent.NULL;
                this.buttoninputYes.Enabled = false;
                map.are.Set();

            }
            catch (Exception)
            {
                cwlog("输入序号\n1:整形  2:字符串", null);
                SetinputNULL(null);
                this.buttoninputYes.Enabled = true;
                MessageBox.Show("非法输入");    // 2023年7月1日23:42:41 英语ge发橘的音
            }
        }
        private void SorteventIndex()
        {
            try
            {
                int tmp = int.Parse(Getinputtext(null));
                map.index = tmp;
                cwlog(Getinputtext(null));
                map.are.Set();
                mevent = MyProjectEvent.NULL;
                buttoninputYes.Enabled = false;
            }
            catch (Exception)
            {
                buttoninputYes.Enabled = true;
                SetinputNULL(null);
                MessageBox.Show("请输入整形索引1~N");
            }
        }
        private void ProcessConsoleEvent()
        {
            switch (mevent)
            {
                case MyProjectEvent.NULL:
                    break;
                case MyProjectEvent.SortIndex:
                    SorteventIndex();
                    break;
                case MyProjectEvent.Sort:
                    Sortevent();
                    break;
                case MyProjectEvent.SortMode:
                    SortMode();
                    break;
                default:
                    break;
            }

        }

        public string Getinputtext(IntPtr? Form1s)
        {
            return textinput.Text;
        }
        public string Getinputtext()
        {
            string ret = "";
            textinput.Invoke(new Action(() =>
            {

                ret = textinput.Text;

            }));
            return ret;
        }
        public void SetinputNULL(IntPtr? p)
        {
            textinput.Text = null;
        }
        public void SetinputNULL()
        {

            textinput.Invoke(new Action(() =>
            {

                textinput.Text = null;

            }));
        }
        void intADmap()
        {
            try
            {
                pictureBox1.Image = Image.FromFile(@"./W");
                pictureBox2.Image = Image.FromFile(@"./Z");
            }
            catch (Exception)
            {

            }

        }
        public Form1()
        {
            InitializeComponent();
            intADmap();
            label2.Text = trackBar1.Value.ToString();
            GetDelmap = topng;
            GetDelmap2 = topng;
            thread_gif = new System.Threading.Thread(giftopng);
            thread_gif.Start();
            thread_gifs = new System.Threading.Thread(filestopng);
            thread_gifs.Start();
            PngsforAllPng.Run(textPngsdir, label2, checkBox2, checkBox4);
            //2023年6月28日14:38:11
            textwrap.ReadOnly = !checkwarp.Checked;
            log = rich_log;
            cwlog("hole", null);
            buttoninputYes.Enabled = false;

        }
        void topng(string gifs, byte value, bool isTransparen)
        {
            try
            {
                char[] gotos = { '.', '\\' };
                string[] names = Path.GetFileName(gifs).Split(gotos);
                string name = names[names.Length - 2];

                DirectoryInfo directory = Directory.CreateDirectory($".\\{ImageNameSort.Folder_ImagesData}\\{ImageNameSort.Folder_OrdinaryImages}\\" + name + "\\");
                Bitmap bitmap = new Bitmap(gifs);
                Image image = bitmap;
                FrameDimension frame = new FrameDimension(image.FrameDimensionsList[0]);
                int index = image.GetFrameCount(frame);

                for (int i = 0; i < index; i++)
                {
                    image.SelectActiveFrame(frame, i);


                    if (isTransparen)
                    {
                        Bitmap b = BlackToTransparent.Run(new Bitmap(image), value);

                        directory = Directory.CreateDirectory($".\\{ImageNameSort.Folder_ImagesData}\\{ImageNameSort.Folder_NoBlackImages}\\" + name + "\\");
                        Form1.cwlog("文件夹路径: " + directory.FullName);
                        b.Save(directory.FullName + $"\\{i}.png", ImageFormat.Png);
                        b.Dispose();
                    }
                    else
                    {
                        Form1.cwlog("文件夹路径: " + directory.FullName);
                        image.Save(directory.FullName + $"\\{i}.png", ImageFormat.Png);

                    }
                }
                image.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("路径错误 这个是单个文件");

            }


        }
        void topng(string NULL, string all, byte value, bool isTransparen)
        {
            if (null == NULL)
            {
                try
                {
                    string[] files = all_stream(all);
                    char[] gotos = { '.', '\\' };
                    foreach (var item in files)
                    {

                        string[] filenames = item.Split(gotos);
                        if (filenames[filenames.Length - 1] == "gif")
                        {


                            string name = filenames[filenames.Length - 2];
                            DirectoryInfo dir = Directory.CreateDirectory($".\\{ImageNameSort.Folder_ImagesData}\\{ImageNameSort.Folder_OrdinaryImages}\\" + $"{name}");

                            Image image = Image.FromFile(item);

                            ////获取基础大小
                            //a_mapsize = image.Size;

                            FrameDimension frame = new FrameDimension(image.FrameDimensionsList[0]);
                            int index = image.GetFrameCount(frame);
                            //设置图片大小
                            all_Mapsize = new Size(a_mapsize.Width * index + (2 * (index + 1)), a_mapsize.Height + 2 * 2);
                            //Bitmap allmap = new Bitmap(all_Mapsize.Width, all_Mapsize.Height);
                            ////初始化画板
                            //Graphics g = Graphics.FromImage(allmap);
                            //int pointW = 0;
                            for (int i = 0; i < index; i++)
                            {
                                image.SelectActiveFrame(frame, i);
                                //g.DrawImage(image,pointW,1,image.Width,image.Height);

                                //pointW += (1+a_mapsize.Width);
                                if (isTransparen)
                                {
                                    dir = Directory.CreateDirectory($".\\{ImageNameSort.Folder_ImagesData}\\{ImageNameSort.Folder_NoBlackImages}\\" + name + "\\");
                                    Bitmap b = BlackToTransparent.Run(new Bitmap(image), value);
                                    Form1.cwlog("文件夹路径: " + dir.FullName);
                                    b.Save(dir.FullName + $"\\{ i}.png", ImageFormat.Png);
                                    b.Dispose();
                                }
                                else
                                {
                                    Form1.cwlog("文件夹路径: " + dir.FullName);
                                    image.Save(dir.FullName + $"\\{i}.png", ImageFormat.Png);

                                }
                            }
                            //allmap.Save(dir.FullName + "\\" +"All_"+name+".png",ImageFormat.Png);
                            image.Dispose();
                        }

                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("路径错误");
                }


            }

        }
        string[] all_stream(string directory)
        {
            return Directory.GetFiles(directory);
        }

        void giftopng()
        {
            reset.WaitOne();
            while (true)
            {
                Console.WriteLine("开始");
                string path = null;
                byte value = 0;
                bool isTransparen = false;

                this.Invoke((MethodInvoker)delegate
                {
                    path = textBox1.Text;
                    value = byte.Parse(label2.Text);
                    isTransparen = checkBox3.Checked;
                });
                //  topng(path,value,isTransparen);
                this.Invoke(GetDelmap, path, value, isTransparen);
                MessageBox.Show("完成");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                reset.WaitOne();
            }

        }
        void filestopng()
        {
            reset2.WaitOne();
            while (true)
            {
                Console.WriteLine("开始");
                string path = null;
                byte value = 0;
                bool isTransparen = false;

                this.Invoke((MethodInvoker)delegate
                {
                    path = textBox2.Text;
                    value = byte.Parse(label2.Text);
                    isTransparen = checkBox1.Checked;
                });
                //topng(null,path,value,isTransparen);
                Invoke(GetDelmap2, null, path, value, isTransparen);
                MessageBox.Show("完成");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                reset2.WaitOne();
            }
        }

        ///// <summary>
        ///// 线程随时挂起
        ///// </summary>
        //void Th()
        //{
        //    reset.WaitOne();
        //    while (true)
        //    {
        //        if (this.dj)
        //        {

        //            Console.WriteLine("开始");
        //        }
        //        else
        //        {
        //            Console.WriteLine("关闭");
        //            reset.WaitOne();
        //        }
        //    }


        //}

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            System.Environment.Exit(0);
            base.OnClosing(e);
        }


        private void Gb_Click(object sender, EventArgs e)
        {
            //dj = false;
            System.Diagnostics.Process.Start("https://space.bilibili.com/401669606?spm_id_from=333.999.0.0");
        }

        private void Qi_Click(object sender, EventArgs e)
        {
            rich_log.Text = string.Empty;
            reset.Set();

        }

        private void Gif_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void All_gif_Click(object sender, EventArgs e)
        {
            rich_log.Text = string.Empty;
            reset2.Set();
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }


        private void allMAPP_Click(object sender, EventArgs e)
        {
            rich_log.Text = string.Empty;
            spacepixel = this.textBoxJianju;
            transparentCH = checkBox2;
            threshold = byte.Parse(label2.Text);
            if (this.checkwarp.Checked && GetText(this.textwrap, null) != null)
            {

                try
                {
                    if (int.Parse(GetText(this.textwrap, null)) >= 1 && int.Parse(textBoxJianju.Text) >= 0)
                    {
                        map = new RectangleMap(this);
                        map.isSort = paixv.Checked;
                        map.rowElement = int.Parse(GetText(this.textwrap, null));
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        map.RestartYesButton(MyProjectEvent.NULL, null);
                        map.are.Set();
                    }
                    else
                    {
                        throw new Exception("");
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("输入数字啊!不能为空\n不能有其他非正整数的数\n每行数:大于等于1\n上下间距:大于等于0");

                }



            }
            else
                PngsforAllPng.are.Set();//去黑头
        }

        private void Gif_AllS_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = trackBar1.Value.ToString();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void checkwarp_CheckedChanged(object sender, EventArgs e)
        {
            textwrap.ReadOnly = !checkwarp.Checked;
            this.textwrap.Text = null;
            if (textwrap.ReadOnly)
            {
                this.textwrap.Text = "9999";
            }
        }


        private void buttoninputYes_Click(object sender, EventArgs e)
        {
            ProcessConsoleEvent();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void paixv_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click_1(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_Folder1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "选择文件夹内有gif格式的文件夹";
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
            // folderBrowserDialog.ShowNewFolderButton = false; 创建文件夹
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) textBox2.Text = folderBrowserDialog.SelectedPath;
        }

        private void button_Folder2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "选择文件夹内有gif/png/jpg格式的文件夹";
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) textPngsdir.Text = folderBrowserDialog.SelectedPath;

        }

        private void button_file_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "动态图|*.gif";
            openFileDialog.Title = "选择gif";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName;
            }

        }
    }
}
