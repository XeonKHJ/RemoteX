using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RemoteX.Controller.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FileControlPage : ContentPage
	{
		public FileControlPage ()
		{
			InitializeComponent ();

            App.ControllerService.OnFileManageWriteCompleted -= ControllerService_OnFileManageWriteCompleted;
            App.ControllerService.OnFileManageWriteCompleted += ControllerService_OnFileManageWriteCompleted;

            App.ControllerService.SendDictionaryRequest("*");
        }

        private void ControllerService_OnFileManageWriteCompleted(object arg)
        {
            List<string> files = (List<string>)arg;
            Files.Clear();
            foreach(var file in files)
            {
                Files.Add(new DictionaryViewModel { FilePath = file });
            }

        }

        private ObservableCollection<DictionaryViewModel> Files { set; get; } = new ObservableCollection<DictionaryViewModel>();
	}
}