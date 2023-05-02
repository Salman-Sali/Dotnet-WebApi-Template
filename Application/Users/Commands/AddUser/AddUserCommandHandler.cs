using Application.Common.ServiceInterfaces;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, bool>
    {
        private readonly IMainDbContext _context;
        public AddUserCommandHandler(IMainDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(User.Create(request.Name, request.Password));
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
