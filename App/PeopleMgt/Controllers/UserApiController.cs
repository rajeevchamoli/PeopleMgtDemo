using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PeopleMgt.Models;
using PeopleMgt.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeopleMgt.Controllers
{
    [Route("api/[controller]")]
    public class UserApiController : Controller
    {
        private IUserRepository _repository;

        public UserApiController(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        // GET: api/UserApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery]PageParameters pagingParams)
        {
            try
            {
                var userTuple = await _repository.GetUsersAsync(pagingParams);
                // Setting Header for out of band paging data
                HttpContext.Response.Headers.Add("Page-Headers", JsonConvert.SerializeObject(userTuple.Item2));

                return Ok(userTuple.Item1);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/UserApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var user = await _repository.GetUserByIdAsync(id);
                return user == null ? (ActionResult<User>)NotFound() : (ActionResult<User>)Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, MessageConstants.GenericServerSideErrorMsg);
            }
        }

        // PUT: api/UserApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromBody]User user)
        {
            
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(Utility.ExtractValidationErrorMsg(ModelState));
                }

                if (user == null || id != user.Id)
                {
                    return base.BadRequest(string.Format(MessageConstants.InputObjectNullOrInvalidIdErrorMsg, nameof(Models.User)));
                }

                if (!(await _repository.IsExistingUser(id)))
                {
                    return NotFound();
                }
                

                await _repository.UpdateUserAsync(user);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, MessageConstants.GenericServerSideErrorMsg);
            }
        }

        // POST: api/UserApi
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody]User user)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(MessageConstants.InValidModelStateErrorMsg);
                }

                if (user == null)
                {
                    return base.BadRequest(string.Format(MessageConstants.InputObjectNullOrInvalidIdErrorMsg, nameof(Models.User)));
                }

                await _repository.CreateUserAsync(user);

                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (Exception)
            {
                return StatusCode(500, MessageConstants.GenericServerSideErrorMsg);
            }
        }

        // DELETE: api/UserApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            try
            {
                var user = await _repository.GetUserByIdAsync(id);

                if (user == null || user.Id != id)
                {
                    //404 code
                    return NotFound();
                }

                await _repository.DeleteUserAsync(user);

                // 204 code
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, MessageConstants.GenericServerSideErrorMsg);
            }
        }
    }
}
