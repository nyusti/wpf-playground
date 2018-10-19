using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1
{
    public interface INumberServiceClient
    {
        Task<List<int>> GetNumberAsync(CancellationToken cancellationToken);
    }
}