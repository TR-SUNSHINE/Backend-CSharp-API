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
        [HttpGet("{taskId}")]
        public IActionResult GetSingleTask(string taskId)
        {
            try
            {
                var result = taskListService.GetTasks(taskId);
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
        [HttpGet]
        public IActionResult GetAllTasks()
        {
            try
            {
                var result = taskListService.GetTasks();
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
        public IActionResult AddTask([FromBody]TaskListModel taskList)
        {
            try
            {
                taskListService.AddTask(taskList);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete]
        public IActionResult DeleteTask([FromBody]TaskListModel taskList)
        {
            try
            {
                taskListService.DeleteTask(taskList.TaskId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public IActionResult UpdateTask([FromBody]TaskListModel taskList)
        {
            try
            {
                taskListService.UpdateTask(taskList);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}