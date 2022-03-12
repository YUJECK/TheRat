using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItemsSlots : MonoBehaviour
{
    public ActiveItem activeItem;   // Предмет который лежит в этом слоте
    public GameObject objectOfItem;   // гейм обжект этого предмета

    public bool isEmpty; // Пустой ли этот слот
    public bool isActiveSlot; // Используется ли этот слот сейчас

    public void Add(ActiveItem newActiveItem) // Добавеление предмета
    {
        if(activeItem != null)
            Drop();
        activeItem = newActiveItem;
    }

    public void Drop() // Выброс предмета в игре
    {
        objectOfItem.SetActive(true);
        objectOfItem.transform.position = FindObjectOfType<Player>().GetComponent<Transform>().position;
        Remove();
    }
    
    public void Remove() // Удаление предмета из слота
    {
        activeItem = null;
    }
}