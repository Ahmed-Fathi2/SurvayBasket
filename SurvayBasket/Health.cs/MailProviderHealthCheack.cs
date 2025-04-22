using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using SurvayBasket.Settigns.cs;

namespace SurvayBasket.Health.cs
{
    public class MailProviderHealthCheack(IOptions<EmailSettings> mailSettings) : IHealthCheck
    {
        private readonly EmailSettings _mailSettings = mailSettings.Value;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var smtp = new SmtpClient(); // fro connect to Email provider

                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls, cancellationToken);

                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password, cancellationToken);

                return await Task.FromResult(HealthCheckResult.Healthy());

            }

            catch (Exception ex)
            {
                {

                    return await Task.FromResult(HealthCheckResult.Unhealthy(exception: ex));
                }
            }
        }

    }
}
