# Sidio.Mediator
A simple implementation of the mediator pattern in .NET.

[![build](https://github.com/marthijn/Sidio.Mediator/actions/workflows/build.yml/badge.svg)](https://github.com/marthijn/Sidio.Mediator/actions/workflows/build.yml)
[![Coverage Status](https://coveralls.io/repos/github/marthijn/Sidio.Mediator/badge.svg?branch=main)](https://coveralls.io/github/marthijn/Sidio.Mediator?branch=main)

## Core package
[![NuGet Version](https://img.shields.io/nuget/v/Sidio.Mediator)](https://www.nuget.org/packages/Sidio.Mediator/)

## Request validation package
[![NuGet Version](https://img.shields.io/nuget/v/Sidio.Mediator.Validation)](https://www.nuget.org/packages/Sidio.Mediator.Validation/)

# Usage
## Requests and requests handlers
```csharp
// Define a request and request handler
public class MyRequest : IRequest<string>
{
    public string Name { get; init; }
}

public class MyRequestHandler : IRequestHandler<MyRequest, string>
{
    public Task<Result<string>> HandleAsync(MyRequest request, CancellationToken cancellationToken = default)
    {
        var result = Result<string>.Success($"Hello {request.Name}");
        // Or: Result<string>.Failure("error code", "error message");
        return Task.FromResult(result);
    }
}

// Register the request handler
services.AddMediator(typeof(MyRequest));

// Get the request handler from the service provider
var requestHander = serviceProvider.GetRequiredService<IRequestHandler<MyRequest, string>>();
var result = await requestHander.HandleAsync(new MyRequest { Name = "World" });
```

### Http requests
```csharp
// Define a request and request handler
public class MyHttpRequest : IHttpRequest<string>
{
    public string Name { get; init; }
}

public class MyHttpRequestHandler : IHttpRequestHandler<MyRequest, string>
{
    public Task<HttpResult<string>> HandleAsync(MyRequest request, CancellationToken cancellationToken = default)
    {
        var result = HttpResult<string>.Ok($"Hello {request.Name}");
        // Or for example: HttpResult<string>.Unauthorized();
        return Task.FromResult(result);
    }
}
```

## Request validation
Request validation uses [FluentValidation](https://docs.fluentvalidation.net/).

```csharp
// Define a validator
public class MyRequestValidator : AbstractValidator<MyRequest>
{
    public MyRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

// Register the validators
services.AddMediatorValidation(typeof(MyRequest));
```