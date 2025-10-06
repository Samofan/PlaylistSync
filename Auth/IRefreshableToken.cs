namespace PlaylistSync.Auth;

public interface IRefreshableToken : IUserToken
{
    public string RefreshToken { get; set; }
}
