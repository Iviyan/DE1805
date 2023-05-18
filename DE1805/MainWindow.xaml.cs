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
    public partial class MainWindow : Window
    {
        ApplicationContext context = new();
        CarFillingStation? station = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            string text = File.ReadAllText("Car_Filling_Station.json");
            CarFillingStation[] stations = JsonSerializer.Deserialize<CarFillingStation[]>(text)!;
            context.CarFillingStations.AddRange(stations);
            context.SaveChanges();
        }

        private async void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            StationInfoPanel.Visibility = Visibility.Collapsed;
            context.ChangeTracker.Clear();

            if (!int.TryParse(StationIdTb.Text, out var stationId))
            {
                MessageBox.Show("Необходимо ввести число.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                station = await context.CarFillingStations.Where(s => s.Id == stationId)
                    .Include(s => s.Data).SingleOrDefaultAsync();

                if (station == null)
                {
                    MessageBox.Show("Станции с таким ID не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка загрузки данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            StationAddressTb.Text = station.Address;

            var fuels = station.Data;

            var fuel92 = fuels.FirstOrDefault(f => f.Name == FuelTypes.AI92);
            if (fuel92 != null)
            {
                Fuel92PriceTb.Text = fuel92.Price.ToString();
                Fuel92AmountTb.Text = fuel92.AmountOfFuel.ToString();
            }

            var fuel95 = fuels.FirstOrDefault(f => f.Name == FuelTypes.AI95);
            if (fuel95 != null)
            {
                Fuel95PriceTb.Text = fuel95.Price.ToString();
                Fuel95AmountTb.Text = fuel95.AmountOfFuel.ToString();
            }

            var fuel98 = fuels.FirstOrDefault(f => f.Name == FuelTypes.AI98);
            if (fuel98 != null)
            {
                Fuel98PriceTb.Text = fuel98.Price.ToString();
                Fuel98AmountTb.Text = fuel98.AmountOfFuel.ToString();
            }

            var fuelDt = fuels.FirstOrDefault(f => f.Name == FuelTypes.DT);
            if (fuelDt != null)
            {
                FuelDtPriceTb.Text = fuelDt.Price.ToString();
                FuelDtAmountTb.Text = fuelDt.AmountOfFuel.ToString();
            }

            StationInfoPanel.Visibility = Visibility.Visible;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            station!.Address = StationAddressTb.Text;

            var fuels = station.Data;

            if (!decimal.TryParse(Fuel92PriceTb.Text, out decimal fuel92Price)
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

            context.SaveChanges();

            MessageBox.Show("Данные сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
