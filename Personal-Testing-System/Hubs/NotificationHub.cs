using DataBase.Repository.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Personal_Testing_System.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Personal_Testing_System.Hubs
{
    public sealed class NotificationHub : Hub<INotificationClient>
    {
        //private static List<Connection> connections = new List<Connection>();
        MasterService ms;
        public NotificationHub(MasterService _ms)
        {
            ms = _ms;
        }
        public override async Task OnConnectedAsync()
        {
            //var connection = new Connection();
            string token = Context.GetHttpContext().Request.Query["token"];
            if (!token.IsNullOrEmpty())
            {
                TokenAdmin? adminToken = await ms.TokenAdmin.GetTokenAdminByToken(token);
                if(adminToken != null && !adminToken.IdAdmin.IsNullOrEmpty())
                {
                    /*connection.ConnectionID = Context.ConnectionId;
                    connection.IdEmployee = adminToken.IdAdmin;
                    connection.Role = Roles.EMPLOYEE;
                    connections.Add(connection);*/
                    adminToken.ConnectionId = Context.ConnectionId;
                    await ms.TokenAdmin.SaveTokenAdmin(adminToken);

                    await Groups.AddToGroupAsync(Context.ConnectionId, Roles.ADMIN.ToString());
                    await Clients.Group(Roles.ADMIN.ToString()).ReceiveMessage($"Admin connected adminId:{adminToken.IdAdmin} connectionId:{Context.ConnectionId}");
                    await base.OnConnectedAsync();
                }
                TokenEmployee? employeeToken = await ms.TokenEmployee.GetTokenEmployeeByToken(token);
                if (employeeToken != null && !employeeToken.IdEmployee.IsNullOrEmpty())
                {
                    /*connection.ConnectionID = Context.ConnectionId;
                    connection.IdEmployee = employeeToken.IdEmployee;
                    connection.Role = Roles.ADMIN;
                    connections.Add(connection);*/
                    employeeToken.ConnectionId = Context.ConnectionId;
                    await ms.TokenEmployee.SaveTokenEmployee(employeeToken);

                    await Groups.AddToGroupAsync(Context.ConnectionId, Roles.EMPLOYEE.ToString());
                    await Clients.Group(Roles.EMPLOYEE.ToString()).ReceiveMessage($"Employee connected employeeId:{employeeToken.IdEmployee} connectionId:{Context.ConnectionId}");
                    await base.OnConnectedAsync();
                }
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            TokenAdmin? adminToken = await ms.TokenAdmin.GetTokenAdminByConnectionId(Context.ConnectionId);
            if (adminToken != null && !adminToken.ConnectionId.IsNullOrEmpty())
            {
                adminToken.ConnectionId = null;
                await ms.TokenAdmin.SaveTokenAdmin(adminToken);
            }
            TokenEmployee? employeeToken = await ms.TokenEmployee.GetTokenEmployeeByConnectionId(Context.ConnectionId);
            if (employeeToken != null && !employeeToken.IdEmployee.IsNullOrEmpty())
            {
                //Console.WriteLine($"employee token disconect idEmployee:{employeeToken.IdEmployee}");
                employeeToken.ConnectionId = null;
                await ms.TokenEmployee.SaveTokenEmployee(employeeToken);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageAll(string message)
        {
            await Clients.All.SendMessage(message);
        }
        public async Task MessageNotification(string message)
        {
            await Clients.All.MessageNotification(message);
        }
        public async Task MessageNotificationToEmployee(string idEmployee, string message)
        {
            /*var user = connections.Where(o => o.IdEmployee.Equals(idEmployee));
            Console.WriteLine(message + $"count:{connections.Count}");
            foreach (var con in connections)
            {
                Console.WriteLine($"connection idActor:{con.IdEmployee}");
            }
            if (user.Any())
            {
                Console.WriteLine(user.First().ConnectionID);
                await Clients.Client(user.First().ConnectionID).MessageNotification(message);
            }*/
            //await Clients.Client(idEmployee).MessageNotification(message);
        }
        public async Task TestCompleteNotification(string message)
        {
            await Clients.All.TestCompleteNotification(message);
        }
        public async Task EmployeeNotification(string message)
        {
            await Clients.All.EmployeeNotification(message);
        }
        public async Task ConfigurationNotification(string message)
        {
            await Clients.All.ConfigurationNotification(message);
        }
    }
}
