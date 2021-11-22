using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ValidPassword.API.ViewModel;
using ValidPassword.Domain.Entities;
using ValidPassword.Infra.Data;
using ValidPassword.Infra.Token;

namespace ValidPassword.API.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<ReturnUserViewModel>> Post([FromServices] Context context, [FromBody] UserViewModel model)
        {
            try
            {
                var user = _mapper.Map<User>(model);

                context.User.Add(user);
                await context.SaveChangesAsync();

                return new ReturnUserViewModel
                {
                    UserName = user.UserName,
                    Role = user.Role,
                    Password = user.Password,
                    ExecutionReturn = new ExecutionReturnViewModel() { Message = "Ok" }
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return BadRequest(new ReturnUserViewModel { ExecutionReturn = new ExecutionReturnViewModel { ErrorCode = 2, Message = "Erro ao criar usuário" } });
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<ReturnAuthViewModel>> Authenticate([FromServices] Context context, [FromBody] UserViewModel model)
        {
            try
            {
                var user = await context
                    .User
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserName == model.UserName && x.Password == model.Password);

                if (user == null)
                {
                    return NotFound(new ReturnAuthViewModel { ExecutionReturn = new ExecutionReturnViewModel() { ErrorCode = 1, Message = "Usuário ou senha inválidos" } });
                }

                var token = TokenService.GenerateToken(user.UserName, user.Role ?? "guest", Settings.Secret);

                return new ReturnAuthViewModel
                {
                    UserName = user.UserName,
                    Token = token,
                    ExecutionReturn = new ExecutionReturnViewModel() { Message = "Ok" }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return BadRequest(new ReturnAuthViewModel { ExecutionReturn = new ExecutionReturnViewModel() { ErrorCode = 2, Message = "Erro ao autenticar usuário" } });
            }
        }
    }
}
