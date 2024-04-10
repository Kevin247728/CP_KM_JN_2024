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

using ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()     //konstruktor klays
        {
            InitializeComponent();
          //  _viewModel = new ViewModel.MainViewModel(new Model.BallService());
        //    DataContext = _viewModel;
        }

        //private void Start_Click(object sender, RoutedEventArgs e)  //obsługuje kliknięcie przycisku Start.
        //{
          //  _viewModel.StartSimulation();
        //}
    }
}

            
         

