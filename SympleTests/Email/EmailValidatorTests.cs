using NUnit.Framework;
using SympleLib.Email;

namespace SympleTests.Email
{
    public class EmailValidatorTests
    {
        EmailValidator emailValidator = new EmailValidator("QOwaFKGNGesJrLcoDMj25Pz7c6FYulunhw0XRK7E0oDNANVDie78e1ZdTXsViKSgcf+vfQ==");

        [Test]
        public void ValidateSyntax()
        {
            var result = emailValidator.VaildateEmailSyntax("dlewis@sympletech.com");

            Assert.IsTrue(result.IsSuccess);

        }

        [Test]
        public void ValidateDisposable()
        {
            var result = emailValidator.VaildateEmailAgianstKnownDisposableLists("dlewis@sympletech.com");

            Assert.IsTrue(result.IsSuccess);

        }

        [Test]
        public void ValidateDNS()
        {
            var result = emailValidator.VaildateEmailUsingDns("dlewis@sympletech.com");

            Assert.IsTrue(result.IsSuccess);

        }

        [Test]
        public void ValidateSMTP()
        {
            var result = emailValidator.VaildateEmailUsingSmtp("dlewis@sympletech.com");

            Assert.IsTrue(result.IsSuccess);

        }

        [Test]
        public void ValidateMailBox()
        {
            var result = emailValidator.VaildateEmailAgianstMailBox("dlewis@sympletech.com");

            Assert.IsTrue(result.IsSuccess);

        }

        [Test]
        public void VaildateEmailIsNotACatchAll()
        {
            var result = emailValidator.VaildateEmailIsNotACatchAll("daniel.aaron.lewis@gmail.com");
            Assert.IsTrue(result.IsSuccess);

        }
    }
}
