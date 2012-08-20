using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using SympleLib.Helpers;

namespace SympleLib.Email
{
    public class Emailer : MailMessage
    {
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public bool SMTPSSL { get; set; }

        private SmtpClient _smtpClient;
        public SmtpClient SMTPClient
        {
            get
            {
                if (_smtpClient == null)
                {
                    _smtpClient = new SmtpClient { 
                        Host = SMTPServer,
                        Port = SMTPPort,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = SMTPSSL
                    };

                    if (SMTPUserName.IsNotEmpty())
                    {
                        _smtpClient.UseDefaultCredentials = false;
                        _smtpClient.Credentials = new NetworkCredential(SMTPUserName, SMTPPassword);
                    }


                }
                return _smtpClient;
            }
            set
            {
                _smtpClient = value;
            }
        }

        protected string EmailHtmlTemplate { get; set; }
        protected string EmailPlainTemplate { get; set; }

        #region Constructors

        /// <summary>
        /// Create a new E-Mail Message
        /// </summary>
        public Emailer()
        {
            this.IsBodyHtml = true;
        }

        /// <summary>
        /// Create a new E-Mail Message and Assign The To And From Values
        /// </summary>
        public Emailer(string to, string fromName, string fromEmail)
        {
            this.To.Add(new MailAddress(to));
            this.From = new MailAddress(fromEmail, fromName);
            this.IsBodyHtml = true;

            GenerateSignature(fromName);
        }

        #endregion

        #region Body

        /// <summary>
        /// Generate The E-Mail Body using the Appropriate Template
        /// </summary>
        public void GenerateBody()
        {
            string template;

            if (string.IsNullOrEmpty(this.Signature))
                GenerateSignature(this.From.DisplayName);

            if (this.IsBodyHtml)
            {
                template = this.EmailHtmlTemplate ?? "<div>#Body#</div><br/><br/><div>#Signature#</div>";
                this.Signature = this.Signature.Replace("\n", "<br />");
            }
            else
                template = this.EmailPlainTemplate ?? "#Body# \n\n #Signature#";



            this.Body = template.
                Replace("#Body#", this.Body).
                Replace("#Signature#", this.Signature);
        }

        #endregion

        #region Signature

        public string Signature { get; set; }

        /// <summary>
        /// Generate The Signature of the e-mail message
        /// </summary>
        public void GenerateSignature(string name)
        {
            GenerateSignature(name, null, null);
        }

        /// <summary>
        /// Generate The Signature of the e-mail message
        /// </summary>
        public void GenerateSignature(string name, string position, string email)
        {
            var sbSig = new StringBuilder("--\n");
            sbSig.AppendLine(name);
            sbSig.AppendLine(position);
            sbSig.AppendLine(email);

            this.Signature = sbSig.ToString();
        }

        #endregion

        #region Send Message

        /// <summary>
        /// Send The Message On It's Way
        /// </summary>
        public void SendMessage()
        {
            //Clean Out Any Duplicate E-Mail 
            var to = this.To.Select(x => x.Address.ToLower()).Distinct().ToList();
            int tCnt = this.To.Count;
            for (int i = 0; i < tCnt; i++)
            {
                this.To.RemoveAt(0);
            }
            foreach (var t in to)
            {
                this.To.Add(t);
            }

            var cc = this.CC.Select(x => x.Address.ToLower()).Distinct().ToList();
            int ccCnt = this.CC.Count;
            for (int i = 0; i < ccCnt; i++)
            {
                this.CC.RemoveAt(0);
            }
            foreach (var c in cc)
            {
                this.CC.Add(c);
            }

            string bodyTemp = this.Body;

            GenerateBody();
           

            this.SMTPClient.Send(this);

            //Set the body back to it's raw value without formatting 
            //to prevent problems when a message is sent multiple times
            this.Body = bodyTemp;
        }

        #endregion
    }
}
