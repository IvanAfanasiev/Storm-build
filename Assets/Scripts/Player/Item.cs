using System;
using UnityEngine;

[Serializable]
public class Item
{
    public Item()
    {
        _amount = 0;
        itemType = ItemType.wood;
    }
    public Item(int am, ItemType type)
    {
        Amount = am;
        itemType = type;
    }

    [SerializeField]
    int _amount;
    public int Amount
    {
        get { return _amount; }
        set 
        {
            if (value >= 0)
                _amount = value;
        }
    }

    public enum ItemType 
    {
        wood,
        stone,
        brick
    }

    public ItemType itemType;
}
