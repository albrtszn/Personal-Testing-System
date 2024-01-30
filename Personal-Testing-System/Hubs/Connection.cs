namespace Personal_Testing_System.Hubs
{
    public enum Roles
    {
        EMPLOYEE,
        ADMIN
    }
    public class Connection
    {
        public string IdEmployee { set; get; }
        public string ConnectionID { set; get; }
        public Roles Role { set; get; }
    }
}
