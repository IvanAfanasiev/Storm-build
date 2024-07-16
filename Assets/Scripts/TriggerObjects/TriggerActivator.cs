using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TriggerActivator : MonoBehaviour
{
    public Transform targetPos;
    [SerializeField]
    ITrigger triggerFuncs;
    
    enum TriggerType
    {
        Source,
        Building,
        Treasure,
        SceneLoader,
        NPC
    }
    [SerializeField]
    TriggerType type;
    [SerializeField]
    TriggerType secondType;
    [SerializeField]
    bool isChangable;

    [SerializeField]
    bool isIn;
    public bool ready = true; //ready to interact

    [SerializeField]
    GameObject triggerUI;

    GameObject player;


    Animator anim; 
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    GameObject button;
    [SerializeField]
    Image buttonImage;
    [SerializeField]
    Sprite newSprite;

    private void Start()
    {
        player = Camera.main.transform.parent.gameObject;
        triggerFuncs = GetComponent<ITrigger>();
        targetPos = Camera.main.transform;
        anim = GetComponent<Animator>();
        DeactvateUI();
    }

    void Update()
    {
        if (!isIn || !ready)
            return;
        triggerUI.transform.forward = (transform.position - targetPos.position);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isIn)
            return;
        isIn = true;
        Show_UI();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!isIn)
            return;
        isIn = false;
        Hide_UI();
    }


    public void Show_UI()
    {
        Check();
        if (isIn && ready)
            anim.Play("Show_UI");
    }
    public void Hide_UI()
    {
        if (!isIn && ready)
            anim.Play("Hide_UI");
    }


    public void DeactvateUI()
    {
        triggerUI.SetActive(false);
    }
    public void ActivateUI()
    {
        triggerUI.SetActive(true);
    }

    public void Interact()
    {
        if (!ready)
            return;
        ready = false;
        Animator playerAnim = player.GetComponent<Animator>();
        string animName = "Action_" + triggerFuncs.ActionName();
        playerAnim.Play(animName);
        switch (type)
        {
            case TriggerType.Source:
                GetResource();
                break;
            case TriggerType.Building:
                BuildNew();
                break;
            case TriggerType.Treasure:
                break;
            case TriggerType.SceneLoader: 
                break;
            case TriggerType.NPC: 
                break;
        }
        player.GetComponent<PlayerMovement>().lastSource = triggerFuncs;
        triggerFuncs.Interact();
    }

    void GetResource()
    {
        //Item item = GetComponent<Source>().item;
        //player.GetComponent<PlayerMovement>().newItem = item;
    }
    void BuildNew() //build a building
    {
    
    }


    public void ChangeType()
    {
        if (!isChangable)
            return;
        ready = true;
        type = secondType;

        Destroy(GetComponent<Building>());
        switch (type)
        {
            case TriggerType.Source:
                break; 
            case TriggerType.Building:
                break;
            case TriggerType.Treasure:
                break;
            case TriggerType.SceneLoader:
                text.text = "wow<br>whose what's that there?";
                buttonImage.sprite = newSprite;
                buttonImage.color = new Color(1, 1, 1);
                triggerFuncs = GetComponent<SceneLoader>();
                break;
            case TriggerType.NPC:
                break;
        }
    }



    void Check()
    {
        //сундук не активирует
        if (type != TriggerType.Source && type != TriggerType.Building)
            return;
        if(triggerFuncs.Check())
        {
            button.GetComponent<Image>().color = new Color(0, 0.55f, 0);
            button.GetComponent<Button>().enabled = true;
        }
        else
        {
            //button.GetComponent<Image>().color = new Color(130, 130, 130);
            button.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            button.GetComponent<Button>().enabled = false;
        }
    }
}
