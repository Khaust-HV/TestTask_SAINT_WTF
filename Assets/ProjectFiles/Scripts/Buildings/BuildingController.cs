using System.Collections;
using ProjectFiles.Scripts.Buildings.Resources;
using ProjectFiles.Scripts.Installers;
using UnityEngine;

namespace ProjectFiles.Scripts.Buildings
{
    public sealed class BuildingController : MonoBehaviour, IInjectable
    {
        [Header("Building settings")]
        [SerializeField] private BuildingWarehouseController _consumedWarehouseController;
        [SerializeField] private BuildingWarehouseController _producedWarehouseController;
        [SerializeField] private ResourceType _resourceTypeToBeCreated;
        [SerializeField] float _resourceCreateDuration;
        [SerializeField] float _reseiveOrGiveResourcesDelay;

        // Component
        private DependencyContainer _container;

        // Base fields
        private bool isProductionActive;

        // Receiving resources
        private bool _isReceivingResourcesActive;
        private Coroutine _receivingResourcesCoroutine;

        // Giving resources
        private bool _isGivingResourcesActive;
        private Coroutine _givingResourcesCoroutine;

        public void Construct(DependencyContainer container)
        {
            // Set components
            _container = container;

            // Set events
            _consumedWarehouseController.OnPlayerEnteredTheTrigger += OnPlayerEnteredConsumedWarehouse;
            _producedWarehouseController.OnPlayerEnteredTheTrigger += OnPlayerEnteredProducedWarehouse;
        }

        private void Start()
        {
            if (!isProductionActive && CheckToStartProduction()) StartCoroutine(ProductionCoroutine());
        }

        #region Production
        private bool CheckToStartProduction()
        {
            if (!_producedWarehouseController.CheckCanSetResource())
            {
                _container.ControlNotification.Show(transform, "Produced warehouse is full");
                
                return false;
            }

            bool canStart = false;

            switch (_resourceTypeToBeCreated)
            {
                case ResourceType.N1:
                    canStart = true;
                    break
                ;

                case ResourceType.N2:
                    if (_consumedWarehouseController.CheckCanGetResource(ResourceType.N1)) canStart = true;
                    break
                ;

                case ResourceType.N3:
                    if (_consumedWarehouseController.CheckCanGetResource(ResourceType.N1) &&
                        _consumedWarehouseController.CheckCanGetResource(ResourceType.N2)
                    ) canStart = true;
                    break
                ;
            }

            if (!canStart) _container.ControlNotification.Show(transform, "Consumed warehouse is empty");
            else _container.ControlNotification.Hide(transform);

            return canStart;
        }

        private IEnumerator ProductionCoroutine()
        {
            isProductionActive = true;

            ResourceController resourcePrefab = null;
            
            while (true)
            {
                switch (_resourceTypeToBeCreated)
                {
                    case ResourceType.N1:
                        resourcePrefab = _container.N1Prefab;
                        break
                    ;

                    case ResourceType.N2:
                        _consumedWarehouseController.GetResource(ResourceType.N1).DestroyResource(); 

                        resourcePrefab = _container.N2Prefab;
                        break
                    ;

                    case ResourceType.N3:
                        _consumedWarehouseController.GetResource(ResourceType.N1).DestroyResource(); 
                        _consumedWarehouseController.GetResource(ResourceType.N2).DestroyResource();

                        resourcePrefab = _container.N3Prefab;
                        break
                    ;
                }

                yield return new WaitForSeconds(_resourceCreateDuration);

                var newResource = Instantiate(resourcePrefab);

                _producedWarehouseController.SetResource(newResource);

                if (!CheckToStartProduction()) break;
            }
            
            isProductionActive = false;
        }
        #endregion

        #region Player interaction with consumed warehouse
        private void OnPlayerEnteredConsumedWarehouse(bool isActive)
        {
            if (isActive)
            {
                if (!_consumedWarehouseController.CheckCanSetResource() || !CheckToStartReceivingResources()) return; 

                _receivingResourcesCoroutine = StartCoroutine(StartReceivingResources());  
            }
            else if (_isReceivingResourcesActive)
            {
                StopCoroutine(_receivingResourcesCoroutine);

                _isReceivingResourcesActive = false;
            }
        }

        private bool CheckToStartReceivingResources()
        {
            if (!_consumedWarehouseController.CheckCanSetResource()) return false;

            switch (_resourceTypeToBeCreated)
            {
                case ResourceType.N1:
                    return false;
                    // break
                ;

                case ResourceType.N2:
                    if (!_container.ControlPlayerInventory.CheckCanGetResource(ResourceType.N1)) return false;
                    break
                ;

                case ResourceType.N3:
                    if (!_container.ControlPlayerInventory.CheckCanGetResource(ResourceType.N1) &&
                        !_container.ControlPlayerInventory.CheckCanGetResource(ResourceType.N2)
                    ) return false;  
                    break
                ;
            } 

            return true;
        }

        private IEnumerator StartReceivingResources()
        {
            _isReceivingResourcesActive = true;

            while (true)
            {
                switch (_resourceTypeToBeCreated)
                {
                    case ResourceType.N2:
                        var newResource = _container.ControlPlayerInventory.GetResource(ResourceType.N1);

                        _consumedWarehouseController.SetResource(newResource);
                        break
                    ;

                    case ResourceType.N3:
                        if (_container.ControlPlayerInventory.CheckCanGetResource(ResourceType.N1) && 
                            !_consumedWarehouseController.Check50PercentFilledWithResource(ResourceType.N1))
                        {
                            newResource = _container.ControlPlayerInventory.GetResource(ResourceType.N1);
                            _consumedWarehouseController.SetResource(newResource);
                        }
                        else if (_container.ControlPlayerInventory.CheckCanGetResource(ResourceType.N2) && 
                            !_consumedWarehouseController.Check50PercentFilledWithResource(ResourceType.N2))
                        {
                            newResource = _container.ControlPlayerInventory.GetResource(ResourceType.N2);
                            _consumedWarehouseController.SetResource(newResource);
                        }
                        else
                        {
                            _isReceivingResourcesActive = false;

                            yield break;
                        }
                        break
                    ;
                }

                Start(); 

                yield return new WaitForSeconds(_reseiveOrGiveResourcesDelay);

                if (!CheckToStartReceivingResources()) break;
            }

            _isReceivingResourcesActive = false;
        }
        #endregion
        
        #region Player interaction with produced warehouse
        private void OnPlayerEnteredProducedWarehouse(bool isActive)
        {
            if (isActive)
            {
                if (!_producedWarehouseController.CheckCanGetResource(_resourceTypeToBeCreated) || !_container.ControlPlayerInventory.CheckCanSetResource()) return;

                _givingResourcesCoroutine = StartCoroutine(StartGivingResources());
            }
            else if (_isGivingResourcesActive)
            {
                StopCoroutine(_givingResourcesCoroutine);

                _isGivingResourcesActive = false;
            }
        }
        
        private IEnumerator StartGivingResources()
        {
            _isGivingResourcesActive = true;

            while (true)
            {
                var resource = _producedWarehouseController.GetResource(_resourceTypeToBeCreated);

                _container.ControlPlayerInventory.SetResource(resource);

                Start();

                yield return new WaitForSeconds(_reseiveOrGiveResourcesDelay);

                if (!_producedWarehouseController.CheckCanGetResource(_resourceTypeToBeCreated) || !_container.ControlPlayerInventory.CheckCanSetResource()) break;
            }

            _isGivingResourcesActive = false;
        }
        #endregion
    }
}