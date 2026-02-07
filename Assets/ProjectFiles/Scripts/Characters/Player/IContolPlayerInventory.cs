using ProjectFiles.Scripts.Buildings.Resources;

namespace ProjectFiles.Scripts.Characters.Player
{
    public interface IContolPlayerInventory
    {
        // Check methods
        public bool CheckCanSetResource();
        public bool CheckCanGetResource(ResourceType type);

        // Get/Set methods
        public void SetResource(IContolTheResource resource);
        public IContolTheResource GetResource(ResourceType type);
    }
}