namespace WebAPI.Exceptions;

public static class ExceptionHandlerConfiguration
{
    public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<SkipableExceptionHandler>();
        services.AddExceptionHandler<DomainExceptionHandler>();
        services.AddExceptionHandler<DefaultExceptionHandler>();

        return services;
    }
}
