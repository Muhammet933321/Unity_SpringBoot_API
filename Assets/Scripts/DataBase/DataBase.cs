using System.Collections.Generic;

public static class DataBase
{
   
    public static string id;
    public static string name;
    public static string password;
    public static string gold;
    public static string level;
    public static string xp;

    public static string errorTXT;

    public static string[,] leaderBoard = new string[5,5];

    public static Weapon[] weapons;

    public static Weapon GetWeaponByName(string weaponName)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.name.Equals(weaponName, System.StringComparison.OrdinalIgnoreCase))
            {
                return weapon;
            }
        }
        return null;
    }

}
