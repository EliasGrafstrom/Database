using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, ICustomerRepository customerRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    public async Task<bool> CreateProjectAsync(ProjectRegistrationForm form)
    {

        if (!await _customerRepository.ExsistsAsync(customer => customer.Id == form.CustomerId))
            return false;

        var projectEntity = ProjectFactory.Create(form);
        if (projectEntity == null)
            return false;

        bool result = await _projectRepository.AddAsync(projectEntity);
        return result;
    }

    public async Task<IEnumerable<Project?>> GetProjectsAsync()
    {
        var entities = await _projectRepository.GetAllAsync();
        var projects = entities.Select(ProjectFactory.Create);
        return projects;
    }

    public async Task<Project?> GetProjectAsync(int id)
    {
        var projectEntity = await _projectRepository.GetAsync(x => x.Id == id);
        if (projectEntity == null)
            return null;

        var project = ProjectFactory.Create(projectEntity);
        return project;
    }

    public async Task<bool> UpdateProjectAsync(Project project)
    {
        var projectEntity = await _projectRepository.GetAsync(x => x.Id == project.Id);
        if (projectEntity == null)
            return false;
        projectEntity.ProjectName = project.ProjectName;
        projectEntity.Description = project.Description;
        projectEntity.CustomerId = project.CustomerId;
        bool result = await _projectRepository.UpdateAsync(projectEntity);
        return result;
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        var projectEntity = await _projectRepository.GetAsync(x => x.Id == id);
        if (projectEntity == null)
            return false;
        bool result = await _projectRepository.RemoveAsync(projectEntity);
        return result;
    }

}
