using System.Security.Cryptography;
using System.Text;

namespace EPIC.ClearView.Utilities
{
    // Token: 0x02000065 RID: 101
    public class Hash
    {
        // Token: 0x06000319 RID: 793 RVA: 0x00019C44 File Offset: 0x00017E44
        public static byte[] GetHash(string data)
        {
            using (var sha = SHA512.Create())
            {
                return sha.ComputeHash(Encoding.ASCII.GetBytes(data));
            }
        }

        // Token: 0x0600031A RID: 794 RVA: 0x00019C6C File Offset: 0x00017E6C
        public static string GetHashString(string data)
        {
            StringBuilder stringBuilder = new StringBuilder(128);
            foreach (byte b in Hash.GetHash(data))
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
    }
}
