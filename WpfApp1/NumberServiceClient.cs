using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class NumberServiceClient : INumberServiceClient
    {
        public async Task<List<int>> GetNumberAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested)
            {
                Debug.WriteLine("Service call cancelled");
            }
            return new List<int> { 200, 300 };
        }
    }
}