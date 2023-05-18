using DE1805.Models;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;
using static System.Collections.Specialized.BitVector32;

namespace DE1805
{
    /// <summary>
    /// Логика взаимодействия для StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        private ApplicationContext context = new();
        CarFillingStation? station = null;

        public int StationId { get; }

        public StationWindow(int stationId)
        {
            InitializeComponent();
            StationId = stationId;

            Init();
        }

        /// <summary>
        /// Загрузка данных из БД и инициализация полей
        /// </summary>
        private async Task Init()
        {
            try
            {
                station = await context.CarFillingStations.Where(s => s.Id == StationId)
                    .Include(s => s.Data).SingleOrDefaultAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка загрузки данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            if (station == null)
            {
                Title = $"Новая станция {StationId}";
                StationInfoPanel.Visibility = Visibility.Visible;
                return;
            }
            else
                Title = $"Станция {StationId}";

            StationAddressTb.Text = station.Address;

            var fuels = station.Data;

            var fuel92 = fuels.FirstOrDefault(f => f.Name == FuelTypes.AI92);
            Fuel92PriceTb.Text = (fuel92?.Price ?? 0).ToString();
            Fuel92AmountTb.Text = (fuel92?.AmountOfFuel ?? 0).ToString();

            var fuel95 = fuels.FirstOrDefault(f => f.Name == FuelTypes.AI95);
            Fuel95PriceTb.Text = (fuel95?.Price ?? 0).ToString();
            Fuel95AmountTb.Text = (fuel95?.AmountOfFuel ?? 0).ToString();

            var fuel98 = fuels.FirstOrDefault(f => f.Name == FuelTypes.AI98);
            Fuel98PriceTb.Text = (fuel98?.Price ?? 0).ToString();
            Fuel98AmountTb.Text = (fuel98?.AmountOfFuel ?? 0).ToString();

            var fuelDt = fuels.FirstOrDefault(f => f.Name == FuelTypes.DT);
            FuelDtPriceTb.Text = (fuelDt?.Price ?? 0).ToString();
            FuelDtAmountTb.Text = (fuelDt?.AmountOfFuel ?? 0).ToString();

            StationInfoPanel.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// Обработчик нажатия кнопки "Сохранить изменения"
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (station == null)
            {
                station = new() { Id = StationId };
                context.Add(station);
            }

            var fuels = station.Data;

            if (String.IsNullOrWhiteSpace(StationAddressTb.Text)
                || !decimal.TryParse(Fuel92PriceTb.Text, out decimal fuel92Price)
                || !int.TryParse(Fuel92AmountTb.Text, out int fuel92Amount)
                || !decimal.TryParse(Fuel95PriceTb.Text, out decimal fuel95Price)
                || !int.TryParse(Fuel95AmountTb.Text, out int fuel95Amount)
                || !decimal.TryParse(Fuel98PriceTb.Text, out decimal fuel98Price)
                || !int.TryParse(Fuel98AmountTb.Text, out int fuel98Amount)
                || !decimal.TryParse(FuelDtPriceTb.Text, out decimal fuelDtPrice)
                || !int.TryParse(FuelDtAmountTb.Text, out int fuelDtAmount)
            )
            {
                MessageBox.Show("Одно или несколько полей заполнены некорректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            station!.Address = StationAddressTb.Text;

            var fuel92 = fuels.FirstOrDefault(f => f.Name == FuelTypes.AI92);
            if (fuel92 == null)
                station.Data.Add(fuel92 = new() { Price = fuel92Price, AmountOfFuel = fuel92Amount, Name = FuelTypes.AI92 });
            else
                (fuel92.Price, fuel92.AmountOfFuel) = (fuel92Price, fuel92Amount);

            var fuel95 = fuels.FirstOrDefault(f => f.Name == FuelTypes.AI95);
            if (fuel95 == null)
                station.Data.Add(fuel95 = new() { Price = fuel95Price, AmountOfFuel = fuel95Amount, Name = FuelTypes.AI95 });
            else
                (fuel95.Price, fuel95.AmountOfFuel) = (fuel95Price, fuel95Amount);

            var fuel98 = fuels.FirstOrDefault(f => f.Name == FuelTypes.AI98);
            if (fuel98 == null)
                station.Data.Add(fuel98 = new() { Price = fuel98Price, AmountOfFuel = fuel98Amount, Name = FuelTypes.AI98 });
            else
                (fuel98.Price, fuel98.AmountOfFuel) = (fuel98Price, fuel98Amount);

            var fuelDt = fuels.FirstOrDefault(f => f.Name == FuelTypes.DT);
            if (fuelDt == null)
                station.Data.Add(fuelDt = new() { Price = fuelDtPrice, AmountOfFuel = fuelDtAmount, Name = FuelTypes.DT });
            else
                (fuelDt.Price, fuelDt.AmountOfFuel) = (fuelDtPrice, fuelDtAmount);

            try
            {
                context.SaveChanges();

                MessageBox.Show("Данные сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception)
            {
                MessageBox.Show("Ошибка сохранения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}
