using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DockerAPI2.Controllers
{
    [Produces("application/json")]
    [Route("api/reverseString")]
    public class ReverseStringController : Controller
    {
        public string Get(String str)
        {
            if (string.IsNullOrEmpty(str))
                return "Ups! Something wrong happened";

            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            string result = new string(charArray);
            return result;
        }
    }

    [Produces("application/json")]
    [Route("api/binaryToText")]
    public class BinaryToTextController : Controller
    {
        public string Get(String str)
        {
            if (string.IsNullOrEmpty(str) || (str.Length % 8) != 0)
                return "Ups! Something wrong happened";

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < str.Length; i += 8)
            {
                string section = str.Substring(i, 8);
                int ascii = 0;
                try
                {
                    ascii = Convert.ToInt32(section, 2);
                }
                catch
                {
                    return "Ups! Something wrong happened";
                }
                builder.Append((char)ascii);
            }
            string result = builder.ToString();
            return result;
        }
    }

    [Produces("application/json")]
    [Route("api/encryptString")]
    public class EncryptStringController : Controller
    {
        public string Get(String str)
        {
            if (string.IsNullOrEmpty(str))
                return "Ups! Something wrong happened";

            byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(str);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            string result = Convert.ToBase64String(outputBuffer);

            return result;
        }
    }

    [Produces("application/json")]
    [Route("api/sortString")]
    public class SortStringController : Controller
    {
        public string Get(String str)
        {
            if (string.IsNullOrEmpty(str))
                return "Ups! Something wrong happened";

            // Convert to char array.
            char[] a = str.ToCharArray();

            // Sort letters.
            Array.Sort(a);

            // Return modified string.
            string result = new string(a);

            return result;
        }
    }

    [Produces("application/json")]
    [Route("api/login")]
    public class LoginController : Controller
    {
        public async Task<bool> Get(String user, String password)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                return false;

            string urlEncrypt = "https://stringencryption2.azurewebsites.net/api/StringEncryption";
            string userEncrypted = await encryptFunction(user, urlEncrypt);
            string passwordEncrypted = await encryptFunction(password, urlEncrypt);

            userEncrypted = userEncrypted.Substring(1, userEncrypted.Length - 2);
            passwordEncrypted = passwordEncrypted.Substring(1, passwordEncrypted.Length - 2);

            string urlAuth = "https://loginauthentication2.azurewebsites.net/api/LoginAuthentication";

            return await authFunction(userEncrypted, passwordEncrypted, urlAuth);
        }

        public static async Task<string> encryptFunction(string text, string url)
        {

            var _httpClient = new HttpClient();

            string str = "{\"str\":\"" + text + "\"}";
            _httpClient.DefaultRequestHeaders
             .Accept
             .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var content = new StringContent(str, Encoding.UTF8, "application/json"))
            {
                var result = await _httpClient.PostAsync($"{url}", content).ConfigureAwait(false);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return await result.Content.ReadAsStringAsync();
                }
                else
                {
                    // Something wrong happened
                    return "Ups! Something wrong happened";
                }
            }
        }

        public static async Task<bool> authFunction(string user, string password, string url)
        {
            var _httpClient = new HttpClient();

            string str = "{\"user\":\"" + user + "\", \"password\":\"" + password + "\"}";
            _httpClient.DefaultRequestHeaders
             .Accept
             .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var content = new StringContent(str, Encoding.UTF8, "application/json"))
            {
                var result = await _httpClient.PostAsync($"{url}", content).ConfigureAwait(false);

                return(result.StatusCode == HttpStatusCode.OK);
            }
        }
    }
}