namespace CreateNotification.Models
{
    public class ChangedRank
    {
        public string ContestId { get; set; }
        public string ContestName { get; set; }
        public string Handle { get; set; }
        public int Rank { get; set; }
        public int OldRating { get; set; }
        public int NewRating { get; set; }
        
        public override string ToString() => Handle;
        
    }
}