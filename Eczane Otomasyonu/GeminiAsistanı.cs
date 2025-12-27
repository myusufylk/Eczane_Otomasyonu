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
        // 🛑 BURAYA KENDİ UZUN API ŞİFRENİ YAPIŞTIRMAYI UNUTMA!
        private static readonly string ApiKey = "AIzaSyBFeYhpcz6JA-7pwADh01G8c1LNJacSrlE";

        // ✅ LİSTENDEKİ EN GÜÇLÜ MODELİ SEÇTİM
        private static readonly string Model = "gemini-2.5-pro";

        private static readonly string ApiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{Model}:generateContent?key={ApiKey}";

        // Yapay Zekanın Rolü
        private static readonly string SystemInstruction =
            "SENİN GÖREVİN: Sen 'Eczane Otomasyonu' içindeki uzman bir Eczacı Asistanısın. " +
            "Adın 'PharmAI'. " +
            "Kullanıcıya ilaçlar, yan etkiler ve stok yönetimi konusunda yardım edersin. " +
            "Cevapların kısa, net, profesyonel ve Türkçe olmalı.";

        public static async Task<string> Yorumla(string kullaniciMesaji)
        {
            using (HttpClient client = new HttpClient())
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                // Model talimatı (System Instruction) ile kullanıcı sorusunu birleştiriyoruz
                                new { text = SystemInstruction + "\n\nKULLANICI SORUSU: " + kullaniciMesaji }
                            }
                        }
                    }
                };

                string jsonContent = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(ApiUrl, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = JObject.Parse(responseString);

                        if (jsonResponse["candidates"] != null && jsonResponse["candidates"][0]["content"] != null)
                        {
                            return jsonResponse["candidates"][0]["content"]["parts"][0]["text"].ToString();
                        }
                        return "⚠️ Yapay zeka boş cevap döndü.";
                    }
                    else
                    {
                        return $"⚠️ Hata: {response.StatusCode} - {responseString}";
                    }
                }
                catch (Exception ex)
                {
                    return "⚠️ Bağlantı Hatası: " + ex.Message;
                }
            }
        }
    }
}