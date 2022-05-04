using System.Windows;
using System.Windows.Controls;

namespace LabaOOP5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MakeTransaction(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(TransactionAmount.Text, out var transactionAmount))
            {
                MessageBox.Show("Неверно введена сумма транзакции");
                return;
            }

            var transactionType = (CurrencyType)TransactionCurrency.SelectedIndex;

            var transactionMoney = new Money(transactionAmount, transactionType);
            var (peer, seed) = TransactionTarget.SelectedIndex == 0 ? (AccountOne, AccountTwo) : (AccountTwo, AccountOne);
            try
            {
                seed.Account.TransitionToAnotherAccount(transactionMoney, peer.Account);
            }
            catch (AccountingException)
            {
                MessageBox.Show("Недостаточно денег");
            }
            peer.Update();
            seed.Update();
        }
    }
}
