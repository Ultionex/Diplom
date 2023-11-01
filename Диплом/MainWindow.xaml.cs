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

namespace Диплом
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new PageMain());
            Manager.Frame1 = MainFrame;
        }

        private void btn_Back_CLick(object sender, RoutedEventArgs e)
        {
            try
            {
                Manager.Frame1.Navigate(new PageMain());

                //отключения маркера для страниц
                Btn_Zayavki.IsChecked = false;
                Btn_Data.IsChecked = false;
                Btn_Otchet.IsChecked = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            //активация и скрывание кнопки "На главную"
            if (MainFrame.CanGoBack)
            {
                btn_Back.Visibility = Visibility.Visible;
            }
            else
            {
                btn_Back.Visibility = Visibility.Hidden;
            }
        }

        private void btn_Zayavki_CLick(object sender, RoutedEventArgs e)
        {
            Manager.Frame1.Navigate(new Zayavki());
        }

        private void btn_Data_CLick(object sender, RoutedEventArgs e)
        {
            Manager.Frame1.Navigate(new PageData());
        }

        private void btn_Otchet_CLick(object sender, RoutedEventArgs e)
        {
            Manager.Frame1.Navigate(new PageOtchet());
        }
    }
}
