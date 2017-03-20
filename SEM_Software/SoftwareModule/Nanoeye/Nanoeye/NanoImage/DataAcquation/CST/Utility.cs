using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SEC.Nanoeye.NanoImage.DataAcquation.CSTDaq
{
    public static class Utility
    {
       
        public static short[,] NetworkBytesToHostInt16(byte[] networkBytes , bool dualEnable)
        {
            if (networkBytes == null)
                throw new ArgumentNullException("networkBytes");

            int cnt = 0;

            if (dualEnable)
            {
                cnt = 2;
            }
            else
            {
                cnt = 1;
            }

            short[,] result = new short[cnt, networkBytes.Length / (2 * cnt)];
            int count = 0;

            for (int i = 0; i < result.Length; i++)
            {
                networkBytes[i * 2] = (networkBytes[i * 2] >= 0x80) ? networkBytes[i * 2] -= 0x80 : networkBytes[i * 2];

                if (dualEnable)
                {
                    if (i % 2 == 0)
                    {
                        result[0, count] = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(networkBytes, i * 2));
                    }
                    else
                    {
                        result[1, count] = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(networkBytes, i * 2));
                        count++;
                    }
                }
                else
                {
                    result[0, i] = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(networkBytes, i * 2));
                }
                
                //result[i] = BitConverter.ToInt16(networkBytes, i * 2);
            }

            return result;
        }
        public static void AddRange<T>(this ICollection<T> destination, IEnumerable<T> source)
        {
            List<T> list = destination as List<T>;

            if (list != null)
            {
                list.AddRange(source);
            }
            else
            {
                foreach (T item in source)
                {
                    destination.Add(item);
                }
            }
        }
    }
}
