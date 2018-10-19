using System.Threading;
using GalaSoft.MvvmLight;
using Unity.Attributes;

namespace WpfApp1.ViewModel
{
    public abstract class ComponentViewModel : ViewModelBase
    {
        [InjectionMethod]
        public abstract void OnInit();

        [Dependency]
        public CancellationToken CancellationToken { get; set; }
    }
}