using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider slider;

    [Header("Settings")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float decayPerSecond = 1f;

    private float currentHP;

    public float CurrentHP => currentHP;
    public bool IsDead => currentHP <= 0f;

    private void Start()
    {
        currentHP = maxHP;

        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = maxHP;
            slider.value = currentHP;
        }
    }

    private void Update()
    {
        if (IsDead) return;

        currentHP -= decayPerSecond * Time.deltaTime;
        if (currentHP < 0f) currentHP = 0f;

        if (slider != null)
        {
            slider.value = currentHP;
        }
    }

    public void Damage(float amount)
    {
        currentHP = Mathf.Clamp(currentHP - amount, 0f, maxHP);
        if (slider != null) slider.value = currentHP;
    }

    public void Heal(float amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0f, maxHP);
        if (slider != null) slider.value = currentHP;
    }
}
