using Cobisi.EmailVerify;

namespace SympleLib.Email
{
    /// <summary>
    /// Class is simply a wrapper for Cobisi E-Mail Validator 
    /// (http://cobisi.com/email-validation/.net-component)
    /// License Is Required to use this componet
    /// </summary>
    public class EmailValidator
    {
        private EmailVerifier CobisiVerifier;

        public EmailValidator(string CobisiRunTimeKey)
        {
            EmailVerifier.RuntimeLicenseKey = CobisiRunTimeKey;
            CobisiVerifier = new EmailVerifier();
        }

        /// <summary>
        /// Performs a syntactical validation of the provided
        /// email address against RFC 1123, RFC 2821, RFC
        /// 2822, RFC 3696, RFC 4291, RFC 5321 and RFC 5322
        /// technical specifications, among others.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public VerificationResult VaildateEmailSyntax(string emailAddress)
        {
            return CobisiVerifier.Verify(emailAddress, VerificationLevel.Syntax);
        }

        /// <summary>
        /// All of the features above, plus:
        /// Checks the email address domain against a wellknown
        /// list of disposable email address (DEA)
        /// providers.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public VerificationResult VaildateEmailAgianstKnownDisposableLists(string emailAddress)
        {
            return CobisiVerifier.Verify(emailAddress, VerificationLevel.Disposable);
        }

        /// <summary>
        /// All of the features above, plus:
        /// Queries the configured DNS server(s) and looks for
        /// records about the e-mail address domain and
        /// checks their validity and consistency.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public VerificationResult VaildateEmailUsingDns(string emailAddress)
        {
            return CobisiVerifier.Verify(emailAddress, VerificationLevel.Dns);
        }

        /// <summary>
        /// All of the features above, plus:
        /// Checks if a connection could be performed to the
        /// mail server of the email address domain.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public VerificationResult VaildateEmailUsingSmtp(string emailAddress)
        {
            return CobisiVerifier.Verify(emailAddress, VerificationLevel.Smtp);
        }

        /// <summary>
        /// All of the features above, plus:
        /// Checks whether the mail server accepts or rejects
        /// messages sent to the required email address.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public VerificationResult VaildateEmailAgianstMailBox(string emailAddress)
        {
            return CobisiVerifier.Verify(emailAddress, VerificationLevel.Mailbox);
        }

        /// <summary>
        /// All of the features above, plus:
        /// Checks if the mail server correctly rejects messages
        /// sent to nonexistent email addresses. This
        /// verification level allows you to check if a given email
        /// address is actually a catch-all address.
        /// -- IN MY TESTING THIS FAILS ALOT --
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public VerificationResult VaildateEmailIsNotACatchAll(string emailAddress)
        {
            return CobisiVerifier.Verify(emailAddress, VerificationLevel.CatchAll);
        }

    }
}
