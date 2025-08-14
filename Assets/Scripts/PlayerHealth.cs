using UnityEngine;

public sealed class PlayerHealth : CharacterHealth
{
    protected override void TriggerDeath()
    {
        base.TriggerDeath();
        Time.timeScale = 0;
    }
}