
namespace Pooling
{
    public interface IGenericPoolsReturner
    {
        void ReturnObjToPool<T>(T genericElement) where T : IGenericPoolElement;
    }
}