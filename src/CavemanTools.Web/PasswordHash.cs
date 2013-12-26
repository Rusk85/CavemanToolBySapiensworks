using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using CavemanTools.Strings;

namespace CavemanTools.Web
{
    //public class PasswordHash
    //{
    //    private byte[] _salt;
    //    private int _iterations;
    //    private byte[] _finalHash;
    //    private byte[] _pwdHash;
    //    private const int KeySize=32;

        
    //    public PasswordHash(string hash)
    //    {
    //        hash.MustNotBeEmpty();
    //        _finalHash= Convert.FromBase64String(hash);   
    //        ExtractParts();
    //    }

    //    void ExtractParts()
    //    {
    //        //var itbytes = _finalHash.TakeWhile((d, i) => i < 3).ToArray();
    //        _iterations = BitConverter.ToInt32(_finalHash,0);
    //        _pwdHash=new byte[KeySize];
    //        Array.Copy(_finalHash,4,_pwdHash,0,KeySize);
    //        _salt=new byte[_finalHash.Length-4-KeySize];
    //        _finalHash.CopyTo(_salt,KeySize+4);
    //    }

    //    public bool IsValidPassword(string pwd)
    //    {
    //        var pwdHash = Pbkdf2Hash(pwd, _salt, _iterations);
    //        for (int i = 0; i < _pwdHash.Length; i++)
    //        {
    //            if (_pwdHash[i] != pwdHash[i]) return false;
    //        }
    //        return true;
    //    }

    //    public PasswordHash(string password,Salt salt=null,Int32 iterations=50000)
    //    {
    //        _iterations = iterations;
    //        if (salt==null) salt = Salt.Generate();
    //        _salt = salt.Bytes;
            
    //        _pwdHash = Pbkdf2Hash(password,_salt,iterations);

    //        _finalHash = GetFinalHash(_pwdHash);
            
    //    }

    //    public override string ToString()
    //    {
    //        return Convert.ToBase64String(_finalHash);
    //    }


    //    byte[] GetFinalHash(byte[] pwdHash)
    //    {
    //        byte[] final=new byte[pwdHash.Length+_salt.Length+4];
    //        BitConverter.GetBytes(_iterations).CopyTo(final,0);
    //        pwdHash.CopyTo(final,4);
    //        _salt.CopyTo(final,KeySize+4);
    //        return final;
    //    }

    //    static byte[] Pbkdf2Hash(string data,byte[] salt,int iterations)
    //    {
    //        using (var pbkdf2 = new Rfc2898DeriveBytes(data, salt, iterations))
    //        {
    //            return  pbkdf2.GetBytes(KeySize);
    //        };
            
    //    }

    //    //public PasswordHash(string hash,string salt="")
    //    //{
    //    //    if (String.IsNullOrWhiteSpace(hash)) throw new ArgumentNullException("hash");
    //    //    if (salt == null) throw new ArgumentNullException("salt");
    //    //    Hash = hash;
    //    //    Salt = salt;
    //    //}

    //    ///// <summary>
    //    ///// Gets hash
    //    ///// </summary>
    //    //public string Hash { get; private set; }
    //    ///// <summary>
    //    ///// Gets salt used for hashing
    //    ///// </summary>
    //    //public string Salt { get; private set; }
    //    ///// <summary>
    //    ///// True if the supplied argument matches hash
    //    ///// </summary>
    //    ///// <param name="otherHash"></param>
    //    ///// <returns></returns>
    //    //public bool Matches(string otherHash)
    //    //{
    //    //    return otherHash != null && (Hash.Equals(otherHash, StringComparison.InvariantCultureIgnoreCase));
    //    //}

    //    ///// <summary>
    //    ///// Creates a SHA 512 password hash with a 16 char salt
    //    ///// </summary>
    //    ///// <param name="password"></param>
    //    ///// <returns></returns>
    //    //public static PasswordHash CreateCavemanHash(string password)
    //    //{
    //    //    password.MustNotBeEmpty();
    //    //    var hasher = new CavemanHashStrategy();
    //    //    var salt = StringUtils.CreateRandomString(16);
    //    //    return new PasswordHash(hasher.Hash(password, salt), salt);
    //    //}
    //}
}