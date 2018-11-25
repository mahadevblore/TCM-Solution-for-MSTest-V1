using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Net.Mail;

namespace TCMDailyRuns
{
    public class EmailNotification : Program
    {
        public static string msgBody;
        #region SendEmail
        /// <summary>
        /// Method to send email notification.
        /// </summary>
        public static void SendEmail()
        {
            try
            {
                string projectName = ConfigFile.proj;
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("<EMAIL_ID_DL>");
                XML.GetEmailConfig();
                msgBody = null;
                MessageBody();
                foreach (string toAdd in ConfigFile.toEmailID.Split(','))
                {
                    msg.To.Add(toAdd);
                }
                foreach (string ccAdd in ConfigFile.ccEmailID.Split(','))
                {
                    msg.CC.Add(ccAdd);
                }
                
                double passPercentage = Math.Round((double)sp.passed * 100 / (double)sp.total, 2, MidpointRounding.AwayFromZero);
                msg.Bcc.Add("<EMAIL_BCC_ADDRESS>");
                msg.Subject = projectName + "'s TEST RUN report for date: '" + DateTime.Now.ToString("dd/MM/yyyy") + "'";
                msg.IsBodyHtml = true;
                msg.BodyEncoding = Encoding.ASCII;
                msg.Body = "<html><body>Hi, <br>Please find below the TEST RUN report of " + projectName + " for date: '" + DateTime.Now.ToString("dd/MM/yyyy") + "'</br>";
                msg.Body += "<br><table border=\"5\"><tr><th>Run ID</th><th>Run Title</th><th>Total</th><th>Pass</th><th>Fail</th><th>Percentage</th><th>Link</th></tr>";
                msg.Body += msgBody;
                msg.Body += "<tr><td></td><td>Total</td><td>" + sp.total + "</td><td>" + sp.passed + "</td><td>" + (sp.failed + sp.Inconclusive + sp.error + sp.timeout + sp.notExecuted + sp.aborted) + "</td><td>" + passPercentage + "%</td><td><a></a></td></tr>";
                msg.Body += "</table></br>";
                msg.Body += "<br>Regards,</br>Automation Team</body></html>";
                sendMail(msg);
                System.Threading.Thread.Sleep(2000);
                LogMessageToFile("TEST RUN report sent successfully.");
            }
            catch(Exception exe)
            {
                throw exe;

            }
        }

        private static void sendMail(MailMessage msg)
        {
            SmtpClient mClient = new SmtpClient();
            mClient.Host = ConfigFile.smtpServerIP;
            mClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mClient.Timeout = 100000;
            mClient.Send(msg);
        }
        #endregion

        public static void MessageBody()
        {
            try
            {
                if (ConfigFile.splitRun)
                {
                    TFSFunctions.PublishResultsInMail(sp.curRunId);
                    for (int i = 1; i <= ConfigFile.systems - 1; i++)
                    {
                        sp.EndTime = DateTime.Now.ToString();
                        TFSFunctions.PublishResultsInMail(i, true);
                    }
                }
                else if (ConfigFile.distribute)
                {
                    TFSFunctions.PublishResultsInMail(sp.curRunId);
                    for (int i = 1; i <= ConfigFile.systems - 1; i++)
                    {
                        sp.EndTime = DateTime.Now.ToString();
                        TFSFunctions.PublishResultsInMail(i);
                    }
                }
                else
                {
                    TFSFunctions.PublishResultsInMail(sp.curRunId);
                }
            }
            catch(Exception exe)
            {
                throw exe;
            }
        }
    }
}
