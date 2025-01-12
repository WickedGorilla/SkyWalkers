using System.Collections;

namespace Infrastructure.Coroutine
{
    public interface ICoroutineRunner
    {
        void StartCoroutine(IEnumerator routine);
    }
}