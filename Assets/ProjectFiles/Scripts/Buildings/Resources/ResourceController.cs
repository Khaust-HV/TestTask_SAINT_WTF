using ProjectFiles.Scripts.Installers;
using UnityEngine;

namespace ProjectFiles.Scripts.Buildings.Resources
{
    public sealed class ResourceController : MonoBehaviour, IContolTheResource
    {
        [field: SerializeField] public ResourceType ResourceType { get; private set; }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void DestroyResource() => Destroy(gameObject);
    }

    public enum ResourceType
    {
        None,
        N1,
        N2,
        N3
    }
}