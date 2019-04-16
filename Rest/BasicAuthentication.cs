namespace Boxroom.Rest
{

    public class BasicAuthentication : IAuthentication
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}