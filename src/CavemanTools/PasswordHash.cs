using System;
using System.Security.Cryptography;

namespace CavemanTools
{
    public class PasswordHash
    {
        private byte[] _salt;
        private int _iterations;
        private byte[] _finalHash;
        private byte[] _pwdHash;
        private const int KeySize=32;

        
        public static PasswordHash FromHash(string hash)
        {
            hash.MustNotBeEmpty();
            var pwd = new PasswordHash();
            pwd._finalHash= Convert.FromBase64String(hash);   
            pwd.ExtractParts();
            return pwd;
        }

        private PasswordHash()
        {
            
        }
        void ExtractParts()
        {
           _iterations = BitConverter.ToInt32(_finalHash,0);
            _pwdHash=new byte[KeySize];
            Array.Copy(_finalHash,4,_pwdHash,0,KeySize);
            _salt=new byte[_finalHash.Length-4-KeySize];
            Array.Copy(_finalHash,KeySize+4,_salt,0,_salt.Length);            
        }

        public bool IsValidPassword(string pwd)
        {
            var pwdHash = Pbkdf2Hash(pwd, _salt, _iterations);
            for (int i = 0; i < _pwdHash.Length; i++)
            {
                if (_pwdHash[i] != pwdHash[i]) return false;
            }
            return true;
        }

        public PasswordHash(string password,Salt salt=null,Int32 iterations=50000)
        {
            _iterations = iterations;
            if (salt==null) salt = Salt.Generate();
            _salt = salt.Bytes;
            
            _pwdHash = Pbkdf2Hash(password,_salt,iterations);

            _finalHash = GetFinalHash(_pwdHash);
            
        }

        public override string ToString()
        {
            return Convert.ToBase64String(_finalHash);
        }


        byte[] GetFinalHash(byte[] pwdHash)
        {
            byte[] final=new byte[pwdHash.Length+_salt.Length+4];
            BitConverter.GetBytes(_iterations).CopyTo(final,0);
            pwdHash.CopyTo(final,4);
            _salt.CopyTo(final,KeySize+4);
            return final;
        }

        static byte[] Pbkdf2Hash(string data,byte[] salt,int iterations)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(data, salt, iterations))
            {
                return  pbkdf2.GetBytes(KeySize);
            };
            
        }

     
    }
}