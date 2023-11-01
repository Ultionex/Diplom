using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для Zayavki.xaml
    /// </summary>
    public partial class Zayavki : Page
    {
        public double LKam;
        public double LPlast;
        public double LStav;
        public double Nz;
        public double KolStavNaFullKam;
        public double KolEdUglyaNaKam;
        public double KolKam;
        public static Диплом_МищенкоEntities pr;
        public Zayavki()
        {
          pr = new Диплом_МищенкоEntities();
            InitializeComponent();
        }
        public void Schitaet()
        {
            //Расчёт параметров
            Nz = Math.Round((LKam) / (LStav),2);
            KolStavNaFullKam = Math.Round((LKam) / (LStav),0);
            KolEdUglyaNaKam = Math.Round((KolStavNaFullKam) * Nz,2);
            KolKam = Math.Round((LPlast) / (LKam),0);
        }

        private void btnCalc_Click(object sender, RoutedEventArgs e)
        {
            //Вывод и ввод данных
            try
            {
                LKam = double.Parse(tbxLKam.Text);
                LPlast = double.Parse(tbxLPlast.Text);
                LStav = double.Parse(tbxLStav.Text);
                Schitaet();
                KolUglyaStav.Content = Nz;
                KolUglyaKam.Content = KolEdUglyaNaKam;
                KolStav.Content = KolStavNaFullKam;
                KolKamPlast.Content = KolKam;

            //формирование отчёта
            string FilePath = @"TextReports/" + (DateTime.Now.ToString()).Replace(':', ' ') + ".txt"; ;
                using (StreamWriter fileStream = File.CreateText(FilePath))
                {

                    fileStream.WriteLine($"Длина камеры {LKam} метров");
                    fileStream.WriteLine($"Длина става {LStav} метров");
                    fileStream.WriteLine($"Длина пласта {LPlast} метров ");
                    fileStream.WriteLine($"Количество угля с одного става {Nz}");
                    fileStream.WriteLine($"Количество единиц угля с камеры {KolEdUglyaNaKam}");
                    fileStream.WriteLine($"Количество ставов на камеру {KolStavNaFullKam}");
                    fileStream.WriteLine($"Количество камер в пласте {KolKam}");

                    //Добавление данных в бд
                    Журнал_параметров param = new Журнал_параметров();
                    param.Длина_камеры = LKam;
                    param.Длина_пласта = LPlast;
                    param.Длина_става = LStav;
                    param.Количество_единиц_угля_с_камеры = KolEdUglyaNaKam;
                    param.Количество_камер_в_пласте = KolKam;
                    param.Количество_ставов_на_камеру = KolStavNaFullKam;
                    param.Количество_угля_с_одного_става = Nz;
                    pr.Журнал_параметров.Add(param);
                    pr.SaveChanges();
                    tbxLKam.Clear();
                    tbxLPlast.Clear();
                    tbxLStav.Clear();
                    MessageBox.Show("Расчёт параметров выполнен");
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"Данные введены некорректно");
            }

        }
    }
}
