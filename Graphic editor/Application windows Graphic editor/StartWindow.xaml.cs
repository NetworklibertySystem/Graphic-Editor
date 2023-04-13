using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FluentValidation;
using Users;
using System.Timers;
using System.Globalization;
using System.Linq;
using System.Data.SQLite;
using System.Data;
using System.Windows.Media.Animation;
namespace Graphic_editor
{
    static class ControlValidationExtension
    {
        public static Startwindow.Validator Rules(this TextBox control) => new Startwindow.Validator(control, control.Text);
        public static Startwindow.Validator Rules(this PasswordBox control) => new Startwindow.Validator(control, control.Password);
    }
    public partial class Startwindow : Window
    {
        UsersContext DB;//Тестовый массив данных авторизации для приложения
        ApplicationContext db; //SQLiteConnection
        byte ch;
        int a;
        byte schert = 3;   // счетчик для количества попыток
        byte seconds = 60;   // переменная для определения оставшегося времени до разблокировки
        public Startwindow()
        {
            InitializeComponent();
            db = new ApplicationContext();
            if (!db.Database.Exists())
            {
                MessageBox.Show("Нет требуемой БД!");
                return;
            }//На случай сбоев SQLite
            DoubleAnimation exAnimation = new DoubleAnimation();
            exAnimation.From = 0;
            exAnimation.To = 450;
            exAnimation.Duration = System.TimeSpan.FromSeconds(3);
            Register.BeginAnimation(Button.WidthProperty, exAnimation);
            var user = new User
            {
                Login = "admin",
                Slogin = "@adm",
                Password = "12345",
            };
            var validator = new UserValidator();
            var validator2 = new UserValidator();
            DB = new UsersContext();
            using (var connection = new SQLiteConnection("Data Source=Databases/Spiel.db"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO Users (Login, Slogin, Password) VALUES ('admin', '@adm', '12345')"; //Модуль тестирования SQLLite
                int number = command.ExecuteNonQuery();
            }
        }
        private static Timer aTimer;
        public static void SetTimer()
        {
            // Создание таймера с двухсекундным интервалом.
            aTimer = new Timer(2000);
            // Подключение произошедшего события к таймеру.
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        private static void OnTimedEvent(ElapsedEventArgs e)
        {
            aTimer.Stop();
            MessageBox.Show("Время регистрации закончилось");
            //Иначе включить проверку CheckOnBot
        }
        private void StartWindow_Load(object sender)
        {
            aTimer.Enabled = false;
            textBoxLogin.IsEnabled = true;
            textBoxLoginShort.IsEnabled = true;
            ch = 3;
        }
        //private void TxLOG_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (Convert.ToInt32(e.KeyChar) == 13)    // нажатие enter
        //        textBoxLogin.Focus();                      // получение фокуса строке логина
        //}
        private void SaveUser(User user)
        {
            UserValidator validationRules = new UserValidator();
            var Result = validationRules.Validate(user);
        }
        public class Validator
        {
            public Validator ContentEquals(string s)
            {
                _success = (_content == s);
                return this;
            }
            private readonly Control _control;
            private readonly string _content;
            private bool _success = true;
            public Validator(Control control, string content) => (_control, _content) = (control, content);
            public Validator MinCharacters(int count)
            {
                if (_content.Length < count)
                    _success = false;
                return this;
            }
            public Validator IsSlogin()
            {
                return null;
            }
            internal bool Validate()
            {
                return _success;
            }
        }
        internal class UserValidator : AbstractValidator<User>
        {
            public UserValidator()
            {
                RuleFor(x => x.Login).NotEmpty().MaximumLength(20);
                RuleFor(x => x.Login).Must(x => string.IsNullOrEmpty(x)).OverridePropertyName("Логин").WithMessage("Не задано ни одного логина!");
                RuleFor(x => x.Login).Must(x => string.IsNullOrEmpty(x)).OverridePropertyName("@Логин").WithMessage("Не задано ни одного @логина!").WithSeverity(Severity.Warning);
                RuleFor(x => x.Slogin).NotEmpty().Must(x => x.Contains("@") || x.Length < 15).When(x => x.Login.ToLower() == "admin");
                RuleFor(x => x.Slogin).NotEmpty().Must(x => x.Contains("@") || x.Length < 25).When(x => x.Login.ToLower() == "creator");
                RuleFor(x => x.Slogin).NotEmpty().NotEqual("Тег").WithMessage("Тег не может быть пустым!");
                ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("ru");
            }
        }
      public  void MarkInvalid(Control control)
        {
            string login = textBoxLogin.Text.Trim();
            string slogin = textBoxLoginShort.Text.Trim().ToLower();
            string pass = passBox.Password.Trim();
            string rpass = repeatPassBox.Password.Trim();
            if

                (textBoxLogin.Text != login)
            {
                control.ToolTip = "Это поле введено не корректно!";
                control.Background = Brushes.DarkRed;
            }
            else
            {
                control.ToolTip = "Это поле введено правильно!";
                control.Background = Brushes.Green;
            }
            if
        (textBoxLoginShort.Text != slogin)
            {
                control.ToolTip = "Это поле введено не корректно!";
                control.Background = Brushes.DarkRed;
            }
           else
            {
                control.ToolTip = "Это поле введено правильно!";
                control.Background = Brushes.Green;
            }
            if (passBox.Password != pass)
            {
                control.ToolTip = "Это поле введено не корректно!";
                control.Background = Brushes.DarkRed;
            }
            else
            {
                control.ToolTip = "Это поле введено правильно!";
                control.Background = Brushes.Green;
            }
            if (pass != rpass)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }
        }
        public void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string slogin = textBoxLoginShort.Text.Trim().ToLower();
            string pass = passBox.Password.Trim();
            string rpass = repeatPassBox.Password.Trim();
            if (login.Length < 5)
            {
                MarkInvalid(textBoxLogin);
            }
            if (pass.Length < 5)
            {
                MarkInvalid(passBox);
            }
      
            if (!textBoxLogin.Rules().MinCharacters(5).Validate())
                MarkInvalid(textBoxLogin);
            if (!textBoxLoginShort.Rules().MinCharacters(5).Validate())
                MarkInvalid(textBoxLoginShort);
            if (!passBox.Rules().MinCharacters(5).ContentEquals(rpass).Validate())
                MarkInvalid(passBox);
            if (!repeatPassBox.Rules().MinCharacters(5).ContentEquals(pass).Validate())
                MarkInvalid(repeatPassBox);
            ////////////////////////////
            ///Проверка rpass = pass///
            if (pass != rpass)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }
            //////////////////////////
            User users = new User(login, slogin, pass);
            if (textBoxLogin.Text == "Введите логин")
            {
                MessageBox.Show("Введите логин");
                return;
            }
            if (passBox.Password == " ")
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            if (users != null)
            {
                MessageBox.Show("Такой логин уже присутствует");
            }
            else
                db.users.Add(users);
            db.SaveChanges();
            MessageBox.Show("Регистрация прошла успешно!");
            UserPageWindow userPageWindow = new UserPageWindow();
            userPageWindow.Show();
            this.Close();//Можно использовать Hide();
        }
            private void LoginEnter_Click(object sender, RoutedEventArgs e)
            {
            string login = textBoxLogin1.Text.Trim();
            string pass = passBox1.Password.Trim();
                
                if (!textBoxLogin.Rules().MinCharacters(5).Validate())
                textBoxLogin1.MarkInvalid();
                if (!passBox.Rules().MinCharacters(5).ContentEquals(pass).Validate())
                    passBox1.MarkInvalid();

                User authUser = null;
                
                //var ff = db.users.ToList();
                authUser = db.users.SingleOrDefault(user => user.Login == login && user.Password == pass);

                if (authUser != null)
                {
                    MessageBox.Show("Авторизация прошла успешно!");
                CheckOnBot checkOnBot = new CheckOnBot();
                checkOnBot.ShowDialog();
                //UserPageWindow userPageWindow = new UserPageWindow();
                //userPageWindow.Show();
                Close();//Можно использовать Hide();
                }
                else
                {
                    if (authUser == null)
                    {
                        MessageBox.Show("Неверный логин");
                    }
                    else
                    {
                        if (authUser.Password != passBox1.Password)
                        {
                            MessageBox.Show("Неверная пара Логин-Пароль");
                            schert -= 1;
                            //erorrM.Text = "Неверно введен логин или пароль" +
                            //         "\n" + "    Попыток осталось " + schert;
                            passBox1.Password = "";
                            if (schert < 1)
                            {
                                textBoxLogin1.Text = "";
                                textBoxLogin1.IsEnabled = false;
                                passBox1.IsEnabled = false;
                                LoginEnter.IsEnabled = false;
                                //block.Enabled = true;
                                //sec.Enabled = true;
                            }
                        }
                        else
                        {
                            //schert = 3;
                            //erorrM.Text = "";
                            this.Close();//Можно использовать this.Hide();
                            CheckOnBot checkOnBot = new CheckOnBot();
                            checkOnBot.ShowDialog();
                        }
                        DataTable table = new DataTable();
                    }
                }
            }
    }
    public static class Invalider
    {
        public static void MarkInvalid(this Control control)
        {
            control.ToolTip = "Это поле введено не корректно!";
            control.Background = Brushes.DarkRed;
        }
    }
    }

        
    

