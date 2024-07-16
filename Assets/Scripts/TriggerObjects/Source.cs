using TMPro;
using UnityEngine;

public class Source : MonoBehaviour, ITrigger
{
    public Item item;

    public int amount = 0;
    public Item.ItemType type;

    public GameObject unActiveSource;
    [SerializeField]
    Transform model;
    [SerializeField]
    ItemAssets playerInv;
    [SerializeField]
    int reqLvl;
    [SerializeField]
    TextMeshProUGUI text;

    public enum ActionType
    {
        chop,
        pick,
        mine,
        dig
    }
    [SerializeField]
    ActionType action;

    void Start()
    {
        item = new Item(amount, type);
    }

    public void Interact()
    {
        Debug.Log("You have interacted with the resource " + gameObject.name);
        GetComponent<Animator>().Play("Chop");
        playerInv.AddNewItem(item);
    }

    public string ActionName()
    {
        switch (action)
        {
            case ActionType.chop:
                return "Chop";
            case ActionType.pick:
                return "Pick";
            case ActionType.mine:
                return "Mine";
            case ActionType.dig:
                return "Dig";
            default:
                return "Pick";
        }
    }

    public void ActionChanges()
    {
        Destroy(model.GetChild(0).gameObject);
        Instantiate(unActiveSource, model);
        GetComponent<TriggerActivator>().Show_UI();
    }
    public bool Check()//check the availability of the required tools
    {
        text.text = "Cut me down <br>pls";
        switch (type)
        {
            case Item.ItemType.wood:
                return playerInv.Axe >= reqLvl;
            case Item.ItemType.stone:
                return playerInv.Pickaxe >= reqLvl;
            case Item.ItemType.brick:
                return playerInv.Rod >= reqLvl;
            default:
                text.text = "Needed instrument lvl is " + reqLvl;
                return false;
        }
    }
}
