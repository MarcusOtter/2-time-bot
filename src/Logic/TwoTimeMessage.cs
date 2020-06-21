namespace Logic
{
    public class TwoTimeMessage
    {
        public string Text { get; private set; }
        public string MediaUrl { get; private set; }
        public int Year { get; private set; }
        public int Month { get; private set; }
        public int DayOfYear { get; private set; }
        public int DayOfMonth { get; private set; }

        public TwoTimeMessage(string text, string mediaUrl = "", int year = -1, int month = -1, int dayOfYear = -1, int dayOfMonth = -1)
        {
            Text = text;
            MediaUrl = mediaUrl;
            Year = year;
            Month = month;
            DayOfYear = dayOfYear;
            DayOfMonth = dayOfMonth;
        }
    }
}
