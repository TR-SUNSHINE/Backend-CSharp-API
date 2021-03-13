using LambdaCSharpWebAPI.Models;
using LambdaCSharpWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LambdaCSharpWebAPI.Controllers
{
    [Route("v1/taskList")]
    public class TaskListController : Controller
    {
        private readonly ITaskListService taskListService;

        public TaskListController(ITaskListService taskListService)
        {
            this.taskListService = taskListService;
        }
        [HttpGet("task/{taskId}")]
        public IActionResult GetSingleTask(string taskId)
        {
            try
            {
                var result = taskListService.GetSingleItemFromTaskList(taskId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public IActionResult AddItemToTaskList([FromBody]TaskListModel taskList)
        {
            try
            {
                taskListService.AddItemsToTaskList(taskList);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete]
        public IActionResult DeleteItemsFromTaskList([FromBody]TaskListModel taskList)
        {
            try
            {
                taskListService.RemoveItem(taskList.Description);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}