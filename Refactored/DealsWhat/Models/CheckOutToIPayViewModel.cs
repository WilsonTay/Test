namespace DealsWhat.Models
{
    public class CheckOutToIPayViewModel
    {
        public string MerchantCode { get; set; }
        public string RefNo { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string ProdDesc { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserContact { get; set; }
        public string Remark { get; set; }
        public string Lang { get; set; }
        public string Signature { get; set; }
        public string ResponseURL { get; set; }
        public string BackendURL { get; set; }

        public string PaymentId { get; set; }
    }
}