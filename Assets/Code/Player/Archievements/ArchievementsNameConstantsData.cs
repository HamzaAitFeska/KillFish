using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;

[System.Serializable]
public static class ArchievementsNameConstantsData
{
    // Lista que contendrá todas las instancias de ArchievementData
    public static List<ArchievementData> archievementList = new List<ArchievementData>();

    public static string LevelOneName = "FORSAKEN FACILITY"; //Scene number 0
    public static int LevelOneBuildIndex = 1;
    public static string LevelTwoName = "THE LAST FORGE"; //Scene number 1
    public static int LevelTwoBuildIndex = 2;

    //Instancias de ArchievementData que deseas crear
    public static ArchievementData RichI =
        new ArchievementData("Rich, But Not Really", "Reach 200.000 Points", "Arch_RichI");

    public static ArchievementData RichII =
        new ArchievementData("Rich, Really", "Reach 300.000 Points", "Arch_RichII");

    public static ArchievementData RichIII =
        new ArchievementData("Can you even get this rich?", "Reach 400.000 Points", "Arch_RichIII");

    public static ArchievementData Tryhard =
        new ArchievementData("Tryhard", "Reach the SSS Style while playing", "Arch_Tryhard");

    public static ArchievementData ProfessionalKillfisherI =
        new ArchievementData("Professional Killfisher I", "Reach the S on Time and Style at the end of Forsaken Facility", "Arch_ProfessionalKillfisherI");

    public static ArchievementData ProfessionalKillfisherII =
        new ArchievementData("Professional Killfisher II", "Reach the S on Time and Style at the end of The Last Forge", "Arch_ProfessionalKillfisherII");

    public static ArchievementData Rawdogger =
        new ArchievementData("Rawdogger", "Damage 5 enemies with a single explosion", "Arch_Rawdogger");

    public static ArchievementData LevitationI =
        new ArchievementData("Levitation Geek", "Stay more than 15 seconds without touching the ground while fighting enemies", "Arch_LevitationI");

    public static ArchievementData LevitationII =
        new ArchievementData("Levitation Master", "Stay more than 30 seconds without touching the ground while fighting enemies", "Arch_LevitationII");

    public static ArchievementData Speedrunner =
        new ArchievementData("Speedrunner", "Complete the game in less than 10 minutes", "Arch_Speedrunner");

    public static ArchievementData MetallicPinchito =
        new ArchievementData("Metallic pinchito", "Kill more than one enemy with a single sniper shot", "Arch_MetallicPinchito");

    public static ArchievementData Pirouette =
        new ArchievementData("Pirouette", "Do 20 HookKills/AirHookKills in a single level", "Arch_Pirouette");

    public static ArchievementData DontNeedToMoveWorldMovesForMe =
        new ArchievementData("I dont need to move, the world moves for me", "Complete the transition of The Last Forge without using the dash", "Arch_DontNeedToMove");

    public static ArchievementData TheFloorIsLava =
        new ArchievementData("The Floor is Lava", "Complete The Last Forge without touching the lava", "Arch_FloorIsLava");
    static ArchievementsNameConstantsData()
    {
        archievementList.Add(RichI);
        archievementList.Add(RichII);
        archievementList.Add(RichIII);
        archievementList.Add(Tryhard);
        archievementList.Add(ProfessionalKillfisherI);
        archievementList.Add(ProfessionalKillfisherII);
        archievementList.Add(Rawdogger);
        archievementList.Add(LevitationI);
        archievementList.Add(LevitationII);
        archievementList.Add(Speedrunner);
        archievementList.Add(MetallicPinchito);
        archievementList.Add(Pirouette);
        archievementList.Add(DontNeedToMoveWorldMovesForMe);
        archievementList.Add(TheFloorIsLava);
    }
    public static void RichArchievementsAccomplished(float points)
    {
        if(points > 200000.0f)
        {
            RichI.SetAccomplished();
        }
        if (points > 300000.0f)
        {
            RichII.SetAccomplished();
        }
        if (points > 400000.0f)
        {
            RichIII.SetAccomplished();
        }
    }

    public static void TryhardAccomplisher()
    {
        Tryhard.SetAccomplished();
    }

    public static void ProfessionalKillfisherChecker()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        if (scene == LevelOneBuildIndex)
        { 
            ProfessionalKillfisherI.SetAccomplished();
        }
        else if (scene == LevelTwoBuildIndex)
        {
            ProfessionalKillfisherII.SetAccomplished();

        }
    }
    public static void LevitationChecker(float time)
    {
        if(time >= 15f)
        {
            LevitationI.SetAccomplished();
        }
        if(time >= 30f)
        {
            LevitationII.SetAccomplished();
        }
    }
    public static bool HaveToCheckOnLevitation()
    {
        return !LevitationI.IsAccomplished() || !LevitationII.IsAccomplished();
    }
    public static void PirouetteChecker(int kills)
    {
        if(kills == 20) {
            Pirouette.SetAccomplished();
        }
    }
    public static void MetallicPinchitoAccomplisher()
    {
        MetallicPinchito.SetAccomplished();
    }
    public static void SpeedRunnerAccomplisher()
    {
        Speedrunner.SetAccomplished();
    }
    public static void DontNeedToMoveWorldMovesForMeAccomplisher()
    {
        DontNeedToMoveWorldMovesForMe.SetAccomplished();
    }
    public static void TheFloorIsLavaAccomplisher()
    {
        TheFloorIsLava.SetAccomplished();
    }

    public static void RawdoggerAccomplisher()
    {
        Rawdogger.SetAccomplished();
    }



}
[System.Serializable]
public class ArchievementData
{
    /// <summary>
    /// The Name of this Archievement
    /// </summary>
    public string Name;
    /// <summary>
    /// The description on this archievement
    /// </summary>
    public string Description;
    /// <summary>
    /// The ID for the string on the Player Prefs Data for this archievement
    /// </summary>
    public string SteamIDApiString;

    public ArchievementData(string nName, string nDesc, string nPlayerPrefsID)
    {
        Name = nName;
        Description = nDesc;
        SteamIDApiString = nPlayerPrefsID;
    }
    public void SetAccomplished()
    {
        if (!SteamManager.Initialized) return;

        SteamUserStats.SetAchievement(SteamIDApiString);
        SteamUserStats.StoreStats();
    }
    public bool IsAccomplished()
    {
        return PlayerPrefs.GetInt(SteamIDApiString) == 1;
    }
    public void ResetAccomplished()
    {
        if (!SteamManager.Initialized) return;

        SteamUserStats.ClearAchievement(SteamIDApiString);
    }
}
