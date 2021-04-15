using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.App.Pages
{
    public partial class Index
    {
        //[Inject]
        //private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        //[Inject]
        //public IAuthenticationService AuthenticationService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            //await ((CustomAuthenticationStateProvider)AuthenticationStateProvider).GetAuthenticationStateAsync();
        }

        protected void NavigateToLogin()
        {
            NavigationManager.NavigateTo("login");
        }

        protected void NavigateToRegister()
        {
            NavigationManager.NavigateTo("register");
        }

        protected void Logout()
        {
            //await AuthenticationService.Logout();
        }
    }
}
