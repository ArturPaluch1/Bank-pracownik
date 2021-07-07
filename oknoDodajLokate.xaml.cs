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
    public partial class oknoDodajLokate : Window
    {
        Window okno;

        LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);
        private List<Klienci> listaKlientow = new List<Klienci>();
        private List<Lokaty> listaLokat = new List<Lokaty>();
        private List<Rodzaje_lokat> listaRodzajowLokat = new List<Rodzaje_lokat>();
        List<int> indeksy = new List<int>();
        List<int> indeksy2 = new List<int>();
        int idPracownika;
        public oknoDodajLokate(Window okno, int iDPracownika)
        {
            InitializeComponent();
            this.okno = okno;
            wczytajBaze("Klienci");
            wczytajBaze("RodzajeLokat");
            this.idPracownika = iDPracownika;

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
                        dataGridKlienci1.ItemsSource = null;
                        dataGridKlienci1.ItemsSource = listaKlientow;



                        break;
                    }
                case "RodzajeLokat":
                    {
                        dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Rodzaje_lokat);
                        var zapytanie = dc.ExecuteQuery<Rodzaje_lokat>("select [Id rodzaju lokaty],[aktywny],[Nazwa],[Oprocentowanie],[Okres(w mies.)],[Prowizja],[Lokatę utworzył] from [Rodzaje lokat]");
                        listaRodzajowLokat.Clear();

                        foreach (Rodzaje_lokat item in zapytanie)
                        {
                            if (item.aktywny.Equals(true))
                            {
                                item.zaznaczony = false;
                                listaRodzajowLokat.Add(item);
                            }
                            dataGridRodzajeLokat.ItemsSource = null;
                            dataGridRodzajeLokat.ItemsSource = listaRodzajowLokat;


                        }




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
                case "RodzajeLokat":
                    {
                        Rodzaje_lokat temp = new Rodzaje_lokat();
                        foreach (var item in listaRodzajowLokat)
                        {

                            if (listaRodzajowLokat[i].zaznaczony == true)
                            {

                                indeksy.Add(listaRodzajowLokat[i].Id_rodzaju_lokaty);
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
            indeksy2 = znajdzZaznaczenie("RodzajeLokat");
         
                if (indeksy.Count().Equals(0) || indeksy2.Count().Equals(0))
                {
                    MessageBox.Show("Musisz wybrać klienta i rodzaj lokaty");
                }
                else if (indeksy.Count().Equals(2) || indeksy2.Count().Equals(2))
                {
                    MessageBox.Show("Wybierz jednego klienta i jeden rodzaj lokaty.");
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

                        MessageBox.Show("Błędna wysokość lokaty. Musi być liczba.");
                        textBoxKwotaKredytu.Text = "";
                    }
                    else
                {
                    if (int.Parse(textBoxKwotaKredytu.Text) < 1000)
                    {
                        MessageBox.Show("Za niska kwota lokaty.");
                    }
                    else
                    {
                        Lokaty bob = new Lokaty();
                        bob.Klient = indeksy[0];
                        bob.Wysokość_lokaty = decimalParse1;
                        bob.Id_Rodzaju_lokaty = indeksy2[0];
                        bob.Lokaty_udzielił = idPracownika;
                        bob.Data_założenia = tempData;
                        bob.aktywny = true;
                        dc.Lokaty.InsertOnSubmit(bob);

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