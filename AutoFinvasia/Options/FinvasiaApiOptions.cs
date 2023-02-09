namespace AutoFinvasia.Options
{
    public class FinvasiaApiOptions
    {
        public const string FinvasiaApi = "FinvasiaApi";

        public string BaseAddress { get; set; } =  string.Empty;
        public int Timeout { get; set; } = 0;
    }
}
