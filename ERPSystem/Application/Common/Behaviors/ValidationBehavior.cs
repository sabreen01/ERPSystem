using ERPSystem.Application.Helper.models;
using FluentValidation;
using MediatR;

namespace ERPSystem.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            var errors = string.Join(" | ", failures.Select(f => f.ErrorMessage));
            
            // Checking if TResponse is RequestResult<> to return a formatted error
            var responseType = typeof(TResponse);
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(RequestResult<>))
            {
                var resultType = responseType.GetGenericArguments()[0];
                var failureMethod = typeof(RequestResult<>)
                    .MakeGenericType(resultType)
                    .GetMethod("Failure");

                if (failureMethod != null)
                {
                    return (TResponse)failureMethod.Invoke(null, [errors])!;
                }
            }

            throw new ValidationException(failures);
        }

        return await next();
    }
}
