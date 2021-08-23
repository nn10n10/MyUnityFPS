using UnityEngine;

namespace Items
{
    public abstract class BaseItem: MonoBehaviour
    {
        public enum ItemType
        {
            Firearms,
            Oters
        }

        public ItemType CurrentItemType;
        public int ItemId;
    }
}