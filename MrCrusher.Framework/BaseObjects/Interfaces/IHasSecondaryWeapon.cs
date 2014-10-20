namespace MrCrusher.Framework.BaseObjects.Interfaces {
    public interface IHasSecondaryWeapon {
        Weapon SecondaryWeapon { get; set; }
        bool ShootSecondaryWeapon();
    }
}
