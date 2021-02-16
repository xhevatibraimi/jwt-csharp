using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace JwtDemo.ConsoleDemo
{
    class Program
    {
        const string signingSecret = "Lee Harvey Oswald";

        static void Main(string[] args)
        {
            GenerateTokenDemo();
            DecodeTokenDemo();
            VerifyTokenDemo();
        }

        private static void GenerateTokenDemo()
        {
            Console.WriteLine("GENERATE TOKEN DEMO:");
            var token = GenerateToken(signingSecret);
            Console.WriteLine(token);
        }

        private static void VerifyTokenDemo()
        {
            Console.WriteLine("VERIFY TOKEN DEMO:");
            var token = GenerateToken(signingSecret);
            var isValid = VerifyToken(token, signingSecret);
            Console.WriteLine(token);
            Console.WriteLine(isValid);
        }

        private static void DecodeTokenDemo()
        {
            Console.WriteLine("DECODE TOKEN DEMO:");
            var token = GenerateToken(signingSecret);
            var tokenObject = DecodeToken(token);
            var tokenObjectJson = JsonConvert.SerializeObject(tokenObject, Formatting.Indented);
            Console.WriteLine(tokenObjectJson);
        }

        private static bool VerifyToken(string token, string secret)
        {
            var tokenSplit = token.Split('.', StringSplitOptions.RemoveEmptyEntries);

            if (tokenSplit.Length != 3)
            {
                return false;
            }

            var tokenSigningInput = $"{tokenSplit[0]}.{tokenSplit[1]}";
            var tokenSignature = tokenSplit[2];

            var signature = GetSignature(tokenSigningInput, secret);

            return tokenSignature == signature;
        }

        public static JwtToken DecodeToken(string token)
        {
            var tokenSplit = token.Split('.', StringSplitOptions.RemoveEmptyEntries);

            if (tokenSplit.Length != 3)
            {
                return null;
            }

            var headerBase64String = NormalizeToBase64String(tokenSplit[0]);
            var payloadBase64String = NormalizeToBase64String(tokenSplit[1]);

            var headerBytes = Convert.FromBase64String(headerBase64String);
            var payloadBytes = Convert.FromBase64String(payloadBase64String);

            var headerJsonString = Encoding.UTF8.GetString(headerBytes);
            var payloadJsonString = Encoding.UTF8.GetString(payloadBytes);

            return new JwtToken
            {
                Header = JsonConvert.DeserializeObject(headerJsonString),
                Payload = JsonConvert.DeserializeObject(payloadJsonString),
            };
        }

        private static string NormalizeToBase64String(string str)
        {
            var mod = str.Length % 4;
            if (mod != 0)
            {
                return str.PadRight(str.Length + 4 - mod, '=');
            }

            return str;
        }

        private static string GenerateToken(string secret)
        {
            var header = new
            {
                alg = "HS256",
                typ = "JWT"
            };

            var payload = new
            {
                Username = "xhevo",
                Roles = "admin",
                Language = "en-us"
            };

            // Serialize data
            var headerString = JsonConvert.SerializeObject(header);
            var payloadString = JsonConvert.SerializeObject(payload);

            // Get bytes
            var headerBytes = Encoding.ASCII.GetBytes(headerString);
            var payloadBytes = Encoding.ASCII.GetBytes(payloadString);

            // Get Base64 string
            var headerBase64 = Convert.ToBase64String(headerBytes).TrimEnd('=');
            var payloadBase64 = Convert.ToBase64String(payloadBytes).TrimEnd('=');

            // Pack token
            var tokenSigningInput = $"{headerBase64}.{payloadBase64}";

            // Generate Signature
            var tokenSignature = GetSignature(tokenSigningInput, secret);

            // return token;
            var token = $"{tokenSigningInput}.{tokenSignature}";
            return token;
        }

        private static string GetSignature(string input, string secret)
        {
            var secretBytes = Encoding.UTF8.GetBytes(secret);

            using var hashedBytes = new HMACSHA256(secretBytes);

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashedBytes.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // hexadecimal string.
            var signature = sBuilder.ToString();
            // return base64 encoding of the signature;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(signature)).TrimEnd('=');
        }
    }
}
