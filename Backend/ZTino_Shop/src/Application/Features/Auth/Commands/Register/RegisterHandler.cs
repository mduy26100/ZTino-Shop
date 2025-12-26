using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Command.Register;

namespace Application.Features.Auth.Commands.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
    {
        private readonly IRegisterService _registerService;

        public RegisterHandler(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await _registerService.RegisterAsync(request.Dto, cancellationToken);
            return result;
        }
    }
}
