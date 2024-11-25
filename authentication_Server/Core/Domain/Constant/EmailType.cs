using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constant
{
    public class EmailType
    {

        public static class EmailBodyMessages
        {
            public const string EmailOTP = "Please Enter you'r OTP and verify user =>";
            public const string ForgotPasswordTemplate = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Reset Password</title>
</head>
<body style='width:100%;height:100%;padding:0;Margin:0'>
    <div dir='ltr' style='background-color:#FAFAFA'>
        <table width='100%' cellspacing='0' cellpadding='0' style='width:100%;height:100%;background-color:#FAFAFA'>
            <tr>
                <td valign='top' style='padding:0;Margin:0'>
                    <table cellpadding='0' cellspacing='0' align='center' style='width:100%;table-layout:fixed !important'>
                        <tr>
                            <td align='center' style='padding:0;Margin:0'>
                                <table align='center' cellpadding='0' cellspacing='0' bgcolor='#00000000' style='background-color:transparent;width:600px'>
                                    <tr>
                                        <td align='left' style='padding:20px;Margin:0'>
                                            <table cellpadding='0' cellspacing='0' width='100%' style='width:100%'>
                                                <tr>
                                                    <td align='center' valign='top' style='padding:0;Margin:0;width:560px'>
                                                        <table cellpadding='0' cellspacing='0' width='100%'>
                                                            <tr>
                                                                <td align='center' style='padding:0;Margin:0;padding-bottom:10px;font-size:0px'>
                                                                    <a target='_blank' href='mailto:cssoft45@gmail.com?subject=Hiii&body=test'>
                                                                        <img src='https://fljheqr.stripocdn.email/content/guids/cab_pub_7cbbc409ec990f19c78c75bd1e06f215/images/Black_Friday_Banner_with_Bow_Huge_Discounts.png' alt='logo' width='255' title='logo' style='display:block;font-size:12px;border:0;outline:none;text-decoration:none;border-radius:0'>
                                                                    </a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                    <!-- Content Section -->
                    <table cellpadding='0' cellspacing='0' align='center' style='width:100%;table-layout:fixed !important'>
                        <tr>
                            <td align='center' style='padding:0;Margin:0'>
                                <table bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' style='background-color:#ffffff;width:600px'>
                                    <tr>
                                        <td align='left' style='padding:20px;Margin:0;padding-top:15px;padding-right:20px;padding-left:20px'>
                                            <table cellpadding='0' cellspacing='0' width='100%' style='width:100%'>
                                                <tr>
                                                    <td align='center' valign='top' style='padding:0;Margin:0;width:560px'>
                                                        <table cellpadding='0' cellspacing='0' width='100%'>
                                                            <tr>
                                                                <td align='center' style='padding:0;Margin:0;padding-bottom:10px;padding-top:10px;font-size:0px'>
                                                                    <img src='https://fljheqr.stripocdn.email/content/guids/CABINET_91d375bbb7ce4a7f7b848a611a0368a7/images/69901618385469411.png' alt='' width='100' style='display:block;font-size:14px;border:0;outline:none;text-decoration:none'>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='center' style='Margin:0;padding-top:15px;padding-right:40px;padding-bottom:15px;padding-left:40px'>
                                                                    <h1 style='Margin:0;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:55.2px;color:#333333;font-size:46px;font-weight:bold'>Reset Password</h1>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='left' style='padding:0;Margin:0;padding-top:10px'>
                                                                    <p style='font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px'>
                                                                        Hello, {1} <br><br>
                                                                        You have requested to reset your password. Please click the button below to reset your password.
                                                                    </p>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                    <!-- Reset Password Button -->
                                    <tr>
                                        <td align='left' style='padding:0;Margin:0;padding-right:20px;padding-left:20px;padding-bottom:20px'>
                                            <table cellpadding='0' cellspacing='0' width='100%'>
                                                <tr>
                                                    <td align='center' valign='top' style='padding:0;Margin:0;width:560px'>
                                                        <table cellpadding='0' cellspacing='0' width='100%' style='width:100%'>
                                                            <tr>
                                                                <td align='center' style='padding:0;Margin:0;padding-top:10px'>
                                                                    <span class='es-button-border' style='border-style:solid;border-color:#2CB543;background:#5C68E2;border-width:0px;display:inline-block;border-radius:6px;width:auto'>
                                                                        <a href='{0}' target='_blank' class='es-button' style='text-decoration:none;color:#FFFFFF;padding:10px 30px;display:inline-block;background:#5C68E2;border-radius:6px;font-family:arial, sans-serif;'>RESET PASSWORD</a>
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='center' style='padding:0;Margin:0;padding-top:10px'>
                                                                    <h3 style='font-family:arial, sans-serif;font-weight:bold;line-height:30px;color:#333333;font-size:20px'>This link is valid for one use only. Expires in 24 hours.</h3>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='center' style='padding:0;Margin:0;padding-bottom:10px;padding-top:10px'>
                                                                    <p style='font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px'>
                                                                        If you did not request a password reset, please disregard this message or contact our support team.
                                                                    </p>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>";

            public const string ConfirmEmailTemplate = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html dir=""ltr"" xmlns=""http://www.w3.org/1999/xhtml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" lang=""en"">
  <head>
    <meta charset=""UTF-8"">
    <meta content=""width=device-width, initial-scale=1"" name=""viewport"">
    <meta name=""x-apple-disable-message-reformatting"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta content=""telephone=no"" name=""format-detection"">
    <title>Email Confirmation</title>    
  </head>
  <body class=""body"" style=""width:100%;height:100%;padding:0;Margin:0"">
    <div dir=""ltr"" class=""es-wrapper-color"" lang=""en"" style=""background-color:#FAFAFA"">
      <table width=""100%"" cellspacing=""0"" cellpadding=""0"" class=""es-wrapper"" style=""width:100%;height:100%;background-color:#FAFAFA"">
        <tr>
          <td valign=""top"" style=""padding:0;Margin:0"">
            <table cellpadding=""0"" cellspacing=""0"" align=""center"" class=""es-content"" style=""width:100%;table-layout:fixed !important"">
              <tr>
                <td align=""center"" class=""es-info-area"" style=""padding:0;Margin:0"">
                  <table align=""center"" cellpadding=""0"" cellspacing=""0"" bgcolor=""#00000000"" class=""es-content-body"" style=""background-color:transparent;width:600px"">
                    <tr>
                      <td align=""left"" style=""padding:20px;Margin:0"">
                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""width:100%"">
                          <tr>
                            <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:560px"">
                              <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                  <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;font-size:0px"">
                                    <a target=""_blank"" href=""mailto:cssoft45@gmail.com?subject=Hiii&body=test"">
                                      <img src=""https://fljheqr.stripocdn.email/content/guids/cab_pub_7cbbc409ec990f19c78c75bd1e06f215/images/Black_Friday_Banner_with_Bow_Huge_Discounts.png"" alt=""logo"" width=""255"" title=""logo"" style=""display:block;font-size:12px;border:0;outline:none;text-decoration:none;border-radius:0"">
                                    </a>
                                  </td>
                                </tr>
                              </table>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>

            <!-- Content Section -->
            <table cellpadding=""0"" cellspacing=""0"" align=""center"" class=""es-content"" style=""width:100%;table-layout:fixed !important"">
              <tr>
                <td align=""center"" style=""padding:0;Margin:0"">
                  <table bgcolor=""#ffffff"" align=""center"" cellpadding=""0"" cellspacing=""0"" class=""es-content-body"" style=""background-color:#ffffff;width:600px"">
                    <tr>
                      <td align=""left"" style=""padding:20px;Margin:0;padding-top:15px;padding-right:20px;padding-left:20px"">
                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""width:100%"">
                          <tr>
                            <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:560px"">
                              <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                  <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;padding-top:10px;font-size:0px"">
                                    <img src=""https://fljheqr.stripocdn.email/content/guids/CABINET_91d375bbb7ce4a7f7b848a611a0368a7/images/69901618385469411.png"" alt="""" width=""100"" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none"">
                                  </td>
                                </tr>
                                <tr>
                                  <td align=""center"" class=""es-m-p0r es-m-p0l"" style=""Margin:0;padding-top:15px;padding-right:40px;padding-bottom:15px;padding-left:40px"">
                                    <h1 class=""es-m-txt-c"" style=""Margin:0;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:55.2px;color:#333333;font-size:46px;font-weight:bold"">Email Confirmation</h1>
                                  </td>
                                </tr>
                                <tr>
                                  <td align=""left"" style=""padding:0;Margin:0;padding-top:10px"">
                                    <p style=""font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">
                                      Hello, {1} <br><br>
                                      Thank you for registering. Please confirm your email by clicking the button below:
                                    </p>
                                  </td>
                                </tr>
                              </table>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>

                    <!-- Confirmation Button -->
                    <tr>
                      <td align=""left"" style=""padding:0;Margin:0;padding-right:20px;padding-left:20px;padding-bottom:20px"">
                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                          <tr>
                            <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:560px"">
                              <table cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""width:100%"">
                                <tr>
                                  <td align=""center"" style=""padding:0;Margin:0;padding-top:10px"">
                                    <span class=""es-button-border"" style=""border-style:solid;border-color:#2CB543;background:#5C68E2;border-width:0px;display:inline-block;border-radius:6px;width:auto"">
                                      <a href=""{0}"" target=""_blank"" class=""es-button"" style=""text-decoration:none;color:#FFFFFF;padding:10px 30px;display:inline-block;background:#5C68E2;border-radius:6px;font-family:arial, sans-serif;"">CONFIRM EMAIL</a>
                                    </span>
                                  </td>
                                </tr>
                                <tr>
                                  <td align=""center"" style=""padding:0;Margin:0;padding-top:10px"">
                                    <h3 class=""es-m-txt-c"" style=""font-family:arial, sans-serif;font-weight:bold;line-height:30px;color:#333333;font-size:20px"">This link is valid for one use only. Expires in 24 hours.</h3>
                                  </td>
                                </tr>
                                <tr>
                                  <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;padding-top:10px"">
                                    <p style=""font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">
                                      If you did not request to confirm your email, please disregard this message or contact our support team.
                                    </p>
                                  </td>
                                </tr>
                              </table>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </div>
  </body>
</html>
";

            public const string OrderConfirmTemplate = @"
                                                 <!DOCTYPE html>
<html dir=""ltr"" xmlns=""http://www.w3.org/1999/xhtml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" lang=""en"">
  <head>
    <meta charset=""UTF-8"">
    <meta content=""width=device-width, initial-scale=1"" name=""viewport"">
    <meta name=""x-apple-disable-message-reformatting"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta content=""telephone=no"" name=""format-detection"">
    <title>Order Confirmation</title>   
    
  </head>
  <body class=""body"" style=""width:100%;height:100%;padding:0;Margin:0"">
    <div dir=""ltr"" class=""es-wrapper-color"" lang=""en"" style=""background-color:#FAFAFA"">
      <table width=""100%"" cellspacing=""0"" cellpadding=""0"" class=""es-wrapper"" style=""width:100%;height:100%;background-color:#FAFAFA"">
        <tr>
          <td valign=""top"" style=""padding:0;Margin:0"">
            <table cellpadding=""0"" cellspacing=""0"" align=""center"" class=""es-content"" style=""width:100%;table-layout:fixed !important"">
              <tr>
                <td align=""center"" class=""es-info-area"" style=""padding:0;Margin:0"">
                  <table align=""center"" cellpadding=""0"" cellspacing=""0"" bgcolor=""#00000000"" class=""es-content-body"" style=""background-color:transparent;width:600px"">
                    <tr>
                      <td align=""left"" style=""padding:20px;Margin:0"">
                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""width:100%"">
                          <tr>
                            <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:560px"">
                              <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                  <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;font-size:0px"">
                                    <a target=""_blank"" href=""mailto:cssoft45@gmail.com?subject=Order Confirmation&body=test"">
                                      <img src=""https://fljheqr.stripocdn.email/content/guids/cab_pub_7cbbc409ec990f19c78c75bd1e06f215/images/Black_Friday_Banner_with_Bow_Huge_Discounts.png"" alt=""logo"" width=""255"" title=""logo"" style=""display:block;font-size:12px;border:0;outline:none;text-decoration:none;border-radius:0"">
                                    </a>
                                  </td>
                                </tr>
                              </table>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>

            <!-- Content Section -->
            <table cellpadding=""0"" cellspacing=""0"" align=""center"" class=""es-content"" style=""width:100%;table-layout:fixed !important"">
              <tr>
                <td align=""center"" style=""padding:0;Margin:0"">
                  <table bgcolor=""#ffffff"" align=""center"" cellpadding=""0"" cellspacing=""0"" class=""es-content-body"" style=""background-color:#ffffff;width:600px"">
                    <tr>
                      <td align=""left"" style=""padding:20px;Margin:0;padding-top:15px;padding-right:20px;padding-left:20px"">
                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""width:100%"">
                          <tr>
                            <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:560px"">
                              <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                  <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;padding-top:10px;font-size:0px"">
                                    <img src=""https://fljheqr.stripocdn.email/content/guids/CABINET_91d375bbb7ce4a7f7b848a611a0368a7/images/69901618385469411.png"" alt="""" width=""100"" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none"">
                                  </td>
                                </tr>
                                <tr>
                                  <td align=""center"" class=""es-m-p0r es-m-p0l"" style=""Margin:0;padding-top:15px;padding-right:40px;padding-bottom:15px;padding-left:40px"">
                                    <h1 class=""es-m-txt-c"" style=""Margin:0;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:55.2px;color:#333333;font-size:46px;font-weight:bold"">Order Confirmation</h1>
                                  </td>
                                </tr>
                                <tr>
                                  <td align=""left"" style=""padding:0;Margin:0;padding-top:10px"">
                                    <p style=""font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">
                                      Hi, {1} <br><br>
                                      This is to inform you that your order <b>#4444</b> has shipped!<br>
                                      You can track it with <b>222</b>.
                                    </p>
                                  </td>
                                </tr>
                              </table>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>

                    <!-- Tracking Button -->
                    <tr>
                      <td align=""left"" style=""padding:0;Margin:0;padding-right:20px;padding-left:20px;padding-bottom:20px"">
                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                          <tr>
                            <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:560px"">
                              <table cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""width:100%"">
                                <tr>
                                  <td align=""center"" style=""padding:0;Margin:0;padding-top:10px"">
                                    <span class=""es-button-border"" style=""border-style:solid;border-color:#2CB543;background:#5C68E2;border-width:0px;display:inline-block;border-radius:6px;width:auto"">
                                      <a target=""_blank"" class=""es-button"" style=""text-decoration:none;color:#FFFFFF;padding:10px 30px;display:inline-block;background:#5C68E2;border-radius:6px;font-family:arial, sans-serif;"">TRACK YOUR ORDER</a>
                                    </span>
                                  </td>
                                </tr>
                                <tr>
                                  <td align=""center"" style=""padding:0;Margin:0;padding-top:10px"">
                                    <h3 class=""es-m-txt-c"" style=""font-family:arial, sans-serif;font-weight:bold;line-height:30px;color:#333333;font-size:20px"">Track your order easily with the link above.</h3>
                                  </td>
                                </tr>
                                <tr>
                                  <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;padding-top:10px"">
                                    <p style=""font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">
                                      Thank you for choosing us!<br><br>
                                      Best regards,<br>
                                    </p>
                                  </td>
                                </tr>
                              </table>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </div>
  </body>
</html>
";
        }

        public static class EmailSubjects
        {
            public const string ConfirmEmail = "Confirm Email Request";
            public const string OTPVerify = "Confirm You'r OTP";
            public const string ForgotPassword = "Password Reset Request";
            public const string OrderConfirm = "Order is shipped";
        }
        public static string GenerateCallbackUrl(string frontendUrl, string email, string token, UrlType type)
        {
            if (string.IsNullOrEmpty(frontendUrl))
                throw new ArgumentNullException(nameof(frontendUrl), "Configuration could not be null or empty");
            return type switch
            {
                UrlType.EmailConfirmation => $"{frontendUrl}/authentication/Emailconfirmation?token={token}&email={email}",
                UrlType.ForgetPassword => $"{frontendUrl}/authentication/ResetPassword?token={token}&email={email}",
                UrlType.ResetPassword => $"{frontendUrl}?email={email}&token={token}",
                _ => throw new NotImplementedException()
            };
        }
    }


    public enum UrlType
    {
        ResetPassword,
        ForgetPassword,
        EmailConfirmation
    }
}
