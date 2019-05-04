using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RemoteX.Controller.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MouseControlPage : ContentPage
	{
		public MouseControlPage ()
		{
			InitializeComponent ();

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += PanGesture_PanUpdated;
            touchpadGrid.GestureRecognizers.Add(panGesture);
        }

        private void PanGesture_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.TotalX.ToString() + ", " + e.TotalY.ToString());
        }
    }
}