using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
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

        public UserPage()
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
            Filter();
        }

        private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void Filter()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ProductList.ItemsSource);
            if (view == null) return;
            viewcount = 0;
            view.Filter = new Predicate<object>(obj =>
            {
                Database.Product p = obj as Database.Product;
                bool result = true;
                if (SearchText.Text.Trim().Length > 0)
                {
                    string searchtext = SearchText.Text.Trim().ToLower();
                    result = p.ProductName.ToLower().Contains(searchtext)||
                    p.ProductDescription.ToLower().Contains(searchtext)||
                    p.ProductCategory.ToLower().Contains(searchtext)||
                    p.ProductManufacturer.ToLower().Contains(searchtext)||
                    p.ProductSupplier.ToLower().Contains(searchtext);
                }
                if (FilterCombo.SelectedIndex > 0)
                {
                    var manufacturer = FilterCombo.SelectedItem as Database.Manufacturer;
                    result &= p.Manufacturer.ManufacturerName == manufacturer.ManufacturerName;
                }
                return result;
            });
            view.SortDescriptions.Clear();
            if (SortCombo.SelectedIndex > 0)
            {
                CostSort sorting = SortCombo.SelectedItem as CostSort;
                view.SortDescriptions.Add(new SortDescription(sorting.PropertyName, sorting.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending));
            }
            viewcount=ViewCounter(view);
            LabelCount.Content = $"{viewcount}/{totalcount}";
        }

        private int ViewCounter(ICollectionView view)
        {
            int counter = 0;
            foreach (var item in view) {counter++;}
            return counter;
        }
    }
}
