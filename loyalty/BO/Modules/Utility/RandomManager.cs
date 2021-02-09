using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmary.Loyalty.BO.Modules.Utility
{
    public static class RandomManager
    {
        public static string GenerateNumberAndLetter(int noOfChar)
        {
            byte[] dataBytes = new byte[noOfChar];

            Random rnd = new Random(Guid.NewGuid().GetHashCode());  // random seed
          
            for (int i = 0; i < noOfChar; i++)
            {
                // 0-9, A-Z, a-z  total 62 Char
                // random integers from 1 to 62
                int rndNum = rnd.Next(1, 63);

                if (rndNum >= 1 && rndNum <= 10)
                    rndNum += 47;     // 0-9: ASCII decimal value 48 to 57
                else if (rndNum >= 11 && rndNum <= 36)
                    rndNum += 54;    // A-Z: ASCII decimal value 65 to 90
                else if (rndNum >= 37 && rndNum <= 62)
                    rndNum += 60;    // a-z: ASCII decimal value 97 to 122

                dataBytes[i] = (byte)rndNum;
            }

            foreach (var a in dataBytes)
            {
                System.Diagnostics.Debug.Write(a.ToString() + ", ");
            }
            System.Diagnostics.Debug.WriteLine("");
             return Encoding.ASCII.GetString(dataBytes);
        }
    }
}
