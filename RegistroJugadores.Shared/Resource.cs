namespace RegistroJugadores.Shared;
public abstract record Resource<T>(T? Data, string? Message)
{
    public record Loading(T? Data = default) : Resource<T>(Data, null);
    public record Success(T Data) : Resource<T>(Data, null);
    public record Error(string Message, T? Data = default) : Resource<T>(Data, Message);
}
