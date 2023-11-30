using System.ComponentModel.DataAnnotations;
using Lab05.Models; 

namespace Lab05.ViewModels
{
    /// Data Transfer Object for transferring message data.
    public class MessageDTO
    {
        /// Gets or sets the unique identifier of the message.
        public int MessageId { get; set; }

        /// Gets or sets the unique identifier of the recipient account.
        [Required(ErrorMessage = "Recipient ID is required.")]
        public int RecipientId { get; set; }

        /// Gets or sets the recipient account associated with the message.
        public Account Recipient { get; set; } = new Account();

        /// Gets or sets the unique identifier of the sender account.
        [Required(ErrorMessage = "Sender ID is required.")]
        public int SenderId { get; set; }

        /// Gets or sets the sender account associated with the message.
        public Account Sender { get; set; } = new Account();

        /// Gets or sets the content of the message.
        [Required(ErrorMessage = "Message content is required.")]
        [StringLength(500, ErrorMessage = "Message content can be up to 500 characters.")]
        public string Content { get; set; } = string.Empty;
    }
}
