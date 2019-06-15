using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FollowerManagementApi.Models;
using FollowerManagementApi.DataAccess;
using Microsoft.EntityFrameworkCore;
using FollowerManagementApi.Commands;

namespace FollowerManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class followerController : ControllerBase
    {
        FollowerManagementDBContext _dbContext;

        public followerController(FollowerManagementDBContext context) {
            _dbContext = context;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            // return new string[] { "value1", "VALUE2" };
            return Ok(await _dbContext.Followers.ToListAsync());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            // var customers = await _dbContext.Followers.Where(u => u.FollowerId == id).ToListAsync();
            var customers = await (from user in _dbContext.Followers where user.FollowerId == id select user.EmailAddress).ToListAsync();
            if (customers == null)
            {
                return NotFound();
            }
            return Ok(customers);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Follower follower)
        {
            _dbContext.Followers.Add(follower);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // // DELETE api/values/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}
