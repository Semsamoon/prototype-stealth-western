using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    protected bool _isAlive = true;

    public virtual void TakeDamage()
    {
        if (!_isAlive) return;
        _isAlive = false;
        TriggerDeath();
    }

    protected virtual void TriggerDeath()
    {
    }

    public virtual bool IsAlive()
    {
        return _isAlive;
    }
}