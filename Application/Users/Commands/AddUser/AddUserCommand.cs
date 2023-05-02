using MediatR;

namespace Application.Users.Commands.AddUser
{
    public class AddUserCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
