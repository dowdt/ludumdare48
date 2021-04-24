using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    public static ItemStack[] Items = new ItemStack[0];

    public Transform ItemHolder;

    public static int SelectedSlot = 0;

    public ItemStack[] initItems = new ItemStack[0];
    private void Awake()
    {
        Items = initItems;
    }

    private void LateUpdate()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 100f;
        if (scroll != 0f)
        {
            if (scroll > 0f)
                SelectedSlot++;
            if (scroll < 0f)
                SelectedSlot--;
            if (SelectedSlot >= Items.Length)
                SelectedSlot = 0;
            if (SelectedSlot < 0)
                SelectedSlot = Items.Length-1;

            while (!Items[SelectedSlot].hasItem)
            {
                SelectedSlot++;
                if (SelectedSlot > Items.Length)
                    SelectedSlot = 0;
            }
        }


        for (int i = 0; i < Items.Length; i++)
        {

            if (SelectedSlot == i)
            {
                if (Items[i].item == null)
                {
                    Items[i].item = Instantiate(Items[i].itemAsset, ItemHolder);
                }

                Items[i].item.transform.rotation = ItemHolder.rotation;
                

                Vector3 posOffset = Items[i].item.getOffsetPos();
                Items[i].item.transform.position = ItemHolder.position + ItemHolder.up * posOffset.y + ItemHolder.right* posOffset.x + ItemHolder.forward* posOffset.z;


                Vector3 rotOffset = Items[i].item.getOffsetRot();
                Items[i].item.transform.localRotation = Items[i].item.transform.localRotation * Quaternion.Euler(rotOffset);

                Items[i].item.ItemInventoryUpdate();

            }
            if(Items[i].item != null)
                Items[i].item.gameObject.SetActive((SelectedSlot == i));
        }
    }
}

[System.Serializable]
public class ItemStack {
    public Item itemAsset;
    public Item item;
    public bool hasItem = false;
}
