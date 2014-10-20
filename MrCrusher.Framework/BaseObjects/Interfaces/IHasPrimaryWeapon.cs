namespace MrCrusher.Framework.BaseObjects.Interfaces {
    public interface IHasPrimaryWeapon {
        Weapon PrimaryWeapon { get; set; }
        bool ShootPrimaryWeapon();
    }
}
