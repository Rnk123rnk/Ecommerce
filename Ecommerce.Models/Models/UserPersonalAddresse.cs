using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models.Models
{
    [Table("UserPersonalAddresse")]
    public class UserPersonalAddresse
    {
        [Key]
        public int UserPersonalAddresseId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }

        public string Address {  get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string MobileNumber { get; set; }
        public int? PinCode { get; set; }
    }
}
