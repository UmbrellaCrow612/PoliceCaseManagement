namespace CAPTCHA.Core.Banks
{
    internal static class WordBank
    {
        public static HashSet<string> Words { get; set; } =
        [
            "Cat", "Dog", "Apple", "Ball", "Car", "Tree", "Happy", "Blue", "Red",
            "Green", "Up", "Down", "Left", "Right", "Open", "Close", "Day", "Night", "Sky", "Rain", "Sun",
            "Good", "Bad", "Fast", "Slow", "Big", "Small", "Fast", "Slow", "High", "Low", "Good", "Morning",
            "Evening", "Night", "Friend", "Family", "Love", "Smile", "Laugh", "Music", "Book", "Chair", "Table",
            "Door", "Window", "Light", "Dark", "Street", "Park", "Mountain", "River", "Ocean", "Island", "Beach"
        ];

        public static List<string> GetRandomWords(int count)
        {
            Random rand = new();
            return Words.OrderBy(x => rand.Next()).Take(count).ToList();
        }
    }
}
