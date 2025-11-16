using GeoGoAPI._models.dtos.categories;
using GeoGoAPI._services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeoGoAPI._controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Admin endpoints for managing categories.")]
public class CategoriesController(ICategoryService service) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all categories",
        Description = "Returns all categories ordered by name."
    )]
    [SwaggerResponse(200, "List of categories returned", typeof(List<CategoryDto>))]
    public async Task<ActionResult<List<CategoryDto>>> GetAll() => Ok(await service.GetAllAsync());

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Get category by id")]
    [SwaggerResponse(200, "Category found", typeof(CategoryDto))]
    [SwaggerResponse(404, "Category not found")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var cat = await service.GetByIdAsync(id);
        return cat is null ? NotFound() : Ok(cat);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new category")]
    [SwaggerResponse(201, "Category created", typeof(CategoryDto))]
    [SwaggerResponse(409, "Category name already exists")]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto dto)
    {
        try
        {
            var created = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex) when (ex.ParamName == nameof(dto.Name))
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Update an existing category")]
    [SwaggerResponse(200, "Category updated", typeof(CategoryDto))]
    [SwaggerResponse(404, "Category not found")]
    [SwaggerResponse(409, "Category name already exists")]
    public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] UpdateCategoryDto dto)
    {
        try
        {
            var updated = await service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex) when (ex.ParamName == nameof(dto.Name))
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Delete a category")]
    [SwaggerResponse(204, "Deleted successfully")]
    [SwaggerResponse(404, "Category not found")]
    [SwaggerResponse(409, "Category is in use by places")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var ok = await service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
