using System.ComponentModel.DataAnnotations;
using System.Windows;
using HairSalonApp.Models;

namespace HairSalonApp
{
    public partial class HairstyleForm : Window
    {
        public Hairstyle ResultHairstyle { get; private set; }
        private bool _isClosedByButton = false;

        public HairstyleForm(List<Hairdresser> existingMasters, Hairstyle existingHairstyle = null)
        {
            InitializeComponent();

            InitializeClientTypes();
            InitializeHairdressersList(existingMasters);

            if (existingHairstyle != null)
            {
                LoadExistingHairstyleData(existingHairstyle, existingMasters);
            }
        }

        private void InitializeClientTypes()
        {
            cmbClientType.ItemsSource = Enum.GetValues(typeof(ClientType));
            cmbClientType.SelectedIndex = 0;
        }

        private void InitializeHairdressersList(List<Hairdresser> masters)
        {
            cmbExistingHairdressers.ItemsSource = masters;

            if (masters.Any())
            {
                cmbExistingHairdressers.SelectedIndex = 0;
                chkNewHairdresser.IsChecked = false;
            }
            else
            {
                chkNewHairdresser.IsChecked = true;
            }
        }

        private void LoadExistingHairstyleData(Hairstyle hairstyle, List<Hairdresser> masters)
        {
            txtName.Text = hairstyle.Name;
            cmbClientType.SelectedItem = hairstyle.ClientCategory;
            txtPrice.Text = hairstyle.Price.ToString();
            chkNeedsAdditional.IsChecked = hairstyle.NeedsAdditionalServices;

            SelectOrEnterHairdresser(hairstyle.Hairdresser, masters);
        }

        private void SelectOrEnterHairdresser(Hairdresser hairdresser, List<Hairdresser> masters)
        {
            if (hairdresser == null) return;

            var match = masters.FirstOrDefault(m =>
                m.FirstName == hairdresser.FirstName &&
                m.LastName == hairdresser.LastName);

            if (match != null)
            {
                cmbExistingHairdressers.SelectedItem = match;
                chkNewHairdresser.IsChecked = false;
            }
            else
            {
                chkNewHairdresser.IsChecked = true;
                txtHairdresserFirstName.Text = hairdresser.FirstName;
                txtHairdresserLastName.Text = hairdresser.LastName;
            }
        }

        private void ToggleHairdresserInput(object sender, RoutedEventArgs e)
        {
            bool isNew = chkNewHairdresser.IsChecked ?? false;

            stackNewHairdresser.IsEnabled = isNew;
            stackNewHairdresser.Opacity = isNew ? 1.0 : 0.5;
            cmbExistingHairdressers.IsEnabled = !isNew;

            if (!isNew)
            {
                txtHairdresserFirstName.Clear();
                txtHairdresserLastName.Clear();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (TrySaveData())
            {
                _isClosedByButton = true;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _isClosedByButton = true;
            this.DialogResult = false;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isClosedByButton)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (TrySaveData()) this.DialogResult = true;
                    else e.Cancel = true;
                }
                else if (result == MessageBoxResult.Cancel) e.Cancel = true;
                else this.DialogResult = false;
            }
        }

        private bool TrySaveData()
        {
            if (!int.TryParse(txtPrice.Text, out int price))
            {
                MessageBox.Show("Price must be a valid number!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            Hairdresser selectedMaster;
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (chkNewHairdresser.IsChecked == true)
            {
                selectedMaster = new Hairdresser
                {
                    FirstName = txtHairdresserFirstName.Text,
                    LastName = txtHairdresserLastName.Text
                };

                var hdContext = new ValidationContext(selectedMaster);
                Validator.TryValidateObject(selectedMaster, hdContext, results, true);
            }
            else
            {
                selectedMaster = cmbExistingHairdressers.SelectedItem as Hairdresser;
                if (selectedMaster == null)
                {
                    MessageBox.Show("Please select an existing hairdresser or add a new one.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            var hairstyle = new Hairstyle
            {
                Name = txtName.Text,
                ClientCategory = (ClientType)cmbClientType.SelectedItem,
                Hairdresser = selectedMaster,
                Price = price,
                NeedsAdditionalServices = chkNeedsAdditional.IsChecked ?? false
            };

            var hsContext = new ValidationContext(hairstyle);
            Validator.TryValidateObject(hairstyle, hsContext, results, true);

            if (results.Any())
            {
                string errors = string.Join("\n", results.Select(r => r.ErrorMessage));
                MessageBox.Show(errors, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            ResultHairstyle = hairstyle;
            return true;
        }
    }
}