using System.Data.SqlClient;

namespace Eczane_Otomasyonu
{
    class SqlBaglantisi
    {
        public SqlConnection baglanti()
        {
            // Veritabanı bağlantı cümlen
            SqlConnection baglan = new SqlConnection(@"Data Source=.;Initial Catalog=Eczane_Otomasyonu;Integrated Security=True");
            baglan.Open();
            return baglan;
        }
    }
}