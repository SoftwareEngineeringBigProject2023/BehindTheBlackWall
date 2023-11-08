namespace SEServer.Statements.Domain.Shared
{
    public sealed class JwtTokenOptions
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? Key { get; set; }
    }
}
