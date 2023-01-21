using OhLordAgain.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OhLordAgain.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        Windows.AddWin addWin;
        Windows.EditWin editWin;

        public class CostSort
        {
            public string DisplayName { get; set; }
            public string PropertyName { get; set; }
            public bool Ascending { get; set; }
        }

        public static List<CostSort> costSorts { get; set; } = new List<CostSort>()
        {
            new CostSort(){DisplayName="Без сортировки", PropertyName=null, Ascending=true},
            new CostSort(){DisplayName="Возрастание цены", PropertyName="ProductCost", Ascending=true},
            new CostSort(){DisplayName="Убывание цены", PropertyName="ProductCost", Ascending=false},
        };

        public static Database.user10Entities Connection = Classes.Connector.GetDatabase();
        public static ObservableCollection<Database.Product> products;
        public static ObservableCollection<Database.Manufacturer> manufacturers;
        int viewcount = 0;
        int totalcount = 0;

        public AdminPage()
        {
            InitializeComponent();
            products = new ObservableCollection<Database.Product>(Connection.Product.ToList());
            manufacturers = new ObservableCollection<Database.Manufacturer>(Connection.Manufacturer.ToList());
            manufacturers.Insert(0, new Database.Manufacturer() { ManufacturerName = "Все производители" });
            ProductList.ItemsSource = products;
            FilterCombo.ItemsSource = manufacturers;
            SortCombo.SelectedIndex = 0;
            FilterCombo.SelectedIndex = 0;
            viewcount = products.Count;
            totalcount = products.Count;
            LabelCount.Content = $"{viewcount}/{totalcount}";
            DataContext = this;
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(SearchText.Text.Trim());
        }

        private void Search(string substring)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ProductList.ItemsSource);
            if (view == null) return;
            viewcount = 0;
            view.Filter = new Predicate<object>(obj =>
            {
                bool IsView = ((Database.Product)obj).ProductDescription.ToLower().Contains(substring.ToLower());
                if (IsView) viewcount++;
                return IsView;
            });
            LabelCount.Content = $"{viewcount}/{totalcount}";
        }

        private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CostSort itemSorting = SortCombo.SelectedItem as CostSort;
            Sort(itemSorting.PropertyName, itemSorting.Ascending);
        }

        private void Sort(string property, bool asc)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ProductList.ItemsSource);
            if (view == null) return;
            view.SortDescriptions.Clear();
            if (property != null)
            {
                view.SortDescriptions.Add(new SortDescription(property, asc ? ListSortDirection.Ascending : ListSortDirection.Descending));
            }
        }

        private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Database.Manufacturer manufacturer = FilterCombo.SelectedItem as Database.Manufacturer;
            Filter(manufacturer.ManufacturerName);
        }

        private void Filter(string manufacturer)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ProductList.ItemsSource);
            if (view == null) return;
            viewcount = 0;
            view.Filter = new Predicate<object>(obj =>
            {
                if (manufacturer == "Все производители")
                {
                    viewcount = products.Count;
                    return true;
                }
                bool IsView = ((Database.Product)obj).ProductManufacturer == manufacturer;
                if (IsView) viewcount++;
                return IsView;
                //return ((Database.Product)obj).ProductManufacturer == manufacturer;
            });
            LabelCount.Content = $"{viewcount}/{totalcount}";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (addWin == null)
            {
                addWin = new Windows.AddWin();
            }
            addWin.Closed += AddWin_Closed;
            addWin.ShowDialog();
        }

        private void AddWin_Closed(object sender, EventArgs e)
        {
            addWin = null;
        }

        private void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editWin != null) return;
            editWin = new EditWin(ProductList.SelectedItem as Database.Product);
            editWin.Closed += EditWin_Closed;
            editWin.Topmost = true;
            editWin.Show();
            ProductList.SelectedIndex = -1;
        }

        private void EditWin_Closed(object sender, EventArgs e)
        {
            editWin = null;
        }
    }
}