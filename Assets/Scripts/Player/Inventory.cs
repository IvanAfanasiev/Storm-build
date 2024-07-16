using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IStorage
{
    public List<Item> itemList;

    [SerializeField]
    Transform content;

    [SerializeField]
    ItemAssets asset;

    [SerializeField]
    GameObject itemCell;

    public List<Cell> cells;
    int activeCell = 0;

    public int axe_lvl;
    public int pickaxe_lvl;
    public int fishingRod_lvl;

    private void Start()
    {
        OpenBag();
        LvlsUpdate();
    }

    void LvlsUpdate()
    {
        axe_lvl = asset.Axe;
        pickaxe_lvl = asset.Pickaxe;
        fishingRod_lvl = asset.Rod;
    }

    public void OpenBag()
    {
        itemList = asset.playerInv;
        ReBuildUI();
        ActivityUpdate(0);
    }
    
    public void ReBuildUI()
    {
        for (int i = content.childCount-1; i >= 0; i--)
            Destroy(content.GetChild(i).gameObject);
        BuildUI();
    }
    public void BuildUI()
    {
        cells.Clear();
        for (int i = 0; i < itemList.Count/*inventorySize*/; i++) 
        {
            Cell cell = Instantiate(itemCell, content).GetComponent<Cell>();
            cells.Add(cell);
            cell.storage = GetComponent<IStorage>();
            cell.index = i;
            cell.image.sprite = asset.GetImage((int)itemList[i].itemType);//image
            cell.image.color = new Color(255, 255, 255, 255);
            cell.text.text = itemList[i].Amount.ToString();//text
        }
    }




    public void UpdateActivity()
    {
        ActivityUpdate(activeCell);
    }

    public void AddNewItem(Item newItem)
    {
        if (DoIHave(newItem))
            return;
        itemList.Add(newItem);
        activeCell = itemList.Count-1;
        asset.playerInv = itemList;
    }
    bool DoIHave(Item newItem)
    {
        for(int i=0; i<itemList.Count;i++)
            if (newItem.itemType == itemList[i].itemType)
            {
                itemList[i].Amount += newItem.Amount;
                activeCell = i;
                asset.playerInv = itemList;
                return true;
            }
        return false;
    }





    public void ActivityUpdate(int indx)
    {
        if (indx >= cells.Count)
            indx = cells.Count - 1;
        if (indx < 0)
            return;

        foreach (Cell cell in cells)
        {
            cell.isActive = false;
            cell.pointer.SetActive(false);
        }
        cells[indx].SetActivity(true);
    }

}
