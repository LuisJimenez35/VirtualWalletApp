using System.ComponentModel.DataAnnotations;

namespace VirtualWalletApp.Models
{
    public class DataTarjet
    {
        public string? Logo { get; set; }
        public string? Bank { get; set; }
        public string? Emitter { get; set; }
        public string? OwnerName { get; set; }
        public string? TarjetNumber { get; set; }
        public string? CVV { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpirationDate { get; set; }
        public int? ID { get; set; }
        public int? OwnerDNI { get; set; }
        
    }
}
