using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Tr
{
    public struct ImageName
    {
       public string name;
        public Image image;
    };
    public static class  ImageNameSort
    {

        public static long SumASCII(string str) 
        {
            long Length = 0;
            foreach (var item in str)
            {
                if ((int)item>=48&& (int)item<=57 || (int)item>=65&&(int)item<=90 || (int)item>=97&&(int)item<=122)
                     Length += (long)((int)item);
            }
            return Length;
        }      
        public static string StrArrayToString(in string[] array) 
        {
            string str =null;
            foreach (var item in array)
            {
                str += item;
            }
            return str;
        }

        public static string RemoveClutter(string str) 
        {
           char[] sp = { ' ' ,'_', '+', '-', '/' ,'(',')', '（', '）' ,'.','。'};
           string[] strs = str.Split(sp,StringSplitOptions.RemoveEmptyEntries);
           return StrArrayToString(in strs);
        }
    
       public static long StrToint(string str) 
        {
           string rc = RemoveClutter(str);
            string tmp = null;
            var tmp1 =rc.Where( (c)=> char.IsDigit(c) );
            foreach (var item in tmp1)
            {
                tmp += item;
            }

            string ret = null;
            int first = 0;
    
            for (var i=0;i< tmp.Length;i++)
            {
                //零开头的报错 2023年7月4日19:23:26
                if (first == 0 && Int32.Parse(tmp[i].ToString()) != 0)
                {
                    //第一个不为0的;
                    ret += tmp[i];
                    ++first;
                
                }
                else if (first == 1)
                {
                    ret += tmp[i];
                }
                else if(tmp.Length==1)
                {
                    ret += tmp[i];
                }
            }
            return long.Parse(ret);
        }  
        public static List<ImageName> IntSort(List<ImageName> names) 
        {
            List<ImageName> ret = names;
            for (int i = 0; i < names.Count; i++)
            {
                for (int j = i+1; j < names.Count; j++)
                {
                    if (StrToint(ret[i].name) > StrToint(ret[j].name))
                    {
                        ImageName tmp = ret[i];
                        ret[i] = ret[j];
                        ret[j] = tmp;
                    }
                }
            }
            return ret;
        }
        public static List< ImageName >SortBIgForward(List<ImageName> imageNames)
        {
            List<ImageName> imageName = imageNames;
            for (int i = 0; i < imageNames.Count; i++)
            {
                for (int j = i+1;j <imageName.Count; j++)
                if (SumASCII(imageName[i].name) < SumASCII(imageName[j].name)) 
                {
                        ImageName tmp = imageName[j];
                        imageName[j] = imageName[i];
                        imageName[i] = tmp; 
                }
            }
            return imageNames;
        }
        public static List<ImageName> StrSort(List<ImageName> names) 
        {
            string[] str = new string[names.Count];
            List< ImageName > ret = new List<ImageName>(names.Count);
            for (int i = 0; i < str.Length; i++) 
            {
                str[i] = names[i].name;
            }
            Array.Sort(str);
            for (int i = 0; i < str.Length; i++)
            {
                foreach (var item in names)
                {
                    if (item.name == str[i])
                    {
                        ret.Add(item);
                        break;
                    }
                }
            }

            return ret;
        }
    }
}
