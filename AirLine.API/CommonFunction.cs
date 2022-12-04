using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace AirLine.API
{
    public class CommonFunction
    {
        private readonly IConfiguration _configuration;
        public CommonFunction(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                //expires: DateTime.Now.AddHours(3),
                expires: DateTime.UtcNow.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        public async Task<string> Post<T>(T model, string requestMethod, string baseUrl, string token) where T : new()
        {
            try
            {
                string BaseUrl = baseUrl;
                var client = new RestClient(BaseUrl);
                var request = new RestRequest(requestMethod, Method.Post);
                request.AddBody(model);
                //string authHeader = System.Net.HttpRequestHeader.Authorization.ToString();
                request.AddHeader("Content-Type", "application/json");
                // request.AddHeader(authHeader, string.Format("Bearer{0}", token));
                var response = await client.ExecuteAsync<List<string>>(request);
                return response.StatusCode.ToString();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public string Post<T>(T model, string requestMethod, string baseUrl) where T : new()
        {
            try
            {
                string BaseUrl = baseUrl;
                var client = new RestClient(BaseUrl);
                var request = new RestRequest(requestMethod, Method.Post);
                request.AddBody(model);
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader(authHeader, string.Format("Bearer{0}", token));
                var response = client.ExecuteAsync<List<string>>(request).Result;
                return response.StatusCode.ToString();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public string Post<T>(string requestMethod, string baseUrl) where T : new()
        {
            try
            {
                string BaseUrl = baseUrl;
                var client = new RestClient(BaseUrl);
                var request = new RestRequest(requestMethod, Method.Post);
                //request.AddBody(parameter);
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader(authHeader, string.Format("Bearer{0}", token));
                var response = client.ExecuteAsync<List<string>>(request).Result;
                return response.StatusCode.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<T> Get<T>(string requestMethod, string token, string baseUrl) where T : new()
        //{
        //    try
        //    {
        //        string BaseUrl = baseUrl;
        //        var client = new RestClient(BaseUrl);
        //        var request = new RestRequest(requestMethod, Method.Get);
        //        //request.AddBody(parameter);
        //        string authHeader = System.Net.HttpRequestHeader.Authorization.ToString();
        //        request.AddHeader("Content-Type", "application/json");
        //        request.AddHeader(authHeader, string.Format("Bearer{0}", token));
        //        var response = client.ExecuteAsync<T>(request).Result;
        //        T resp = JsonConvert.DeserializeObject<T>(response.Content);
        //        return resp;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


        // get model property data 
        public async Task<T> Get<T>(string requestMethod, string baseUrl) where T : new()
        {
            try
            {
                string BaseUrl = baseUrl;
                var client = new RestClient(BaseUrl);
                //var request = new RestRequest("api/Employee/EditEmployee/?employeeId=" + parameter + "", Method.Get);
                var request = new RestRequest(requestMethod, Method.Get);
                //request.AddBody(parameter);
                //string authHeader = System.Net.HttpRequestHeader.Authorization.ToString();
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader(authHeader, string.Format("Bearer{0}", token));
                var response = await client.ExecuteAsync<T>(request);
                T resp = JsonConvert.DeserializeObject<T>(response.Content);
                return resp;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Get List of model data
        public async Task<List<T>> Get<T>(string requestMethod, string baseUrl, string token) where T : new()
        {
            try
            {
                string BaseUrl = baseUrl;
                var client = new RestClient(BaseUrl);
                var request = new RestRequest(requestMethod, Method.Get);
                //request.AddBody(parameter);
                //string authHeader = System.Net.HttpRequestHeader.Authorization.ToString();
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader(authHeader, string.Format("Bearer{0}", token));
                var response = await client.ExecuteAsync<T>(request);
                var resp = JsonConvert.DeserializeObject<List<T>>(response.Content);
                return resp;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // return string type
        public async Task<string> GetAPI(string requestMethod, string baseUrl)
        {
            try
            {
                string BaseUrl = baseUrl;
                var client = new RestClient(BaseUrl);
                //var request = new RestRequest("api/Employee/EditEmployee/?employeeId=" + parameter + "", Method.Get);
                var request = new RestRequest(requestMethod, Method.Get);
                //request.AddBody(parameter);
                //string authHeader = System.Net.HttpRequestHeader.Authorization.ToString();
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader(authHeader, string.Format("Bearer{0}", token));
                var response = await client.ExecuteAsync<string>(request);
                string resp = JsonConvert.DeserializeObject<string>(response.Content);
                return resp;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
        public string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}

