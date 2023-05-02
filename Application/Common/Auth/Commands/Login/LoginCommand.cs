using MediatR;

namespace Application.Auth.Commands.Login
{
    public class LoginCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
