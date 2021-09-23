using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IMailSenderService mailService;

        public MailController(IMailSenderService mailService)
        {
            this.mailService = mailService;
        }

        public async Task<ActionResult> GetEmailDemo()
        {
            const string from = "sdknetwork123@gmail.com";
            const string to = "radiun42@gmail.com";
            const string subject = "Testing Purpose MailKit Aspnet Core";
            const string content = "Mail is testing";

            await mailService.Send(from, to, subject, content);

            return Ok();
        }
    }
}