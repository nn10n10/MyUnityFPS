using UnityEngine;

namespace Items
{
    public abstract class BaseItem: MonoBehaviour
    {
        public enum ItemType
        {
            Firearms,
            Attachment,
            Oters
        }

        public ItemType CurrentItemType;
        public int ItemId;
        public string ItemName;
    }
}