using Microsoft.Win32;
using OhLordAgain.Pages;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace OhLordAgain.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditWin.xaml
    /// </summary>
    public partial class EditWin : Window
    {
        public static Database.user10Entities Connection = Classes.Connector.GetDatabase();

        public Database.Product SelectedProduct { get; set; }
        public List<Database.Category> category { get; set; }
        public List<Database.Manufacturer> manufacturers { get; set; }
        public List<Database.Supplier> suppliers { get; set; }
        public List<Database.Unit> units { get; set; }

        public EditWin(Database.Product product)
        {
            InitializeComponent();
            SelectedProduct = product;
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
            try
            {
                Connection.SaveChanges();
                MessageBox.Show("Данные обновлены");
                EditGrid.GetBindingExpression(DataContextProperty).UpdateTarget();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (Database.OrderProduct order in Connection.OrderProduct) 
            {
                if (order.Product.ProductArticleNumber==SelectedProduct.ProductArticleNumber) 
                {
                    MessageBox.Show("Товар заказан и присутствует в списке заказанных товаров!");
                    return;
                }
            }
            Connection.Product.Remove(SelectedProduct);
            AdminPage.products.Remove(SelectedProduct);
            MessageBox.Show("Данные удалены");
            Connection.SaveChanges();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog addImg = new OpenFileDialog();
            addImg.Multiselect = false;
            if (addImg.ShowDialog().Value == true)
            {
                SelectedProduct.ProductPhoto = "\\Images\\" + addImg.SafeFileName;
                ImgName.GetBindingExpression(Image.SourceProperty).UpdateTarget();
            }
        }
    }
    }

