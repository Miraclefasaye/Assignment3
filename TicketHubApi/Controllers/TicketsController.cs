// Controllers/TicketsController.cs
[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly IQueueService _queueService;
    private readonly ILogger<TicketsController> _logger;

    public TicketsController(IQueueService queueService, ILogger<TicketsController> logger)
    {
        _queueService = queueService;
        _logger = logger;
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> PurchaseTicket([FromBody] TicketPurchase purchase)
    {
        // Validate the model
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Additional validation
        var validationErrors = ValidatePurchase(purchase);
        if (validationErrors.Any())
        {
            return BadRequest(new { Errors = validationErrors });
        }

        try
        {
            // Send to queue
            await _queueService.SendMessageAsync(purchase);
            return Ok(new { Message = "Ticket purchase received and being processed" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing ticket purchase");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    private List<string> ValidatePurchase(TicketPurchase purchase)
    {
        var errors = new List<string>();

        if (purchase.Quantity <= 0)
            errors.Add("Quantity must be greater than 0");

        if (string.IsNullOrWhiteSpace(purchase.Email) || !IsValidEmail(purchase.Email))
            errors.Add("Invalid email address");

        if (string.IsNullOrWhiteSpace(purchase.CreditCard) || !IsValidCreditCard(purchase.CreditCard))
            errors.Add("Invalid credit card number");

        // Add more validations as needed...

        return errors;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidCreditCard(string cardNumber)
    {
        // Simple validation - in production use a proper validation library
        return !string.IsNullOrWhiteSpace(cardNumber) && cardNumber.Length >= 13 && cardNumber.Length <= 19
            && cardNumber.All(char.IsDigit);
    }
}