using Cysharp.Threading.Tasks;
using Zenject;

namespace Managers.Interfaces
{
    public interface IController 
    {
        bool IsInit { get; }
        
        public void Init(DiContainer container);

    }
}
