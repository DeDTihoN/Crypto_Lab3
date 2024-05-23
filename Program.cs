// See https://aka.ms/new-console-template for more information
using Crypto_Lab1_;
using System;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Diagnostics;

public class Program
{
    public static List<RSA> rsaList = new List<RSA>();
    public static void Main()
    {
        Random random = new Random();
        Stopwatch stopwatch = new Stopwatch();

        string filePath = "output.txt";

        using (StreamWriter writer = new StreamWriter(filePath, append: true))
        {

            for (int i = 5; i <= 500; ++i)
            {
                RSA rsa = new RSA("Bob", i);

                int len = rsa.p.ToString().Length / 4;

                string message = "";

                for (int j = 0; j < len; ++j)
                {
                    message += char.ConvertFromUtf32('a' + random.Next(0, 26));
                }
                // Запуск заміру часу
                stopwatch.Start();

                string EncryptedMessage = RSA.EncryptMessage(message, rsa.e, rsa.n);
                string DecryptedMessage = rsa.DecryptMessage(EncryptedMessage);

                // Зупинка заміру часу
                stopwatch.Stop();
                //  Console.WriteLine(DecryptedMessage);

                writer.WriteLine("Time for {0} binary len primes rsa encrypting and decrypting: {1} ms", i, stopwatch.ElapsedMilliseconds);
            }
        }

        // RSA rsaBob = new RSA("Bob");
        // RSA rsaAlice = new RSA("Alice");

        // rsaList.Add(rsaBob);
        // rsaList.Add(rsaAlice);

        // while(true){
        //     int sender = 0;

        //     Console.WriteLine("Choose sender: 0 - {0}, 1 - {1}", rsaList[0].senderName, rsaList[1].senderName);
        //     sender = Convert.ToInt32(Console.ReadLine());

        //     if (!sender.Equals(0) && !sender.Equals(1)){
        //         Console.WriteLine("Wrong number");
        //         continue;
        //     }

        //     int receiver = 1 - sender;

        //     Console.WriteLine("Enter message:");

        //     string message = Console.ReadLine();

        //     SentMessage(sender, receiver, message);
        // }
    }

    static void SentMessage(int sender, int receiver, string message)
    {
        if (rsaList[sender].checkMessageSupported(message) == false)
        {
            //Console.WriteLine("Message is not supported by rsa");
            return;
        }

        //Console.WriteLine("{0} sent message \"{1}\", to {2}", rsaList[sender].senderName, message, rsaList[receiver].senderName);

        string EncryptedMessage = RSA.EncryptMessage(message, rsaList[sender].e, rsaList[sender].n);

        string DecryptedMessage = rsaList[sender].DecryptMessage(EncryptedMessage);

        //Console.WriteLine("Decrypted message: \n{0}", DecryptedMessage);
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