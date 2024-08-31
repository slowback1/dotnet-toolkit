namespace Slowback.FeatureFlags;

public class FeatureFlagService
{
    private readonly IFeatureFlagProvider _featureFlagProvider;

    public FeatureFlagService(IFeatureFlagProvider featureFlagProvider)
    {
        _featureFlagProvider = featureFlagProvider;
    }

    public async Task<bool> FeatureIsEnabled(string featureFlag)
    {
        var featureFlags = (await GetFeatureFlags()).ToList();

        return await FeatureExists(featureFlags, featureFlag) && await FeatureIsEnabled(featureFlags, featureFlag);
    }

    private async Task<IEnumerable<FeatureFlag>> GetFeatureFlags()
    {
        return await _featureFlagProvider.GetFeatureFlags();
    }

    private async Task<bool> FeatureExists(IEnumerable<FeatureFlag> featureFlags, string feature)
    {
        return await Task.FromResult(featureFlags.Any(ff => ff.Name == feature));
    }

    private async Task<bool> FeatureIsEnabled(IEnumerable<FeatureFlag> featureFlags, string feature)
    {
        return await Task.FromResult(featureFlags.Any(ff => ff.Name == feature && ff.IsEnabled));
    }
}