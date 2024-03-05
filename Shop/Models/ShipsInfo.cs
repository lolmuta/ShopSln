namespace Shop.Models
{
    public class ShipsInfo
    {
        public int IDNo { get; set; }
        public string uuid { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public DateTime? PaidDatetime { get; set; }
        public DateTime? ShipDatetime { get; set; }
        public DateTime? ReceivedDatetime { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Addr { get; set; }
    }
}
