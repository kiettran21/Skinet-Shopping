using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IMailSenderService
    {
        Task Send(string from, string to, string subject, string html);
    }
}