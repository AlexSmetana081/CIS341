using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab05.Models
{
    public class Message
    {
        // Keys
        [Key]
        public int MessageId { get; set; }
        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }
        [ForeignKey(nameof(Recipient))]
        public int RecipientID { get; set; }
        // Props
        [StringLength(1000, ErrorMessage = "Message max length {1} characters.")]
        public string Content { get; set; } = null!;
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Sent")]
        public DateTime DateSent { get; set; }
        // Nav props
        public Account Sender { get; set; } = null!;
        public Account Recipient { get; set; } = null!;
    }
}
