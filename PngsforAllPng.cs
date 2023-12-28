using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//bilibili W傲奇W
namespace Tr
{
    public static class PngsforAllPng
    {
        //public static  Color black = Color.FromArgb(0, 0, 0);
        //public static Color transparent = Color.FromArgb(0, 0, 0, 0);
        //public static Bitmap blackToTransparent(Bitmap b) 
        //{
        //    Bitmap bitmap = b;
        //    for (int i = 0; i < bitmap.Height; i++)
        //    {
        //        for (int i2 = 0; i2 < bitmap.Width; i2++)
        //        {
        //            Color point = bitmap.GetPixel(i2, i);
        //            if (point == black)
        //            {
        //                bitmap.SetPixel(i2, i,transparent);
        //            }
        //        }
        //    }
        //    return bitmap;
        //}
        static TextBox te;
        static Label la;
        private static CheckBox istran;
        private static CheckBox ispng;

        public static AutoResetEvent are = new AutoResetEvent(false);
        static Thread thread = null;
        public static string GetTime(string ToLocalTime)
        {
            char[] chars = { '/', ' ', ':' };
            string re = "";
            string[] arr = ToLocalTime.Split(chars);
            for (int i = 0; i < arr.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        re += arr[i] + "年";
                        break;
                    case 1:
                        re += arr[i] + "月";
                        break;
                    case 2:
                        re += arr[i] + "日";
                        break;
                    case 3:
                        re += arr[i] + "时";
                        break;
                    case 4:
                        re += arr[i] + "分";
                        break;
                    case 5:
                        re += arr[i] + "秒";
                        break;
                    default:
                        break;
                }
            }
            Console.WriteLine(re);
            return re;
        }
        static async ValueTask<List<Image>> GIF2PNG(string path, bool p, bool isT, byte value)
        {

            if (p)
            {
                await im(path, isT, value);
                return null;
            }

            return await retIM(path);
        }

        static async Task im(string path, bool isT, byte value)
        {
            string[] file = path.Split('/', '\\', '.');

            DirectoryInfo dir = isT ?
                Directory.CreateDirectory($"{ImageNameSort.Folder_ImagesData}\\{ImageNameSort.Folder_NoBlackImages}\\" + (file[file.Length - 2] + "\\")) :
              Directory.CreateDirectory($"{ImageNameSort.Folder_ImagesData}\\{ImageNameSort.Folder_OrdinaryImages}\\" + (file[file.Length - 2] + "\\"))
                ;
            Image image = Image.FromFile(path);

            FrameDimension f = new FrameDimension(image.FrameDimensionsList[0]);
            int maxf = image.GetFrameCount(f);
            for (int i = 0; i < maxf; i++)
            {
                image.SelectActiveFrame(f, i);
                Bitmap b = new Bitmap(image);
                if (isT)
                {

                    await Task.Run(() => { BlackToTransparent.Run(b, value); }); 
                }
                b.Save(dir.FullName + $"{i}.png", ImageFormat.Png);
                b.Dispose();
            }
            image.Dispose();
        }
        static async ValueTask<List<Image>> retIM(string path)
        {
            Image image = Image.FromFile(path);
            List<Image> ret = new List<Image>(8);
            FrameDimension f = new FrameDimension(image.FrameDimensionsList[0]);
            int maxf = image.GetFrameCount(f);
            for (int i = 0; i < maxf; i++)
            {
                image.SelectActiveFrame(f, i);
                Bitmap b = new Bitmap(image);
                ret.Add(b);
            }

            return ret;
        }

        static string[] FilesPaths(string path)
        {
            return Directory.GetFiles(path);
        }

        static  void PngsToPng(string path, byte value, bool isTransparen, bool isPng)
        {
            try
            {
                string name = "";
                Bitmap bitmap = null;
                Size size = new Size(0, 0);
                List<Image> images = new List<Image>();

                foreach (string item in FilesPaths(path))
                {
                    string[] save = item.Split('\\');
                    string[] file = item.Split('.');

                    List<Image> gifs = null;


                    if (file[file.Length - 1] == "png" || file[file.Length - 1] == "jpg")
                    {
                        string suffix = file[file.Length - 1] != "png" ? ".png" : "";

                        //是否转存png
                        if (isPng)
                        {
                            if (isTransparen)
                            {
                                Bitmap map = BlackToTransparent.Run(new Bitmap(item), value);

                                DirectoryInfo directory = Directory.CreateDirectory($".\\{ImageNameSort.Folder_ImagesData}\\{ImageNameSort.Folder_NoBlackImages}\\" + save[save.Length - 2]);

                                Form1.cwlog("文件夹路径: " + directory.FullName);
                                map.Save(directory.FullName + @"\" + save[save.Length - 1] + suffix, ImageFormat.Png);
                                map.Dispose();
                            }
                            else
                            {
                                Bitmap map = new Bitmap(item);
                                DirectoryInfo directory = Directory.CreateDirectory($".\\{ImageNameSort.Folder_ImagesData}\\{ImageNameSort.Folder_OrdinaryImages}\\" + save[save.Length - 2]);
                                Form1.cwlog("文件夹路径: " + directory.FullName);
                                try
                                {
                                    map.Save(directory.FullName + @"\" + save[save.Length - 1] + suffix, ImageFormat.Png);
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("文件已被打开！");
                                    map.Dispose();
                                    throw;
                                }
                              
                                
                            }
                        }
                        else
                        {
                            images.Add(Image.FromFile(item));
                            if (bitmap == null)
                            {
                                name = save[save.Length - 1];
                                bitmap = new Bitmap(images[images.Count - 1].Size.Width, images[images.Count - 1].Size.Height);
                                size += images[images.Count - 1].Size;
                            }
                            else
                            {
                                size += images[images.Count - 1].Size;
                                bitmap = bitmap.Size.Width < images[images.Count - 1].Size.Width ? new Bitmap(images[images.Count - 1].Size.Width, bitmap.Size.Height) : bitmap;
                                bitmap = bitmap.Size.Height < images[images.Count - 1].Size.Height ? new Bitmap(bitmap.Size.Width, images[images.Count - 1].Size.Height) : bitmap;
                            }
                        }
                        //长图
                    }
                    else if (file[file.Length - 1] == "gif")
                    {
                        if ((gifs = GIF2PNG(item, isPng, isTransparen, value).Result) != null)
                        {
                            images.AddRange(gifs);
                            
                            if (bitmap == null)
                            {
                                name = save[save.Length - 1];
                                 size.Width += gifs[0].Size.Width * gifs.Count;
                                bitmap = new Bitmap(size.Width, gifs[0].Size.Height);
                              
                            }
                            else
                            {

                                size.Width += gifs[0].Size.Width * gifs.Count;
                                bitmap = bitmap.Height < gifs[0].Height ? new Bitmap((int)size.Width, gifs[0].Height) : bitmap;
                            }
                        }
                    }
                }

                if (isPng)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return;
                }
                if (images.Count != 0 && !isPng)
                {
                    //Console.WriteLine("size" + size.Width + ",," + images[0].Size.Height);
                    bitmap = new Bitmap(size.Width + (2 * (images.Count + 1)), bitmap.Size.Height + 2);

                    Graphics g = Graphics.FromImage(bitmap);

                    int Wtmp = 2;
                    for (int i = 0; i < images.Count; i++)
                    {
                        Console.WriteLine("长度" + i);

                        g.DrawImage(images[i], Wtmp, 1, images[i].Width, images[i].Height);

                        Wtmp += (images[i].Size.Width + 2);

                        //images[i].Dispose();
                    }
                    images.Clear();
                    //  string name = DateTime.Now.ToLocalTime().ToString().ToLowerInvariant();

                    DirectoryInfo dir = Directory.CreateDirectory($".\\{ImageNameSort.Folder_ImagesData}\\{ImageNameSort.Folder_SpriteSheet}");

                    if (isTransparen)
                        bitmap = BlackToTransparent.Run(bitmap, value);
                    Form1.cwlog("大小: " + bitmap.Size.ToString());
                    Form1.cwlog("文件夹路径: " + dir.FullName);

                    try
                    {
                        bitmap.Save(dir.FullName + "\\" + $"{name}_long.png", ImageFormat.Png);
                    }
                    catch (Exception)
                    {
                        Form1.cwlog("超长图片 报存失败 因为超越了GDI限制！");
                        MessageBox.Show(@"超长图片 报存失败 因为超越了GDI限制！建议把""每行几张""给勾选上并填写合适的大小！");
                        bitmap.Dispose();
                        g.Dispose();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        throw;
                    }
                   
                    bitmap.Dispose();
                    g.Dispose();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

            }
            catch (Exception)
            {
                Form1.cwlog("路径错误");
                MessageBox.Show("路径错误");
            }



        }
        static void wait()
        {
            are.WaitOne();
            while (true)
            {
                Console.WriteLine("开始");
                string temp = null;
                byte value = 0;
                bool isTransparen = false, isPng = false;
                te.Invoke((MethodInvoker)delegate
                {
                    temp = te.Text;
                    value = byte.Parse(la.Text);
                    isTransparen = istran.Checked;
                    isPng = ispng.Checked;
                });
                PngsToPng(temp, value, isTransparen, isPng);
                MessageBox.Show("完成");
                are.WaitOne();
            }
        }

        public static void Run(TextBox t, Label value, CheckBox isTransparens, CheckBox isPng)
        {
            te = t;
            la = value;
            istran = isTransparens;
            ispng = isPng;
            thread = new Thread(wait);
            thread.Start();
        }


        public delegate void RUN(TextBox path, Label label, CheckBox isTransparens, CheckBox isPng);
        public static RUN GetRUN()
        {
            return new RUN(Run);
        }


    }

}
