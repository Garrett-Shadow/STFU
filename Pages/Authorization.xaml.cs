using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace OhLordAgain.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        int CaptchaCounter = 0;
        string captchaSample = "";
        Random random = new Random();

        public Authorization()
        {
            InitializeComponent();
            //CaptchaSample.Text = "";
            Captcha.Text = "";
            Labelcap.Visibility = Visibility.Collapsed;
            //CaptchaSample.Visibility = Visibility.Collapsed;
            Captcha.Visibility= Visibility.Collapsed; 
            CaptchaCanvas.Visibility = Visibility.Collapsed;
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            if (Login.Text != "" && Password.Password != "")
            {
                if (Classes.Connector.Authorize(Login.Text, Password.Password) == true)
                {
                    if (Captcha.Text == captchaSample)
                    {
                        var userdata = Classes.Connector.GetUserData();
                        if (userdata != null)
                        {
                            var userrole = userdata.UserRole;
                            switch (userrole)
                            {
                                case "Администратор":
                                    NavigationService.Navigate(Classes.PageClass.GetAdmin());
                                    break;
                                case "Менеджер":
                                    NavigationService.Navigate(Classes.PageClass.GetUser());
                                    break;
                                case "Клиент":
                                    NavigationService.Navigate(Classes.PageClass.GetUser());
                                    break;
                                default:
                                    return;
                            }
                        }
                    }
                    else if (CaptchaCounter<3)
                    {
                        CaptchaCounter++;
                        MessageBox.Show("Неверная капча!");
                        CaptchaCreate();
                        //CaptchaSample.Visibility = Visibility.Visible;
                        CaptchaCanvas.Visibility = Visibility.Visible;
                        Captcha.Visibility = Visibility.Visible;
                        Labelcap.Visibility=Visibility.Visible;
                        Login.Text = null;
                        Password.Password = null;
                        Captcha.Text = null;
                        return;
                    }
                    else
                    {
                        CaptchaCounter = 0;
                        BlockActivate();
                        Login.Text = null;
                        Password.Password = null;
                        Captcha.Text = null;
                        return;
                    }
                }
                else
                {
                    CaptchaCounter++;
                    MessageBox.Show("Не существует такого пользователя!");
                    CaptchaCreate();
                    //CaptchaSample.Visibility = Visibility.Visible;
                    CaptchaCanvas.Visibility = Visibility.Visible;
                    Captcha.Visibility = Visibility.Visible;
                    Labelcap.Visibility = Visibility.Visible;
                    Login.Text = null;
                    Password.Password = null;
                    Captcha.Text = null;
                    return;
                }
            }
            else
            {
                MessageBox.Show("Не введен пароль и логин!");
                return;
            }
        }

        private void CaptchaCreate()
        {
            CaptchaCanvas.Children.Clear();
            //CaptchaSample.Text = "";
            string letters = "QWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            
            for (int i=0; i < 5; i++)
            {
                var Char =letters.Substring(random.Next(0, letters.Length - 1), 1);
                captchaSample += Char;
                //CaptchaSample.Text += letters.Substring(random.Next(0, letters.Length - 1), 1);
                LetterCreate(Char, i);
                LineCreate();
            }
        }

        private void LineCreate()
        {
            Line a= new Line();
            a.StrokeThickness=random.Next(1,4);
            a.Stroke = new SolidColorBrush(Color.FromRgb(255,255,255));
            a.X1 = 5;
            a.Y1 = random.Next(10, 40);
            a.X2 = 195;
            a.Y2 = random.Next(10, 40);
            CaptchaCanvas.Children.Add(a);
        }

        private void LetterCreate(string a, int i)
        {
        Label letter = new Label();
            letter.Content= a;
            letter.FontSize= 24;
            letter.RenderTransformOrigin = new Point(0.5, 0.5);
            letter.RenderTransform = new RotateTransform(random.Next(-30,30));
            letter.FontWeight= FontWeights.Bold;
            CaptchaCanvas.Children.Add(letter);
            Canvas.SetLeft(letter, 50+(i*20));
            Canvas.SetTop(letter, random.Next(1,10));
        }

        private void BlockActivate()
        {
            StackPanel.IsEnabled = false;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval=TimeSpan.FromSeconds(10);
            timer.Tick += new EventHandler((s, e) =>
            {
                timer.Stop();
                StackPanel.IsEnabled = true;
            });
            timer.Start();
        }
    }
}
