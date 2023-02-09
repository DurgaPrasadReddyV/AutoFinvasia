namespace AutoFinvasia.Options
{
    public class FinvasiaSocketOptions
    {
        public const string FinvasiaSocket = "FinvasiaSocket";

        public string Uri { get; set; } = string.Empty;
        public int ReconnectTimeout { get; set; } = 0;
        public int ErrorReconnectTimeout { get; set; } = 0;
    }
}
