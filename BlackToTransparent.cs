using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tr
{
    public static class BlackToTransparent
    {
        static long iiii = 1;
        /// <summary>
        /// 如果rgb都小于阈值 T 否则F
        /// </summary>
        /// <param name="color"></param>
        /// <param name="value">阈值</param>
        /// <returns></returns>
        static bool issetColor(Color color, byte value)
        {
            return color.R <= value && color.G <= value && color.B <= value;
        }
        /// <summary>
        /// 如果都小于阈值的话 选其中最大的 并且把 透明度设置成最大的
        /// </summary>
        /// <param name="color"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static Color newcolor(Color color, byte value)
        {
            if (issetColor(color, value))
            {
                byte[] color1 = { color.R, color.G, color.B };
                byte Max = color1[0];
                for (int i = 1; i < color1.Length; i++)
                {
                    if (Max < color1[i])
                    {
                        Max = color1[i];
                    }
                }
                return Color.FromArgb(Max, color.R, color.G, color.B);
            }
            return color;
        }
        public static   Bitmap Run(Bitmap bitmap, byte value)
        {

            Bitmap b = (Bitmap)bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), PixelFormat.Format32bppArgb);

            Form1.cwlog("总_开始第" + iiii.ToString() + "张处理");
            iiii++;
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                   b.SetPixel(x, y, newcolor(bitmap.GetPixel(x, y), value));
                }
            }
            Form1.cwlog("总_结束第" + iiii.ToString() + "张处理");
            return b;
        }
    }
}
