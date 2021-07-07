using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using Bank;

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);

        public MainWindow()
        {
            InitializeComponent();

        }
        private void zalogujButtonClick(object sender, RoutedEventArgs e)
        {
            var zapytanie =
from c in dc.Pracownicy
select new { c.Id_Pracownika, c.Imię_pracownika, c.Nazwisko_pracownika, c.Password, c.aktywny };
            bool zalogowany = false;
            foreach (var item in zapytanie)
            {

                if (item.Imię_pracownika.TrimEnd().Equals(textBoxImie.Text) && item.Nazwisko_pracownika.TrimEnd().Equals(textBoxNazwisko.Text) && item.Password.TrimEnd().Equals(PasswordBox.Password) && item.aktywny.Equals(true))
                {
                    OknoPracownika okno1 = new OknoPracownika(item.Id_Pracownika);
                    zalogowany = true;
                    okno1.Show();
                    this.Close();
                    break;

                }

            };

            if (zalogowany.Equals(false))
                MessageBox.Show("Błąd logowania do bazy danych.");

        }

        private void zarejestrujButtonClick(object sender, RoutedEventArgs e)
        {
            OknoDodajPracownika okno1 = new OknoDodajPracownika(this);
            okno1.Show();
        }
    }
}

