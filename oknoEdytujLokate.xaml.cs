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
    public partial class oknoEdytujLokate : Window
    {
        List<int> ind;
        LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);
        
        private List<Rodzaje_lokat> listaRodzajowLokat = new List<Rodzaje_lokat>();
        private List<Klienci> listaKlientow = new List<Klienci>();
        public oknoEdytujLokate(Window okno, List<int> indeksy)
        {
            InitializeComponent();
            ind = indeksy;
            wczytajBaze("Klienci");
            wczytajBaze("RodzajeLokat");

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
                        }
                        dataGridRodzajeLokat.ItemsSource = null;
                        dataGridRodzajeLokat.ItemsSource = listaRodzajowLokat;


                    }
                    break;
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

        private void edytuj_Click(object sender, RoutedEventArgs e)
        {


            List<int> idxKlienta = znajdzZaznaczenie("Klienci");
            List<int> idxRodzajuLokaty = znajdzZaznaczenie("RodzajeLokat");
            if (idxKlienta.Count() != 1 || idxRodzajuLokaty.Count() != 1)
            {
                MessageBox.Show("Wybierz po jednym kliencie i rodzaju lokaty.");
            }
            else
            {
                if (textBoxKwotaLokaty.Text != "" && textBoxKwotaLokaty.Text != "Musisz uzupełnić" || textBoxKwotaLokaty.Text.Length > 30)
                {
                    Lokaty LokataDoZamiany = dc.Lokaty.First(p => p.Id_Lokaty == ind[0]);
                    LokataDoZamiany.Wysokość_lokaty = Decimal.Parse(textBoxKwotaLokaty.Text);
                    LokataDoZamiany.Klient = idxKlienta[0];
                    LokataDoZamiany.Id_Rodzaju_lokaty = idxRodzajuLokaty[0];
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

                if (dataGridRodzajeLokat.ItemsSource != null)
                {
                    for (int i = 0; i < listaRodzajowLokat.Count(); i++)
                    {
                        listaRodzajowLokat[i].zaznaczony = false;
                    }
                    dataGridRodzajeLokat.ItemsSource = null;
                    dataGridRodzajeLokat.ItemsSource = listaRodzajowLokat;
                    dataGridRodzajeLokat.Items.Refresh();
                }

            }

        }



    }
}
