namespace Slowback.FeatureFlags;

public interface IFeatureFlagProvider
{
    Task<IEnumerable<FeatureFlag>> GetFeatureFlags();
}