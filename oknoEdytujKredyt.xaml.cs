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
    public partial class oknoEdytujKredyt : Window
    {
        List<int> ind;
        LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);
        private List<Kredyty> listaKredytow = new List<Kredyty>();
        private List<Rodzaje_kredytów> listaRodzajowKredytow = new List<Rodzaje_kredytów>();
        private List<Klienci> listaKlientow = new List<Klienci>();
        public oknoEdytujKredyt(Window okno, List<int> indeksy)
        {
            InitializeComponent();
            ind = indeksy;
            var zapytanie =
   from c in dc.Kredyty
   join a in dc.Rodzaje_kredytów on c.Rodzaj_kredytu equals a.Id_rodzaju_kredytu
   join b in dc.Klienci on c.Klient equals b.Id_klienta
  select new { c.Id_Kredytu, c.zaznaczony, c.Kwota_kredytu, b.Id_klienta, a.Id_rodzaju_kredytu };
            
            textBoxKwotaKredytu.Text = dc.Kredyty.First(p => p.Id_Kredytu == indeksy[0]).Kwota_kredytu.ToString();


            wczytajBaze("Klienci");
            wczytajBaze("RodzajeKredytow");
            Klienci bob = new Klienci();

            foreach (Klienci item in listaKlientow)
            {
                if (item.Id_klienta== dc.Kredyty.First(p => p.Id_Kredytu == indeksy[0]).Klient)
                {
                    item.zaznaczony = true;
                }
            }
            dataGridKlienci.ItemsSource = null;
            dataGridKlienci.ItemsSource = listaKlientow;

            Rodzaje_kredytów bob1 = new Rodzaje_kredytów();
            foreach (Rodzaje_kredytów item in listaRodzajowKredytow)
            {
                if (item.Id_rodzaju_kredytu == dc.Kredyty.First(p => p.Id_Kredytu == indeksy[0]).Rodzaj_kredytu)
                {
                    item.zaznaczony = true;
                }
            }


            dataGridRodzajeKredytow.ItemsSource = null;
            dataGridRodzajeKredytow.ItemsSource = listaRodzajowKredytow;

          

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
                        var zapytanie = dc.ExecuteQuery<Rodzaje_kredytów>("select [Id rodzaju kredytu],[Nazwa],[Oprocentowanie],[Okres kredytowania(w mies.)],[Prowizja],[Kredyt utworzył] from [Rodzaje kredytów]");
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

        private void edytuj_Click(object sender, RoutedEventArgs e)
        {
            List<int> idxKlienta = znajdzZaznaczenie("Klienci");
            List<int> idxRodzajuKredytu = znajdzZaznaczenie("RodzajeKredytow");
            if (idxKlienta.Count() != 1 || idxRodzajuKredytu.Count()!=1)
            {
                MessageBox.Show("Wybierz po jednym kliencie i rodzaju kredytu.");
            }
            else
            {
               
                decimal decimalParse1;
                if (!decimal.TryParse(textBoxKwotaKredytu.Text, out decimalParse1) || textBoxKwotaKredytu.Text.Length > 30)
                {

                    MessageBox.Show("Błędna kwota kredytu. Musi być liczba.");
                    textBoxKwotaKredytu.Text = "";
                }

                if (textBoxKwotaKredytu.Text != "" && textBoxKwotaKredytu.Text != "Musisz uzupełnić")
                {
                    Kredyty KredytDoZamiany = dc.Kredyty.First(p => p.Id_Kredytu == ind[0]);



                    KredytDoZamiany.Kwota_kredytu = decimalParse1;
                    KredytDoZamiany.Klient = idxKlienta[0];
                    KredytDoZamiany.Rodzaj_kredytu = idxRodzajuKredytu[0];
                    dc.SubmitChanges();
                    MessageBox.Show("Edycja powiodła się.");
                    this.Close();
                }
                else
                {
               
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
               
               if (dataGridKlienci.ItemsSource != null)
                {
                    for (int i = 0; i < listaKlientow.Count(); i++)
                    {
                        listaKlientow[i].zaznaczony = true;
                    }

                    dataGridKlienci.ItemsSource = null;
                    dataGridKlienci.ItemsSource = listaKlientow;
                    dataGridKlienci.Items.Refresh();
                }
               
               if (dataGridRodzajeKredytow.ItemsSource != null)
                {
                    for (int i = 0; i < listaRodzajowKredytow.Count(); i++)
                    {
                        listaRodzajowKredytow[i].zaznaczony = false;
                    }
                    dataGridRodzajeKredytow.ItemsSource = null;
                    dataGridRodzajeKredytow.ItemsSource = listaRodzajowKredytow;
                    dataGridRodzajeKredytow.Items.Refresh();
                }

            }

        }





    }
}
