using DE1805.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

namespace DE1805
{
    /// <summary>
    /// Логика взаимодействия для StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        HttpClient http = new();
        const string baseUrl = "http://localhost:5052";

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
            bool success = false;
            try
            {
                HttpResponseMessage result = await http.GetAsync($"{baseUrl}/getStationInfo?id={StationId}");
                if (result.StatusCode == HttpStatusCode.NotFound) success = true;
                else if (result.StatusCode == HttpStatusCode.OK)
                {
                    station = await result.Content.ReadFromJsonAsync<CarFillingStation>();
                    success = true;
                }
            }
            catch (Exception) { }

            if (!success)
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
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            CarFillingStation stationPatch = new() { Id = StationId };

            var fuels = stationPatch.Data;

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

            stationPatch!.Address = StationAddressTb.Text;

            stationPatch.Data.Add(new() { Price = fuel92Price, AmountOfFuel = fuel92Amount, Name = FuelTypes.AI92 });
            stationPatch.Data.Add(new() { Price = fuel95Price, AmountOfFuel = fuel95Amount, Name = FuelTypes.AI95 });
            stationPatch.Data.Add(new() { Price = fuel98Price, AmountOfFuel = fuel98Amount, Name = FuelTypes.AI98 });
            stationPatch.Data.Add(new() { Price = fuelDtPrice, AmountOfFuel = fuelDtAmount, Name = FuelTypes.DT });


            bool success = false;
            try
            {
                JsonContent content = JsonContent.Create(stationPatch);
                var result = await http.PostAsync($"{baseUrl}/setStation", content);
                if (result.IsSuccessStatusCode) {
                    MessageBox.Show("Данные сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    success = true;
                }
            }
            catch (Exception) { }

            if (!success)
                MessageBox.Show("Ошибка сохранения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}
