using DE1805.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

namespace DE1805
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {

        public SearchWindow()
        {
            InitializeComponent();
        }

        /*private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationContext context = new();
            string text = File.ReadAllText("Car_Filling_Station.json");
            CarFillingStation[] stations = JsonSerializer.Deserialize<CarFillingStation[]>(text)!;
            context.CarFillingStations.AddRange(stations);
            context.SaveChanges();
        }*/

        /// <summary>
        /// Обработчик нажатия кнопки "Загрузить данные"
        /// </summary>
        private void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(StationIdTb.Text, out var stationId))
            {
                MessageBox.Show("Необходимо ввести число.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                StationWindow stationWindow = new(stationId);
                stationWindow.Show();
                stationWindow.Closing += (_, _) => this.Show();
                this.Hide();
            }
            catch (Exception) { } // Т.к. в процессе открытия окна может возникнуть ошибка, необходимо предотвратить вылет приложения
        }
    }
}
