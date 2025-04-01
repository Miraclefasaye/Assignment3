namespace TicketHubApi.Models;

public class TicketPurchase
{
    public int ConcertId { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public int Quantity { get; set; }
    public required string CreditCard { get; set; }
    public required string Expiration { get; set; }
    public required string SecurityCode { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string Province { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }
}