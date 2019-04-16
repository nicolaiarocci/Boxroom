namespace Boxroom.Rest
{

    public interface IAuthentication
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}