using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Eczane_Otomasyonu
{
    public static class ModelOlusturucu
    {
        // 1. Veri Yapısı
        public class ModelInput
        {
            public string IlacAdi { get; set; }
            public float TarihSayisal { get; set; } // Tarihi sayıya çevireceğiz
            public float ToplamSatis { get; set; }
        }

        public class ModelOutput
        {
            [ColumnName("Score")]
            public float Score { get; set; }
        }

        // 2. Modeli Eğitip ZIP Dosyasını Oluşturan Fonksiyon
        public static void ModeliEgitVeKaydet()
        {
            // A. Verileri SQL'den Çek (Listenin içine at)
            List<ModelInput> egitimVerisi = new List<ModelInput>();
            SqlBaglantisi bgl = new SqlBaglantisi(); // Senin bağlantı sınıfın
            SqlConnection conn = bgl.baglanti();

            // View yoksa diye garanti sorgu: İlaç ve Tarih bazlı gruplama
            string sorgu = "SELECT ilacAdi, DATEDIFF(day, '2020-01-01', tarih) as TarihIndex, SUM(adet) " +
                           "FROM Hareketler WHERE ilacAdi IS NOT NULL GROUP BY ilacAdi, DATEDIFF(day, '2020-01-01', tarih)";

            SqlCommand cmd = new SqlCommand(sorgu, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                egitimVerisi.Add(new ModelInput
                {
                    IlacAdi = dr[0].ToString(),
                    TarihSayisal = float.Parse(dr[1].ToString()), // Tarihi sayısal indexe çevirdik
                    ToplamSatis = float.Parse(dr[2].ToString())
                });
            }
            conn.Close();

            if (egitimVerisi.Count == 0)
                throw new Exception("Veritabanında eğitim için yeterli veri yok! Lütfen Hareketler tablosuna satış verisi girin.");

            // B. ML.NET Ayarları (Manual Eğitim)
            MLContext mlContext = new MLContext();
            IDataView dataView = mlContext.Data.LoadFromEnumerable(egitimVerisi);

            // C. Öğrenme Hattı (Pipeline)
            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding("IlacAdi") // İlaç ismini sayıya çevir
                .Append(mlContext.Transforms.Concatenate("Features", "IlacAdi", "TarihSayisal")) // Özellikleri birleştir
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "ToplamSatis", maximumNumberOfIterations: 100)); // Hızlı eğitim algoritması

            // D. Eğitimi Başlat
            var model = pipeline.Fit(dataView);

            // E. DOSYAYI KAYDET (İşte aradığımız zip dosyası!)
            string dosyaYolu = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SatisTahminModel.zip");
            mlContext.Model.Save(model, dataView.Schema, dosyaYolu);
        }
    }
}