namespace pm.Models
{
    public class ExternalCommand
    {
        public Before Before { get; set; } //The command that run's in first
    }

    public class Before
    {
        public string run { get; set; }
    }
}