// See https://aka.ms/new-console-template for more information
using Crypto_Lab1_;
using System;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;

public class Program
{
    public static List<RSA> rsaList = new List<RSA>();
    public static void Main()
    {

        RSA rsaBob = new RSA("Bob");
        RSA rsaAlice = new RSA("Alice");

        rsaList.Add(rsaBob);
        rsaList.Add(rsaAlice);

        while(true){
            int sender = 0;

            Console.WriteLine("Choose sender: 0 - {0}, 1 - {1}", rsaList[0].senderName, rsaList[1].senderName);
            sender = Convert.ToInt32(Console.ReadLine());

            if (!sender.Equals(0) && !sender.Equals(1)){
                Console.WriteLine("Wrong number");
                continue;
            }

            int receiver = 1 - sender;

            Console.WriteLine("Enter message:");

            string message = Console.ReadLine();

            SentMessage(sender, receiver, message);
        }
    }

    static void SentMessage(int sender, int receiver, string message){
        if (rsaList[sender].checkMessageSupported(message) == false){
            Console.WriteLine("Message is not supported by rsa");
            return;
        }

        Console.WriteLine("{0} sent message \"{1}\", to {2}", rsaList[sender].senderName, message, rsaList[receiver].senderName);

        string EncryptedMessage = rsaList[sender].EncryptMessage(message);

        string DecryptedMessage = RSA.DecryptMessage(EncryptedMessage, rsaList[sender].e, rsaList[sender].n);

        Console.WriteLine("Decrypted message: \n{0}", DecryptedMessage);
    }

    static string ToBase2(BigInteger number)
    {
        return number.ToString("B");
    }

    static string ToBase64(BigInteger number)
    {
        byte[] bytes = number.ToByteArray();
        return Convert.ToBase64String(bytes);
    }

    static string ToByteArray(BigInteger number)
    {
        byte[] byteArray = number.ToByteArray();
        string res = "";
        res += "ByteArray: [";
        for (int i = 0; i < byteArray.Length; i++)
        {
            res += byteArray[i].ToString("X");
            if (i < byteArray.Length - 1)
            {
                res += ", ";
            }
        }
        res += "]";
        return res;
    }
}