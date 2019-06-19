namespace SendNotification.models
{
    public class Notification
    {
        public string EmailAddress { get; set; }
        public string FollowerId { get; set; }
        public int OldRank { get; set; }
        public int NewRank { get; set; }
        public Notification(string email, string fid, int oldrank, int newrank)
        {
            EmailAddress = email;
            FollowerId = fid;
            OldRank = oldrank;
            NewRank = newrank;
        }
    }
}