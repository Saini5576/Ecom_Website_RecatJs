using Domain.DTO;
using Domain.IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class GetIpAddress : IGetIpAddress
    {
        //private readonly GetIpAddressSettings _addressSettings; 
        private readonly HttpClient _client;
        public GetIpAddress(HttpClient client
            //, GetIpAddressSettings addressSettings
            )
        {
            //_addressSettings = addressSettings;
            _client = client;
        }
        public async Task<GetIpLocation> GetIpAddressAsync()
        {
            try
            {
                // Send HTTP request to the configured URL
                HttpResponseMessage response = await _client.GetAsync("");

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(responseContent))
                    {                        
                        GetIpLocation getIpLocation = JsonConvert.DeserializeObject<GetIpLocation>(responseContent);
                        return getIpLocation;
                    }
                    else
                    {
                        return null;
                    }
                
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve IP location. Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                return null; 
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
