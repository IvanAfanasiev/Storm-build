using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerDownHandler
{
    public Image image;
    public TextMeshProUGUI text;
    public int index = 0;

    public IStorage storage;

    public GameObject pointer;

    public bool isActive = false;

    [SerializeField]
    bool canBeActivated = true;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canBeActivated)
            return;
        storage.ActivityUpdate(index);
        SetActivity(true);
    }
    public void SetActivity(bool actv)
    {
        isActive = actv;
        pointer.SetActive(actv);
    }
}
