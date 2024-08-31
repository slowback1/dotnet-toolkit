namespace Slowback.FeatureFlags;

public class DictionaryFeatureFlagProvider : IFeatureFlagProvider
{
    private readonly Dictionary<string, bool> _featureFlags;

    public DictionaryFeatureFlagProvider(Dictionary<string, bool> featureFlags)
    {
        _featureFlags = featureFlags;
    }

    public async Task<IEnumerable<FeatureFlag>> GetFeatureFlags()
    {
        return _featureFlags.Select(kvp => new FeatureFlag
        {
            Name = kvp.Key,
            IsEnabled = kvp.Value
        });
    }
}