namespace BlazorApp.Models
{
    public class ProfileHistoryModel
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
    }

}
