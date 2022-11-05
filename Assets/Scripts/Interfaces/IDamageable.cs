public interface IDamageable
{
    public void ReceiveDamage(ReceiveDamageAction receiveDamageAction);
}

public enum ReceiveDamageAction
{
    punch,
    
}