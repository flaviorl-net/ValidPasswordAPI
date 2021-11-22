using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using ValidPassword.API.ViewModel;
using ValidPassword.Domain.Interfaces.Service;

namespace ValidPassword.API.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class ValidPasswordController : ControllerBase
    {
        private readonly ILogger<ValidPasswordController> _logger;
        private readonly IPasswordValidService _passwordValidService;

        public ValidPasswordController(ILogger<ValidPasswordController> logger,
            IPasswordValidService passwordValidService)
        {
            _logger = logger;
            _passwordValidService = passwordValidService;
        }

        [HttpGet]
        [Authorize]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public ActionResult<ReturnValidPasswordViewModel> Get([FromQuery] string password)
        {
            try
            {
                bool isValid = _passwordValidService.PasswordIsValid(password);

                if (isValid)
                {
                    return Ok(isValid);
                }

                return BadRequest(isValid);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);

                return BadRequest(new ReturnValidPasswordViewModel
                {
                    ExecutionReturn = new ExecutionReturnViewModel() { ErrorCode = 2, Message = "Error" }
                });
            }
        }
    }
}
