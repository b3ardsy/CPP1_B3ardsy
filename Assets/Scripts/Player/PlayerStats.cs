using UnityEngine;

public enum SpecialArrowType
{
    None,
    Fire,
    Ice
}

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int startingHealth = 5;
    [SerializeField] private int maxHealthCapacity = 20;
    [SerializeField] private int healthTankUpgradeAmount = 5;

    [Header("Special Ammo")]
    [SerializeField] private int startingSpecialAmmoCapacity = 5;
    [SerializeField] private int maxSpecialAmmoCapacity = 20;
    [SerializeField] private int specialAmmoUpgradeAmount = 5;

    private int currentHealth;
    private int currentMaxHealth;

    private int currentSpecialAmmo;
    private int currentSpecialAmmoCapacity;

    private int bountyTokens = 0;

    private bool hasRoll = false;
    private bool hasFireArrow = false;
    private bool hasIceArrow = false;
    private bool hasKey = false;

    private SpecialArrowType equippedSpecialArrow = SpecialArrowType.None;

    public int CurrentHealth { get { return currentHealth; } }
    public int MaxHealth { get { return currentMaxHealth; } }

    public int CurrentSpecialAmmo { get { return currentSpecialAmmo; } }
    public int MaxSpecialAmmo { get { return currentSpecialAmmoCapacity; } }

    public int BountyTokens { get { return bountyTokens; } }

    public bool HasRoll { get { return hasRoll; } }
    public bool HasFireArrow { get { return hasFireArrow; } }
    public bool HasIceArrow { get { return hasIceArrow; } }
    public bool HasKey { get { return hasKey; } }

    public bool HasAnySpecialArrow
    {
        get { return hasFireArrow || hasIceArrow; }
    }

    public SpecialArrowType EquippedSpecialArrow
    {
        get { return equippedSpecialArrow; }
    }

    void Start()
    {
        ResetStats();
    }

    public void ResetStats()
    {
        currentMaxHealth = startingHealth;
        currentHealth = currentMaxHealth;

        currentSpecialAmmoCapacity = startingSpecialAmmoCapacity;
        currentSpecialAmmo = currentSpecialAmmoCapacity;

        bountyTokens = 0;

        hasRoll = false;
        hasFireArrow = false;
        hasIceArrow = false;
        hasKey = false;

        equippedSpecialArrow = SpecialArrowType.None;

        Debug.Log($"Player stats reset. Health: {currentHealth}/{currentMaxHealth}, Special Ammo: {currentSpecialAmmo}/{currentSpecialAmmoCapacity}");
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, currentMaxHealth);
        Debug.Log($"Health: {currentHealth}/{currentMaxHealth}");
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, currentMaxHealth);
        Debug.Log($"Health: {currentHealth}/{currentMaxHealth}");
    }

    public void UpgradeHealthTank()
    {
        currentMaxHealth += healthTankUpgradeAmount;
        currentMaxHealth = Mathf.Clamp(currentMaxHealth, startingHealth, maxHealthCapacity);

        currentHealth = currentMaxHealth;

        Debug.Log($"Health tank upgraded. Health: {currentHealth}/{currentMaxHealth}");
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public bool TrySpendSpecialAmmo(int amount)
    {
        if (currentSpecialAmmo < amount)
            return false;

        currentSpecialAmmo -= amount;
        Debug.Log($"Special Ammo: {currentSpecialAmmo}/{currentSpecialAmmoCapacity}");
        return true;
    }

    public void AddSpecialAmmo(int amount)
    {
        currentSpecialAmmo = Mathf.Clamp(currentSpecialAmmo + amount, 0, currentSpecialAmmoCapacity);
        Debug.Log($"Special Ammo: {currentSpecialAmmo}/{currentSpecialAmmoCapacity}");
    }

    public void SetSpecialAmmo(int amount)
    {
        currentSpecialAmmo = Mathf.Clamp(amount, 0, currentSpecialAmmoCapacity);
        Debug.Log($"Special Ammo: {currentSpecialAmmo}/{currentSpecialAmmoCapacity}");
    }

    public void PickUpAmmoTank()
    {
        currentSpecialAmmoCapacity += specialAmmoUpgradeAmount;
        currentSpecialAmmoCapacity = Mathf.Clamp(currentSpecialAmmoCapacity, startingSpecialAmmoCapacity, maxSpecialAmmoCapacity);

        currentSpecialAmmo = currentSpecialAmmoCapacity;

        Debug.Log($"Ammo capacity upgraded. Special Ammo: {currentSpecialAmmo}/{currentSpecialAmmoCapacity}");
    }

    public void AddBountyToken()
    {
        bountyTokens++;
        Debug.Log($"Bounty Tokens Acquired: {bountyTokens}");
    }

    public void SetBountyTokens(int amount)
    {
        bountyTokens = Mathf.Max(0, amount);
        Debug.Log($"Bounty Tokens Acquired: {bountyTokens}");
    }

    public void UnlockRoll()
    {
        hasRoll = true;
        Debug.Log("Roll unlocked");
    }

    public void UnlockFireArrow()
    {
        hasFireArrow = true;
        equippedSpecialArrow = SpecialArrowType.Fire;

        Debug.Log("Fire Arrow unlocked");
    }

    public void UnlockIceArrow()
    {
        hasIceArrow = true;
        equippedSpecialArrow = SpecialArrowType.Ice;

        Debug.Log("Ice Arrow unlocked");
    }

    public void ToggleSpecialArrow()
    {
        if (hasFireArrow && hasIceArrow)
        {
            if (equippedSpecialArrow == SpecialArrowType.Fire)
                equippedSpecialArrow = SpecialArrowType.Ice;
            else
                equippedSpecialArrow = SpecialArrowType.Fire;
        }
        else if (hasFireArrow)
        {
            equippedSpecialArrow = SpecialArrowType.Fire;
        }
        else if (hasIceArrow)
        {
            equippedSpecialArrow = SpecialArrowType.Ice;
        }
        else
        {
            equippedSpecialArrow = SpecialArrowType.None;
        }

        Debug.Log($"Equipped Special Arrow: {equippedSpecialArrow}");
    }

    public void PickUpKey()
    {
        hasKey = true;
        Debug.Log("Key acquired");
    }
}