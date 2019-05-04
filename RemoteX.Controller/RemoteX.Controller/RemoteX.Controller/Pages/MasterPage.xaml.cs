using RemoteX.Controller.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RemoteX.Controller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        public ListView ListView;

        public MasterPage()
        {
            InitializeComponent();

            BindingContext = new ControlPageViewModel();
            ListView = MenuItemsListView;
        }
       
        class ControlPageViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<ControllerMenuItem> MenuItems { get; set; }
            
            public ControlPageViewModel()
            {
                MenuItems = new ObservableCollection<ControllerMenuItem>(new[]
                {
                    new ControllerMenuItem { Id = 0, Title = "媒体遥控", TargetType = typeof(MediaControlPage) },
                    new ControllerMenuItem { Id = 1, Title = "演示文稿遥控", TargetType = typeof(SlideControlPage) },
                    new ControllerMenuItem { Id = 2, Title = "文件浏览", TargetType = typeof(FileControlListPage) },
                    new ControllerMenuItem { Id = 3, Title = "键盘控制", TargetType = typeof(KeyboardControlPage) },
                    new ControllerMenuItem { Id = 4, Title = "鼠标控制", TargetType = typeof(MouseControlPage) },
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}