using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Tr
{
    partial class RectangleMap
    {
        internal AutoResetEvent are = new AutoResetEvent(false);
        internal int index = 0;
        internal long rowElement = 999999; 
        internal string chars = null;
        internal MySortModes sortModes = MySortModes.NULL;
        internal void RestartYesButton(MyProjectEvent eve)
        {
            if (eve != MyProjectEvent.NULL)
            {
                form1.SetinputNULL();//输入框清零
                Form1.mevent = eve;//状态
                form1.buttoninputYes.Invoke((MethodInvoker)delegate
                {
                    form1.buttoninputYes.Enabled = true;//可以点击按钮
                });
            }
            else
                throw new Exception("重启失败");
        }
        internal void RestartYesButton(MyProjectEvent eve ,IntPtr? NULL)
        {
                form1.SetinputNULL();//输入框清零
                Form1.mevent = eve;//状态
                form1.buttoninputYes.Invoke((MethodInvoker)delegate
                {
                    form1.buttoninputYes.Enabled = true;//可以点击按钮
                });
        }
        private List<ImageName> selectmde(List<ImageName> names)
        {
            switch (sortModes)
            {

                case MySortModes.NULL:
                    throw new Exception("不能为空");

                case MySortModes.Int:
                    return ImageNameSort.IntSort(names);

                case MySortModes.String:
                    return ImageNameSort.StrSort(names);
            }
            throw new Exception("不能为空");

        }
        private char[] GetChars()
        {
            char[] c = new char[this.chars.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = this.chars[i];
            }
            return c;
        }
        private Form1 form1 { get; }
        public RectangleMap(Form1 form)
        {
            this.form1 = form;
            Thread thread = new Thread(wait);
            thread.Start();
        }
        private string ReturnString(string[] s)
        {
            string ret = "\n";
            foreach (string item in s)
            {
                ret += item + " ";
            }
            return ret;
        }


        internal void wait()
        {
            are.WaitOne();
            while (true)
            {
                string paths = null;
                form1.textPngsdir.Invoke(new Action(() =>
                {
                    paths = form1.textPngsdir.Text;

                }));
                draw(paths);
                Form1.cwlog("完成");
                MessageBox.Show("完成");
                are.WaitOne();
            }

        }

        internal void draw(string path)
        {
            chars = null;
            string[] paths = Directory.GetFiles(path);
            List<ImageName> images = new List<ImageName>(126);
            //string str = paths[0].Split(new char[] { '\\', '.' }, StringSplitOptions.RemoveEmptyEntries)[paths[0].Split(new char[] { '\\', '.' }, StringSplitOptions.RemoveEmptyEntries).Length - 2];
            int v = 0;
            foreach (string ph in paths)
            {
                string[] sp = ph.Split('\\', '.');
                FilesName filesName = new FilesName(ph);
                string suffix = filesName.suffixName; 
                string name = filesName.name;
                if (suffix == "png" || suffix == "jpg")
                {

                    string[] name_tmp = null;
                    images.Add(new ImageName
                    {
                        image = Image.FromFile(ph),
                        name = name
                    });
                    ImageName writetmp = images[images.Count - 1];
                    if (v == 0)
                    {
                        ++v;
                        Form1.cwlog("第一个文件名:" + images[0].name);
                        Form1.cwlog("请输入筛选字符");
                        this.form1.buttoninputYes.Invoke((MethodInvoker)delegate
                        {
                            this.form1.buttoninputYes.Enabled = true;
                            Form1.mevent = MyProjectEvent.Sort;
                        });
                        this.are.WaitOne();

                        chars += form1.Getinputtext();
                        while (true)
                        {
                            form1.SetinputNULL();
                            name_tmp = name.Split(GetChars(), StringSplitOptions.RemoveEmptyEntries);
                            Form1.cwlog("当前划分:" + ReturnString(name_tmp));
                            Form1.cwlog("结束? @y/ 继续筛选");
                            this.form1.buttoninputYes.Invoke((MethodInvoker)delegate
                            {
                                this.form1.buttoninputYes.Enabled = true;
                                Form1.mevent = MyProjectEvent.Sort;
                            });
                            this.are.WaitOne();
                            if (form1.Getinputtext() == "@y" || form1.Getinputtext() == "@Y")
                            {
                                break;
                            }
                            else { chars += form1.Getinputtext(); }
                        }

                        while (true)
                        {
                            Form1.cwlog("当前划分:" + ReturnString(name_tmp));
                            Form1.cwlog("使用1~N 选择排序的块");
                            Form1.mevent = MyProjectEvent.SortIndex;
                            form1.SetinputNULL();
                            this.form1.buttoninputYes.Invoke((MethodInvoker)delegate
                            {
                                form1.buttoninputYes.Enabled = true;
                            });
                            this.are.WaitOne();
                            int ind = int.Parse(form1.Getinputtext());
                            if (this.index > 0 && index <= name_tmp.Length)
                            {
                                writetmp.name = name_tmp[index - 1];
                                images[0] = writetmp;
                                break;
                            }
                            MessageBox.Show("索引超界");

                        }
                        form1.SetinputNULL();
                    }
                    else
                    {
                        name_tmp = name.Split(GetChars(), StringSplitOptions.RemoveEmptyEntries);
                        writetmp.name = name_tmp[index - 1];
                        images[images.Count - 1] = writetmp;
                    }
                }
            }

            while (true)
            {
                Form1.cwlog("输入序号\n1:整形  2:字符串");
                RestartYesButton(MyProjectEvent.SortMode);
                are.WaitOne();
                if (this.sortModes != MySortModes.NULL)
                {
                    break;
                }

            }

            images = selectmde(images);
            Form1.cwlog("_______________");

            Graphics g = Graphics.FromImage();
            foreach (var item in images)
            {
                Form1.cwlog(item.name);
            }
            MessageBox.Show("draw完成");

        }

    }
}
