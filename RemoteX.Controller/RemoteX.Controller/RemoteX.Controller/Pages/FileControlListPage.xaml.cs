using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RemoteX.Controller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileControlListPage : ContentPage
    {
        static public ObservableCollection<string> Items { get; set; }
        static private Dictionary<string, FileViewModel> FileModels { get; set; } = new Dictionary<string, FileViewModel>();
        public FileControlListPage()
        {
            InitializeComponent();

            Items = new ObservableCollection<string>
            {
                "正在获取磁盘信息……"
            };
            MyListView.ItemsSource = Items;
            App.ControllerService.SendDictionaryRequest("*");
        }

        static private FileViewModel PreviousFolder = new FileViewModel("Root");

        static private bool isEnumStop = true;
        /// <summary>
        /// 接收到目录内容信息
        /// </summary>
        /// <param name="arg"></param>
        static public void ControllerService_OnFileManageWriteCompleted(object arg)
        {
            if(isEnumStop)
            {
                isEnumStop = false;
                Items.Clear();
                FileModels.Clear();
            }

            System.Diagnostics.Debug.WriteLine("Received Outside");
            List<string> files = (List<string>)arg;
            
            foreach(var file in files)
            {
                if (file == "RemoteFileEnumEnds")
                {
                    System.Diagnostics.Debug.WriteLine("枚举完成");
                    isEnumStop = true;
                    return;
                }
                var fileModel = new FileViewModel(file);
                Items.Add(fileModel.FileName);
                FileModels.Add(fileModel.FileName, fileModel);
            }
        }

        /// <summary>
        /// 点击路径后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            else
            {
                //Title = (string)e.Item;
                var fileModel = FileModels[e.Item.ToString()];
                var type = fileModel.Type;
                var path = fileModel.FilePath;
                if(type == PathType.Folder)
                {
                    Title = fileModel.FileName;
                    PreviousFolder = new FileViewModel(path);
                    Items.Clear();
                    FileModels.Clear();
                    isEnumStop = false;
                }
                App.ControllerService.SendDictionaryRequest(path);
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        enum PathType { File, Folder };
        private class FileViewModel
        {
            internal FileViewModel(string path)
            {
                FilePath = path;
            }
            internal string FilePath { set; get; }
            internal string FileName
            {
                get
                {
                    int fileNameBeginIndex = 0;
                    for(int i = FilePath.Length - 2; i >= 0; --i)
                    {
                        if(FilePath[i] == '/' || FilePath[i] == '\\')
                        {
                            fileNameBeginIndex = i + 1;
                            break;
                        }
                    }
                    char[] fileName = null;
                    if(Type == PathType.Folder)
                    {
                        fileName = new char[FilePath.Length - fileNameBeginIndex - 1];
                        FilePath.CopyTo(fileNameBeginIndex, fileName, 0, FilePath.Length - fileNameBeginIndex - 1);
                    }
                    else
                    {
                        fileName = new char[FilePath.Length - fileNameBeginIndex];
                        FilePath.CopyTo(fileNameBeginIndex, fileName, 0, FilePath.Length - fileNameBeginIndex);
                    }
                    System.Diagnostics.Debug.WriteLine(fileName);
                    return new string(fileName);
                }
            }
            internal PathType Type
            {
                get
                {
                    if (FilePath.Last() == '/' || FilePath.Last() == '\\')
                    {
                        return PathType.Folder;
                    }
                    else
                    {
                        return PathType.File;
                    }
                }
            }
        }
    }
}
