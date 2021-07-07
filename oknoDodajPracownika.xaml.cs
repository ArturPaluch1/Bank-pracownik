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

namespace WpfApplication1
{
    public partial class OknoDodajPracownika : Window
    {
        Window okno;

       
        LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);

        public OknoDodajPracownika(Window okno)
        {
          
            InitializeComponent();

            this.okno = okno;
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

            string Imie = "", nazwisko = "", wynagr1 = "", wynagr2 = "", pesel = "", telefon = "", telefon2 = "", haslo = "";

            decimal decimalParse1;
            if (!decimal.TryParse(textBoxWynagrodzenie.Text, out decimalParse1) || textBoxWynagrodzenie.Text.Length > 30)
            {

                wynagr1= "Błędne dane wynagrodzenia. Musi być liczba. \n";
                textBoxWynagrodzenie.Text = "";
            }
            else
            {
                if (decimalParse1 < 1800 || textBoxWynagrodzenie.Text.Length > 30)
                {
                   wynagr2= "Błędna kwota wynagrodzenia. Musi być większa od 1800. \n";
                    textBoxWynagrodzenie.Text = "";
                }
            }
           
            int intParse1;
            if (!int.TryParse(textBoxTelefon.Text, out intParse1))
            {
               telefon2= "Błędny telefon. Musi być liczbą. \n";
                textBoxTelefon.Text = "";
            }
            if (textBoxTelefon.Text.Length != 9)
            {
                telefon= "Podaj 9 cyfr telefonu. \n";
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
            if (PasswordBoxHaslo.Password.Length < 9 || PasswordBoxHaslo.Password.Length > 30)
            {
               haslo= "Podaj dłuższe hasło. \n";
                PasswordBoxHaslo.Password = "";
            }

            for (int i = 0; i < textBoxPESEL.Text.Length; i++)
            {
                if (!char.IsNumber(textBoxPESEL.Text, i)||textBoxPESEL.Text.Length!=11)
                {
                   pesel= "Błędny PESEL. \n";
                    textBoxPESEL.Text = "";
                }
            }

            if (Imie=="" && nazwisko == "" && pesel == "" && haslo == "" && telefon == "" && telefon2 == "" && wynagr1 == "" && wynagr2 == "" )
            {
                if (textBoxImie.Text != "" && textBoxNazwisko.Text != "" && textBoxImie.Text != "Musisz uzupełnić" && textBoxNazwisko.Text != "Musisz uzupełnić" && PasswordBoxHaslo.Password != "" && PasswordBoxHaslo.Password != "Musisz uzupełnić" && textBoxTelefon.Text != "" && textBoxTelefon.Text != "Musisz uzupełnić" && textBoxPESEL.Text != "" && textBoxPESEL.Text != "Musisz uzupełnić" && textBoxWynagrodzenie.Text != "" && textBoxWynagrodzenie.Text != "Musisz uzupełnić")
                {

                    DateTime tempData = DateTime.Now;
                    string data = tempData.ToShortDateString();

                    LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);

                    Pracownicy bob = new Pracownicy();
                    bob.Imię_pracownika = textBoxImie.Text.TrimEnd();
                    bob.Nazwisko_pracownika = textBoxNazwisko.Text.TrimEnd();
                    bob.Password = PasswordBoxHaslo.Password.ToString().TrimEnd();
                    bob.PESEL = textBoxPESEL.Text;
                    bob.Data_zatrudnienia = tempData;
                    bob.Wynagrodzenie = decimalParse1;
                    bob.Telefon = intParse1;
                    bob.zaznaczony = false;
                    bob.aktywny = true;

                    dc.Pracownicy.InsertOnSubmit(bob);
                    dc.SubmitChanges();

                    MessageBox.Show("Dodawanie powiodło się.");

                    this.Close();

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
                MessageBox.Show("Podałeś błędne dane.  \n" + Imie + nazwisko + pesel + haslo + telefon + telefon2 + wynagr1 + wynagr2 + "");

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

    }
}
