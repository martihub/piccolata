using Opsive.UltimateCharacterController.Inventory;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    /// <summary>
    /// Creates a SharedVariable for the ItemType class.
    /// </summary>
    [System.Serializable]
    public class SharedItemType : SharedVariable<ItemType>
    {
        public static implicit operator SharedItemType(ItemType value) { var sharedVariable = new SharedItemType(); sharedVariable.SetValue(value); return sharedVariable; }
    }
}