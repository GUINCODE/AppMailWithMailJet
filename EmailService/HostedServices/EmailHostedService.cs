using System.Threading.Tasks.Dataflow;
using EmailService.Common.Email.EmailProvider;
using EmailService.Common.Email.Model;
using EmailService.Interfaces;
using Microsoft.Extensions.Hosting;


namespace EmailService.HostedServices
{
    public class EmailHostedService : IHostedService, IDisposable
    {
        private Task? _sendTask ;
        private CancellationTokenSource? _cancellationToken;
        private readonly BufferBlock<EmailModel> _mailQueue;
        private readonly IEmailSender _mailSender;

        public EmailHostedService()
        {
            _mailSender = new MailJetProvider();
            _mailQueue = new BufferBlock<EmailModel>();
            _cancellationToken = new CancellationTokenSource();

        }

        public async Task SendEmailAsync(EmailModel emailModel)
        {
            await _mailQueue.SendAsync(emailModel);
        }

        public void Dispose()
        {
            DestroyTask();
        }

        private void DestroyTask()
        {
            try
            {
                if(_cancellationToken != null)
                {
                    _cancellationToken.Cancel();
                    _cancellationToken = null;
                }
                Console.WriteLine("[EMAIL SERVICE] DESTROY SERVICE");

            } catch { }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("[EMAIL SERVICE] START SERVICE");
            _sendTask = BackgroundSendEmailAsync(_cancellationToken!.Token);
            return Task.CompletedTask;
        }

        private async Task BackgroundSendEmailAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var email = await _mailQueue.ReceiveAsync();
                    await _mailSender.SendMail(email);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[EMAIL SERVICE] Exception:  {ex.Message}");
                }
            }

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            DestroyTask();
            await Task.WhenAny(_sendTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }

    
}
 