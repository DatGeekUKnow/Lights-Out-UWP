using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LightsOutUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        private int gridSize = 3;
        public BlankPage1()
        {
            this.InitializeComponent();
            // by default
            size3.IsChecked = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int gameSize = int.Parse(e.Parameter as string);
            switch (gameSize)
            {
                case 3:
                    size3.IsChecked = true;
                    gridSize = 3;
                    break;
                case 4:
                    size4.IsChecked = true;
                    gridSize = 4;
                    break;
                case 5:
                    size5.IsChecked = true;
                    gridSize = 5;
                    break;
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("gridSize"))
            {
                int loadedSize = int.Parse(ApplicationData.Current.LocalSettings.Values["gridSize"] as string);
                // load in the gridSize and check the corresponding radio button
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("gridColor"))
            {
                string json = ApplicationData.Current.LocalSettings.Values["gridColor"] as string;
                gridColorPicker.Color = (Color)XamlBindingHelper.ConvertValue(typeof(Color), json);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["gridSize"] = gridSize.ToString();
            ApplicationData.Current.LocalSettings.Values["gridColor"] = gridColorPicker.Color.ToString();
        }

        private void size3_Checked(object sender, RoutedEventArgs e)
        {
            if (size3.IsChecked != null)
            {
                gridSize = 3;
            }
        }

        private void size4_Checked(object sender, RoutedEventArgs e)
        {
            if (size4.IsChecked != null)
            {
                gridSize = 4;
            }
        }

        private void size5_Checked(object sender, RoutedEventArgs e)
        {
            if (size4.IsChecked != null)
            {
                gridSize = 5;
            }
        }
    }
}
