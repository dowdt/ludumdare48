using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> Items = new List<Item>();

    public Transform ItemHolder;

    public int SelectedSlot = 0;

    public void pickupItem(Item item) {
        if(!Items.Contains(item))
            Items.Add(item);
    }

    public void dropItem(Item item)
    {
        Items.Remove(item);
    }

    private void LateUpdate()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (SelectedSlot == i)
            {
                
                Items[i].transform.rotation = ItemHolder.rotation;
                

                Vector3 posOffset = Items[i].getOffsetPos();
                Items[i].transform.position = ItemHolder.position + ItemHolder.up * posOffset.y + ItemHolder.right* posOffset.x + ItemHolder.forward* posOffset.z;


                Vector3 rotOffset = Items[i].getOffsetRot();
                Items[i].transform.localRotation = Items[i].transform.localRotation * Quaternion.Euler(rotOffset);

                Items[i].ItemInventoryUpdate();

            }
            Items[i].gameObject.SetActive((SelectedSlot == i));
        }
    }
}
