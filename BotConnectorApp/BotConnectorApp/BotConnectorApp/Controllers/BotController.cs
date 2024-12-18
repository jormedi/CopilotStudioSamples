using Microsoft.AspNetCore.Mvc;
using BotConnectorApp.Service;
using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace BotConnectorApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BotController : ControllerBase
    {
        private readonly IBotService _botService;
        private readonly AppSettings _appSettings;

        public BotController(IBotService botService, IConfiguration configuration)
        {
            _botService = botService;
            _appSettings = configuration.GetRequiredSection("Settings").Get<AppSettings>();
        }

        [HttpPost("sendMessage")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> SendMessage([FromForm] string Body, [FromForm] string From)
        {
            var directLineToken = await _botService.GetTokenAsync(_appSettings.BotTokenEndpoint);
            using (var directLineClient = new DirectLineClient(directLineToken.Token))
            {
                var conversation = await directLineClient.Conversations.StartConversationAsync();
                var conversationId = conversation.ConversationId;

                await directLineClient.Conversations.PostActivityAsync(conversationId, new Activity()
                {
                    Type = ActivityTypes.Message,
                    From = new ChannelAccount { Id = From, Name = "userName" },
                    Text = Body,
                    TextFormat = "plain",
                    Locale = "en-Us",
                });

                List<Activity> responses = await GetBotResponseActivitiesAsync(directLineClient, conversationId);
                return Ok(responses.Select(r => r.Text).ToList());
            }
        }

        private async Task<List<Activity>> GetBotResponseActivitiesAsync(DirectLineClient directLineClient, string conversationId)
        {
            ActivitySet response = null;
            List<Activity> result = new List<Activity>();

            do
            {
                response = await directLineClient.Conversations.GetActivitiesAsync(conversationId, null);
                result = response?.Activities?.Where(x => x.Type == ActivityTypes.Message && string.Equals(x.From.Name, _appSettings.BotName, StringComparison.Ordinal)).ToList();
                if (result != null && result.Any())
                {
                    return result;
                }
                await Task.Delay(1000);
            } while (response != null && response.Activities.Any());

            return new List<Activity>();
        }
    }

    public class UserMessage
    {
        [FromForm(Name = "Body")]
        public string Message { get; set; }

        [FromForm(Name = "From")]
        public string From { get; set; }
    }

    public class AppSettings
    {
        public string BotId { get; set; }
        public string BotTenantId { get; set; }
        public string BotName { get; set; }
        public string BotTokenEndpoint { get; set; }
        public string EndConversationMessage { get; set; }
    }
}
