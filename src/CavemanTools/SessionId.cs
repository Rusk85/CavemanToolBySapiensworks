﻿using System;
using System.Security.Cryptography;

namespace CavemanTools
{
    public struct SessionId:IEquatable<SessionId>
    {
        private const int BytesLength = 16;
        private byte[] _bytes;

        public byte[] Bytes
        {
            get
            {
                if (_bytes == null)
                {
                    return _bytes = new byte[0];
                }
                return _bytes;
            }
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return Convert.ToBase64String(Bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SessionId Parse(string data)
        {
            data.MustNotBeNull();
            SessionId id;
            if (!TryParse(data, out id))
            {
                throw new ArgumentException("Data is not a valid SessionId");
            }
            return id;
        }

        public static bool TryParse(string data, out SessionId id)
        {
            id=new SessionId();
            if (data == null) return false;
            try
            {
                var bytes = Convert.FromBase64String(data);
                if (bytes.Length != BytesLength)
                {
                    return false;
                }
                id=new SessionId(bytes);
                return true;
            }
            catch (FormatException ex)
            {
                return false;
            }
        }

        public static SessionId NewId()
        {
            var bytes = new byte[BytesLength];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetNonZeroBytes(bytes);
            }
            return new SessionId(bytes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public SessionId(byte[] bytes)
        {
            bytes.MustNotBeEmpty();
            bytes.MustComplyWith(d => d.Length == BytesLength, "Length must be {0}".ToFormat(BytesLength));
            _bytes = bytes;
        }
        


        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(SessionId other)
        {
            if (Bytes.Length != other.Bytes.Length) return false;
            for (byte i = 0; i < Bytes.Length; i++)
            {
                if (Bytes[i] != other.Bytes[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return Equals((SessionId)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return Bytes.GetHashCode();
        }
    }
}