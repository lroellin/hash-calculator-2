using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hash_Calculator_2
{
    abstract class HashAlgorithm
    {
        byte[] CalculateHash(String filePath, System.Security.Cryptography.HashAlgorithm hashAlgorithm)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                return hashAlgorithm.ComputeHash(stream);
            }
        }

        String BytesToString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        Task<String> CalculateHashTask(String filePath, System.Security.Cryptography.HashAlgorithm hashAlgorithm)
        {
            return new Task<String>(() => BytesToString(CalculateHash(filePath, hashAlgorithm)));
        }

        async Task<String> CalculateHashTaskAsync(String filePath, System.Security.Cryptography.HashAlgorithm hashAlgorithm)
        {
            return await CalculateHashTask(filePath, hashAlgorithm);
        }
    }
}
