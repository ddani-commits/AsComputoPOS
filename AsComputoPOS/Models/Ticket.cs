namespace TamoPOS.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public DateTime Date { get; set; }
        public ICollection<CartItem>? Products { get; set; }
        public Employee? Employee { get; set; }
        public decimal Total { get; set; }
        public decimal ChangeDue { get; set; }
        public decimal Paid { get; set; }
    }
}