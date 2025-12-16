using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeApi.Application.Dtos.OrderDtos
{
    public class OrderStatus
    {
        public const string ALINDI = "Siparişiniz oluşturuldu.";
        public const string HAZIRLANIYOR = "Siparişiniz hazırlanıyor.";
        public const string GUNCELLENDI = "Siparişiniz güncellendi.";
        public const string TESLIMEDILDI = "Siparişiniz teslim edildi.";
        public const string YOLDA = "Siparişiniz yola çıktı.";
        public const string UCRETODENDI = "Sipariş ücreti ödendi.";


    }
}
