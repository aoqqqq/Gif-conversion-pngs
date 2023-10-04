using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tr
{
    public static class BlackToTransparent
    {
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
        static public Bitmap Run(Bitmap bitmap, byte value)
        {
            Bitmap b = bitmap;
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    b.SetPixel(x, y, newcolor(bitmap.GetPixel(x, y), value));
                }
            }
            return b;
        }
    }
}
