using Queue.WebApi.Queues;
using System.Xml.Linq;

namespace Queue.WebApi
{
    public class QueueHostedService : BackgroundService
    {
        private readonly ILogger<QueueHostedService> _logger;
        private readonly IBackgroundTaskQueue<string> _queue;
        public QueueHostedService(ILogger<QueueHostedService> logger, IBackgroundTaskQueue<string> queue)
        {
            _logger = logger;
            _queue = queue;
        }


        //servis ayağa kaltığında 1 kere çalışır. Ama sürekli çalışır.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var name = await _queue.DeQueue(stoppingToken);   //DeQueue metoduna gider. Birisi kayıt atana kadar DeQueue da bekler. Aslında bir bekleme mekanizmasıdır bu. Ne zamanki kayıt eklendi, Ozaman DeQueue geriye bir kayıt döndürür.
                // burada queue dinlemeye başlanır. queue ya ne zaman kayıt gelirse çalışır ve devam eder. //saatler boyunca kayıt gelmese bile buraa sistem sürekli dinler.

                _logger.LogInformation($"ExecuteAsync worked for {name}");
            }
        }
    }
}
