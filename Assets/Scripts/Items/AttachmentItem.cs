using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class AttachmentItem : BaseItem
{
    public enum attachmentType
    {
        Scope,
        Other,
    }

    public attachmentType CurrentAttachmentType;

}
