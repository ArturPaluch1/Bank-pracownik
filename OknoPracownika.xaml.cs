using Bank;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.OleDb;
using System.Data.SqlClient;
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

    //.\SQLEXPRESS
    
   
    public partial class OknoPracownika : Window
    {
        LINQBazaBankDataContext dc = new LINQBazaBankDataContext(Bank.Properties.Settings.Default.BankConnectionString);
    
        private List<Klienci> listaKlientow = new List<Klienci>();
        private List<Kredyty> listaKredytow = new List<Kredyty>();
        private List<Lokaty> listaLokat = new List<Lokaty>();
        private List<Przelewy> listaPrzelewow = new List<Przelewy>();
        private List<Rodzaje_kredytów> listaRodzajowKredytow = new List<Rodzaje_kredytów>();
        private List<Rodzaje_lokat> listaRodzajowLokat = new List<Rodzaje_lokat>();
        private List<Pracownicy> listaPracownikow = new List<Pracownicy>();
    
        int idPracownika;
        
        private List<int> IdZaznaczenia = new List<int>();
        public OknoPracownika(int idPracownika1)
        {
            InitializeComponent();
            idPracownika = idPracownika1;
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
                                if(listaKlientow[i].aktywny==false)
                                {

                                }
                                else
                                indeksy.Add(listaKlientow[i].Id_klienta);
                            }


                            i++;
                        }
                        return indeksy;
                    }
                case "Kredyty":
                    {
                        Kredyty temp = new Kredyty();
                        
                        foreach (var item in listaKredytow)
                        {

                            if ((listaKredytow[i].zaznaczony == true) &&(listaKredytow[i].aktywny!=false))
                            {
                             
                                    indeksy.Add(listaKredytow[i].Id_Kredytu);
                            }


                            i++;
                        }
                        return indeksy;
                    }
                case "Lokaty":
                    {
                        Lokaty temp = new Lokaty();  
                        foreach (var item in listaLokat)
                        {

                            if (listaLokat[i].zaznaczony == true)
                            {
                                if (listaLokat[i].aktywny == false)
                                { }
                                
                                else
                                    indeksy.Add(listaLokat[i].Id_Lokaty);
                            }


                            i++;
                        }
                        return indeksy;
                    }
                case "Przelewy":
                    {
                        Przelewy temp = new Przelewy();
                        foreach (var item in listaPrzelewow)
                        {

                            if (listaPrzelewow[i].zaznaczony == true)
                            {

                                indeksy.Add(listaPrzelewow[i].Id_Przelewu);
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
                case "Pracownicy":
                    {
                        Pracownicy temp = new Pracownicy();
                       
                        foreach (var item in listaPracownikow)
                        {

                            if (listaPracownikow[i].zaznaczony == true)
                            {
                                if (listaPracownikow[i].aktywny == false)
                                {

                                }
                                else
                                    indeksy.Add(listaPracownikow[i].Id_Pracownika);
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
        public void wczytajBaze(string tabela)
        {
       switch(tabela)
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
                case "Pracownicy":
                    {
                        dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Pracownicy);
                        try
                        {
                            var zapytanie =
 from c in dc.Pracownicy
 select c;

                            listaPracownikow.Clear();
                            foreach (Pracownicy item in zapytanie)
                            {
                                if (item.aktywny.Equals(true))
                                {
                                    item.zaznaczony = false;
                                    listaPracownikow.Add(item);
                                }
                            }
                           
                            dataGridPracownicy.ItemsSource = null;
                            dataGridPracownicy.ItemsSource = listaPracownikow;

                        }
                        catch
                        {
               
                            dataGridPracownicy.Visibility = Visibility.Hidden;
                            MessageBox.Show("Tabela jest pusta.");
                        }
                        break;
                    }
                case "Kredyty":
                    {
                        dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Kredyty);
                        var zapytanie = dc.ExecuteQuery<Kredyty>("select [Id kredytu],[aktywny],[kwota kredytu],[Data założenia] ,klient,[rodzaj kredytu],[kredytu udzielił]from [kredyty]");
                        listaKredytow.Clear();

                        


                        foreach (Kredyty item in zapytanie)
                        {
                            if (item.aktywny.Equals(true))
                            {

                                item.zaznaczony = false;
                                listaKredytow.Add(item);
                            }
                        }
                        dataGridKlienciKredyt.ItemsSource = null;
                        dataGridKlienciKredyt.ItemsSource = listaKredytow;

                        break;
                    }

                case "Lokaty":
                    {
                        dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Lokaty);
                        listaLokat.Clear();
                        var zapytanie = dc.ExecuteQuery<Lokaty>("select [Id lokaty],[aktywny],[wysokość lokaty],[Data założenia] ,klient,[id rodzaju lokaty],[lokaty udzielił]from [lokaty]");
                                                    

                        foreach (Lokaty item in zapytanie)
                        {
                            if (item.aktywny.Equals(true))
                            {
                                item.zaznaczony = false;
                                listaLokat.Add(item);
                            }
                        }
                        dataGridKlienciLokata.ItemsSource = null;
                        dataGridKlienciLokata.ItemsSource = listaLokat;

                        break;
                    }
                case "Przelewy":
                    {
                        dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Przelewy);
                        listaPrzelewow.Clear();
                        var zapytanie = dc.ExecuteQuery<Przelewy>("select [Id Przelewu],[Kwota],[Nazwa odbiorcy],[Nadawca],[Tytuł przelewu] from Przelewy");

                     
                        foreach (Przelewy item in zapytanie)
                        {
                            item.zaznaczony = false;
                            listaPrzelewow.Add(item);
                        }
                          
                        
                        dataGridKlienciPrzelew.ItemsSource = null;
dataGridKlienciPrzelew.ItemsSource = listaPrzelewow;
                        

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
                case "RodzajeLokat":
                    {
                        dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Rodzaje_lokat);
                        var zapytanie = dc.ExecuteQuery<Rodzaje_lokat>("select [Id rodzaju lokaty],[aktywny],[Nazwa],[Oprocentowanie],[Okres(w mies.)],[Prowizja],[Lokatę utworzył] from [Rodzaje lokat]");
                        listaRodzajowLokat.Clear();

                        foreach (Rodzaje_lokat item in zapytanie)
                        {
                            if(item.aktywny.Equals(true))
                            {
                                item.zaznaczony = false;
                                listaRodzajowLokat.Add(item);
                            }
                            dataGridRodzajeLokat.ItemsSource = null;
                            dataGridRodzajeLokat.ItemsSource = listaRodzajowLokat;


                        }
                        
                        break;
                    }



                case "UsunieciKlienci":
                    {
                        dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Klienci);
                        var zapytanie = dc.ExecuteQuery<Klienci>("select [Id klienta],[aktywny],[imię] ,nazwisko,[telefon],[miasto], ulica,[środki]from [klienci]");
                        listaKlientow.Clear();




                        foreach (Klienci item in zapytanie)
                        {
                            if (item.aktywny.Equals(false))
                            {

                                listaKlientow.Add(item);
                            }
                        }
                        dataGridKlienci.ItemsSource = null;
                        dataGridKlienci.ItemsSource = listaKlientow;



                        break;
                    }
                case "UsunieciPracownicy":
                    {
                        dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Pracownicy);
                        try
                        {
                            var zapytanie =
 from c in dc.Pracownicy
 select c;
                            listaPracownikow.Clear();

                  
                            foreach (Pracownicy item in zapytanie)
                            {
                                if (item.aktywny.Equals(false))
                                {
                                    listaPracownikow.Add(item);
                                }
                            }

                            dataGridPracownicy.ItemsSource = null;
                            dataGridPracownicy.ItemsSource = listaPracownikow;

                        }
                        catch
                        {
                          
                            dataGridPracownicy.Visibility = Visibility.Hidden;
                            MessageBox.Show("Tabela jest pusta.");
                        }
                        break;
                    }
                case "UsunieteKredyty":
                    {
                        dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Kredyty);
                        var zapytanie = dc.ExecuteQuery<Kredyty>("select [Id kredytu],[aktywny],[kwota kredytu],[Data założenia] ,klient,[rodzaj kredytu],[kredytu udzielił]from [kredyty]");
                        listaKredytow.Clear();

                        


                        foreach (Kredyty item in zapytanie)
                        {
                            if (item.aktywny.Equals(false))
                            {

                              
                                listaKredytow.Add(item);
                            }
                        }
                        dataGridKlienciKredyt.ItemsSource = null;
                        dataGridKlienciKredyt.ItemsSource = listaKredytow;

                        



                        break;
                    }

                case "UsunieteLokaty":
                    {
                        dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Lokaty);
                        listaLokat.Clear();
                        var zapytanie = dc.ExecuteQuery<Lokaty>("select [Id lokaty],[aktywny],[wysokość lokaty],[Data założenia] ,klient,[id rodzaju lokaty],[lokaty udzielił]from [lokaty]");


                        foreach (Lokaty item in zapytanie)
                        {
                            if (item.aktywny.Equals(false))
                            {
                             
                                listaLokat.Add(item);
                            }
                        }
                        dataGridKlienciLokata.ItemsSource = null;
                        dataGridKlienciLokata.ItemsSource = listaLokat;
                        
                        break;
                    }

            }


        }

     
        public void oknoClosed(object sender, System.EventArgs e)
        {
            switch((sender as Window).Name)
            {
                case "NameoknododajKlienta":
                    {
                        if (buttonKlienciWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("Klienci");
                        }
                        break;
                    }
                case "Nameokno_DodajKredyt":
                    {
                        if (buttonKlienciKredytWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("Kredyty");
                        }
                        break;
                    }
                case "Nameokno_DodajLokate":
                    {
                        if (buttonKlienciLokataWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("Lokaty");
                        }
                        break;
                    }
                case "NameOknoDodajPrzelew":
                    {
                        if (buttonKlienciPrzelewWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("Przelewy");
                        }
                        break;
                    }
                /////////
                case "NameoknoEdytujKlienta":
                    {
                        if (buttonKlienciWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("Klienci");
                        }
                        break;
                    }
                case "NameoknoEdytujKredytj":
                    {
                        if (buttonKlienciKredytWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("Kredyty");
                        }
                        break;
                    }
                case "NameoknoEdytujLokate":
                    {
                        if (buttonKlienciLokataWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("Lokaty");
                        }
                        break;
                    }
                case "NameOknoDodajPracownika":
                    {
                        if (buttonPracownicyWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("Pracownicy");
                        }
                        break;
                    }
                case "NameOknoEdytujPracownika":
                    {
                        if (buttonPracownicyWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("Pracownicy");
                        }
                        break;
                    }
                case "NameOknoDodajRodzajKredytu":
                    {
                        if (buttonKredytKredytWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("RodzajeKredytow");
                        }
                        break;
                    }
           
                case "NameoknoEdytujRodzajeLokat":
                    {
                        if (buttonLokataLokataWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("RodzajeLokat");
                        }
                        break;
                    }

                case "NameoknoDodajRodzajLokaty":
                    {
                        if (buttonLokataLokataWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("RodzajeLokat");
                        }
                        break;
                    }
                case "NameoknoEdytujRodzajKredytu":
                    {
                        if (buttonKredytKredytWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("RodzajeKredytow");
                        }
                        break;
                    }
        
                case "NameoknoEdytujPracownika":
                    {
                        if (buttonPracownicyWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("Pracownicy");
                        }
                        break;
                    }
                case "NameOknoDodajWplata":
                    {
                        if (buttonKlienciPrzelewWyswietl.Content.Equals("Schowaj"))
                        {

                            wczytajBaze("Przelewy");
                        }
                        break;
                    }



            }

            
        }


        private void button_Click_wyloguj(object sender, RoutedEventArgs e)
        {
            MainWindow okno = new MainWindow();
            okno.Show();
            this.Close();

        }

            
        private void button_Click_wyjdz(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ZablokujPrzyciski(String rodzajElementu)
        {
            List<FrameworkElement> logicalElements1 = new List<FrameworkElement>();
            logicalElements1=GetLogicalElements(OknoPracownikaNazwa, logicalElements1);
            if (logicalElements1.Count!=0)
            foreach (FrameworkElement element in logicalElements1)
            {
                    if (element.Tag == null)
                    {
                        
                    }
                    else
                    {
                        
                        //blokuje ktore nie są w stringu
                        if (!element.Tag.Equals(rodzajElementu))
                        {
                            element.IsEnabled = false;
                            if(element.Tag.Equals("TabKlienci"))
                            {
                                element.IsEnabled = true;
                            }
                        }
                          
                        else
                        {
                            
                          
                        }
                    }
                }
        }
        private void OdblokujPrzyciski(String rodzajElementu)
        {
            List<FrameworkElement> logicalElements1 = new List<FrameworkElement>();
            logicalElements1 = GetLogicalElements(OknoPracownikaNazwa, logicalElements1);
            if (logicalElements1.Count != 0)
                foreach (FrameworkElement element in logicalElements1)
                {
                    if (element.Tag == null)
                    {

                    }
                    else
                    {// odblokowywuje te ktore nie sa w stringu
                        if (!element.Tag.Equals(rodzajElementu))
                        {
                           
                           
                                element.IsEnabled = true;
                        }
                           
                        else
                        {
                            

                        }

                    }
                }
        }
       
        private List<FrameworkElement> GetLogicalElements(object parent, List<FrameworkElement> logicalElements)
        {
            if (parent == null) return logicalElements;

            if (parent.GetType().IsSubclassOf(typeof(FrameworkElement)))
                logicalElements.Add((FrameworkElement)parent);

            DependencyObject doParent = parent as DependencyObject;

            if (doParent == null) return logicalElements;

            foreach (object child in LogicalTreeHelper.GetChildren(doParent))
            {
                GetLogicalElements(child, logicalElements);
            }
            return logicalElements;

        }

      



        private void Click_buttonKredytKredytWyswietl(object sender, RoutedEventArgs e)
        {
            if (buttonKredytKredytWyswietl.Content.Equals("Wyświetl"))
            {
                buttonKredytKredytWyswietl.Content = "Schowaj";

               TabelaRodzajowKredytow.Visibility = Visibility.Visible;
                dataGridRodzajeKredytow.Visibility = Visibility.Visible;
           
                wczytajBaze("RodzajeKredytow");
            }


            else
            {
                buttonKlienciHistoria.IsEnabled = true;
                buttonKredytKredytWyswietl.Content = "Wyświetl";
                dataGridRodzajeKredytow.Visibility = Visibility.Hidden;
                TabelaKlienta.Visibility = Visibility.Hidden;
         
                dataGridRodzajeKredytow.ItemsSource = null;
              
                TabelaRodzajowKredytow.Visibility = Visibility.Hidden;
              
            }

        }


        private void Click_buttonPracownicyWyswietl(object sender, RoutedEventArgs e)
        {
            if (buttonPracownicyWyswietl.Content.Equals("Wyświetl"))
            {
                buttonPracownicyWyswietl.Content = "Schowaj";

               TabelaPracownikow.Visibility = Visibility.Visible;
                dataGridPracownicy.Visibility = Visibility.Visible;
         
                wczytajBaze("Pracownicy");
            }


            else
            {
       
                buttonPracownicyWyswietl.Content = "Wyświetl";
                dataGridPracownicy.Visibility = Visibility.Hidden;
           TabelaPracownikow.Visibility = Visibility.Hidden;
        

                dataGridPracownicy.ItemsSource = null;
             
            }


        }

      

        private void Click_buttonKlienciPrzelewWyswietl(object sender, RoutedEventArgs e)
        {
            if (buttonKlienciPrzelewWyswietl.Content.Equals("Wyświetl"))
            {
                TabelaPrzelewow.Visibility = Visibility.Visible;
                buttonKlienciPrzelewWyswietl.Content = "Schowaj";
                dataGridKlienciPrzelew.Visibility = Visibility.Visible;
                ZablokujPrzyciski("Przelew");
                wczytajBaze("Przelewy");
            }
            else
            {
                TabelaPrzelewow.Visibility = Visibility.Hidden;
                buttonKlienciPrzelewWyswietl.Content = "Wyświetl";
                dataGridKlienciPrzelew.Visibility = Visibility.Hidden;
                OdblokujPrzyciski("Przelew");
                dataGridKlienciPrzelew.ItemsSource = null;
            }
        }


        
        private void Click_buttonKlienciUsun(object sender, RoutedEventArgs e)
        {
            List<int> indeksy = new List<int>();

            switch ((sender as Button).Name)
            {

                case "buttonKlienciUsun":
                    {

                        indeksy = znajdzZaznaczenie("Klienci");
                        if (indeksy.Count() > 0)
                        {


                            for (int i = 0; i < indeksy.Count(); i++)
                            {
                                
                                    var kasujWKlientach =
               from Klienci in dc.Klienci
               where Klienci.Id_klienta == indeksy[i]
               select Klienci;
                                    foreach (var item in kasujWKlientach)
                                    {
                                        dc.Refresh(System.Data.Linq.RefreshMode.KeepChanges, item);

                                        item.aktywny = false;
                                        dc.SubmitChanges();
                                      
                                    }


                                var kasujWKredytach =
          from Kredyty in dc.Kredyty
          where Kredyty.Klient == indeksy[i]
          select Kredyty;
                                foreach (var item1 in kasujWKredytach)
                                {
                                    item1.aktywny = false;
                                    dc.SubmitChanges();

                                }



                                var kasujWLokatach =
                               from Lokaty in dc.Lokaty
                               where Lokaty.Klient == indeksy[i]
                               select Lokaty;
                                foreach (var item2 in kasujWLokatach)
                                {
                         
                                    item2.aktywny = false;
                                    dc.SubmitChanges();

                                }
            wczytajBaze("Klienci"); 
                            }
                                }
                                break;
                    }
                case "buttonKlienciKredytUsun":
                    {
                     
                        indeksy = znajdzZaznaczenie("Kredyty");
                        if (indeksy.Count() > 0)
                        {
                            if (listaKredytow[indeksy[0]].aktywny == false)
                            {
                                MessageBox.Show("Nie można edytować usuniętych kredytów");
                                break;
                            }
                            else
                            {
                                for (int i = 0; i < indeksy.Count(); i++)
                                {



                                    var kasujWKredytach =
                                        from Kredyty in dc.Kredyty
                                        where Kredyty.Id_Kredytu == indeksy[i]
                                        select Kredyty;
                                    foreach (var item in kasujWKredytach)
                                    {
                                        dc.Refresh(System.Data.Linq.RefreshMode.KeepChanges, item);
                                        item.aktywny = false;
                                        dc.SubmitChanges();
                                        wczytajBaze("Kredyty");
                                    }

                                }
                            }



                        }
                        break;

                      
                    }
                case "buttonKlienciLokataUsun":
                    {
                      

                        indeksy = znajdzZaznaczenie("Lokaty");
                        if (indeksy.Count() > 0)
                        {
                            if (listaLokat[indeksy[0]].aktywny == false)
                            {
                                MessageBox.Show("Nie można edytować usuniętych lokat");
                                break;
                            }
                            else
                            {
                                for (int i = 0; i < indeksy.Count(); i++)
                                {



                                    var kasujWLokatach =
                                        from Lokaty in dc.Lokaty
                                        where Lokaty.Id_Lokaty == indeksy[i]
                                        select Lokaty;
                                    foreach (var item in kasujWLokatach)
                                    {
                                        dc.Refresh(System.Data.Linq.RefreshMode.KeepChanges, item);
                                        item.aktywny = false;
                                        dc.SubmitChanges();
                                        wczytajBaze("Lokaty");
                                    }



                                }
                            }
                          



                        }

                        break;

                       
                    }
                case "buttonPracownicyUsun":
                    {
                      

                        indeksy = znajdzZaznaczenie("Pracownicy");
                        if (indeksy[0].Equals(idPracownika))
                        {
                            MessageBox.Show("Nie możesz usunąć sam siebie!");
                            break;

                        }
                        else
                        {
                            if (indeksy.Count() > 0)
                            {
                                if (listaPracownikow[indeksy[0]].aktywny == false)
                                {
                                    MessageBox.Show("Nie można edytować usuniętych klientów");
                                    break;
                                }
                                else
                                {
                                    for (int i = 0; i < indeksy.Count(); i++)
                                    {

                                        var kasujWPracownikach =
                                         from Pracownicy in dc.Pracownicy
                                         where Pracownicy.Id_Pracownika == indeksy[i]
                                         select Pracownicy;
                                        foreach (var item in kasujWPracownikach)
                                        {
                                            dc.Refresh(System.Data.Linq.RefreshMode.KeepChanges, item);
                                            item.aktywny = false;
                                            dc.SubmitChanges();
                                            wczytajBaze("Pracownicy");
                                        }



                                    }
                                }

                              



                            }
                        }

                        break;
                      
                    }
                case "buttonKredytKredytUsun":
                    {
                      

                        indeksy = znajdzZaznaczenie("RodzajeKredytow");
                        if (indeksy.Count() > 0)
                        {

                            for (int i = 0; i < indeksy.Count(); i++)
                            {

                                var kasujWRodzajachKredytow =
                                    from Rodzaje_kredytów in dc.Rodzaje_kredytów
                                    where Rodzaje_kredytów.Id_rodzaju_kredytu == indeksy[i]
                                    select Rodzaje_kredytów;
                                foreach (var item in kasujWRodzajachKredytow)
                                {
                                    dc.Refresh(System.Data.Linq.RefreshMode.KeepChanges, item);

                                    item.aktywny = false;
                                    dc.SubmitChanges();
                                    wczytajBaze("RodzajeKredytow");
                                }

                            }
                            
                        }
                        break;

                       
                    }
                case "buttonLokataLokataUsun":
                    {
                       
                        indeksy = znajdzZaznaczenie("RodzajeLokat");
                        if (indeksy.Count() > 0)
                        {

                            for (int i = 0; i < indeksy.Count(); i++)
                            {

                                var kasujWRodzajachLokat =
                                   from Rodzaje_lokat in dc.Rodzaje_lokat
                                   where Rodzaje_lokat.Id_rodzaju_lokaty == indeksy[i]
                                   select Rodzaje_lokat;
                                foreach (var item in kasujWRodzajachLokat)
                                {
                                    dc.Refresh(System.Data.Linq.RefreshMode.KeepChanges, item);
                                    item.aktywny = false;
                                    dc.SubmitChanges();
                                    wczytajBaze("RodzajeLokat");
                                }

                                
                            }



                        }
                        break;

                     
                    }

            }

        }

        private void Click_buttonKlienciKredytWyswietl(object sender, RoutedEventArgs e)
        {
             if (buttonKlienciKredytWyswietl.Content.Equals("Wyświetl"))
            {
                TabelaKredytow.Visibility = Visibility.Visible;
                buttonKlienciKredytWyswietl.Content = "Schowaj";
                dataGridKlienciKredyt.Visibility = Visibility.Visible;
                ZablokujPrzyciski("Kredyt");
                wczytajBaze("Kredyty");
            }


            else
            {
                TabelaKredytow.Visibility = Visibility.Hidden;
                OdblokujPrzyciski("Kredyt");
                buttonKlienciKredytWyswietl.Content = "Wyświetl";
                dataGridKlienciKredyt.Visibility = Visibility.Hidden;
                dataGridKlienciKredyt.ItemsSource = null;
            }
      
        }


 


        private void Click_buttonKlienciLokataWyswietl(object sender, RoutedEventArgs e)
        {
            if (buttonKlienciLokataWyswietl.Content.Equals("Wyświetl"))
            {
                TabelaLokat.Visibility = Visibility.Visible;
                ZablokujPrzyciski("Lokata");
                buttonKlienciLokataWyswietl.Content = "Schowaj";
                dataGridKlienciLokata.Visibility = Visibility.Visible;
                wczytajBaze("Lokaty");
            }


            else
            {
                TabelaLokat.Visibility = Visibility.Hidden;
                buttonKlienciLokataWyswietl.Content = "Wyświetl";
                dataGridKlienciLokata.Visibility = Visibility.Hidden;
                OdblokujPrzyciski("Lokata");
                dataGridKlienciLokata.ItemsSource = null;
            }
        }

        

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

           else if (checkBox.IsChecked == true)
            {
                if (dataGridKlienciKredyt.ItemsSource != null)
                {
                    for (int i = 0; i < listaKredytow.Count(); i++)
                    {
                        listaKredytow[i].zaznaczony = true;
                    }
                    dataGridKlienciKredyt.ItemsSource = null;
                    dataGridKlienciKredyt.ItemsSource = listaKredytow;
                    dataGridKlienciKredyt.Items.Refresh();
                }
                else if (dataGridKlienci.ItemsSource != null)
                {
                    for (int i = 0; i < listaKlientow.Count(); i++)
                    {
                        listaKlientow[i].zaznaczony = true;
                    }

                    dataGridKlienci.ItemsSource = null;
                    dataGridKlienci.ItemsSource = listaKlientow;
                    dataGridKlienci.Items.Refresh();
                }
                else if (dataGridKlienciPrzelew.ItemsSource != null)
                {
                    for (int i = 0; i < listaPrzelewow.Count(); i++)
                    {
                        listaPrzelewow[i].zaznaczony = true;
                    }
                    dataGridKlienciPrzelew.ItemsSource = null;
                    dataGridKlienciPrzelew.ItemsSource = listaPrzelewow;
                    dataGridKlienciPrzelew.Items.Refresh();
                }
                else if (dataGridKlienciLokata.ItemsSource != null)
                {
                    for (int i = 0; i < listaLokat.Count(); i++)
                    {
                        listaLokat[i].zaznaczony = true;
                    }
                    dataGridKlienciLokata.ItemsSource = null;
                    dataGridKlienciLokata.ItemsSource = listaLokat;
                    dataGridKlienciLokata.Items.Refresh();
                }
                else if (dataGridPracownicy.ItemsSource != null)
                {
                    for (int i = 0; i < listaPracownikow.Count(); i++)
                    {
                        listaPracownikow[i].zaznaczony = true;
                    }
                    dataGridPracownicy.ItemsSource = null;
                    dataGridPracownicy.ItemsSource = listaPracownikow;
                    dataGridPracownicy.Items.Refresh();
                }
                else if (dataGridRodzajeKredytow.ItemsSource != null)
                {
                    for (int i = 0; i < listaRodzajowKredytow.Count(); i++)
                    {
                        listaRodzajowKredytow[i].zaznaczony = true;
                    }
                    dataGridRodzajeKredytow.ItemsSource = null;
                    dataGridRodzajeKredytow.ItemsSource = listaRodzajowKredytow;
                    dataGridRodzajeKredytow.Items.Refresh();
                }
                else if (dataGridRodzajeLokat.ItemsSource != null)
                {
                    for (int i = 0; i < listaRodzajowLokat.Count(); i++)
                    {
                        listaRodzajowLokat[i].zaznaczony = true;
                    }
                    dataGridRodzajeLokat.ItemsSource = null;
                    dataGridRodzajeLokat.ItemsSource = listaRodzajowLokat;
                    dataGridRodzajeLokat.Items.Refresh();
                }

            }
            else
            {
                if (dataGridKlienciKredyt.ItemsSource != null)
                {
                    for (int i = 0; i < listaKredytow.Count(); i++)
                    {
                        listaKredytow[i].zaznaczony = false;
                    }
                    dataGridKlienciKredyt.ItemsSource = null;
                    dataGridKlienciKredyt.ItemsSource = listaKredytow;
                    dataGridKlienciKredyt.Items.Refresh();
                }
                else if (dataGridKlienci.ItemsSource != null)
                {
                  for (int i = 0; i < listaKlientow.Count(); i++)
                {
                    listaKlientow[i].zaznaczony = false;
                }

                    dataGridKlienci.ItemsSource = null;
                    dataGridKlienci.ItemsSource = listaKlientow;
                    dataGridKlienci.Items.Refresh();
                }
                else if (dataGridKlienciPrzelew.ItemsSource != null)
                {
                    for (int i = 0; i < listaPrzelewow.Count(); i++)
                    {
                        listaPrzelewow[i].zaznaczony = false;
                    }
                    dataGridKlienciPrzelew.ItemsSource = null;
                    dataGridKlienciPrzelew.ItemsSource = listaPrzelewow;
                    dataGridKlienciPrzelew.Items.Refresh();
                }
                else if (dataGridKlienciLokata.ItemsSource != null)
                {
                    for (int i = 0; i < listaLokat.Count(); i++)
                    {
                        listaLokat[i].zaznaczony = false;
                    }
                    dataGridKlienciLokata.ItemsSource = null;
                    dataGridKlienciLokata.ItemsSource = listaLokat;
                    dataGridKlienciLokata.Items.Refresh();
                }
                else if (dataGridPracownicy.ItemsSource != null)
                {
                    for (int i = 0; i < listaPracownikow.Count(); i++)
                    {
                        listaPracownikow[i].zaznaczony = false;
                    }
                    dataGridPracownicy.ItemsSource = null;
                    dataGridPracownicy.ItemsSource = listaPracownikow;
                    dataGridPracownicy.Items.Refresh();
                }
                else if (dataGridRodzajeKredytow.ItemsSource != null)
                {
                    for (int i = 0; i < listaRodzajowKredytow.Count(); i++)
                    {
                        listaRodzajowKredytow[i].zaznaczony = false;
                    }
                    dataGridRodzajeKredytow.ItemsSource = null;
                    dataGridRodzajeKredytow.ItemsSource = listaRodzajowKredytow;
                    dataGridRodzajeKredytow.Items.Refresh();
                }
                else if (dataGridRodzajeLokat.ItemsSource != null)
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

        private void Click_buttonKlienciWyswietl(object sender, RoutedEventArgs e)
        {
            if (buttonKlienciWyswietl.Content.Equals("Wyświetl"))
            {
                buttonKlienciWyswietl.Content = "Schowaj";

                TabelaKlienta.Visibility = Visibility.Visible;
                dataGridKlienci.Visibility = Visibility.Visible;
                ZablokujPrzyciski("Klienci");
                wczytajBaze("Klienci");
            }


            else
            {
                buttonKlienciHistoria.IsEnabled = true;
                buttonKlienciWyswietl.Content = "Wyświetl";
                dataGridKlienci.Visibility = Visibility.Hidden;
                TabelaKlienta.Visibility = Visibility.Hidden;
                OdblokujPrzyciski("Klienci");
                dataGridKlienci.ItemsSource = null;
                //
               
                TabelaKredytowHistoria.Visibility = Visibility.Hidden;
                TabelaLokatHistoria.Visibility = Visibility.Hidden;
                TabelaPrzelewowHistoria.Visibility = Visibility.Hidden;
                dataGridKlienciHistoria.Visibility = Visibility.Hidden;
                dataGridKlienciKredytHistoria.Visibility = Visibility.Hidden;
                dataGridKlienciLokataHistoria.Visibility = Visibility.Hidden;
                dataGridKlienciPrzelewHistoria.Visibility = Visibility.Hidden;
            }
        }

        private void Click_buttonDodaj(object sender, RoutedEventArgs e)
        {
            switch((sender as Button).Name)
            {
                
                case "buttonKlienciDodaj":
                    {
                        oknoDodajKlienta okno1 = new oknoDodajKlienta(this);
                        okno1.Show();

                        okno1.Closed += oknoClosed;//
                        break;
                    }
                case "buttonKlienciKredytDodaj":
                    {
                        oknoDodajKredyt okno1 = new oknoDodajKredyt(this, idPracownika);
                        okno1.Show();

                        okno1.Closed += oknoClosed;//
                        break;
                    }
                case "buttonKlienciLokataDodaj":
                    {
                        oknoDodajLokate okno1 = new oknoDodajLokate(this, idPracownika);
                        okno1.Show();

                        okno1.Closed += oknoClosed;//
                        break;
                    }
                case "buttonKlienciPrzelew":
                    {
                        oknoDodajPrzelew okno1 = new oknoDodajPrzelew(this);
                        okno1.Show();

                        okno1.Closed += oknoClosed;//
                        break;
                    }
                case "buttonKredytKredytDodaj":
                    {
                        oknoDodajRodzajKredytu okno1 = new oknoDodajRodzajKredytu(this, idPracownika);
                        okno1.Show();

                        okno1.Closed += oknoClosed;//
                        break;
                    }
                case "buttonLokataLokataDodaj":
                    {
                        oknoDodajRodzajLokaty okno1 = new oknoDodajRodzajLokaty(this, idPracownika);
                        okno1.Show();

                        okno1.Closed += oknoClosed;//
                        break;
                    }
                case "buttonPracownicyDodaj":
                    {
                        OknoDodajPracownika okno1 = new OknoDodajPracownika(this);
                        okno1.Show();

                        okno1.Closed += oknoClosed;//
                        break;
                    }
                    
                         case "buttonKlienciWplata":
                    {
                        OknoDodajWplata okno1 = new OknoDodajWplata(this);
                        okno1.Show();

                        okno1.Closed += oknoClosed;//
                        break;
                    }
            }
        }

        private void Click_buttonEdytuj(object sender, RoutedEventArgs e)
        {
           
            
                switch ((sender as Button).Name)
                {

                    case "buttonKlienciEdytuj":
                        {
                            List<int> lista_indeksow = znajdzZaznaczenie("Klienci");
                            if (lista_indeksow.Count == 0 || lista_indeksow.Count() > 1)
                            {
                                MessageBox.Show("Wybierz 1 rekord do edycji");
                                break;
                            }
                            else
                            {
            
                                oknoEdytujKlienta okno1 = new oknoEdytujKlienta(this, lista_indeksow);
                                okno1.Show();
                                okno1.Closed += oknoClosed;
                                break;
                 
                                
                            }
                            


                        }
                    case "buttonKlienciKredytEdytuj":
                    {
                        List<int> lista_indeksow = znajdzZaznaczenie("Kredyty");
                        if (lista_indeksow.Count == 0 || lista_indeksow.Count() > 1)
                        {
                            MessageBox.Show("Wybierz rekord do edycji");
                            break;
                        }
                        else
                        {
        
       
                                oknoEdytujKredyt okno1 = new oknoEdytujKredyt((this), lista_indeksow);
                                okno1.Show();
                                okno1.Closed += oknoClosed;
                                break;
         
                           
                    }
            }
                    case "buttonKlienciLokataEdytuj":
                        {
                            List<int> lista_indeksow = znajdzZaznaczenie("Lokaty");
                            if (lista_indeksow.Count == 0 || lista_indeksow.Count() > 1)
                            {
                                MessageBox.Show("Wybierz rekord do edycji");
                                break;
                            }
                            else
                            {
            
             
                                oknoEdytujLokate okno1 = new oknoEdytujLokate(this, lista_indeksow);
                                okno1.Show();
                                okno1.Closed += oknoClosed;
                                break;
                       
                           
                            }
                        }
                case "buttonPracownicyEdytuj":
                    {
                        List<int> lista_indeksow = znajdzZaznaczenie("Pracownicy");
                        if (lista_indeksow.Count == 0 || lista_indeksow.Count() > 1)
                        {
                            MessageBox.Show("Wybierz rekord do edycji");
                            break;
                        }
                        else
                        {
           
                                oknoEdytujPracownika okno1 = new oknoEdytujPracownika(this, lista_indeksow, idPracownika);
                                okno1.Show();
                                okno1.Closed += oknoClosed;
                                break;
                            
                          
                        }
                    }
                case "buttonKredytKredytEdytuj":
                    {
                        List<int> lista_indeksow = znajdzZaznaczenie("RodzajeKredytow");
                        if (lista_indeksow.Count == 0 || lista_indeksow.Count() > 1)
                        {
                            MessageBox.Show("Wybierz rekord do edycji");
                            break;
                        }
                        else
                        {
                            oknoEdytujRodzajKredytu okno1 = new oknoEdytujRodzajKredytu(this, lista_indeksow);
                            okno1.Show();
                            okno1.Closed += oknoClosed;
                            break;
                        }
                    }
                case "buttonLokataLokataEdytuj":
                    {
                        List<int> lista_indeksow = znajdzZaznaczenie("RodzajeLokat");
                        if (lista_indeksow.Count == 0 || lista_indeksow.Count() > 1)
                        {
                            MessageBox.Show("Wybierz rekord do edycji");
                            break;
                        }
                        else
                        {
                            oknoEdytujRodzajeLokat okno1 = new oknoEdytujRodzajeLokat(this, lista_indeksow);
                            okno1.Show();
                            okno1.Closed += oknoClosed;
                            break;
                        }
                    }
            }
            


          
        }

        private void Click_buttonHistoria(object sender, RoutedEventArgs e)
        {
         

            List<int> lista_indeksow = znajdzZaznaczenie("Klienci");
            if (lista_indeksow.Count() != 1)
            {
                MessageBox.Show("Zaznacz kogo wyświetlić.");
            }
            else
            {
                
                    dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Klienci);
                    var zapytanie = dc.ExecuteQuery<Klienci>("select [Id klienta],[imię] ,nazwisko,[telefon],[miasto], ulica,[środki]from [klienci] where [Id klienta] =" + lista_indeksow[0] + "");
                    listaKlientow.Clear();

                    foreach (Klienci item in zapytanie)
                    {
                        listaKlientow.Add(item);
                    }
                    dataGridKlienciHistoria.ItemsSource = null;
                    dataGridKlienciHistoria.ItemsSource = listaKlientow;


                    dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Kredyty);
                    var zapytanie1 = dc.ExecuteQuery<Kredyty>("select [Id kredytu],[Data założenia],[kwota kredytu] ,klient,[rodzaj kredytu],[kredytu udzielił]from [kredyty] where klient =" + lista_indeksow[0] + " ");
                    listaKredytow.Clear();

                    foreach (Kredyty item in zapytanie1)
                    {
                  

                    if(item.aktywny==true)
                        listaKredytow.Add(item);
                    }
                    dataGridKlienciKredytHistoria.ItemsSource = null;
                    dataGridKlienciKredytHistoria.ItemsSource = listaKredytow;
                    //////////////////////////
                    dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Lokaty);
                    listaLokat.Clear();
                    var zapytanie6 = dc.ExecuteQuery<Lokaty>("select [Id lokaty],[wysokość lokaty],[Data założenia] ,klient,[id rodzaju lokaty],[lokaty udzielił]from [lokaty] where klient = " + lista_indeksow[0] + "");


                    foreach (Lokaty item in zapytanie6)
                    {
                  
                    if (item.aktywny == true)
                        listaLokat.Add(item);
                    }
                    dataGridKlienciLokataHistoria.ItemsSource = null;
                    dataGridKlienciLokataHistoria.ItemsSource = listaLokat;

                    ///////////

                    dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Przelewy);
                    listaPrzelewow.Clear();
                    var zapytanie10 = dc.ExecuteQuery<Przelewy>("select [Id Przelewu],[Kwota],[Nazwa odbiorcy],[Nadawca],[Tytuł przelewu] from Przelewy where nadawca =" + lista_indeksow[0] + "");


                    foreach (Przelewy item in zapytanie10)
                    {
                 
                        listaPrzelewow.Add(item);
                    }
                    dataGridKlienciPrzelewHistoria.ItemsSource = null;
                    dataGridKlienciPrzelewHistoria.ItemsSource = listaPrzelewow;
              

                buttonKlienciHistoria.IsEnabled = false;
                TabelaKredytowHistoria.Visibility = Visibility.Visible;
                TabelaLokatHistoria.Visibility = Visibility.Visible;
                TabelaPrzelewowHistoria.Visibility = Visibility.Visible;
               
        
                dataGridKlienci.Visibility = Visibility.Hidden;
                dataGridKlienci.ItemsSource = null;

                dataGridKlienciHistoria.Visibility = Visibility.Visible;
                dataGridKlienciKredytHistoria.Visibility = Visibility.Visible;
                dataGridKlienciLokataHistoria.Visibility = Visibility.Visible;
                dataGridKlienciPrzelewHistoria.Visibility = Visibility.Visible;

            
            }
        }

        private void Click_buttonLokataLokataWyswietl(object sender, RoutedEventArgs e)
        {

           


            if (buttonLokataLokataWyswietl.Content.Equals("Wyświetl"))
            {
                TabelaRodzajowLokat.Visibility = Visibility.Visible;
                buttonLokataLokataWyswietl.Content = "Schowaj";
                dataGridRodzajeLokat.Visibility = Visibility.Visible;
           
                wczytajBaze("RodzajeLokat");
            }
            else
            {
                TabelaRodzajowLokat.Visibility = Visibility.Hidden;

                TabelaRodzajowLokat.Visibility = Visibility.Hidden;
            
                dataGridRodzajeLokat.ItemsSource = null;
                ////////////////////

                buttonLokataLokataWyswietl.Content = "Wyświetl";
                dataGridRodzajeLokat.Visibility = Visibility.Hidden;
                TabelaKlienta.Visibility = Visibility.Hidden;
             
                dataGridRodzajeLokat.ItemsSource = null;
           
            }
        }

        private void Click_Usunieci(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "buttonKlienciUsunieciLokata":
                    {
                     
                        buttonKlienciLokataWyswietl.Content = "Schowaj";
                        TabelaLokat.Visibility = Visibility.Visible;
                            ZablokujPrzyciski("Lokata");
                      
                            dataGridKlienciLokata.Visibility = Visibility.Visible;
                            wczytajBaze("UsunieteLokaty");
                       
                        break;
                    }
                case "buttonKlienciUsunieciKredyt":
                    {
                        buttonKlienciKredytWyswietl.Content = "Schowaj";
                        TabelaKredytow.Visibility = Visibility.Visible;
                     
                            dataGridKlienciKredyt.Visibility = Visibility.Visible;
                            ZablokujPrzyciski("Kredyt");
                            wczytajBaze("UsunieteKredyty");
                    
                        break;
                    }
                case "buttonKlienciUsunieci":
                    {
                        buttonKlienciWyswietl.Content = "Schowaj";
                        TabelaKlienta.Visibility = Visibility.Visible;
                            dataGridKlienci.Visibility = Visibility.Visible;
                            ZablokujPrzyciski("Klienci");
                            wczytajBaze("UsunieciKlienci");
                     
                        break;
                    }
                case "buttonPracownicyUsunieci":
                    {
                        buttonPracownicyWyswietl.Content = "Schowaj";
                        TabelaPracownikow.Visibility = Visibility.Visible;
                            dataGridPracownicy.Visibility = Visibility.Visible;
                          
                            wczytajBaze("UsunieciPracownicy");
                      
                        break;
                    }
            }
        }
        
    }
}













