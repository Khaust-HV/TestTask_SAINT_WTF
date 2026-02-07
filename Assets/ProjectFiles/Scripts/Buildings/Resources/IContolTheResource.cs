using UnityEngine;

namespace ProjectFiles.Scripts.Buildings.Resources
{
    public interface IContolTheResource
    {
        // Properties
        public ResourceType ResourceType { get; }

        // Methods
        public void SetParent(Transform parent);
        public void DestroyResource();
    }
}