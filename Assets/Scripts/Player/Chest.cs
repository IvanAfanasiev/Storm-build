using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour, IStorage
{
    public List<Item> itemList;

    [SerializeField]
    int oneTimeAmount = 1;
    [SerializeField]
    TextMeshProUGUI onetimetext;

    [SerializeField]
    List<Cell> cells;
    int activeCell = 0;

    [SerializeField]
    Transform content;
    [SerializeField]
    GameObject itemCell;
    [SerializeField]
    ItemAssets asset;

    [SerializeField]
    Inventory inventory;

    void Start()
    {
        onetimetext.text = oneTimeAmount.ToString();
        OpenChest();
    }

    public void OpenChest()
    {
        itemList = asset.playerChest;
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
        for (int i = 0; i < itemList.Count; i++)
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





    public void PutItem()//from inventory => chest
    {
        Item newItem = new();
        for (int i = 0; i < inventory.itemList.Count; i++)
            if (inventory.cells[i].isActive)
            {
                newItem = new(0, inventory.itemList[i].itemType);
                for (int j = 0; j < oneTimeAmount; j++)
                {
                    inventory.itemList[i].Amount--;
                    newItem.Amount++;
                    if(inventory.itemList[i].Amount <= 0)
                    {
                        inventory.itemList.RemoveAt(i);
                        break;
                    }
                }
                inventory.ReBuildUI();
                inventory.ActivityUpdate(i);
                break;
            }

        AddNewItem(newItem);
        ReBuildUI();
        ActivityUpdate(activeCell);
    }
    public void GetItem()//from chest => inventory
    {
        Item newItem = new();
        for (int i = 0; i < cells.Count; i++)
            if (cells[i].isActive)
            {
                newItem = new(0, itemList[i].itemType);
                for (int j = 0; j < oneTimeAmount; j++)
                {
                    itemList[i].Amount--;
                    newItem.Amount++;
                    if (itemList[i].Amount <= 0)
                    {
                        itemList.RemoveAt(i);
                        break;
                    }
                }
                ReBuildUI();
                ActivityUpdate(i);
                break;
            }
        inventory.AddNewItem(newItem);
        inventory.ReBuildUI();
        inventory.UpdateActivity();
    }



    public void AddNewItem(Item newItem)
    {
        if (DoIHave(newItem))
            return;
        itemList.Add(newItem);
        activeCell = itemList.Count - 1;
    }
    bool DoIHave(Item newItem)
    {
        for(int i=0; i<itemList.Count;i++)
            if (newItem.itemType == itemList[i].itemType)
            {
                itemList[i].Amount += newItem.Amount;
                activeCell = i;
                return true;
            }
        return false;
    }

    public void IncAmount()
    {
        if(oneTimeAmount == 1)
            oneTimeAmount = 5;
        else if (oneTimeAmount == 5)
            oneTimeAmount = 10;

        onetimetext.text = oneTimeAmount.ToString();
    }
    public void DecAmount()
    {
        if (oneTimeAmount == 10)
            oneTimeAmount = 5;
        else if (oneTimeAmount == 5)
            oneTimeAmount = 1;

        onetimetext.text = oneTimeAmount.ToString();
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
        activeCell = indx;
    }
}
