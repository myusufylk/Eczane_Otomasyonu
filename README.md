# 💊 PharmAI - Yapay Zeka Destekli Eczane Yönetim Otomasyonu

**PharmAI**, geleneksel eczane otomasyonlarını modern yapay zeka teknolojileriyle birleştiren; stok takibi, satış yönetimi ve hasta güvenliğini tek bir platformda sunan kapsamlı bir masaüstü yazılımıdır.

Proje, Fırat Üniversitesi Teknoloji Fakültesi Yazılım Mühendisliği Bölümü "Nesne Tabanlı Programlama" dersi kapsamında geliştirilmiştir. Sadece veri kaydetmekle kalmaz; **Google Gemini AI** entegrasyonu sayesinde ilaç etkileşimlerini analiz eder ve eczacıya karar destek mekanizması sunar.

---

## 🚀 Öne Çıkan Özellikler

### 1. Yapay Zeka Destekli Risk Analizi (Google Gemini)
* Satış anında sepetteki ilaçların **etken maddelerini** (Örn: Parasetamol, İbuprofen) analiz eder.
* İlaçlar arasında farmakolojik bir etkileşim veya doz aşımı riski varsa, Google Gemini API aracılığıyla eczacıyı uyarır.
* Hastanın sağlığını koruyan aktif bir güvenlik katmanı sağlar.

### 2. Akıllı Eczacı Asistanı (Chatbot)
* Uygulama içinde entegre çalışan yapay zeka asistanıdır.
* Eczacının veya personelin tıbbi sorularını (Örn: *"Mide yanmasına ne iyi gelir?"*) doğal dil işleme (NLP) yeteneği ile yanıtlar.

### 3. Akıllı Müşteri İlişkileri Yönetimi (CRM)
* **Otomatik Hasta Kaydı:** İlk kez gelen hastaları satış anında sisteme otomatik kaydeder.
* **Veri Zenginleştirme Tavsiyesi:** Müşteri 2. kez geldiğinde, eğer sistemde telefon veya adres bilgisi eksikse eczacıyı uyararak veri güncellemeyi önerir.

### 4. Gelişmiş Stok ve Envanter Yönetimi
* **Barkod Entegrasyonu:** Ürünleri barkod okuyucu ile saniyeler içinde bulma ve sepete ekleme.
* **Kritik Stok Bildirimi:** Stoğu belirlenen seviyenin (Örn: 10 adet) altına düşen ilaçlar için dashboard üzerinde anlık uyarı sistemi.
* **İlaç Kartı Yönetimi:** İlaç ekleme, silme, güncelleme, etken madde tanımlama ve görsel yönetimi.

### 5. Reçete Okuma (OCR)
* Tesseract OCR kütüphanesi kullanılarak, reçete görselleri üzerindeki metinleri dijital veriye dönüştürür ve forma otomatik doldurur.

---

## 🛠️ Kullanılan Teknolojiler ve Mimari

* **Programlama Dili:** C# (.NET Framework 4.7.2)
* **Veritabanı:** Microsoft SQL Server (MSSQL - T-SQL)
* **Arayüz (UI):** DevExpress WinForms (Ribbon Control, GridControl, TileBar)
* **Yapay Zeka:** Google Gemini 2.5 Flash API
* **Veri Erişimi:** ADO.NET (SqlBaglantisi sınıfı üzerinden merkezi yönetim)
* **Veri Formatı:** JSON (Newtonsoft.Json ile API iletişimi)
* **Görüntü İşleme:** Tesseract OCR

---

## 🧩 Veritabanı Şeması

Proje ilişkisel veritabanı (RDBMS) yapısı üzerine kurulmuştur:

1.  **TBL_ILACLAR:**
    * `ID`, `Barkod`, `IlacAdi`, `EtkenMadde`, `StokAdedi`, `Fiyat`, `ResimYolu`, `KullaniciID`
2.  **TBL_HASTALAR:**
    * `ID`, `TC`, `Ad`, `Soyad`, `Telefon`, `Guvence`, `Adres`, `KullaniciID`
3.  **TBL_HAREKETLER (Satış Geçmişi):**
    * `IslemID`, `Tarih`, `IlacAdi`, `Adet`, `ToplamFiyat`, `HastaAdi`, `TC`, `KullaniciID`
4.  **TBL_KULLANICILAR:**
    * `ID`, `KullaniciAdi`, `Sifre`, `Rol`

---

## ⚙️ Kurulum ve Çalıştırma Adımları

Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyin:

### 1. Veritabanını Oluşturun
* SQL Server Management Studio'yu açın.
* Proje dosyalarındaki SQL scriptini çalıştırarak `EczaneOtomasyonDB` veritabanını ve ilgili tabloları oluşturun.

### 2. Bağlantı Ayarını Yapın
* Proje içerisindeki `SqlBaglantisi.cs` sınıfını açın.
* `baglanti()` metodundaki Connection String'i kendi bilgisayarınıza göre düzenleyin:
    ```csharp
    "Data Source=BILGISAYAR_ADINIZ;Initial Catalog=EczaneOtomasyonDB;Integrated Security=True"
    ```

### 3. API Anahtarını Girin
* Google AI Studio üzerinden bir API Key alın.
* `FrmHareketler.cs` dosyasındaki `GeminiyeSor` metoduna bu anahtarı yapıştırın:
    ```csharp
    string apiKey = "BURAYA_KENDI_API_KEYINIZI_YAZIN";
    ```

### 4. OCR Dil Dosyası
* Tesseract OCR kullanımı için `tessdata` klasörünün ve Türkçe dil dosyasının (`tur.traineddata`) projenin `bin/Debug` klasöründe olduğundan emin olun.

---

## 🗺️ Gelecek Planları (Roadmap)

* [ ] **Mobil Uygulama:** Eczacının stoğu uzaktan kontrol edebilmesi.
* [ ] **E-Reçete Entegrasyonu:** SGK Medula sistemi ile tam entegrasyon.
* [ ] **QR Kod (Karekod):** İlaç takip sistemi (İTS) uyumluluğu.

---

## 👨‍💻 Geliştirici

Mehmet Yusuf Yılıkoğlu
Fırat Üniversitesi - Yazılım Mühendisliği Bölümü
