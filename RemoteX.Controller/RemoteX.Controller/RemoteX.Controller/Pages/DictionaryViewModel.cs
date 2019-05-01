using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Controller.Pages
{
    class DictionaryViewModel
    {
        public string FilePath { set; get; }
        public string FileName
        {
            get
            {
                int i = FilePath.Length - 1;
                for(; i >= 0; --i)
                {
                    if(FilePath[i] == '\\')
                    {
                        break;
                    }
                }
                return FilePath.Substring(i);
            }
        }
    }
}
