using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string toEmail, string confirmationLink);
        Task SendOtpEmailAsync(string toEmail, string otpCode);
        Task SendAdApprovedEmailAsync( string toEmail, string ownerName, string adTitle );
        Task SendAdRejectedEmailAsync(string toEmail, string ownerName, string adTitle, string rejectionReason);
        Task SendAdRejectedWithoutReasonEmailAsync(string toEmail, string ownerName, string adTitle);

    }
}
