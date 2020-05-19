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
using TransferringParameters.ViewModel;
using TransferringParameters.View;
using TransferringParameters.Model;
using TransferringParameters.MVVM;
using System.ComponentModel;
using MahApps.Metro.Controls;
namespace TransferringParameters.View
   
 
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow :  MetroWindow, IDisposable
    {
        public MainWindowViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Perfom_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        public void Dispose()
        {
            this.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {

        }
    }
}
