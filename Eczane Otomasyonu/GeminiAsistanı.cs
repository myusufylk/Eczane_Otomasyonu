using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eczane_Otomasyonu
{
    public static class GeminiAsistani
    {
        private static readonly HttpClient client = new HttpClient();

        // BURAYA KENDİ API KEY'İNİZİ YAPIŞTIRIN
        private static string ApiKey = "API KEY GIRINIZ";

        // ✅ GÜNCELLEME: Listeden seçilen en uygun model (Gemini 2.5 Flash)
        private static string BaseUrl => $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={ApiKey}";

        public static async Task<string> Yorumla(string metin)
        {
            if (ApiKey.Contains("BURAYA_SENIN_KEYIN"))
            {
                return "⚠️ HATA: API Key yapıştırılmamış! Lütfen GeminiAsistanı.cs dosyasını düzenleyin.";
            }

            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new { parts = new[] { new { text = metin } } }
                    }
                };

                string jsonBody = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(BaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(responseString);
                    string cevap = (string)jsonResponse["candidates"]?[0]?["content"]?["parts"]?[0]?["text"];
                    return cevap ?? "Cevap alınamadı.";
                }
                else
                {
                    // Hata Detayı
                    string hata = await response.Content.ReadAsStringAsync();
                    if ((int)response.StatusCode == 429)
                        return "⚠️ Kota Doldu (429): Çok hızlı istek gönderildi. Lütfen 30-60 saniye bekleyin.";

                    return $"⚠️ Bağlantı Hatası: {response.StatusCode} - {hata}";
                }
            }
            catch (Exception ex)
            {
                return $"Kritik Hata: {ex.Message}";
            }
        }
    }
}
