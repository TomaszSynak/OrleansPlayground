namespace Interfaces
{
    using System.Threading.Tasks;

    public interface IStatelessHelloWorld : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello();
    }
}