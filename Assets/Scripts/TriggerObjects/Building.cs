using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Building : MonoBehaviour, ITrigger
{
    [SerializeField]
    int currLvl;

    [SerializeField]
    List<GameObject> models = new List<GameObject>();

    [SerializeField]
    Transform model;
    [SerializeField]
    ItemAssets playerInv;
    [SerializeField]
    List<Item> reqItems;
    [SerializeField]
    TextMeshProUGUI text;

    public void Interact()
    {
        Debug.Log("You've been working with the building " + gameObject.name);
        GetComponent<Animator>().Play("Building");
        playerInv.RemoveItem(reqItems);
    }

    public string ActionName()
    {
        return "Build";
    }

    public void NextLvl()//calling by animation
    {
        if (currLvl == models.Count - 1)
            return;
        currLvl++;
        Destroy(model.GetChild(0).gameObject);
        Instantiate(models[currLvl], model);
    }

    public void ActionChanges()
    {
        TriggerActivator TA = GetComponent<TriggerActivator>();
        if (currLvl != models.Count - 1)
            TA.ready = true;
        else
            TA.ChangeType();

        TA.Show_UI();
    }
    public bool Check()//check the availability of the required items
    {
        string txt = "Needed items:<br>"; //\n
        foreach (Item item in reqItems)
        {
            txt += item.itemType + ": "+ item.Amount+"<br>";
        }
        text.text = txt;

        foreach (Item item in reqItems)
            foreach (Item playerItem in playerInv.playerInv)
                if (playerItem.itemType == item.itemType && playerItem.Amount < item.Amount)
                {

                    return false;
                }

        return true;
    }
}
