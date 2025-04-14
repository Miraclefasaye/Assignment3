using System.ComponentModel.DataAnnotations;

namespace TicketHubApi.Models
{
    public class TicketPurchase
    {
        public int ConcertId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required, Phone]
        public string Phone { get; set; } = string.Empty;

        [Range(1, 10)]
        public int Quantity { get; set; }

        [Required, CreditCard]
        public string CreditCard { get; set; } = string.Empty;

        [Required]
        public string Expiration { get; set; } = string.Empty;

        [Required]
        public string SecurityCode { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Province { get; set; } = string.Empty;

        [Required]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;
    }
}
