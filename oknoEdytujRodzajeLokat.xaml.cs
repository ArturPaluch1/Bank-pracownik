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
    public partial class oknoEdytujRodzajeLokat : Window
    {
        List<int> ind;
        LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);

        public oknoEdytujRodzajeLokat(Window okno, List<int> indeksy)
        {
            
            ind = indeksy;


            InitializeComponent();
            var zapytanie =
    from c in dc.Rodzaje_lokat
  select new { c.Id_rodzaju_lokaty, c.Nazwa, c.Oprocentowanie, c.Okres_w_mies__, c.Prowizja };


            textBoxNazwa.Text = dc.Rodzaje_lokat.First(p => p.Id_rodzaju_lokaty == indeksy[0]).Nazwa;
            textBoxOprocentowanie.Text = dc.Rodzaje_lokat.First(p => p.Id_rodzaju_lokaty == indeksy[0]).Oprocentowanie.ToString();
            textBoxProwizja.Text = (dc.Rodzaje_lokat.First(p => p.Id_rodzaju_lokaty == indeksy[0]).Prowizja).ToString();
            textBoxOkres.Text = dc.Rodzaje_lokat.First(p => p.Id_rodzaju_lokaty == indeksy[0]).Okres_w_mies__.ToString();






        }
        private void edytuj_Click(object sender, RoutedEventArgs e)
        {




            if (textBoxNazwa.Text != "" && textBoxNazwa.Text != "" && textBoxOprocentowanie.Text != "Musisz uzupełnić" && textBoxOprocentowanie.Text != "Musisz uzupełnić" && textBoxOkres.Text != "" && textBoxOkres.Text != "Musisz uzupełnić" && textBoxProwizja.Text != "" && textBoxProwizja.Text != "Musisz uzupełnić")
            {
                if (
                     textBoxNazwa.Text.Length > 30 ||
                     textBoxOprocentowanie.Text.Length > 30
                 || textBoxOkres.Text.Length > 30
                 || textBoxProwizja.Text.Length > 30)
                {
                    MessageBox.Show("Podałeś za długie dane");
                }
                else
                {

                    var LokataDoZamiany = dc.Rodzaje_lokat.First(p => p.Id_rodzaju_lokaty == ind[0]);

                    LokataDoZamiany.Nazwa = textBoxNazwa.Text;

                  
                        LokataDoZamiany.Oprocentowanie = float.Parse(textBoxOprocentowanie.Text);
                        LokataDoZamiany.Okres_w_mies__ = int.Parse(textBoxOkres.Text);

                        LokataDoZamiany.Prowizja = float.Parse(textBoxProwizja.Text);
                        
                        dc.SubmitChanges();
                        this.Close();
                  
                 
                }
            }
            else
            {
                if (textBoxNazwa.Text == "")
                {
                    textBoxNazwa.Foreground = Brushes.Red;
                    textBoxNazwa.FontWeight = FontWeights.Bold;
                    textBoxNazwa.Text = "Musisz uzupełnić";
                }
                if (textBoxOprocentowanie.Text == "")
                {
                    textBoxOprocentowanie.Foreground = Brushes.Red;
                    textBoxOprocentowanie.FontWeight = FontWeights.Bold;
                    textBoxOprocentowanie.Text = "Musisz uzupełnić";
                }

                if (textBoxOkres.Text == "")
                {
                    textBoxOkres.Foreground = Brushes.Red;
                    textBoxOkres.FontWeight = FontWeights.Bold;
                    textBoxOkres.Text = "Musisz uzupełnić";
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

