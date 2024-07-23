public class WAbility : AbilityUpgrade
{
    public float explosionLenght = 5;

    public override void Upgrade()
    {
        if (level < maxLevel)
        {
            level++;
            explosionLenght += 5;
        }
    }
}