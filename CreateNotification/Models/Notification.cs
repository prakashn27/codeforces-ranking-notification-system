namespace CreateNotification.Models
{
    public class Notification
    {
        public string EmailAddress{get; set;}
        public string FollowerId {get; set;}
        public int OldRank{get;set;}
        public int NewRank { get; set; }
    }
}