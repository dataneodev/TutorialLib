using Ardalis.GuardClauses;
using System.Threading.Tasks;
using System.Windows;

namespace dataneo.TutorialLibs.WPF.UI.CategoryManage
{
    /// <summary>
    /// Interaction logic for CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {
        public CategoryWindow(Window parentHandle)
        {
            InitializeComponent();
            this.Owner = Guard.Against.Null(parentHandle, nameof(parentHandle));
        }

        public async Task ShowCategory()
        {
            this.Show();


        }
    }
}
