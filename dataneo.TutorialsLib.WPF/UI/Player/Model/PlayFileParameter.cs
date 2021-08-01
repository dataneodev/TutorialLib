namespace dataneo.TutorialsLib.WPF.UI
{
    public sealed class PlayFileParameter
    {
        public PlayFileParameter(string path, int position)
        {
            Path = path;
            Position = position;
        }

        public string Path { get; }
        public int Position { get; }

    }
}
