namespace PKHeX.Core;

/// <summary>
/// Synthesized <see cref="WA9"/> templates for HOME 4.0.0 Z-A connectivity Mystery Gifts that
/// are distributed via Pokémon HOME (not via the in-game Mystery Gift menu) and therefore are
/// not yet present in the regular <c>wa9.pkl</c> distribution archive.
/// </summary>
/// <remarks>
/// These entries should be removed when official <c>.wa9</c> templates are added upstream.
/// Distribution metadata derives from <see cref="EncounterServerDate"/> (CardIDs 9031–9033).
/// </remarks>
internal static class Encounters9aHome
{
    private const ushort LocationHomeGift = 30018;
    private const ushort ScaleAny = 256;

    public static WA9[] BuildSynthetic()
    {
        return
        [
            // Alpha Chikorita — HOME 4.0.0 Z-A connectivity gift (CardID 9031)
            Build(9031, species: 0152, nature: Nature.Quiet,
                ivHp: 31, ivAtk: 20, ivDef: 31, ivSpe: 31, ivSpa: 20, ivSpd: 20,
                move1: 033, move2: 039, move3: 605),

            // Alpha Tepig — HOME 4.0.0 Z-A connectivity gift (CardID 9032)
            Build(9032, species: 0498, nature: Nature.Brave,
                ivHp: 31, ivAtk: 31, ivDef: 20, ivSpe: 31, ivSpa: 20, ivSpd: 20,
                move1: 033, move2: 039, move3: 528),

            // Alpha Totodile — HOME 4.0.0 Z-A connectivity gift (CardID 9033)
            Build(9033, species: 0158, nature: Nature.Naughty,
                ivHp: 31, ivAtk: 20, ivDef: 20, ivSpe: 31, ivSpa: 20, ivSpd: 31,
                move1: 033, move2: 043, move3: 337),
        ];
    }

    private static WA9 Build(int cardId, ushort species, Nature nature,
        int ivHp, int ivAtk, int ivDef, int ivSpe, int ivSpa, int ivSpd,
        ushort move1, ushort move2, ushort move3)
    {
        var wa = new WA9
        {
            CardID = cardId,
            CardTitleIndex = 0,
            CardFlags = 0,
            CardType = WA9.GiftType.Pokemon,
            RestrictVersion = 1, // Z-A only

            Species = species,
            Form = 0,
            Level = 5,
            MetLevel = 5,
            Gender = 3,    // any
            OTGender = 3,  // any (skips ID32 / OT-name comparisons)
            Ball = (byte)Ball.Poke,
            HeldItem = 0,

            Location = LocationHomeGift,
            EggLocation = 0,
            OriginGame = 0, // any (defaults compare disabled)

            Nature = nature,
            AbilityType = 0, // OnlyFirst
            PIDType = ShinyType8.Never,

            Move1 = move1,
            Move2 = move2,
            Move3 = move3,
            Move4 = 0,

            IV_HP  = ivHp,
            IV_ATK = ivAtk,
            IV_DEF = ivDef,
            IV_SPE = ivSpe,
            IV_SPA = ivSpa,
            IV_SPD = ivSpd,

            Scale = ScaleAny,
            IsAlpha = true,
        };
        return wa;
    }
}
