namespace ModuleDistributor.Dapr
{
    public sealed class DaprOptions
    {
        public string? PubSub { get; set; }

        public string? SecretStore { get; set; }

        public string? StateStore { get; set; }
    }
}