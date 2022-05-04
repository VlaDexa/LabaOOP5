using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LabaOOP5
{
    /// <summary>
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class AccountControl : UserControl
    {
        private bool Init = true;
        public string Naming { get => AccountName.Content.ToString()!.Remove(0, "Состояние счёта".Length); set => AccountName.Content = $"Состояние счёта {value}"; }
        internal Account Account { get; } = new Account();

        public AccountControl()
        {
            InitializeComponent();
        }

        private void ChangeCurrencyType(object sender, SelectionChangedEventArgs e)
        {
            if (Init) Init = false;
            else MessageBox.Show("С вас будет взята коммисия 1%");
            Account.TransitionToAnotherType((CurrencyType)Type.SelectedIndex);
            Update();
        }

        public void Update()
        {
            Balance.Text = Account.GetBalance();
            Type.SelectedIndex = (int)Account.GetCurrencyType();
        }
    }
}
