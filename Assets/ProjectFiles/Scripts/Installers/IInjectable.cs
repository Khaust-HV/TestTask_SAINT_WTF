using UnityEngine;

namespace ProjectFiles.Scripts.Installers
{
    public interface IInjectable
    {
        public void Construct(DependencyContainer container);
    }
}