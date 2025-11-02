using Application.Common.Interfaces.Logging;
using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Command.Register;

namespace Application.Features.Auth.Commands.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
    {
        private readonly IRegisterService _registerService;
        private readonly ILoggingService<RegisterHandler> _logger;

        public RegisterHandler(IRegisterService registerService,
            ILoggingService<RegisterHandler> logger)
        {
            _registerService = registerService;
            _logger = logger;
        }

        public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[RegisterHandler] Attempting registration for {request.Dto.Email}");

            try
            {
                var result = await _registerService.RegisterAsync(request.Dto, cancellationToken);
                _logger.LogInformation($"[RegisterHandler] Registration successful for {request.Dto.Email}, UserId: {result.UserId}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[RegisterHandler] Registration failed for {request.Dto.Email}", ex);
                throw;
            }
        }
    }
}
