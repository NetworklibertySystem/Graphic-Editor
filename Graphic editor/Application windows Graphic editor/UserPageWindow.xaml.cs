using System.Linq;
using System.Windows;
namespace Graphic_editor
{
    public partial class UserPageWindow : Window
    {
        public UserPageWindow()
        {
            InitializeComponent();
            ApplicationContext db = new ApplicationContext();
            var users = db.users.ToList();
            listOfUsers.ItemsSource = users;
        }
    }
}
