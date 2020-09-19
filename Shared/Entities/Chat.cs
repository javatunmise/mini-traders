using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shared.Entities
{
    public class LatestMessage
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public string RecipientName { get; set; }
        public int RecipientId { get; set; }
        public string Message { get; set; }
        public DateTime LastMessageDate { get; set; }
    }

    public class Chat
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public int InitiatorUserId { get; set; }
        public SiteUser Initiator { get; set; }
        public int RecipientUserId { get; set; }
        public SiteUser Recipient { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }

        [NotMapped]
        public string RecentMessage { get; set; }
    }

    public class ChatMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int SiteUserId { get; set; }
        public SiteUser SiteUser { get; set; }

        [NotMapped]
        public string ProfilePicture { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class MessageContext
    {
        public int UserId { get; set; }
        public int RecipientUserId { get; set; }
        public string Message { get; set; }
        public int MessageUserId { get; set; }
    }
}
