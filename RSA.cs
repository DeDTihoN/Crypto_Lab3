using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Security;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Lab1_
{
    public class RSA
    {
        public string senderName { get; set; } = "noname";
        public BigInteger n { get; set; }
        public BigInteger e { get; set; }
        public BigInteger d { get; set; }
        public BigInteger p { get; set; }
        public BigInteger q { get; set; }
        public BigInteger carmichaelPhi { get; set; }
        public BigInteger dmodpsub1 { get; set; }
        public BigInteger dmodqsub1 { get; set; }
        const int PRIMES_LENGTH_DEFAULT = 200;
        public int PRIMES_LENGTH { get; set; }

        static public string supportedSymbols = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890:;~!@#$%^&*()_+-=[]{}\\.\',\"<>/?|₴№\r\t";

        public bool checkMessageSupported(string message){
            return message.All(x => supportedSymbols.Contains(x)) && ToInteger(message) < n;
        }

        public RSA(string senderName, int PRIMES_LENGTH = PRIMES_LENGTH_DEFAULT)
        {
            this.senderName = senderName;
            this.PRIMES_LENGTH = PRIMES_LENGTH;
            p = Ariphmetic.GeneratePrimeBinary(PRIMES_LENGTH);
            q = Ariphmetic.GeneratePrimeBinary(PRIMES_LENGTH);
            n = p * q;

            carmichaelPhi = Ariphmetic.LCM(p - 1, q - 1);

            e = Ariphmetic.CoPrime(carmichaelPhi);

            d = Ariphmetic.GetInverseNumber(e, carmichaelPhi);

            dmodpsub1 = d % (p - 1);
            dmodqsub1 = d % (q - 1);
        }

        public BigInteger DecryptInteger(BigInteger C){
            BigInteger mp = BigInteger.ModPow(C, dmodpsub1, p);
            BigInteger mq = BigInteger.ModPow(C, dmodqsub1, q);

            BigInteger encrypted = Ariphmetic.ChineseRemainderTheorem(new BigInteger[] { mp, mq }, new BigInteger[] { p, q });
            
            return encrypted;
        }

        public static BigInteger EncryptInteger(BigInteger C, BigInteger E, BigInteger N)
        {
            BigInteger m = BigInteger.ModPow(C, E, N);
            return m;
        }

        public static BigInteger ToInteger(string text){
            int strBase = supportedSymbols.Count();

            BigInteger res = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (!supportedSymbols.Contains(text[i])){
                    throw new ArgumentException("The text contains unsupported symbols");
                }
                res = res*strBase + (BigInteger)(supportedSymbols.IndexOf(text[i]));
            }
            // for (int i= text.Length-1; i>=0;--i){
            //     Console.WriteLine("To integer symbol:" + (BigInteger)(supportedSymbols.IndexOf(text[i])));
            // }
            return res;
        }

        public static string ToText(BigInteger number){
            int strBase = supportedSymbols.Count();
            string res = "";
            while (number > 0)
            {
                // Console.WriteLine("To text symbol:" + (int)(number % strBase));
                res = supportedSymbols[(int)(number % strBase)] + res;
                number /= strBase;
            }
            return res;
        }

        public static BigInteger HASH_MOD = 998244353;
        public static BigInteger HASH_P = 16777619;

        public static BigInteger ToHash(BigInteger number){
            BigInteger res = 0;
            while (number > 0)
            {
                res = (res + (HASH_P * ( number % HASH_MOD +1 )) % HASH_MOD)%HASH_MOD;
                number /= HASH_MOD;
            }
            return res;
        }

        public static string EncryptMessage(string message, BigInteger E, BigInteger N){
            string res = "Signature: ";

            BigInteger messageInteger = ToInteger(message);

            string Signature = ToText(ToHash(messageInteger));

            res += Signature;
            res += "\nMessage: ";

            messageInteger = RSA.EncryptInteger(messageInteger, E, N);

            res += ToText(messageInteger);

            return res;
        }

        public string DecryptMessage(string message){
            string Signature = message.Split('\n')[0].Split(": ")[1];
            string Message = message.Split('\n')[1].Split(": ")[1];

            BigInteger mInteger = ToInteger(Message);
            BigInteger signatureInteger = ToInteger(Signature);

            BigInteger messageInteger = DecryptInteger(mInteger);

            string actualMessage = ToText(messageInteger);

            string res = "";

            if (ToHash(messageInteger) == signatureInteger){
                res += "Signature is correct\n";
                res += "Message: " + actualMessage;
            } else {
                res += "Signature is incorrect\n";
                res += "Message: " + actualMessage;
            }
            return res;
        }
    }
}