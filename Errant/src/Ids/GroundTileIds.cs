
public class GroundIds {
    public static readonly ushort Water = 0;
    public static readonly ushort Grass = 1;
    public static readonly ushort LushGrass = 2;
    public static readonly ushort DryGrass = 3;
    public static readonly ushort ThickGrass = 4;
    public static readonly ushort MossyGrass = 5;
    public static readonly ushort Snow = 6;
    public static readonly ushort Stone = 7;
    public static readonly ushort SnowyGrass = 8;
    public static readonly ushort Sand = 9;
    public static readonly ushort Conglomerate = 10;
    public static readonly ushort Basalt = 11;

    public static readonly ushort[] priorities = {
        Water, Stone, Conglomerate, Basalt, Sand, DryGrass, Grass, LushGrass, SnowyGrass, Snow, ThickGrass, MossyGrass
    };
}
