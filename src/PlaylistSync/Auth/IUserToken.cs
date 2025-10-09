namespace PlaylistSync.Auth;

public interface IUserToken : IToken
  {
    public string Scope { get; set; }
  }
