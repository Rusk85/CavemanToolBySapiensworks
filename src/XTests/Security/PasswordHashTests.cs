using System;
using CavemanTools;
using FluentAssertions;
using Xunit;

namespace XTests.Security
{
    public class PasswordHashTests
    {
        private const string Password = "bla 123";

        [Fact]
        public void valid_password()
        {
            var sut = new PasswordHash(Password);
            sut.IsValidPassword(Password).Should().BeTrue();
            sut.IsValidPassword(Password + "f").Should().BeFalse();
            Console.WriteLine(sut.ToString());
            Console.Write(sut.ToString().Length);
        }

        [Fact]
        public void valid_pwd_existing_hash()
        {
            var hash = new PasswordHash(Password).ToString();
            var sut = PasswordHash.FromHash(hash);
            sut.IsValidPassword(Password).Should().BeTrue();
            sut.IsValidPassword("-" + Password).Should().BeFalse();
        }

        [Fact]
        public void different_salts_generate_different_hashes()
        {
            var hash1 = new PasswordHash(Password);
            var hash2 = new PasswordHash(Password);
            hash1.ToString().Should().NotBe(hash2.ToString());
        }
    }
}