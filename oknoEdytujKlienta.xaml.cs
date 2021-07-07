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

using System.Data.OleDb;
using Bank;

namespace WpfApplication1
{
    public partial class oknoEdytujKlienta : Window
    {
        Window okno;
       
        List<int> ind;
        LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);

        public oknoEdytujKlienta(Window okno, List<int> indeksy)
        {


            ind = indeksy;
            this.okno = okno;
           
            InitializeComponent();
            var zapytanie =
    from c in dc.Klienci


     select new { c.Id_klienta, c.Imię, c.Nazwisko, c.Miasto, c.Ulica, c.Telefon, c.Numer_rachunku, c.zaznaczony };

            textBoxImie.Text = dc.Klienci.First(p => p.Id_klienta == indeksy[0]).Imię.TrimEnd();
            textBoxNazwisko.Text = dc.Klienci.First(p => p.Id_klienta == indeksy[0]).Nazwisko.TrimEnd();
            PasswordBoxHaslo.Password = dc.Klienci.First(p => p.Id_klienta == indeksy[0]).Password.TrimEnd();
            textBoxTelefon.Text = (dc.Klienci.First(p => p.Id_klienta == indeksy[0]).Telefon).ToString();
            textBoxMiasto.Text = dc.Klienci.First(p => p.Id_klienta == indeksy[0]).Miasto.TrimEnd();
            textBoxUlica.Text = dc.Klienci.First(p => p.Id_klienta == indeksy[0]).Ulica.TrimEnd();

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

            if (Imie == "" && nazwisko == "" && miasto == "" && telefon == "" && telefon2 == "" && ulica == "")
            {
                if (textBoxImie.Text != "" && textBoxNazwisko.Text != "" && textBoxImie.Text != "Musisz uzupełnić" && textBoxNazwisko.Text != "Musisz uzupełnić" && PasswordBoxHaslo.Password != "" && PasswordBoxHaslo.Password != "Musisz uzupełnić" && textBoxTelefon.Text != "" && textBoxTelefon.Text != "Musisz uzupełnić" && textBoxMiasto.Text != "" && textBoxMiasto.Text != "Musisz uzupełnić" && textBoxUlica.Text != "" && textBoxUlica.Text != "Musisz uzupełnić")
                {
                    var KlientDoZamiany = dc.Klienci.First(p => p.Id_klienta == ind[0]);

                    KlientDoZamiany.Imię = textBoxImie.Text;
                    KlientDoZamiany.Nazwisko = textBoxNazwisko.Text;
                    KlientDoZamiany.Password = PasswordBoxHaslo.Password.ToString();
                    KlientDoZamiany.zaznaczony = false;


                    KlientDoZamiany.Telefon = intParse1;
                    KlientDoZamiany.Miasto = textBoxMiasto.Text;
                    KlientDoZamiany.Ulica = textBoxUlica.Text;

                    dc.SubmitChanges();


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
            //if ((ind.Count>=(i+1)))
            //{
            //    MessageBoxResult result = MessageBox.Show("Chcesz edytować inne zaznaczone rekordy?", "", MessageBoxButton.YesNo);
            //    switch (result)
            //    {
            //        case MessageBoxResult.Yes:
            //            {
            //                i++;
            //                connection.Open();
            //                OleDbCommand command = new OleDbCommand();
            //                command.Connection = connection;
            //                command.CommandText = "select [Imię],[Nazwisko],[Password],[Telefon],[Miasto],[Ulica] from Klienci where [Id klienta]=" + ind[i] + "";
            //                OleDbDataReader reader = command.ExecuteReader();
            //                reader.Read();
            //                textBoxImie.Text = reader[0].ToString();
            //                textBoxNazwisko.Text = reader[1].ToString();
            //                PasswordBoxHaslo.Password = reader[2].ToString();
            //                textBoxTelefon.Text = reader[3].ToString();
            //                textBoxMiasto.Text = reader[4].ToString();
            //                textBoxUlica.Text = reader[5].ToString();
            //                connection.Close();
            //                break;
            //            }
                        
            //        case MessageBoxResult.No:
            //            {
            //                this.Close();
            //                break;
            //            }
                   
            //    }
            //}
            //else
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
