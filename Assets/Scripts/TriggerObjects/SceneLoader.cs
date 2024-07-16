using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ITrigger
{
    [SerializeField]
    int scene;
    [SerializeField]
    bool IsSavingInventory;


    public void ActionChanges()
    {
        SceneManager.LoadScene(scene);
    }

    public string ActionName()
    {
        return "";
    }

    public void Interact()
    {
        Debug.Log("you just interacted with the loader " + gameObject.name);
        GetComponent<Animator>().Play("");
        SceneManager.LoadScene(scene);
        if (IsSavingInventory)
            return;
    }

    public bool Check()
    {
        return true;
    }
}
