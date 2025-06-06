# Sidio.Mediator
A simple implementation of the [mediator](https://en.wikipedia.org/wiki/Mediator_pattern) pattern in .NET.

[![build](https://github.com/marthijn/Sidio.Mediator/actions/workflows/build.yml/badge.svg)](https://github.com/marthijn/Sidio.Mediator/actions/workflows/build.yml)
[![Coverage Status](https://coveralls.io/repos/github/marthijn/Sidio.Mediator/badge.svg?branch=main)](https://coveralls.io/github/marthijn/Sidio.Mediator?branch=main)

## Core package
[![NuGet Version](https://img.shields.io/nuget/v/Sidio.Mediator)](https://www.nuget.org/packages/Sidio.Mediator/)

## Request validation package
[![NuGet Version](https://img.shields.io/nuget/v/Sidio.Mediator.Validation)](https://www.nuget.org/packages/Sidio.Mediator.Validation/)

## Source generation for the Mediator service
[![NuGet Version](https://img.shields.io/nuget/v/Sidio.Mediator.SourceGenerator)](https://www.nuget.org/packages/Sidio.Mediator.SourceGenerator/)

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

// Provide an arbitrary type to register all request handlers in the assembly of the type:
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

// Provide an arbitrary type to register all validators in the assembly of the type:
services.AddMediatorValidation(typeof(MyRequest));
```

## Source generators (v2.0+)
In version 2.0 and later, Sidio.Mediator.SourceGenerator includes source generators that create an `IMediator` service implementation at 
compile time.
The `IMediator` implementation contains a method for each request. For example, a request named `MyRequest`:
```csharp
public class MyRequest : IRequest<string>;
```
The generated `IMediator` will have a method:
```csharp
Task<Result<string>> MyRequestAsync(MyRequest request, CancellationToken cancellationToken = default);
```

### Setup
- Add package reference to `Sidio.Mediator.SourceGenerator` in the project that contain the `IRequest` or `IHttpRequest` implementations.
- Register the `IMediator` service in your `Startup.cs` or `Program.cs`:

```csharp
services.AddMediatorService();
```

### Limitations
- Requests should have a unique name across the project which implements the source generator.
- Requests should not be nested in other classes.
- Requests should always implement `IRequest`, `IRequest<T>` or `IHttpRequest<T>`. Inheritance of base/abstract requests is not supported.
- Types used in requests that are part of the parent namespace of that request should be included in global usings, e.g. in the csproj file:
```xml
<ItemGroup>
  <Using Include="MyNamespace" />
</ItemGroup>
```