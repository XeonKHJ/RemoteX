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

            Files = new ObservableCollection<string> { "fuck", "you" };

            App.ControllerService.SendDictionaryRequest("*");
        }

        ObservableCollection<string> Files;

        private void ControllerService_OnFileManageWriteCompleted(object arg)
        {
            System.Diagnostics.Debug.WriteLine("Received Outside");
            List<string> files = (List<string>)arg;
            //foreach (var file in files)
            //{
            //    Files.Add(new DictionaryViewModel { FilePath = file });
            //}
        }

        class DictonaryViewModel
        {
            public string FilePath { set; get; }
        }
    }
}