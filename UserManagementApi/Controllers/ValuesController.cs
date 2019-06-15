using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UserManagementApi.Models;
using UserManagementApi.DataAccess;
using Microsoft.EntityFrameworkCore;
using UserManagementApi.Commands;
using AutoMapper;

namespace UserManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        UserManagementDBContext _dbContext;
        public userController(UserManagementDBContext dbContext) {
            _dbContext = dbContext;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            // return new string[] { "value1", "VALUE2" };
            return Ok(await _dbContext.Users.ToListAsync());   
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var customer = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            // insert customer
            // User user = Mapper.Map<User>(command);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // send event
            // UserRegistered e = Mapper.Map<UserRegistered>(command);
            // await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

            // return result
            // return CreatedAtRoute("GetByCustomerId", new { UserId = user.UserId }, user);
            // return Ok(await Get(user.UserId));
            return Ok();
        }

        // // PUT api/values/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {
        // }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return NotFound();
            }
        }
    }
}
