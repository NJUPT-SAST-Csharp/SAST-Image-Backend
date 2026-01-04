using Mediator;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Application.CategoryServices.CreateCategory;

public sealed record class CreateCategoryCommand(CategoryName Name, CategoryDescription Description)
    : ICommand;
