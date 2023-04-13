using System.Windows;
using System.Windows.Controls;
using Graphic_editor.editor;

namespace Graphic_editor
{

    public partial class ImageProperty : Window
    {       
        public ImageProperty(double width,double height)
        {
            InitializeComponent();
            tbWidth.Text = width.ToString();
            tbHeight.Text = height.ToString();
        }

        public double FieldWidth { get; set; }
        public double FieldHeight { get; set; }

      
        private bool CheckSize(string s)
        {    
            for (int i=0;i<s.Length;i++)
            {
                if (s[0]==',' || s.LastIndexOf(',')!=s.IndexOf(',') || (s[i]!=',' && !(s[i] >= '0' && s[i] <= '9')))
                {
                    MessageBox.Show("Введите правильный размер!");
                    return false;
                }
            }
            return true;
        }

   
        private void Width_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!CheckSize(tbWidth.Text))
            {
                tbWidth.Focus();
                return;
            }            
        }

      
        private void Height_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!CheckSize(tbHeight.Text))
            {
                tbHeight.Focus();
                return;
            }            
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (tbWidth.Text == "" || tbHeight.Text == "")
            {
                MessageBox.Show("Введите положительное число!");
                return;
            }
            FieldWidth = double.Parse(tbWidth.Text);
            FieldHeight = double.Parse(tbHeight.Text);
            Editor mainWindow = this.Owner as Editor;
            mainWindow.cnvPaint.Width = FieldWidth;
            mainWindow.cnvPaint.Height = FieldHeight;
            Close();
        }
       
    }
}
