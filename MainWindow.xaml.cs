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
            try
                {
                Pracownicy zalogowanyPracownik = dc.Pracownicy.ToList().Where(
                  a => textBoxImie.Text.TrimEnd().ToLower() == a.Imię_pracownika.TrimEnd().ToLower()
                  && textBoxNazwisko.Text.TrimEnd().ToLower().Equals(a.Nazwisko_pracownika.TrimEnd().ToLower())
                  && PasswordBox.Password.TrimEnd().Equals(a.Password.TrimEnd())
                      ).FirstOrDefault();

                if (zalogowanyPracownik != null)
                {
                    if (zalogowanyPracownik.aktywny == true)
                    {
                        OknoPracownika okno1 = new OknoPracownika((zalogowanyPracownik as Pracownicy).Id_Pracownika);

                        okno1.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Podany pracownik jest nieaktywny");
                    }


                }
                else
                {
                  
                }
            }
            catch
            {
                MessageBox.Show("Błąd logowania do bazy danych.");
            }
          
      
        }

        private void zarejestrujButtonClick(object sender, RoutedEventArgs e)
        {
            OknoDodajPracownika okno1 = new OknoDodajPracownika(this);
            okno1.Show();
        }
    }
}

