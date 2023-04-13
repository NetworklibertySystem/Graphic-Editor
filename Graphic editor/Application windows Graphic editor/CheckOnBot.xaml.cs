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
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Media.Animation;
using Graphic_editor.editor;

namespace Graphic_editor
{
    public partial class CheckOnBot : Window
    {
        byte ch;
        Random rand = new Random();
        int a;
        public CheckOnBot()
        {
            InitializeComponent();
            DoubleAnimation btnAnimation = new DoubleAnimation();
            btnAnimation.From = 0;
            btnAnimation.To = 450;
            btnAnimation.Duration = TimeSpan.FromSeconds(3);
            CodeWindow.BeginAnimation(Button.WidthProperty, btnAnimation);
        }
        private static Timer aTimer;
        public static void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            aTimer.Stop();
            MessageBox.Show("Время прохождение капчи закончилось");
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(CodeWindow==null)
            {
                return;
            }

            if (textBoxsmsR.Text == CodeWindow.Text)
            {
                Editor NextWindow = new Editor();
                NextWindow.Show();
                this.Close();  // всместо this.Close(); можно писать this.Hide(); - так как можно вернуть значение this.Show();
            }
            else
            {
                if (CodeWindow.Text == "")
                {
                    MessegeError.Text = "Введите код";
                }
                else if (CodeWindow.Text.Length >= 4)
                {
                    ch -= 1;
                    MessegeError.Text = "Неверно введенное SMS" +
                                 "\n" + "   попыток осталось " + ch;
                    //Проверка введённого кода
                    if (ch < 1)
                    {
                        CodeWindow.IsEnabled = false;
                        textBoxsmsR.IsEnabled = false;
                        aTimer.Enabled = true;
                        Editor NextWindow = new Editor();
                        NextWindow.Show();
                        this.Close();
                    }
                }
            }
        }
        private void FormCheckOnBot_Load(object sender, EventArgs e)
        {
            aTimer.Enabled = false;
            CodeWindow.IsEnabled = true;
            textBoxsmsR.IsEnabled = true;
            a = rand.Next(1000, 9999);
            textBoxsmsR.Text = Convert.ToString(a);
            ch = 3;
            MessegeError.Text = "";
        }
        }
    }

