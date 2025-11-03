namespace RegistroJugadores.Dtos;
public static class ResourceExtensionscs
{
    /// <summary>
    /// Verifica si el recurso es exitoso.
    /// </summary>
    public static bool IsSuccess<T>(this Resource<T> resource)
    {
        return resource is Resource<T>.Success;
    }

    /// <summary>
    /// Verifica si el recurso tiene error.
    /// </summary>
    public static bool IsError<T>(this Resource<T> resource)
    {
        return resource is Resource<T>.Error;
    }

    /// <summary>
    /// Ejecuta una acción si el recurso es exitoso.
    /// </summary>
    public static Resource<T> OnSuccess<T>(this Resource<T> resource, Action<T> action)
    {
        if (resource is Resource<T>.Success success)
        {
            action(success.Data);
        }
        return resource;
    }

    /// <summary>
    /// Ejecuta una acción asíncrona si el recurso es exitoso.
    /// </summary>
    public static async Task<Resource<T>> OnSuccessAsync<T>(this Resource<T> resource, Func<T, Task> action)
    {
        if (resource is Resource<T>.Success success)
        {
            await action(success.Data);
        }
        return resource;
    }

    /// <summary>
    /// Ejecuta una acción si el recurso tiene error.
    /// </summary>
    public static Resource<T> OnError<T>(this Resource<T> resource, Action<string> action)
    {
        if (resource is Resource<T>.Error error)
        {
            action(error.Message);
        }
        return resource;
    }

    /// <summary>
    /// Ejecuta una acción asíncrona si el recurso tiene error.
    /// </summary>
    public static async Task<Resource<T>> OnErrorAsync<T>(this Resource<T> resource, Func<string, Task> action)
    {
        if (resource is Resource<T>.Error error)
        {
            await action(error.Message);
        }
        return resource;
    }

    /// <summary>
    /// Transforma el valor de éxito a otro tipo.
    /// </summary>
    public static Resource<TResult> Map<T, TResult>(this Resource<T> resource, Func<T, TResult> mapper)
    {
        return resource switch
        {
            Resource<T>.Success success => new Resource<TResult>.Success(mapper(success.Data)),
            Resource<T>.Error error => new Resource<TResult>.Error(error.Message),
            _ => new Resource<TResult>.Error("Unknown error")
        };
    }

    /// <summary>
    /// Transforma el valor de éxito a otro tipo de forma asíncrona.
    /// </summary>
    public static async Task<Resource<TResult>> MapAsync<T, TResult>(this Resource<T> resource, Func<T, Task<TResult>> mapper)
    {
        return resource switch
        {
            Resource<T>.Success success => new Resource<TResult>.Success(await mapper(success.Data)),
            Resource<T>.Error error => new Resource<TResult>.Error(error.Message),
            _ => new Resource<TResult>.Error("Unknown error")
        };
    }
}
