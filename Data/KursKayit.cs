using System.ComponentModel.DataAnnotations;

namespace efcoreApp.Data
{
    //KURSKAYIT 3.BIR TABLO OLUSTURDUK VE BU TABLOYA FKLAR EKLEDIK VE BUNLARIN BILGILERINI ALMAK ICIN 
    //JOIN ISEMLERI YAPMAMIZ GERKMEKTEDIR
    public class KursKayit
    {
        [Key]
        public int KayitId { get; set; }
        public int OgrenciId { get; set; }
        public Ogrenci Ogrenci { get; set; } = null!;
        //bu sekilde join islemini halletmis olduk

        public int KursId { get; set; }
        public Kurs Kurs { get; set; } = null!;
        public DateTime KayitTarihi { get; set; }
    }
}