using Microsoft.AspNetCore.Mvc;
using Queue.WebApi.Queues;

namespace Queue.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        private readonly IBackgroundTaskQueue<string> _queue;

        public TestController(IBackgroundTaskQueue<string> queue)
        {
            _queue = queue;
        }

        [HttpPost]
        public async Task<IActionResult> AddQueue(string[] names)
        {
            foreach (var item in names)
            {
               await _queue.AddQueue(item);
            }
            return Ok();
        }
    }
}
