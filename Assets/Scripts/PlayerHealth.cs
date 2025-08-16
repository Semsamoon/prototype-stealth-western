using System;

public sealed class PlayerHealth : CharacterHealth
{
    public event Action OnDeath;

    protected override void TriggerDeath()
    {
        base.TriggerDeath();
        OnDeath?.Invoke();
    }
}