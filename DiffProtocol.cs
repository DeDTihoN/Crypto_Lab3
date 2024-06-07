using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Crypto_Lab1_
{
    public class DiffProtocol
    {
        public List<ProtUser> users;
        public BigInteger p, g;
        public const int P_Length = 300;
        public DiffProtocol(int n)
        {
            users = new List<ProtUser>();
            p = Ariphmetic.GeneratePrime(P_Length);
            g = Ariphmetic.GetRandBigInteger(p);

            for (int i = 0; i < n; i++)
            {
                users.Add(new ProtUser(p, g,"user" + i.ToString()));
            }
            for (int i = 0; i < n; i++)
            {
                users[i].setNextUser(users[(i + 1) % n]);
            }
            for (int i = 0; i < n; i++)
            {
                users[i].sendMessage(g);
            }
        }

        public void sendMessage(string message,int i,int j)
        {
            if (i<0 || i>=users.Count || j<0 || j >= users.Count)
            {
                throw new IndexOutOfRangeException();
            }
            string json = users[i].EncryptMessage(message);
            Console.WriteLine("json string: {0}", json);
            string DescryptedMessage = users[j].DecryptMessage(json);
        }
    }
}
