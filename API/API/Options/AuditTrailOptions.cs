namespace API.Options
{
    public class AuditTrailOptions
    {
        public string[] RedactFields { get; set; } = [];
        public string[] ExcludedPaths { get; set; } = [];
    }
}
