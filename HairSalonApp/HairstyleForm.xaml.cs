using System.ComponentModel.DataAnnotations;
using System.Windows;
using HairSalonApp.DTOs;
using HairSalonApp.Models;

namespace HairSalonApp
{
    public partial class HairstyleForm : Window
    {
        public Hairstyle ResultHairstyle { get; private set; }

        private bool _isClosedByButton = false;

        public HairstyleForm(Hairstyle existingHairstyle = null)
        {
            InitializeComponent();

            cmbClientType.ItemsSource = Enum.GetValues(typeof(ClientType));
            cmbClientType.SelectedIndex = 0;

            if (existingHairstyle != null)
            {
                txtName.Text = existingHairstyle.Name;
                cmbClientType.SelectedItem = existingHairstyle.ClientCategory;

                if (existingHairstyle.Hairdresser != null)
                {
                    txtHairdresserFirstName.Text = existingHairstyle.Hairdresser.FirstName;
                    txtHairdresserLastName.Text = existingHairstyle.Hairdresser.LastName;
                }

                txtPrice.Text = existingHairstyle.Price.ToString();
                chkNeedsAdditional.IsChecked = existingHairstyle.NeedsAdditionalServices;
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

        private bool TrySaveData()
        {
            if (!int.TryParse(txtPrice.Text, out int price))
            {
                MessageBox.Show("Price must be a valid number!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var hairdresser = new Hairdresser
            {
                FirstName = txtHairdresserFirstName.Text,
                LastName = txtHairdresserLastName.Text
            };

            var hairstyle = new Hairstyle
            {
                Name = txtName.Text,
                ClientCategory = (ClientType)cmbClientType.SelectedItem,
                Hairdresser = hairdresser,
                Price = price,
                NeedsAdditionalServices = chkNeedsAdditional.IsChecked ?? false
            };

            var results = new List<ValidationResult>();

            var hdContext = new ValidationContext(hairdresser);
            Validator.TryValidateObject(hairdresser, hdContext, results, true);

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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isClosedByButton)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (TrySaveData())
                    {
                        this.DialogResult = true;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
                else
                {
                    this.DialogResult = false;
                }
            }
        }
    }
}