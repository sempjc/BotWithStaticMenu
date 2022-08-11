using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace StaticMenuBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenGeneratorController :
        ControllerBase
    {
        private readonly DirectLineTokenGenerator dlGenerator;

        public TokenGeneratorController(
            IConfiguration configuration)
        {
            var secret = configuration.GetValue<string>("DirectLineSecret");
            var dlurl = configuration.GetValue<string>("DirectLineUrl");
            dlGenerator = new DirectLineTokenGenerator(
               dlurl, secret);
        }

        [HttpGet]
        public async Task<ChatConfig> GenerateToken()
            => await dlGenerator.TokenAsync();
    }
}
