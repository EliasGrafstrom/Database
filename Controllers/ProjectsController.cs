using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController(IProjectService projectService) : ControllerBase
{
    private readonly IProjectService _projectService = projectService;

    [HttpPost]
    public async Task<IActionResult> Create(ProjectRegistrationForm form)
    {
        if (!ModelState.IsValid && form.CustomerId < 1)
            return BadRequest();
        var result = await _projectService.CreateProjectAsync(form);
        return result ? Created("", null) : Problem();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectService.GetProjectsAsync();
        return Ok(projects);
    }

    //genererat av chatgpt

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Project project)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingProject = await _projectService.GetProjectAsync(id);
        if (existingProject == null)
            return NotFound();

        // Uppdatera projektets egenskaper
        existingProject.ProjectName = project.ProjectName;
        existingProject.Description = project.Description;
        existingProject.CustomerId = project.CustomerId;

        var result = await _projectService.UpdateProjectAsync(existingProject);
        return result ? Ok() : Problem();
    }

}