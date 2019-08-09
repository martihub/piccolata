using UnityEngine;
using Opsive.UltimateCharacterController.Traits;
using Opsive.UltimateCharacterController.Inventory;

namespace Opsive.UltimateCharacterController.Demo.BehaviorDesigner
{
    /// <summary>
    /// Helper functions for the demo behavior tree.
    /// </summary>
    public class DemoAgent : MonoBehaviour
    {
        private Health m_Health;
        private InventoryBase m_Inventory;
        private ItemType m_ItemType;

        // Expose the health and ammo via a Behavior Designer property mapping.
        public float Health { get { return m_Health.HealthValue; } }
        public float Ammo { get { return m_ItemType != null ? m_Inventory.GetItemTypeCount(m_ItemType) : float.MaxValue; } }

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        private void Start()
        {
            m_Health = GetComponent<Health>();
            m_Inventory = GetComponent<InventoryBase>();

            // Find the ItemType.
            var itemType = m_Inventory.DefaultLoadout[0].ItemType;
            var item = m_Inventory.GetItem(0, itemType);
            // If the first DefaultLoadout element is an item then the consumable ItemType should be retrieved.
            if (item != null) {
                var itemActions = item.ItemActions;
                for (int i = 0; i < itemActions.Length; ++i) {
                    var usableItem = itemActions[i] as Items.Actions.IUsableItem;
                    if (usableItem != null) {
                        m_ItemType = usableItem.GetConsumableItemType();
                        break;
                    }
                }
            } else {
                m_ItemType = itemType;
            }
        }
    }
}