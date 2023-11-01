using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для PageData.xaml
    /// </summary>
    public partial class PageData : Page
    {
        ObservableCollection<Журнал_параметров> ListPos;
        public static Диплом_МищенкоEntities DiplomaEntitiesRaschet { get; set; }
        public PageData()
        {

            DiplomaEntitiesRaschet = new Диплом_МищенкоEntities();
            InitializeComponent();
            ListPos = new ObservableCollection<Журнал_параметров>();
            var queryData = from d in DiplomaEntitiesRaschet.Журнал_параметров
                             orderby d.Код_записи
                             select d;
            foreach (Журнал_параметров d in queryData)
            {
                ListPos.Add(d);
            }
            DataGridParams.ItemsSource = ListPos;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Журнал_параметров pos = DataGridParams.SelectedItem as Журнал_параметров;
            if (pos != null)
            {
                MessageBoxResult res = MessageBox.Show( "Удалить данные с кодом: " + pos.Код_записи + "?", "Удаление записи", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    DiplomaEntitiesRaschet.Журнал_параметров.Remove(pos);
                    DataGridParams.SelectedIndex = DataGridParams.SelectedIndex == 0 ? 1 : DataGridParams.SelectedIndex - 1;
                    ListPos.Remove(pos);
                    DiplomaEntitiesRaschet.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления");
            }
        }
    }
}
