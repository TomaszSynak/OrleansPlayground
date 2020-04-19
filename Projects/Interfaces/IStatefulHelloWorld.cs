namespace Interfaces
{
    using System.Threading.Tasks;

    public interface IStatefulHelloWorld : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello();
    }
}
