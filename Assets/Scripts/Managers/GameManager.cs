using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Разные булева переменные
    public static bool isPlayerStopped;

    //Разные спрайты к которым нужен быстрый доступ
    public Sprite hollowSprite;

    public int LevelCounter = 1; // Счетчик уровней

    [Header("Другие менеджеры")]
    PlayerStats playerStats;
    SkinManager skinManager;
    AudioManager audioManager;

    [Header("Предметы")]
    public List<GameObject> MelleRange; // Лист оружия бл.,боя
    public List<GameObject> Food; // Лист еды
    public List<GameObject> ActiveItems; // Лист активок
    public List<GameObject> PassiveItems; // Лист пассивок
    public List<GameObject> traderItems; // Лист предметов которые продаются

    [Header("Сыр и все что с ним связано")]
    public int playerCheese; // Счетчик сыра игрока
    public GameObject CheesePrefab; //Префаб сыра
    public Text CheeseText; // Счетчик сыра(В UI)

    public static GameManager instance; // Синглтоп

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void OnLevelWasLoaded()
    {
        if (traderItems.Count != 0) //Записываем все не проданные предметы обратно
        {
            int itemsCount = traderItems.Count;

            for (int i = 0; i < itemsCount; i++)
            {
                switch (traderItems[0].GetComponent<ItemInfo>().itemClass)
                {
                    case ItemClass.Food:
                        Food.Add(traderItems[0]);
                        traderItems.Remove(traderItems[0]);
                        break;
                    case ItemClass.MelleRangeWeapon:
                        MelleRange.Add(traderItems[0]);
                        traderItems.Remove(traderItems[0]);
                        break;
                    case ItemClass.ActiveItem:
                        ActiveItems.Add(traderItems[0]);
                        traderItems.Remove(traderItems[0]);
                        break;
                    case ItemClass.PassiveItem:
                        PassiveItems.Add(traderItems[0]);
                        traderItems.Remove(traderItems[0]);
                        break;
                }
            }
        }
    }

    public void SpawnCheese(Vector3 CheesePos, int cheeseCount) // Спавнит сыр
    {
        if (cheeseCount != 0)
        {
            GameObject cheese = Instantiate(CheesePrefab, CheesePos, Quaternion.identity);
            cheese.GetComponent<Cheese>().cheeseScore = cheeseCount;
        }
    }
    public void CheeseScore(int NewCheese) // Зачисляет сыр
    {
        playerCheese += NewCheese;
        CheeseText.text = playerCheese.ToString();
    }
    public IEnumerator ChangeTimeScale(float firstScale, float secondScale, float duration)
    {
        Time.timeScale = firstScale;
        Debug.Log(Time.timeScale);
        yield return new WaitForSeconds(duration);
        Time.timeScale = secondScale;
        Debug.Log(Time.timeScale);
    }
}
