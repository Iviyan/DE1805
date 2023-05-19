using System;
using System.Windows;

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
