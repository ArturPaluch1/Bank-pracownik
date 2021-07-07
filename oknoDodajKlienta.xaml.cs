using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Data.OleDb;
using Bank;
using System.Data.Linq;

namespace WpfApplication1
{
    public partial class oknoDodajKlienta : Window
    {
        Window okno;



        public oknoDodajKlienta(Window okno)
        {

            InitializeComponent();

            this.okno = okno;


        }
        int sprawdzCzyJestJuzWBazie(string stringZapytanie, ref int numer)
        {
            LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);

            Random rand = new Random();
            int powtorzony = 0;
            var zapytanie = dc.ExecuteQuery<Klienci>(stringZapytanie);
            foreach (Klienci item in zapytanie)
            {
                if (item.Numer_rachunku.Equals(numer))
                {
                    powtorzony = 1;
                    numer = rand.Next(000000001, 999999999);  // losowanie nowego numeru, bo się powtórzył
                }
                if (powtorzony == 1)
                {
                    return 0;
                }
                else//bylo 0
                {
                    continue;
                }
            }
            return 1;
        }


        bool sprawdzCzyCyfry(string textboxText)
        {
            for (int i = 0; i < textboxText.Length; i++)
            {
                if (char.IsNumber(textboxText, i))
                {
                    return true;
                }
            }
            return false;
        }

        private void dodaj_Click(object sender, RoutedEventArgs e)
        {
            string Imie = "", nazwisko = "", miasto = "", ulica = "", telefon = "", telefon2 = "";



            int intParse1;
            if (!int.TryParse(textBoxTelefon.Text, out intParse1))
            {
                telefon2 = "Błędny telefon. Musi być liczbą. \n";
                textBoxTelefon.Text = "";
            }
            if (textBoxTelefon.Text.Length != 9)
            {
                telefon = "Podaj 9 cyfr telefonu. \n";
                textBoxTelefon.Text = "";
            }
            if (sprawdzCzyCyfry(textBoxImie.Text) == true || textBoxImie.Text.Length > 30)
            {
                Imie = "Popraw imię. \n";

                textBoxImie.Text = "";
            }
            if (sprawdzCzyCyfry(textBoxNazwisko.Text) == true || textBoxNazwisko.Text.Length > 30)
            {
                nazwisko = "Popraw nazwisko. \n";

                textBoxNazwisko.Text = "";
            }
            if (sprawdzCzyCyfry(textBoxMiasto.Text) == true || textBoxMiasto.Text.Length > 30)
            {
                miasto = "Błędna nazwa miasta. \n";
                textBoxMiasto.Text = "";
            }

          
                if ( textBoxUlica.Text.Length > 30)
                {
                    ulica = "Błędna nazwa ulicy. \n";
                    textBoxMiasto.Text = "";
                }
            

                if(Imie=="" && nazwisko =="" && miasto == "" && telefon == "" && telefon2 == "" && ulica == "")
            {


                if (textBoxImie.Text != "" && textBoxNazwisko.Text != "" && textBoxImie.Text != "Musisz uzupełnić" && textBoxNazwisko.Text != "Musisz uzupełnić" && PasswordBoxHaslo.Password != "" && PasswordBoxHaslo.Password != "Musisz uzupełnić" && textBoxTelefon.Text != "" && textBoxTelefon.Text != "Musisz uzupełnić" && textBoxMiasto.Text != "" && textBoxMiasto.Text != "Musisz uzupełnić" && textBoxUlica.Text != "" && textBoxUlica.Text != "Musisz uzupełnić")
                {

                    DateTime tempData = DateTime.Now;
                    string data = tempData.ToShortDateString();




                    LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);

                    Random rand = new Random();
                    int randomowyNumer = 0;//rand.Next(0, 999999999);

                    dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Klienci);
                    var zapytanie = dc.ExecuteQuery<Klienci>("select [id klienta],[Numer rachunku]from [klienci]");

                    while (true)
                    {

                        if (sprawdzCzyJestJuzWBazie("select [id klienta],[Numer rachunku]from [klienci]", ref randomowyNumer) != 0)  // wylosowany numer nie istnieje jeszcze w bazie
                        {
                            Klienci bob = new Klienci();
                            bob.Telefon = intParse1;
                            bob.Imię = textBoxImie.Text.TrimEnd();
                            bob.Nazwisko = textBoxNazwisko.Text.TrimEnd();
                            bob.Password = PasswordBoxHaslo.Password.ToString().TrimEnd();
                            bob.Numer_rachunku = randomowyNumer;
                            bob.Data_założenia = tempData;

                            bob.aktywny = true;
                            bob.zaznaczony = false;
                            bob.Miasto = textBoxMiasto.Text.TrimEnd();
                            bob.Ulica = textBoxUlica.Text.TrimEnd();
                            bob.Środki = 0;

                            dc.Klienci.InsertOnSubmit(bob);
                            dc.SubmitChanges();
                            MessageBox.Show("Dodawanie powiodło się. Numer rachunku klienta to: " + randomowyNumer + "");

                            this.Close();
                            break;
                        }

                    }

                }

                else
                {
                    if (textBoxImie.Text == "")
                    {
                        textBoxImie.Foreground = Brushes.Red;
                        textBoxImie.FontWeight = FontWeights.Bold;
                        textBoxImie.Text = "Musisz uzupełnić";
                    }
                    if (textBoxNazwisko.Text == "")
                    {
                        textBoxNazwisko.Foreground = Brushes.Red;
                        textBoxNazwisko.FontWeight = FontWeights.Bold;
                        textBoxNazwisko.Text = "Musisz uzupełnić";
                    }
                    if (PasswordBoxHaslo.Password == "")
                    {
                        PasswordBoxHaslo.Foreground = Brushes.Red;
                        PasswordBoxHaslo.FontWeight = FontWeights.Bold;
                        PasswordBoxHaslo.Password = "Musisz uzupełnić";
                    }
                    if (textBoxTelefon.Text == "")
                    {
                        textBoxTelefon.Foreground = Brushes.Red;
                        textBoxTelefon.FontWeight = FontWeights.Bold;
                        textBoxTelefon.Text = "Musisz uzupełnić";
                    }
                    if (textBoxMiasto.Text == "")
                    {
                        textBoxMiasto.Foreground = Brushes.Red;
                        textBoxMiasto.FontWeight = FontWeights.Bold;
                        textBoxMiasto.Text = "Musisz uzupełnić";
                    }
                    if (textBoxUlica.Text == "")
                    {
                        textBoxUlica.Foreground = Brushes.Red;
                        textBoxUlica.FontWeight = FontWeights.Bold;
                        textBoxUlica.Text = "Musisz uzupełnić";
                    }

                }
            }
                else
            {
                MessageBox.Show("Podałeś błędne dane.  \n" + Imie + nazwisko + miasto + telefon + telefon2 + ulica + "");
            }
          


        }

        private void anuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void zaznaczonyTextboxImie(object sender, RoutedEventArgs e)
        {
            textBoxImie.Foreground = Brushes.Black;
            textBoxImie.FontWeight = FontWeights.Normal;
            textBoxImie.Text = "";
        }
        private void zaznaczonyTextboxNazwisko(object sender, RoutedEventArgs e)
        {
            textBoxNazwisko.Foreground = Brushes.Black;
            textBoxNazwisko.FontWeight = FontWeights.Normal;
            textBoxNazwisko.Text = "";
        }

        private void zaznaczonyPasswordBoxHaslo(object sender, RoutedEventArgs e)
        {
            PasswordBoxHaslo.Foreground = Brushes.Black;
            PasswordBoxHaslo.FontWeight = FontWeights.Normal;
            PasswordBoxHaslo.Password = "";
        }

        private void zaznaczonyTextboxTelefon(object sender, RoutedEventArgs e)
        {
            textBoxTelefon.Foreground = Brushes.Black;
            textBoxTelefon.FontWeight = FontWeights.Normal;
            textBoxTelefon.Text = "";
        }

        private void zaznaczonyTextboxMiasto(object sender, RoutedEventArgs e)
        {
            textBoxMiasto.Foreground = Brushes.Black;
            textBoxMiasto.FontWeight = FontWeights.Normal;
            textBoxMiasto.Text = "";
        }

        private void zaznaczonyTextboxUlica(object sender, RoutedEventArgs e)
        {
            textBoxUlica.Foreground = Brushes.Black;
            textBoxUlica.FontWeight = FontWeights.Normal;
            textBoxUlica.Text = "";
        }
    }
}
