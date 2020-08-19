using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Infrastructure.Interfaces;
using ToDoApi.Models;
using ToDoApi.ViewModels;

namespace ToDoApi.Controllers
{
    [Route("[controller]/[action]")]
    public class ToDoController : Controller
    {
        private readonly ITodoService service;
        public ToDoController(ITodoService service)
        {
            this.service = service;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Response.Headers.Add("Authorization","boogie");
            
            return Ok(await service.GetAllTodo());
        }

        [HttpGet(Name="ActivityGet")]
        public async Task<IActionResult> GetByActivity(string activity)
        {
            if(!string.IsNullOrEmpty(activity))
            {
                var data=await service.GetOneByActivity(activity);
                if(data != null)
                {
                    return Ok(data);
                }
                return NotFound(new { message="No activity found" });
            }
            return BadRequest(new ToSend(){Message="The rewuest was terrible"});
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]NewViewModel model)
        {
            if(!ModelState.IsValid || model == null)
            {
                return BadRequest(new ToSend(){Message= "model is null, apparently "});
            }
            var todo=new ToDoModel(){
                DateCreated=DateTime.Now.Date,
                Activity=model.Activity,
                Description=model.Description
            };
            try
            {
                await service.CreateToDo(todo);
            }catch(Exception e)
            {
                return BadRequest(new {message=e.Message});
            }
           // return Ok(new { message="created successfully"});
           return CreatedAtRoute("ActivityGet",model);
        }

        public IActionResult Update()
        {
            return Ok("u");
        }
        public IActionResult Delete()
        {
            return Ok("d");
        }

    }

    class ToSend
    {
        public string Message { get; set; }
    }
}