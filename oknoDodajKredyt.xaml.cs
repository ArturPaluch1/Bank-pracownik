using Bank;
using System;
using System.Collections.Generic;
using System.Data.Linq;
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
    public partial class oknoDodajKredyt : Window
    {
        Window okno;
        
        LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);
        private List<Klienci> listaKlientow = new List<Klienci>();
        private List<Kredyty> listaKredytow = new List<Kredyty>();
        private List<Rodzaje_kredytów> listaRodzajowKredytow = new List<Rodzaje_kredytów>();
        List<int> indeksy = new List<int>();
        List<int> indeksy2 = new List<int>();
        int IdPracownika;
        public oknoDodajKredyt(Window okno, int IdPracownika)
        {
            InitializeComponent();
            this.okno = okno;
            wczytajBaze("Klienci");
            wczytajBaze("RodzajeKredytow");
            this.IdPracownika = IdPracownika;

        }


        public void wczytajBaze(string tabela)
        {
           
            switch (tabela)
            {
                case "Klienci":
                    {
                        dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Klienci);
                        var zapytanie = dc.ExecuteQuery<Klienci>("select [Id klienta],[aktywny],[imię] ,nazwisko,[telefon],[miasto], ulica,[środki]from [klienci]");
                        listaKlientow.Clear();




                        foreach (Klienci item in zapytanie)
                        {
                            if (item.aktywny.Equals(true))
                            {
                                item.zaznaczony = false;
                                listaKlientow.Add(item);
                            }
                        }
                        dataGridKlienci.ItemsSource = null;
                        dataGridKlienci.ItemsSource = listaKlientow;



                        break;
                    }
                case "RodzajeKredytow":
                    {
                        dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Rodzaje_kredytów);
                        var zapytanie = dc.ExecuteQuery<Rodzaje_kredytów>("select [Id rodzaju kredytu],[aktywny],[Nazwa],[Oprocentowanie],[Okres kredytowania(w mies.)],[Prowizja],[Kredyt utworzył] from [Rodzaje kredytów]");
                        listaRodzajowKredytow.Clear();

                        foreach (Rodzaje_kredytów item in zapytanie)
                        {
                            if (item.aktywny.Equals(true))
                            {
                                item.zaznaczony = false;
                                listaRodzajowKredytow.Add(item);
                            }

                        }
                        dataGridRodzajeKredytow.ItemsSource = null;
                        dataGridRodzajeKredytow.ItemsSource = listaRodzajowKredytow;

                       
                        break;
                    }
            }

        }

        private List<int> znajdzZaznaczenie(String tabela) // sprawdza czy zaznaczono pojedynczy rekord, zwraca listę z indeksem
        {
            int i = 0;
            List<int> indeksy = new List<int>();



            switch (tabela)
            {
                case "Klienci":
                    {
                        Klienci temp = new Klienci();
                       
                        foreach (var item in listaKlientow)
                        {

                            if (listaKlientow[i].zaznaczony == true)
                            {

                                indeksy.Add(listaKlientow[i].Id_klienta);
                            }


                            i++;
                        }
                        return indeksy;
                    }
                case "RodzajeKredytow":
                    {
                        Rodzaje_kredytów temp = new Rodzaje_kredytów();
                       
                        foreach (var item in listaRodzajowKredytow)
                        {

                            if (listaRodzajowKredytow[i].zaznaczony == true)
                            {

                                indeksy.Add(listaRodzajowKredytow[i].Id_rodzaju_kredytu);
                            }


                            i++;
                        }
                        return indeksy;
                    }
                default:
                    {
                        return indeksy;
                    }

            }
        }
            private void dodaj_Click(object sender, RoutedEventArgs e)
        {

            DateTime tempData = DateTime.Now;
         

            indeksy = znajdzZaznaczenie("Klienci");
            indeksy2 = znajdzZaznaczenie("RodzajeKredytow");
          
            if (indeksy.Count().Equals(0)|| indeksy2.Count().Equals(0))
            {
                MessageBox.Show("Musisz wybrać klienta i rodzaj kredytu");
            }
            else if (indeksy.Count().Equals(2)|| indeksy.Count().Equals(2))
            {
                MessageBox.Show("Wybierz jednego klienta i jeden rodzaj kredytu");
            }
            else
            {
                var zapytanie =
from c in dc.Klienci
where c.Id_klienta == indeksy[0]
select c;


                decimal decimalParse1;
                if (!decimal.TryParse(textBoxKwotaKredytu.Text, out decimalParse1))
                {

                    MessageBox.Show("Błędna kwota kredytu. Musi być liczba.");
                    textBoxKwotaKredytu.Text = "";
                }
               else
                {
                    if(int.Parse(textBoxKwotaKredytu.Text)<1000)
                    {
                        MessageBox.Show("Za niska kwota kredytu.");
                    }
                      else
                {
                        Kredyty bob = new Kredyty();
                    
                        bob.Klient = indeksy[0];
                        bob.Kwota_kredytu = decimalParse1;
                        bob.Rodzaj_kredytu = indeksy2[0];
                        bob.Kredytu_udzielił = IdPracownika;
                        bob.zaznaczony = false;
                        bob.Data_założenia = tempData;
                        bob.aktywny = true;
                       


                        dc.Kredyty.InsertOnSubmit(bob);
                        dc.SubmitChanges();


                        MessageBox.Show("Dodawanie powiodło się.");

                        this.Close();

                    }
                }
              
            }
        
        }

        private void anuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            if (checkBox.IsChecked == true)
            {
            }
        }


    }
}