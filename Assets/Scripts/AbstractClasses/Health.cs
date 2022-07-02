using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class EffectStats
{
    public EffectStats(float rate, int strength)
    {
        effectRate = rate;
        effectStrength = strength; 
    }

    public float effectRate;
    public int effectStrength;
    [HideInInspector] public float nextTime;
};
public abstract class Health : MonoBehaviour
{
    //���������� ��������
    [Header("��������")]
    public int health;
    public int maxHealth;

    [SerializeField] private Color damageColor;
    public SpriteRenderer effectIndicator;

    [Header("�����")]
    [SerializeField] private string destroySound; // ���� ������
    [SerializeField] private string hitSound; // ���� ��������� �����

    [Header("�������")]
    public List<EffectsList> effectsCanUse;
    [HideInInspector] public EffectStats burn;
    [HideInInspector] public EffectStats poisoned;
    [HideInInspector] public EffectStats bleed;
    [HideInInspector] public EffectStats regeneration;

    //�������
    [Header("�������")]
    public UnityEvent onDie = new UnityEvent();  //������ ������� ���������� ��� ����������� �������
    public UnityEvent<int, int> onHealthChange = new UnityEvent<int, int>();
    public UnityEvent<float> stun = new UnityEvent<float>();
    public UnityEvent effects = new UnityEvent();

    //������
    private Coroutine damageInd = null;
    [HideInInspector] public EffectsInfo effectManager;

    public void DefaultOnDie() => Destroy(gameObject);

    //������ ����������� �� ���������
    public abstract void TakeHit(int damage, float stunDuration = 0f);
    public abstract void TakeAwayHealth(int takeAwayMaxHealth, int takeAwayHealth);
    public abstract void Heal(int heal);
    public abstract void SetHealth(int newMaxHealth, int newHealth);
    public abstract void PlusNewHealth(int newMaxHealth, int newHealth);

    public IEnumerator TakeHitVizualization()
    {
        gameObject.GetComponent<SpriteRenderer>().color = damageColor;
        yield return new WaitForSeconds(0.6f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(100, 100, 100, 100);
    }


    //������� ������� ����� ���������� �� �����    
    private IEnumerator EffectActive(float duration, EffectStats effectStats, EffectsList effect)
    {
        if(effectsCanUse.Contains(effect))
        {
            UnityAction effectMethod = null;
            switch (effect)
            {
                case EffectsList.Burn:
                    effectIndicator.sprite = effectManager.burnIndicator;
                    burn = effectStats;
                    effects.AddListener(Burn);
                    effectMethod = Burn;
                    break;
                case EffectsList.Bleed:
                    effectIndicator.sprite = effectManager.bleedndicator;
                    bleed = effectStats;
                    effects.AddListener(Bleed);
                    effectMethod = Bleed;
                    break;
                case EffectsList.Poisoned:
                    effectIndicator.sprite = effectManager.poisonedIndicator;
                    poisoned = effectStats;
                    effects.AddListener(Poisoned);
                    effectMethod = Poisoned;
                    break;
                case EffectsList.Regeneration:
                    effectIndicator.sprite = effectManager.regenerationIndicator;
                    regeneration = effectStats;
                    effects.AddListener(Regeneration);
                    effectMethod = Regeneration;
                    break;
            }
            yield return new WaitForSeconds(duration);
            effects.RemoveListener(effectMethod);
        }
    }
    public void GetEffect(float duration, EffectStats effectStats, EffectsList effect) => StartCoroutine(EffectActive(duration, effectStats, effect));

    //�������
    public void Burn() 
    {
        if(burn.nextTime <= Time.time)
        {
            burn.nextTime = Time.time + burn.effectRate;
            TakeHit(burn.effectStrength); 
        }
    }
    public void Poisoned()
    {
        if (Time.time >= poisoned.nextTime)
        {
            Debug.Log("effect");
            poisoned.nextTime = Time.time + poisoned.effectRate;
            TakeHit(poisoned.effectStrength);
        }
    }
    public void Bleed()
    {
        if (Time.time >= bleed.nextTime)
        {
            bleed.nextTime = Time.time + bleed.effectRate;
            TakeHit(bleed.effectStrength);
        }
    }
    public void Regeneration()
    {
        if (Time.time >= regeneration.nextTime)
        {
            regeneration.nextTime = Time.time + regeneration.effectRate;
            TakeHit(regeneration.effectStrength);
        }
    }
}