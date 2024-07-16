using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemAssets", menuName = "Asset/Item")]
public class ItemAssets : ScriptableObject
{
    [SerializeField]
    List<Sprite> sprites = new ();

    public List<Item> playerInv;
    public List<Item> playerTmpInv;//temporary inventory
    public List<Item> playerChest;

    public Sprite GetImage(int indx)
    {
        return sprites[indx];
    }
    public int gems;

    public int Axe;
    public int Pickaxe;
    public int Rod;

    public void RemoveItem(List<Item> neededItems)
    {
        foreach (Item NI in neededItems)
            foreach (Item item in playerInv)
                if (item.itemType == NI.itemType)
                    item.Amount -= NI.Amount;
    }
    public void AddNewItem(Item newItem)
    {
        if (DoIHave(newItem))
            return;
        playerInv.Add(newItem);
    }
    bool DoIHave(Item newItem)
    {
        for (int i = 0; i < playerInv.Count; i++)
            if (newItem.itemType == playerInv[i].itemType)
            {
                playerInv[i].Amount += newItem.Amount;
                return true;
            }
        return false;
    }

}
