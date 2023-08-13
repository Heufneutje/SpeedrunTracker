namespace SpeedrunTracker.Services;

public interface IEmbedService
{
    List<EmbeddableUrl> GetEmbeddableUrls(SpeedrunVideos videos);
}
