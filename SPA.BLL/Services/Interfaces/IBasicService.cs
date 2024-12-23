namespace SPA.BLL.Services.Interfaces;

public interface IBasicService<TModel> where TModel : class
{
    Task<TModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}