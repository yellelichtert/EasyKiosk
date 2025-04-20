namespace EasyKiosk.Core.Factory;

public interface ITokenFactory<T>
{
    string CreateToken(T obj);
}