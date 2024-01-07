using AutoMapper;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EcommerceApi.Controllers
{
    [Route("api/UsersAPI")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _dbUser;
        private readonly ICartRepository _dbcart;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public UsersController(IUsersRepository dbUser, IMapper mapper,ICartRepository dbcart)
        {
            _dbUser = dbUser;
            _dbcart = dbcart;
            _mapper = mapper;
            this._response = new();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse>> CreateUser([FromBody] UsersDTO usersDTO)
        {
            try
            {
                if (usersDTO == null)
                {
                    return BadRequest("User data is null.");
                }


                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Users model = _mapper.Map<Users>(usersDTO);
                model.Role = "client";
                model.CreatedDate = DateTime.UtcNow;
                int result = await _dbUser.CreateUser(model);

                if (result == 1)
                {
                    ModelState.AddModelError("MobileNumber", "Mobile Number already exists!");
                    return Conflict(ModelState);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                _response.ExceptionMessage = "An unexpected error occurred. Please contact support.";
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateUser(int id, [FromBody] UsersDTO usersDTO)
        {
            try
            {
                if (usersDTO == null || id <= 0)
                {
                    return BadRequest("Invalid data or Id provided.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Users model = _mapper.Map<Users>(usersDTO);
                model.UserId = id;
                model.Role = "client";
                model.UpdateDate = DateTime.Now;

                int updateResult = await _dbUser.UpdateUser(model);
                if (updateResult == 1)
                {
                    ModelState.AddModelError("Mobile Number", "Mobile Number already exists!");
                    return Conflict(ModelState);
                }

                if (updateResult == 2)
                {
                    return NotFound($"User with Id {id} not found.");
                }

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                _response.ExceptionMessage = "An unexpected error occurred. Please contact support.";
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetUsersList()
        {
            try
            {
                IEnumerable<Users> users = await _dbUser.GetUsers();
                _response.Result = users;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                _response.ExceptionMessage = "An unexpected error occurred. Please contact support.";
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetUserById(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("Id", "Id should be a positive integer.");
                return BadRequest(ModelState);
            }
    
            try
            {
                var user = await _dbUser.GetUser(id);
                if (user == null)
                {
                    return NotFound();
                }
                _response.Result = user;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                _response.ExceptionMessage = "An unexpected error occurred. Please contact support.";
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetLogIn([FromBody] LogInDTO logInDTO)
        {
            if (logInDTO == null)
            {
                return BadRequest("Mobile and password is null.");
            }
            try
            {
                var logInResponse = await _dbUser.LogIn(logInDTO);
                if (string.IsNullOrEmpty(logInResponse.Token))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessage.Add("Username or password is incorrect");
                    return BadRequest(_response);
                }

                await _dbcart.UpdateCartWhenLogin(logInResponse.UserId, logInDTO.RandomUserId);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = logInResponse;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                _response.ExceptionMessage = "An unexpected error occurred. Please contact support.";
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

  


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<ApiResponse>> DeleteUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id provided.");
                }
                bool userExists = await _dbUser.DeleteUser(id);
                if (!userExists)
                {
                    return NotFound($"User with Id {id} not found.");
                }
                _response.IsSuccess = true;
                _response.StatusCode=HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                _response.ExceptionMessage = "An unexpected error occurred. Please contact support.";
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
