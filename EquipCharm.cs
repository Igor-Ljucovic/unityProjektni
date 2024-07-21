using UnityEngine;

public class EquipCharm : MonoBehaviour
{

    //      OBJECTS:


    private PlayerCharms playerCharms;


    //      SETUP:


    private void Start()
    {
        playerCharms = PlayerCharms.Instance;
    }


    //      CHARMS - CLASSIC


    public void EquipStalwartShellCharm() => playerCharms.CharmToggle(ref playerCharms.stalwartShellCharmEquipped, playerCharms.stalwartShellCharmNotchCost);

    public void EquipSoulCatcherCharm() => playerCharms.CharmToggle(ref playerCharms.soulCatcherCharmEquipped, playerCharms.soulCatcherCharmNotchCost);

    public void EquipShamanStoneCharm() => playerCharms.CharmToggle(ref playerCharms.shamanStoneCharmEquipped, playerCharms.shamanStoneCharmNotchCost);

    public void EquipSoulEaterCharm() => playerCharms.CharmToggle(ref playerCharms.soulEaterCharmEquipped, playerCharms.soulEaterCharmNotchCost);

    public void EquipSprintMasterCharm() => playerCharms.CharmToggle(ref playerCharms.sprintMasterCharmEquipped, playerCharms.sprintMasterCharmNotchCost);

    public void EquipSpellTwisterCharm() => playerCharms.CharmToggle(ref playerCharms.spellTwisterCharmEquipped, playerCharms.spellTwisterCharmNotchCost);

    public void EquipUnbreakableStrengthCharm() => playerCharms.CharmToggle(ref playerCharms.unbreakableStrengthCharmEquipped, playerCharms.unbreakableStrengthCharmNotchCost);

    public void EquipQuickSlashCharm() => playerCharms.CharmToggle(ref playerCharms.quickSlashCharmEquipped, playerCharms.quickSlashCharmNotchCost);

    public void EquipSteadyBodyCharm() => playerCharms.CharmToggle(ref playerCharms.steadyBodyCharmEquipped, playerCharms.steadyBodyCharmNotchCost);

    public void EquipMarkOfPrideCharm() => playerCharms.CharmToggle(ref playerCharms.markOfPrideCharmEquipped, playerCharms.markOfPrideCharmNotchCost);

    public void EquipLongNailCharm() => playerCharms.CharmToggle(ref playerCharms.longNailCharmEquipped, playerCharms.longNailCharmNotchCost);

    public void EquipFuryOfTheFallenCharm() => playerCharms.CharmToggle(ref playerCharms.furyOfTheFallenCharmEquipped, playerCharms.furyOfTheFallenCharmNotchCost);


    //      CHARMS - ULTIMATE


    public void EquipSpellboundGraceCharm() => playerCharms.CharmToggle(ref playerCharms.spellboundGraceCharmEquipped, playerCharms.spellboundGraceCharmNotchCost);

    public void EquipSonicSoulCharm() => playerCharms.CharmToggle(ref playerCharms.sonicSoulCharmEquipped, playerCharms.sonicSoulCharmNotchCost);

    public void EquipBladeDanceSymphonyCharm() => playerCharms.CharmToggle(ref playerCharms.bladeDanceSymphonyCharmEquipped, playerCharms.bladeDanceSymphonyCharmNotchCost);

    public void EquipCurseOfTheSoullessCharm() => playerCharms.CharmToggle(ref playerCharms.curseOfTheSoullessCharmEquipped, playerCharms.curseOfTheSoullessCharmNotchCost);

    public void EquipAbyssalDesperationCharm() => playerCharms.CharmToggle(ref playerCharms.abyssalDesperationCharmEquipped, playerCharms.abyssalDesperationCharmNotchCost);

    public void EquipOverflowingEssenceCharm() => playerCharms.CharmToggle(ref playerCharms.overflowingEssenceCharmEquipped, playerCharms.overflowingEssenceCharmNotchCost);

    public void EquipArcaneEnigmaCharm() => playerCharms.CharmToggle(ref playerCharms.arcaneEnigmaCharmEquipped, playerCharms.arcaneEnigmaCharmNotchCost);

    public void EquipAirborneSupremacyCharm() => playerCharms.CharmToggle(ref playerCharms.airborneSupremacyCharmEquipped, playerCharms.airborneSupremacyCharmNotchCost);

    public void EquipDiamondPointCharm() => playerCharms.CharmToggle(ref playerCharms.diamondPointCharmEquipped, playerCharms.diamondPointNotchCost);

    public void EquipDaredevilsGambleCharm() => playerCharms.CharmToggle(ref playerCharms.daredevilsGambleCharmEquipped, playerCharms.daredevilsGambleCharmNotchCost);

    public void EquipUnimpairedCataclysmCharm() => playerCharms.CharmToggle(ref playerCharms.unimpairedCataclysmCharmEquipped, playerCharms.unimpairedCataclysmCharmNotchCost);

    public void EquipExcessivelyExtendedCharm() => playerCharms.CharmToggle(ref playerCharms.excessivelyExtendedCharmEquipped, playerCharms.excessivelyExtendedCharmNotchCost);

    public void UnequipAllClassicCharms() => playerCharms.UnequipAllClassicModeCharmsCheck();

    public void UnequipAllUltimateModeCharms() => playerCharms.UnequipAllUltimateModeCharmsCheck();
}
