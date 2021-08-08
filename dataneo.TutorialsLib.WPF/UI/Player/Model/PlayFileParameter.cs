namespace dataneo.TutorialLibs.WPF.UI
{
    public sealed class PlayFileParameter
    {
        public PlayFileParameter(string path, string title, int position)
        {
            Path = path;
            Position = position;
            Title = title;
        }

        public string Path { get; }
        public string Title { get; }
        public int Position { get; }
    }
}
