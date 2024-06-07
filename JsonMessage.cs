using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Lab1_
{
    public class JsonMessage
    {
        public string Message { get; set; }
        public string IV { get; set; }
        public string Hash { get; set; }
        public string Sender { get; set; }
        public DateTime DateTime { get; set; }
    }
}
