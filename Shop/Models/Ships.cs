namespace Shop.Models
{
    public class Ships
    {
        public int IDNo { get; set; }
        public string uuid { get; set; }
        public int Status { get; set; }
        public DateTime ShipDatetime { get; set; }
        public DateTime ReceivedDatetime { get; set; }
        public string UserId { get; set; }
    }
}
