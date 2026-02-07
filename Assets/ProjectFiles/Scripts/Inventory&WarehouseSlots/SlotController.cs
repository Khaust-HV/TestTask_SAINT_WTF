using ProjectFiles.Scripts.Buildings.Resources;
using UnityEngine;

namespace ProjectFiles.Scripts.Slots
{
    public sealed class SlotController : MonoBehaviour
    {
        // Properties
        public bool IsSlotFull { get; private set; }
        public ResourceType CurrentResourceType { get; private set; }

        // Fields
        private IContolTheResource _currentResource;

        public void SetResource(IContolTheResource resource)
        {
            IsSlotFull = true; 

            CurrentResourceType = resource.ResourceType;

            _currentResource = resource;

            _currentResource.SetParent(transform);
        }

        public IContolTheResource GetResource() 
        {
            IsSlotFull = false;
            
            CurrentResourceType = ResourceType.None;

            return _currentResource;
        }
    }
}