namespace StudentRegister.Classes
{
    public class AppSettings
    {
        public ConnectionStrings? Connections { get; set; }
    }

    public class ConnectionStrings
    {
        public string? DefaultConnection { get; set; }
    }

}
