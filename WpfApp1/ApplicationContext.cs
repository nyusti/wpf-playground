using System.Threading;
using WpfApp1.Configuration;

namespace WpfApp1
{
    public class ApplicationContext
    {
        public IDependencyScope CurrentPageScope { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
    }
}