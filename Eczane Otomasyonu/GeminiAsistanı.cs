using System;
using System.Net; // Güvenlik protokolü için gerekli
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Eczane_Otomasyonu
{
    public class GeminiAsistani
    {
      
        private const string ApiKey = "AIzaSyBFeYhpcz6JA-7pwADh01G8c1LNJacSrlE";
            
        
        private const string ApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key=";

        public static async Task<string> Yorumla(string soru)
        {
            try
            {
                // Bağlantı hatası olmaması için TLS 1.2 protokolünü açıyoruz
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

                using (var client = new HttpClient())
                {
                    var requestBody = new
                    {
                        contents = new[]
                        {
                            new { parts = new[] { new { text = soru } } }
                        }
                    };

                    var json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(ApiUrl + ApiKey, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                        return jsonResponse.candidates[0].content.parts[0].text;
                    }
                    else
                    {
                        // 429 Hatası (Kota Doldu) için özel mesaj
                        if ((int)response.StatusCode == 429)
                        {
                            return "⚠️ Çok hızlı mesaj yazdın! Google 'biraz bekle' diyor. 1-2 dakika sonra tekrar deneyebilirsin.";
                        }

                        return $"Bir hata oluştu. Hata Kodu: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Bağlantı Hatası: " + ex.Message;
            }
        }
    }
}