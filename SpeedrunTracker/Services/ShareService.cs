namespace SpeedrunTracker.Services;

public class ShareService : IShareService
{
    public async Task ShareUriAsync(ShareDetails shareDetails)
    {
        if (string.IsNullOrEmpty(shareDetails.Uri))
            return;

        await Share.Default.RequestAsync(new ShareTextRequest()
        {
            Text = shareDetails.Uri,
            Title = shareDetails.Title
        });
    }
}
