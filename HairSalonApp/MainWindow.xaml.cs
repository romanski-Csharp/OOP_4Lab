using System.Windows;
using HairSalonApp.Models;
using HairSalonApp.Services;

namespace HairSalonApp
{
    public partial class MainWindow : Window
    {
        private HairSalon _currentSalon;
        private SalonDataManager _dataManager;

        public MainWindow()
        {
            InitializeComponent();
            
            _dataManager = new SalonDataManager();
            
            _currentSalon = _dataManager.LoadData();

            RefreshDataGrid();
            UpdateSalonInfo();
        }

        private void RefreshDataGrid()
        {
            HairstylesDataGrid.ItemsSource = null;
            HairstylesDataGrid.ItemsSource = _currentSalon.CompletedHairstyles;
        }

        private void UpdateSalonInfo()
        {
            SalonInfoTextBlock.Text = _currentSalon.ToShortString();
        }

        private void HairstylesDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            BtnEdit.IsEnabled = HairstylesDataGrid.SelectedItem != null;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            HairstyleForm form = new HairstyleForm(GetUniqueMasters());
            form.Owner = this;

            if (form.ShowDialog() == true)
            {
                _currentSalon.AddHairstyle(form.ResultHairstyle);
                UpdateSalonInfo();
                RefreshDataGrid();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (HairstylesDataGrid.SelectedItem is Hairstyle selectedHairstyle)
            {
                HairstyleForm form = new HairstyleForm(GetUniqueMasters(), selectedHairstyle);
                form.Owner = this;

                if (form.ShowDialog() == true)
                {
                    int index = _currentSalon.CompletedHairstyles.IndexOf(selectedHairstyle);
                    if (index != -1)
                    {
                        _currentSalon.CompletedHairstyles[index] = form.ResultHairstyle;
                        UpdateSalonInfo();
                        RefreshDataGrid();
                    }
                }
            }
        }

        private List<Hairdresser> GetUniqueMasters()
        {
            return _currentSalon.CompletedHairstyles
                                .Select(h => h.Hairdresser)
                                .Distinct()
                                .ToList();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _dataManager.SaveData(_currentSalon);
        }
    }
}