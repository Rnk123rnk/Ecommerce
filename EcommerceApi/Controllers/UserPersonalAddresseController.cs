using AutoMapper;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EcommerceApi.Controllers
{
    [Route("api/v5/UserPersonalAddresseAPI")]
    [ApiController]
    [Authorize]
    public class UserPersonalAddresseController : ControllerBase
    {
        private readonly IUserPersonalAddresseRepository _dbUserAddresse;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public UserPersonalAddresseController(IUserPersonalAddresseRepository UserAddresseRepository, IMapper mapper)
        {
            _dbUserAddresse = UserAddresseRepository;
            _mapper = mapper;
            this._response = new();
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> CreateUserPersonalAddresse([FromBody] UserPersonalAddresseDTO UserPersonalAddresseDTO)
        {
            try
            {
                if (UserPersonalAddresseDTO == null)
                {
                    return BadRequest("User data is null.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                UserPersonalAddresse model = _mapper.Map<UserPersonalAddresse>(UserPersonalAddresseDTO);
                bool result = await _dbUserAddresse.CreateUserPersonalAddresse(model);

                if (result == false)
                {
                    ModelState.AddModelError("UserId", "UserId not exists");
                    return BadRequest(ModelState);
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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateUserPersonalAddresse(int id, [FromBody] UserPersonalAddresse UserPersonalAddresseDTO)
        {
            try
            {
                if (UserPersonalAddresseDTO == null || id <= 0)
                {
                    return BadRequest("Invalid data or id provided.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                UserPersonalAddresse model = _mapper.Map<UserPersonalAddresse>(UserPersonalAddresseDTO);
                model.UserPersonalAddresseId = id;

                int updateResult = await _dbUserAddresse.UpdateUserPersonalAddresse(UserPersonalAddresseDTO);
                if (updateResult == 1)
                {
                    ModelState.AddModelError("User", "User not exists.");
                    return BadRequest(ModelState);
                }

                if (updateResult == 2)
                {
                    return NotFound($"UserPersonalAddresse with Id {id} not found.");
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
        public async Task<ActionResult<ApiResponse>> GetUserPersonalAddresses([FromQuery(Name ="FilterUserId")] int? UserId)
        {
            try
             {
                IEnumerable<UserPersonalAddresse> users;

                if (UserId.HasValue && UserId.Value > 0)
                {
                    users = await _dbUserAddresse.GetUserPersonalAddresses(UserId.Value);
                    if (!users.Any())
                    {
                        ModelState.AddModelError("UserId", "UserId does not exist in UserPersonalAddress Table!");
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    users = await _dbUserAddresse.GetUserPersonalAddresses();
                }
                _response.Result = users; ;
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
        public async Task<ActionResult<ApiResponse>> GetUserPersonalAddresseById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    ModelState.AddModelError("Id", "Id should be a positive integer.");
                    return BadRequest(ModelState);
                }
                var user = await _dbUserAddresse.GetUserPersonalAddresse(id);
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

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ApiResponse>> DeleteUserPersonalAddresse(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id provided.");
                }
                bool userExists = await _dbUserAddresse.DeleteUserPersonalAddresse(id);
                if (!userExists)
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
    }
}
