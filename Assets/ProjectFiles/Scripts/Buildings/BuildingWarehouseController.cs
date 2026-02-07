using System;
using ProjectFiles.Scripts.Buildings.Resources;
using ProjectFiles.Scripts.Installers;
using ProjectFiles.Scripts.Slots;
using UnityEngine;

namespace ProjectFiles.Scripts.Buildings
{
    public sealed class BuildingWarehouseController : MonoBehaviour, IInjectable
    {
        // Events
        public event Action<bool> OnPlayerEnteredTheTrigger;

        // Components
        private DependencyContainer _container;

        // Fields
        private SlotController[] _slots;

        public void Construct(DependencyContainer container)
        {
            // Set components
            _container = container;

            // Set fields
            _slots = GetComponentsInChildren<SlotController>();
        }

        // Check methods
        public bool CheckCanSetResource()
        {
            foreach (var slot in _slots) if (!slot.IsSlotFull) return true;

            return false;
        }

        public bool CheckCanGetResource(ResourceType type)
        {
            foreach (var slot in _slots) if (slot.CurrentResourceType == type) return true;

            return false;
        }

        public bool Check50PercentFilledWithResource(ResourceType resourceType)
        {
            byte filledSlotsNumber = 0;

            foreach (var slot in _slots) if (slot.IsSlotFull && slot.CurrentResourceType == resourceType) filledSlotsNumber++;

            return filledSlotsNumber >= _slots.Length / 2;
        }

        // Get/Set methods
        public void SetResource(IContolTheResource resource)
        {
            foreach (var slot in _slots) if (!slot.IsSlotFull)
            {
                slot.SetResource(resource);

                return;
            }

            throw new Exception("Warehouse is full");
        }

        public IContolTheResource GetResource(ResourceType type)
        {
            foreach (var slot in _slots) if (slot.CurrentResourceType == type) return slot.GetResource();

            throw new Exception("Resource not found");
        }

        // Entered and exited triggers
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) OnPlayerEnteredTheTrigger?.Invoke(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) OnPlayerEnteredTheTrigger?.Invoke(false);
        }
    }
}