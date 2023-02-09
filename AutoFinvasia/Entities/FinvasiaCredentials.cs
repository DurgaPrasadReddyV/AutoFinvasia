namespace AutoFinvasia.Entities
{
    public class FinvasiaCredentials
    {
        public int Id { get; set; } 
        public string UserID { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PAN { get; set; } = string.Empty;
        public string APIKeyHash { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string IMEI { get; set; } = string.Empty;
        public string TOTPKey { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
    }
}
