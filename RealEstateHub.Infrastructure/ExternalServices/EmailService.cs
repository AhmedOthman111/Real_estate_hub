using Microsoft.Extensions.Options;
using RealEstateHub.Application.Exceptions;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Infrastructure.Persistence.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace RealEstateHub.Infrastructure.ExternalServices
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true)
        {

            using var client = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(_smtpSettings.SenderEmail, _smtpSettings.Password),
                UseDefaultCredentials = false
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            mailMessage.To.Add(toEmail);

            try
            {
                await client.SendMailAsync(mailMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Failed to send email: {ex.Message}");
            }

        }

        public async Task SendConfirmationEmailAsync(string toEmail, string confirmationLink)
        {
            string subject = "Confirm Your Email - Real Estate Hub";

            string body = $@"
                    <html>
                    <body style='font-family:Arial;'>
                        <h2>Welcome to Real Estate Hub!</h2>
                        <p>Please confirm your email by clicking the link below:</p>
                        <a href='{confirmationLink}' 
                           style='background:#4CAF50;color:white;padding:10px 15px;text-decoration:none;border-radius:5px;'>
                            Confirm Email
                        </a>
                        <p>If you didn’t create an account, you can ignore this message.</p>
                    </body>
                    </html>";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendOtpEmailAsync(string toEmail, string otpCode)
        {

            string subject = "Password Reset OTP - Real Estate Hub";

            var body = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #e0e0e0; border-radius: 5px;'>
                    <h2 style='color: #333;'>Password Reset Request</h2>
                    <p>Hello {toEmail},</p>
                    <p>You requested a password reset for your RealEstateHub account. Please use the following OTP to reset your password:</p>
                    <h1 style='color: #4CAF50; letter-spacing: 5px;'>{otpCode}</h1>
                    <p>This OTP is valid for 5 minutes.</p>
                    <p style='color: #777; font-size: 12px;'>If you did not request this, please ignore this email.</p>
                </div>";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendAdApprovedEmailAsync(string toEmail, string ownerName, string adTitle)
        {

            string subject = "Your Ad Has Been Approved 🎉 - Real Estate Hub";

            string body = $@"
                        <html>
                        <body style='font-family: Arial, sans-serif; background-color:#f9f9f9; padding:20px;'>
                            <div style='max-width:600px; margin:auto; background:white; padding:20px; border-radius:8px;'>
                
                                <h2 style='color:#4CAF50;'>Congratulations {ownerName}!</h2>
                
                                <p>
                                    We’re happy to inform you that your ad 
                                    <strong>""{adTitle}""</strong> has been approved and is now live on 
                                    <strong>Real Estate Hub</strong>.
                                </p>


                                <p style='margin-top:20px;'>
                                    If you have any questions or need help, feel free to contact our support team.
                                </p>

                                <p style='color:#777; font-size:12px; margin-top:30px;'>
                                    Thank you for choosing Real Estate Hub.
                                </p>

                            </div>
                        </body>
                        </html>";

            await SendEmailAsync(toEmail, subject, body);

        }


        public async Task SendAdRejectedEmailAsync( string toEmail, string ownerName, string adTitle, string rejectionReason)
        {
            string subject = "Your Ad Was Rejected ❌ - Real Estate Hub";

            string body = $@"
                            <html>
                            <body style='font-family: Arial, sans-serif; background-color:#f9f9f9; padding:20px;'>
                                <div style='max-width:600px; margin:auto; background:white; padding:20px; border-radius:8px;'>

                                    <h2 style='color:#E53935;'>Hello {ownerName},</h2>

                                    <p>
                                        Thank you for submitting your ad 
                                        <strong>""{adTitle}""</strong> on <strong>Real Estate Hub</strong>.
                                    </p>

                                    <p>
                                        After reviewing your ad, unfortunately it could not be approved at this time.
                                    </p>

                                    <div style='background:#FFF3F3; border-left:4px solid #E53935; padding:10px; margin:15px 0;'>
                                        <strong>Reason for rejection:</strong>
                                        <p style='margin:5px 0;'>{rejectionReason}</p>
                                    </div>

                                    <p>
                                        You may edit your ad and resubmit it for review after addressing the issue above.
                                    </p>

                                    <p style='margin-top:20px;'>
                                        If you believe this was a mistake or need help, please contact our support team.
                                    </p>

                                    <p style='color:#777; font-size:12px; margin-top:30px;'>
                                        Thank you for your understanding.<br/>
                                        Real Estate Hub Team
                                    </p>

                                </div>
                            </body>
                            </html>";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendAdRejectedWithoutReasonEmailAsync( string toEmail , string ownerName, string adTitle)
        {
            string subject = "Your Ad Was Not Approved ❌ - Real Estate Hub";

            string body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; background-color:#f9f9f9; padding:20px;'>
                        <div style='max-width:600px; margin:auto; background:white; padding:20px; border-radius:8px;'>

                            <h2 style='color:#E53935;'>Hello {ownerName},</h2>

                            <p>
                                Thank you for submitting your ad 
                                <strong>""{adTitle}""</strong> on <strong>Real Estate Hub</strong>.
                            </p>

                            <p>
                                After reviewing your submission, we regret to inform you that your ad could not be approved at this time.
                            </p>

                            <p>
                                You may review and update your ad details and submit it again for approval.
                            </p>

                            <p style='margin-top:20px;'>
                                If you have any questions, please feel free to contact our support team.
                            </p>

                            <p style='color:#777; font-size:12px; margin-top:30px;'>
                                Thank you for your understanding.<br/>
                                Real Estate Hub Team
                            </p>

                        </div>
                    </body>
                    </html>";

            await SendEmailAsync(toEmail, subject, body);
        }

    }
}
