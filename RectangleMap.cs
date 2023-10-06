using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
//bilibili W傲奇W
namespace Tr
{
    partial class RectangleMap
    {
        internal AutoResetEvent are = new AutoResetEvent(false);
        internal int index = 0;
        internal bool isSort = false;
        private bool isstr = false;
        internal long rowElement = 4; //每行几张?
        internal string chars = null;
        int spacepixel = Form1.Getspacepixel();//暂且固定2像素
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
        internal void RestartYesButton(MyProjectEvent eve, IntPtr? NULL)
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
            int i = 1;
            foreach (string item in s)
            {
                ret += $"序号{i}:" + item + " \n";
                i++;
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

        internal void GetImages(string path, ref List<ImageName> images, ref List<int> Gifheaddex)
        {
            long pathdexPos = 0;

            chars = null;
            string[] paths = Directory.GetFiles(path);
            //List<ImageName> images = new List<ImageName>(126);
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
                    Image tmp = Image.FromFile(ph);
                    byte value = 0;
                    if (Form1.istransparent(out value))
                    {
                        tmp = BlackToTransparent.Run((Bitmap)tmp, value);
                    }
                    images.Add(new ImageName
                    {
                        image = tmp,
                        name = name,
                        oldname = name,
                    });//获取图片与名字
                    #region 报废排序
                    //if (this.isSort)
                    //               {
                    //                   string[] name_tmp = null;

                    //                   ImageName writetmp = images[images.Count - 1];
                    //                   if (v == 0)
                    //                   {
                    //                       ++v;
                    //                       Form1.cwlog("第一个文件名:" + images[0].name);
                    //                       Form1.cwlog("请输入筛选字符,@N取消筛选直接字符串排序");
                    //                       this.form1.buttoninputYes.Invoke((MethodInvoker)delegate
                    //                       {
                    //                           this.form1.buttoninputYes.Enabled = true;
                    //                           Form1.mevent = MyProjectEvent.Sort;
                    //                       });
                    //                       this.are.WaitOne();
                    //                       if (form1.Getinputtext() == "@n" || "@N" == form1.Getinputtext())
                    //                       {
                    //                           //不筛选直接结束 上面已经赋值
                    //                           isstr = true;
                    //                           continue;
                    //                       }
                    //                       chars += form1.Getinputtext();
                    //                       while (true)
                    //                       {
                    //                           form1.SetinputNULL();
                    //                           name_tmp = name.Split(GetChars(), StringSplitOptions.RemoveEmptyEntries);
                    //                           Form1.cwlog("当前划分:" + ReturnString(name_tmp));
                    //                           Form1.cwlog("结束? @y/ 继续筛选");
                    //                           this.form1.buttoninputYes.Invoke((MethodInvoker)delegate
                    //                           {
                    //                               this.form1.buttoninputYes.Enabled = true;
                    //                               Form1.mevent = MyProjectEvent.Sort;
                    //                           });
                    //                           this.are.WaitOne();
                    //                           if (form1.Getinputtext() == "@y" || form1.Getinputtext() == "@Y")
                    //                           {
                    //                               break;
                    //                           }
                    //                           else { chars += form1.Getinputtext(); }
                    //                       }

                    //                       while (true)
                    //                       {
                    //                           Form1.cwlog("当前划分:" + ReturnString(name_tmp));
                    //                           Form1.cwlog("使用1~N 选择排序的块");
                    //                           Form1.mevent = MyProjectEvent.SortIndex;
                    //                           form1.SetinputNULL();
                    //                           this.form1.buttoninputYes.Invoke((MethodInvoker)delegate
                    //                           {
                    //                               form1.buttoninputYes.Enabled = true;
                    //                           });
                    //                           this.are.WaitOne();
                    //                           int ind = int.Parse(form1.Getinputtext());
                    //                           if (this.index > 0 && index <= name_tmp.Length)
                    //                           {
                    //                               writetmp.name = name_tmp[index - 1];
                    //                               images[0] = writetmp;
                    //                               break;
                    //                           }
                    //                           MessageBox.Show("索引超界");

                    //                       }
                    //                       form1.SetinputNULL();
                    //                   }
                    //                   else if (!isstr)
                    //                   {
                    //                       //有些名字没有相同特征 可能会产生错误 BUG 以后修
                    //                       name_tmp = name.Split(GetChars(), StringSplitOptions.RemoveEmptyEntries);
                    //                       writetmp.name = name_tmp[index - 1];
                    //                       images[images.Count - 1] = writetmp;
                    //                   }
                    //               }
                    #endregion
                }
                else if (suffix == "gif")
                {
                    Gifheaddex.Add((int)pathdexPos);//首地址
                    Image gif = new Bitmap(ph);
                    FrameDimension pngs = new FrameDimension(gif.FrameDimensionsList[0]);
                    int index = gif.GetFrameCount(pngs);
                    for (int i = 0; i < index; i++)
                    {
                        gif.SelectActiveFrame(pngs, i);
                        byte value = 0;
                        if (Form1.istransparent(out value))
                        {
                            images.Add(new ImageName
                            {
                                image = BlackToTransparent.Run(new Bitmap(gif), value),
                                name = name,
                                oldname = suffix,
                            });
                        }
                        else
                        {
                            Bitmap gf=new Bitmap( gif);
                            images.Add(new ImageName
                            {
                                image = gf,
                                name = name,
                                oldname = suffix,
                            });

                        }

                    }
                }
                pathdexPos++;
            }

            if (isSort)
            {
                sortModes = MySortModes.String;
                images = selectmde(images);
                return;
            }
            //2023年9月30日01:22:34 debug 暂且使用首个对比 废除int对比

            #region 废除1 字符排序
            //if (isSort)
            //{
            //    if (isstr)
            //    {
            //        this.sortModes = MySortModes.String;
            //    }
            //    else
            //    {

            //        while (true)
            //        {
            //            Form1.cwlog("输入序号\n1:整形  2:字符串");
            //            RestartYesButton(MyProjectEvent.SortMode);
            //            are.WaitOne();
            //            if (this.sortModes != MySortModes.NULL)
            //            {
            //                break;
            //            }
            //        }
            //    }

            //}       
            //images = selectmde(images);
            #endregion


            Form1.cwlog("_______________");
            return;
        }


        void draw(string path)
        {
            List<int> gifheads = new List<int>(10);
            List<ImageName> images = new List<ImageName>(10);
            try
            {
                GetImages(path, ref images, ref gifheads);
            }
            catch (Exception)
            {

                MessageBox.Show("路径错误");
                return;
            }


            int MAXHeight = images[0].image.Size.Height, MAXWidth = images[0].image.Size.Width;

            for (int i = 1; i < images.Count; i++)
            {
                MAXHeight = MAXHeight < images[i].image.Size.Height ? images[i].image.Size.Height : MAXHeight;
                MAXWidth = MAXWidth < images[i].image.Size.Width ? images[i].image.Size.Width : MAXWidth;
            }

            Size size = new Size(MAXWidth, MAXHeight);

            int ImageNumber = images.Count;
            int Column = ((int)(ImageNumber / this.rowElement)) + 1;

            //如果每行图片个数大于总共图片数 返回0 因为没有剩余了否则 在下面循环会越界的
            int Remaining_images = this.rowElement <= ImageNumber ? (int)(ImageNumber % this.rowElement) : 0;

            int W = size.Width * (Column == 1 ? ImageNumber : (int)this.rowElement),
                H = size.Height * (Column == 1 ? Column : Remaining_images != 0 ? Column : Column - 1); //长宽度
            //行首与尾巴不需要空格像素
            int row_spacepixels = (Column == 1 ? (ImageNumber - 1) * spacepixel : (int)this.rowElement - 1) * spacepixel;
            //列最上与最下不需要空格像素 //如果只有1行 那么上下根本不用空格 
            int column_spacepixels = (Column == 1 ? 0 : Remaining_images != 0 ? (Column - 1) * spacepixel : (Column - 2) * spacepixel);
            //
            Bitmap image = new Bitmap(W + row_spacepixels, H + column_spacepixels);

            Graphics g = Graphics.FromImage(image);
            g.SmoothingMode =System.Drawing.Drawing2D.SmoothingMode.None;
            //如果只剩下一行的时候可能会遇见99999(循环99999次) 所以 ==1返回数组长度避免出界,如果不为1 使用用户指定的每行元素个数
            int securerow = Column == 1 ? images.Count : (int)this.rowElement;

            int hlength = 0;
            int ii = 0;
            int forcol = Column == 1 ? Column : Column - 1;
            for (int i = 0; i < forcol; i++)
            {
                //2023年7月5日00:26:54 这玩意没归零导致图片消失
                int wlength = 0;

                for (int j = 0; j < securerow; j++)
                {
                   
                 g.DrawImage(images[ii].image, wlength, hlength, images[ii].image.Width, images[ii].image.Height);
                    Form1.cwlog(images[ii].name);

                    wlength += size.Width + spacepixel;
                    //wlength += images[ii].image.Width + spacepixel;
                    if (images[ii].oldname != "gif")
                        images[ii].image.Dispose();
                    ii++;
                }

                hlength += size.Height + spacepixel;

                if (i + 1 == forcol && Remaining_images > 0)
                {
                    wlength = 0;
                    for (int iii = 0; iii < Remaining_images; iii++)
                    {
                      
                         g.DrawImage(images[ii].image, wlength, hlength, images[ii].image.Width, images[ii].image.Height);
                        Form1.cwlog(images[ii].name);
                        //因为我原先就是直接算好图片的大小不是动态的 所以没有必要动态的改变图片间隔 否则失去意义
                        wlength += size.Width + spacepixel;
                        //wlength += images[ii].image.Width + spacepixel;
                        if (images[ii].oldname != "gif")
                            images[ii].image.Dispose();//2023年9月30日02:30:18
                        ii++;

                    }
                }
            }

            foreach (var item in gifheads)
            {
                images[item].image.Dispose();
            }



            DirectoryInfo dir = Directory.CreateDirectory($".\\{ImageNameSort.Folder_ImagesData}\\{ImageNameSort.Folder_SpriteSheet}");
            string fpath = dir.FullName + @"\" + $"{images[0].name}.png";
            Form1.cwlog("尺寸: " + image.Size.ToString());
            Form1.cwlog("文件夹路径: " + dir.FullName);
            image.Save(fpath, ImageFormat.Png);

            g.Dispose();
            gifheads.Clear();
            image.Dispose();
            images.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            MessageBox.Show("draw完成");
        }

    }
}
