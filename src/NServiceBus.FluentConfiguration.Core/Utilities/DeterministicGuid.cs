﻿/*

    This class was taken from an official NServiceBus demo. It is included here for conveinince. 

 */

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NServiceBus.FluentConfiguration.Core.Utilities
{
    public static class DeterministicGuid
    {
        public static Guid Create(params object[] data)
        {
            // use MD5 hash to get a 16-byte hash of the string
            using (var provider = new MD5CryptoServiceProvider())
            {
                var inputBytes = Encoding.Default.GetBytes(string.Concat(data));
                var hashBytes = provider.ComputeHash(inputBytes);
                // generate a guid from the hash:
                return new Guid(hashBytes);
            }
        }
    }
}
