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
    public partial class oknoDodajRodzajLokaty : Window
    {
        List<Klienci> listaKlientow;
        int pracownik;
        public oknoDodajRodzajLokaty(Window okno, int IdPracownika)
        {
            listaKlientow = new List<Klienci>();
            InitializeComponent();
            pracownik = IdPracownika;
        }
        private void dodaj_Click(object sender, RoutedEventArgs e)
        {

            LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);


            float floatParse1;
            if (!float.TryParse(textBoxOprocentowanie.Text, out floatParse1) || textBoxOprocentowanie.Text.Length > 30)
            {

                MessageBox.Show("Błędna wartość oprocentowania. Musi być liczba.");
                textBoxOprocentowanie.Text = "";
            }
            float floatParse2;
            if (!float.TryParse(textBoxProwizja.Text, out floatParse2) || textBoxProwizja.Text.Length > 30)
            {

                MessageBox.Show("Błędna wartość prowizji. Musi być liczba.");
                textBoxProwizja.Text = "";
            }
            int intParse1;
            if (!int.TryParse(textBoxOkres.Text, out intParse1) || textBoxOkres.Text.Length > 30)
            {

                MessageBox.Show("Błędny okres. Musi być liczba.");
                textBoxOkres.Text = "";
            }

            if (textBoxNazwa.Text != "" && textBoxNazwa.Text != "" && textBoxOprocentowanie.Text != "Musisz uzupełnić" && textBoxOprocentowanie.Text != "Musisz uzupełnić" && textBoxOkres.Text != "" && textBoxOkres.Text != "Musisz uzupełnić" && textBoxProwizja.Text != "" && textBoxProwizja.Text != "Musisz uzupełnić")
            {
                Rodzaje_lokat bob = new Rodzaje_lokat();
                bob.Nazwa = textBoxNazwa.Text;
                bob.Oprocentowanie = floatParse1;
                bob.Okres_w_mies__ = intParse1;
                bob.Prowizja = floatParse2;
                bob.Lokatę_utworzył = pracownik;
                bob.aktywny = true;
                bob.zaznaczony = false;
                
                dc.Rodzaje_lokat.InsertOnSubmit(bob);
                dc.SubmitChanges();
                
                MessageBox.Show("Dodawanie powiodło się.");

                this.Close();

            }

            else
            {
                if (textBoxNazwa.Text == "")
                {
                    textBoxNazwa.Foreground = Brushes.Red;
                    textBoxNazwa.FontWeight = FontWeights.Bold;
                    textBoxNazwa.Text = "Musisz uzupełnić";
                }
                if (textBoxOkres.Text == "")
                {
                    textBoxOkres.Foreground = Brushes.Red;
                    textBoxOkres.FontWeight = FontWeights.Bold;
                    textBoxOkres.Text = "Musisz uzupełnić";
                }
                if (textBoxOprocentowanie.Text == "")
                {
                    textBoxOprocentowanie.Foreground = Brushes.Red;
                    textBoxOprocentowanie.FontWeight = FontWeights.Bold;
                    textBoxOprocentowanie.Text = "Musisz uzupełnić";
                }
                if (textBoxProwizja.Text == "")
                {
                    textBoxProwizja.Foreground = Brushes.Red;
                    textBoxProwizja.FontWeight = FontWeights.Bold;
                    textBoxProwizja.Text = "Musisz uzupełnić";
                }

            }
            

        }

        private void anuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void zaznaczonyTextboxNazwa(object sender, RoutedEventArgs e)
        {
            textBoxNazwa.Foreground = Brushes.Black;
            textBoxNazwa.FontWeight = FontWeights.Normal;
            textBoxNazwa.Text = "";
        }
        private void zaznaczonyTextboxOprocentowanie(object sender, RoutedEventArgs e)
        {
            textBoxOprocentowanie.Foreground = Brushes.Black;
            textBoxOprocentowanie.FontWeight = FontWeights.Normal;
            textBoxOprocentowanie.Text = "";
        }

        private void zaznaczonyTextboxOkres(object sender, RoutedEventArgs e)
        {
            textBoxOkres.Foreground = Brushes.Black;
            textBoxOkres.FontWeight = FontWeights.Normal;
            textBoxOkres.Text = "";
        }

        private void zaznaczonyTextboxProwizja(object sender, RoutedEventArgs e)
        {
            textBoxProwizja.Foreground = Brushes.Black;
            textBoxProwizja.FontWeight = FontWeights.Normal;
            textBoxProwizja.Text = "";
        }


    }






}

