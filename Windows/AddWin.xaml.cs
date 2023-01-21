using OhLordAgain.Database;
using OhLordAgain.Pages;
using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.Win32;

namespace OhLordAgain.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddWin.xaml
    /// </summary>
    public partial class AddWin : Window
    {
        public static Database.user10Entities Connection = Classes.Connector.GetDatabase();

        public Database.Product NewProduct { get; set; }
        public List<Database.Category> category { get; set; }
        public List<Database.Manufacturer> manufacturers { get; set; }
        public List<Database.Supplier> suppliers { get; set; }
        public List<Database.Unit> units { get; set; }

        public AddWin()
        {
            InitializeComponent();
            NewProduct = new Database.Product();
            category = new List<Database.Category>(Connection.Category.ToList());
            CategoryCombo.SetBinding(ComboBox.ItemsSourceProperty, new Binding() { Source = category });
            manufacturers = new List<Database.Manufacturer>(Connection.Manufacturer.ToList());
            ManufacturerCombo.SetBinding(ComboBox.ItemsSourceProperty, new Binding() { Source = manufacturers });
            suppliers = new List<Database.Supplier>(Connection.Supplier.ToList());
            SupplierCombo.SetBinding(ComboBox.ItemsSourceProperty, new Binding() { Source = suppliers });
            units = new List<Database.Unit>(Connection.Unit.ToList());
            UnitCombo.SetBinding(ComboBox.ItemsSourceProperty, new Binding() { Source = units });
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdminPage.Connection.Product.Add(NewProduct);
            if (AdminPage.Connection.SaveChanges() == 1)
            {
                AdminPage.products.Add(NewProduct);
                NewProduct = new Database.Product();
                AddGrid.GetBindingExpression(DataContextProperty).UpdateTarget();
                MessageBox.Show("Товар добавлен");
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog addKar=new OpenFileDialog();
            addKar.Multiselect = false;
            if(addKar.ShowDialog().Value== true )
            {

                NewProduct.ProductPhoto = "\\Images\\"+addKar.SafeFileName;
                
            }
        }
    }
}
