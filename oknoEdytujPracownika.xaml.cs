using Bank;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfApplication1
{
    public partial class oknoEdytujPracownika : Window
    {
      
        Window okno;

      
        List<int> ind;
        LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);
        int id_pracownika;
        public oknoEdytujPracownika(Window okno, List<int> indeksy, int idPracownika)
        {
         

            id_pracownika = idPracownika;

            ind = indeksy;
            this.okno = okno;

            InitializeComponent();
            var zapytanie =
    from c in dc.Pracownicy
    select new { c.Id_Pracownika, c.Imię_pracownika, c.Nazwisko_pracownika, c.PESEL, c.Wynagrodzenie, c.Telefon,  c.zaznaczony , c.Password};



            textBoxImie.Text = dc.Pracownicy.First(p => p.Id_Pracownika == indeksy[0]).Imię_pracownika;
            textBoxNazwisko.Text = dc.Pracownicy.First(p => p.Id_Pracownika == indeksy[0]).Nazwisko_pracownika;
           textBoxTelefon.Text = (dc.Pracownicy.First(p => p.Id_Pracownika == indeksy[0]).Telefon).ToString();
            textBoxPESEL.Text = dc.Pracownicy.First(p => p.Id_Pracownika == indeksy[0]).PESEL.ToString();
            textBoxWynagrodzenie.Text = dc.Pracownicy.First(p => p.Id_Pracownika == indeksy[0]).Wynagrodzenie.ToString();
            if (dc.Pracownicy.First(p => p.Id_Pracownika == indeksy[0]).Id_Pracownika.Equals(id_pracownika))
                {
                textBoxPassword.Visibility = Visibility.Visible;
                LabelHaslo.Visibility = Visibility.Visible;
            };

            
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
        private void edytuj_Click(object sender, RoutedEventArgs e)
        {

            
                string Imie = "", nazwisko = "", wynagr1 = "", wynagr2 = "", pesel = "", telefon = "", telefon2 = "";

                decimal decimalParse1;
                if (!decimal.TryParse(textBoxWynagrodzenie.Text, out decimalParse1) || textBoxWynagrodzenie.Text.Length > 30)
                {

                    wynagr1 = "Błędne dane wynagrodzenia. Musi być liczba. \n";
                    textBoxWynagrodzenie.Text = "";
                }
                else
                {
                    if (decimalParse1 < 1800 || textBoxWynagrodzenie.Text.Length > 30)
                    {
                        wynagr2 = "Błędna kwota wynagrodzenia. Musi być większa od 1800. \n";
                        textBoxWynagrodzenie.Text = "";
                    }
                }

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
               

                for (int i = 0; i < textBoxPESEL.Text.Length; i++)
                {
                    if (!char.IsNumber(textBoxPESEL.Text, i) || textBoxPESEL.Text.Length != 11)
                    {
                        pesel = "Błędny PESEL. \n";
                        textBoxPESEL.Text = "";
                    }
                }
                if (Imie == "" && nazwisko == "" && pesel == "" && telefon == "" && telefon2 == "" && wynagr1 == "" || wynagr2 == "")
                {
                if (textBoxImie.Text != "" && textBoxNazwisko.Text != "" && textBoxImie.Text != "Musisz uzupełnić" && textBoxNazwisko.Text != "Musisz uzupełnić" && textBoxTelefon.Text != "" && textBoxTelefon.Text != "Musisz uzupełnić" && textBoxWynagrodzenie.Text != "" && textBoxWynagrodzenie.Text != "Musisz uzupełnić" && textBoxPESEL.Text != "" && textBoxPESEL.Text != "Musisz uzupełnić")
                {

                    var PracownikDoZamiany = dc.Pracownicy.First(p => p.Id_Pracownika == ind[0]);

                    PracownikDoZamiany.Imię_pracownika = textBoxImie.Text.TrimEnd();
                    PracownikDoZamiany.Nazwisko_pracownika = textBoxNazwisko.Text.TrimEnd();

                    try
                    {
                        PracownikDoZamiany.Telefon = intParse1;
                        PracownikDoZamiany.Wynagrodzenie = decimalParse1;
                        PracownikDoZamiany.PESEL = textBoxPESEL.Text;
                        dc.SubmitChanges();
                        textBoxPassword.Visibility = Visibility.Hidden;
                        LabelHaslo.Visibility = Visibility.Hidden;
                        this.Close();
                    }

                    catch
                    {
                        textBoxPassword.Visibility = Visibility.Hidden;
                        LabelHaslo.Visibility = Visibility.Hidden;
                        MessageBox.Show("PESEL, wynagrodzenie i telefon to liczby. Wpisz poprawnie.");
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

                    if (textBoxTelefon.Text == "")
                    {
                        textBoxTelefon.Foreground = Brushes.Red;
                        textBoxTelefon.FontWeight = FontWeights.Bold;
                        textBoxTelefon.Text = "Musisz uzupełnić";
                    }
                    if (textBoxPESEL.Text == "")
                    {
                        textBoxPESEL.Foreground = Brushes.Red;
                        textBoxPESEL.FontWeight = FontWeights.Bold;
                        textBoxPESEL.Text = "Musisz uzupełnić";
                    }
                    if (textBoxWynagrodzenie.Text == "")
                    {
                        textBoxWynagrodzenie.Foreground = Brushes.Red;
                        textBoxWynagrodzenie.FontWeight = FontWeights.Bold;
                        textBoxWynagrodzenie.Text = "Musisz uzupełnić";
                    }
                }

            }
            else
                {
                    MessageBox.Show("Podałeś błędne dane.  \n" + Imie + nazwisko + pesel + telefon + telefon2 + wynagr1 + wynagr2 + "");

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

     

        private void zaznaczonyTextboxTelefon(object sender, RoutedEventArgs e)
        {
            textBoxTelefon.Foreground = Brushes.Black;
            textBoxTelefon.FontWeight = FontWeights.Normal;
            textBoxTelefon.Text = "";
        }

        private void zaznaczonyTextboxPESEL(object sender, RoutedEventArgs e)
        {
            textBoxPESEL.Foreground = Brushes.Black;
            textBoxPESEL.FontWeight = FontWeights.Normal;
            textBoxPESEL.Text = "";
        }

        private void zaznaczonyTextboxWynagrodzenie(object sender, RoutedEventArgs e)
        {
            textBoxWynagrodzenie.Foreground = Brushes.Black;
            textBoxWynagrodzenie.FontWeight = FontWeights.Normal;
            textBoxWynagrodzenie.Text = "";
        }

        private void zaznaczonyTextboxPassword(object sender, RoutedEventArgs e)
        {
            textBoxPassword.Foreground = Brushes.Black;
            textBoxPassword.FontWeight = FontWeights.Normal;
            textBoxPassword.Text = "";
        }
    }
}
