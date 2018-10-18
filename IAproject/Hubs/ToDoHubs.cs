using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;

namespace SignalRChat
{
    public class ToDoHub : Hub
    {
        // GET: ToDoHubs
        public void Send(string  message)
        {
            Clients.All.addNewMessageToPage(message);
        }
    }
}