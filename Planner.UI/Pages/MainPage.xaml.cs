using Planner.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Planner.UI.Pages
{
    /// <summary>
    /// Interaction logic for MainVIewPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage(IMainPageViewModel mainPageVM)
        {
            InitializeComponent();
            DataContext = mainPageVM;
        }
    }
}
