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
        private readonly CategoryVM _categoryVM;
        public CategoryWindow(Window parentHandle)
        {
            InitializeComponent();
            this.Owner = Guard.Against.Null(parentHandle, nameof(parentHandle));
            this._categoryVM = new CategoryVM();
            this.DataContext = this._categoryVM;
        }

        public async Task ShowCategory()
        {
            this.Show();
            await this._categoryVM.LoadCategoriesAsync();
        }
    }
}
