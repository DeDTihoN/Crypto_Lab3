using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Crypto_Lab1_
{
    public class ProtUser
    {
        private BigInteger p;
        private BigInteger g;
        private BigInteger a;
        private bool sentMessage = false;
        public ProtUser nextUser;
        private BigInteger encKey;
        public string name;

        public ProtUser(BigInteger p, BigInteger g, string name="User")
        {
            Random random = new Random();
            this.p = p;
            this.g = g;
            this.a = Ariphmetic.GetRandBigInteger(p - 1);
            this.sentMessage = false;
            this.name = name;
        }
        public void setNextUser (ProtUser nextUser)
        {
            this.nextUser = nextUser;
        }
        public void sendMessage(BigInteger b)
        {
            if (sentMessage)
            {
                encKey = b;
            }
            else
            {
                sentMessage = true;
                BigInteger newB = Ariphmetic.PowMod(b, a, p);
                nextUser.sendMessage(newB);
            }
            sentMessage = false;
        }

        public string EncryptMessage(string message)
        {
            var v = SymmetricEncryption.Encrypt(message,encKey.ToString());
            string hash = Ariphmetic.ComputeHMACSHA256(message, encKey.ToString());

            JsonMessage json = new JsonMessage
            {
                Message = v.encryptedText,
                IV = v.iv,
                Hash = hash,
                Sender = "SenderName", // Замените на ваше имя
                DateTime = DateTime.Now
            };
            string jsonString = JsonSerializer.Serialize(json);
            Console.WriteLine(jsonString);

            return jsonString;
        }

        public string DecryptMessage(string jsonMessage)
        {
            JsonMessage json = JsonSerializer.Deserialize<JsonMessage>(jsonMessage);
            string decryptedMessage = SymmetricEncryption.Decrypt(json.Message, encKey.ToString(), json.IV);
            string hash = Ariphmetic.ComputeHMACSHA256(decryptedMessage, encKey.ToString());
            if (json.Hash == hash)
            {
                Console.WriteLine("Sending time: {0}", json.DateTime);
                Console.WriteLine("Sender {0}, send message {1}", json.Sender, decryptedMessage);
                Console.WriteLine("Passed HMAC check");
                return decryptedMessage;
            }
            else
            {
                return "Error";
            }
        }
    }
}
