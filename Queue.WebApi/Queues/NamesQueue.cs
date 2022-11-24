using System.Threading.Channels;

namespace Queue.WebApi.Queues
{
    public class NamesQueue : IBackgroundTaskQueue<string>
    {
        private readonly Channel<string> queue; //rabitmqdaki gibi bir queue düşünülebilir. çalışma mantığı aynı.
        public NamesQueue(IConfiguration configuration)
        {
            int.TryParse(configuration["QueueCapacity"], out int capacity); //QueueCapacity appsetting dosyasında tanımlı. Max kapasite değeri verilebiliyor.

            BoundedChannelOptions options = new(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait, //QueueCapacity değeri 100dü. Wait 101. kayıt geldiğinde 1 kayıdı sistemden silinmesini bekleyecek. Silinme işlemi bitince 101. kayıdı ekleyecek.
            };

            queue = Channel.CreateBounded<string>(options); //queue oluşturma..

        }

        public async ValueTask AddQueue(string workItem)
        {
            ArgumentNullException.ThrowIfNull(workItem); //nullsa boşsa hata fırlatır.

            await queue.Writer.WriteAsync(workItem); //gelen listeyi ekle. eklendiği anda DeQueue tetiklenir.


        }

        public ValueTask<string> DeQueue(CancellationToken cancellationToken)
        {
            var workItem = queue.Reader.ReadAsync(cancellationToken); //1 tane okuma yapar.
            return workItem;
        }
    }
}
