using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightApplication3
{
    public partial class ProductPurchase : ChildWindow
    {
        public event EventHandler SubmitClicked;

        private string _amountPurchased;
        private string _amountAvailable;
        private string _price;

        public string AmountPurchased
        {
            get { return _amountPurchased; }
            set { _amountPurchased = value; }
        }

        public ProductPurchase(double price, int quantity)
        {
            InitializeComponent();
            _amountAvailable = quantity.ToString();
            _price = price.ToString();
            txtAmountAvailable.Text = _amountAvailable;
            txtPrice.Text = _price;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubmitClicked != null)
            {
                _amountPurchased = txtAmountPurchased.Text;
                SubmitClicked(this, new EventArgs());
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void txtAmountPurchased_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TODO:compute purchasetotal and update textbox

        }

        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int result;
            if (int.TryParse(txtAmountPurchased.Text, out result))
            {
                if (result > int.Parse(_amountAvailable))
                {
                    txtError.Text = "The amount to purchase is greater than the amount available.";
                    txtAmountPurchased.Text = "";
                    e.Cancel = true;
                }
            }
            else
            {
                txtError.Text = "You must enter an integer.";
                e.Cancel = true;
            }

        }

    }
}

