// See https://aka.ms/new-console-template for more information
using Crypto_Lab1_;
using System;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Diagnostics;

public class Program
{

    public static void Main()
    {
        DiffProtocol diffProtocol = new DiffProtocol(5);
        diffProtocol.sendMessage("Hello", 0, 1);
        diffProtocol.sendMessage("New message", 2, 4);
    }

}