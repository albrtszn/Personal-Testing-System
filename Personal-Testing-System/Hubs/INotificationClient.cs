namespace Personal_Testing_System.Hubs
{
    public interface INotificationClient
    {
        Task ReceiveMessage(string message);
        Task SendMessage(string message);
        Task MessageNotification(string message);
        Task MessageNotificationToEmployee(string idEmployee, string message);
        Task TestCompleteNotification(string message);
        Task EmployeeNotification(string message);
        Task ConfigurationNotification(string message);
    }
}
