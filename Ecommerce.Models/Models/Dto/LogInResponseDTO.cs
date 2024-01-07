namespace Ecommerce.Models.Models.Dto
{
    public class LogInResponseDTO
    {
        public int UserId { get; set; }
        public string role { get; set; }

        public string? Token { get; set; }

    }
}
