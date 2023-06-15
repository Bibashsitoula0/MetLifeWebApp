namespace MetLifeInsurance.Models
{
    public class Response
    {
        public bool IsLockedOut { get; set; }
        public bool IsNotAllowed { get; set; }
        public bool RequiresTwoFactor { get; set; }
        public bool Succeeded { get; set; }
    }
}
