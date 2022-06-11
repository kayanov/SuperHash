using System.Text;

namespace SuperHash
{
    internal class Dictionary
    {
        public string path,
                      name;
        public Encoding encoding;

        public Dictionary(string path, string name, Encoding encoding)
        {
            this.path = path;
            this.name = name;
            this.encoding = encoding;
        }
    }
}
