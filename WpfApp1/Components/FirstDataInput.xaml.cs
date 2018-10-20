using System.Diagnostics;
using System.Windows.Controls;
using Unity.Attributes;

namespace WpfApp1.Components
{
    /// <summary>
    /// Interaction logic for FirstDataInput.xaml
    /// </summary>
    public partial class FirstDataInput : Page
    {
        public FirstDataInput()
        {
            InitializeComponent();
        }

        [Dependency]
        public FirstDataInputViewModel ViewModel
        {
            get => this.DataContext as FirstDataInputViewModel;
            set => this.DataContext = value;
        }

        ~FirstDataInput()
        {
            Debug.WriteLine("Page 1 finalized");
        }
    }
}