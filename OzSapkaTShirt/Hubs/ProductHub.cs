
using Microsoft.AspNetCore.SignalR;

namespace OzSapkaTShirt.Hubs
{
    public class ProductHub : Hub
    {
        public async Task CreateProduct(string name)
        {
            await Clients.All.SendAsync("showProduct", name);
        }

    }
}
