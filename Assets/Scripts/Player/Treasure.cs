using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class Treasure : MonoBehaviour, ITrigger
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

    public void ActionChanges()
    {
        GetComponent<TriggerActivator>().ready = true;
        GetComponent<TriggerActivator>().Show_UI();
    }

    public string ActionName()
    {
        return "Pick";
    }

    public bool Check()//doesn't require anything
    {
        return true;
    }

    public void Interact()
    {
        Debug.Log("You've been working with the treasure " + gameObject.name);
        //GetComponent<Animator>().Play("myAnim");
    }


    void Start()
    {
        onetimetext.text = oneTimeAmount.ToString();
        OpenChest();
    }

    public void OpenChest()
    {
        ReBuildUI();
        ActivityUpdate(0);
    }

    public void ReBuildUI()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
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


    public void GetItem()//treasureChest => inv
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
    public void GetAllItems()//all treasureChest => inv
    {
        Item newItem = new();
        for (int i = 0; i < cells.Count; i++)
        {
            newItem = new(0, itemList[i].itemType);
            while (itemList[i].Amount > 0)
            {
                itemList[i].Amount--;
                newItem.Amount++;
            }
            inventory.AddNewItem(newItem);
        }
        itemList.Clear();
        ReBuildUI();
        ActivityUpdate(0);
             
        inventory.ReBuildUI();
        inventory.UpdateActivity();
    }


    public void IncAmount()
    {
        if (oneTimeAmount == 1)
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
