
namespace Tr
{
    public class FilesName
    {
        public string name { get; }
        public string suffixName { get; }
        public  FilesName (string path) 
        {
            string[] A = path.Split(new char[] { '.' });
            this.suffixName = A[A.Length-1];    
            string tmp = null;
            for (int i =0; i<A.Length-1;i++) 
            {
                tmp+= A[i];
            }
            A = tmp.Split(new char[] { '\\','/' });
            this.name = A[A.Length-1];
         }
    }
}
