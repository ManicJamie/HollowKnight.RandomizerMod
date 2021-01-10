using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalEnums;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using RandomizerMod.Components;
using RandomizerMod.FsmStateActions;
using SeanprCore;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;
using static RandomizerMod.LogHelper;
using System.Collections;
using RandomizerMod.Randomization;
using System;
using Object = UnityEngine.Object;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using static RandomizerMod.GiveItemActions;

namespace RandomizerMod
{
    public static class StartSaveChanges
    {
        public const string RESPAWN_MARKER_NAME = "Randomizer Respawn Marker";
        public const string RESPAWN_TAG = "RespawnPoint";
        private static StartDef start => LogicManager.GetStartLocation(RandomizerMod.Instance.Settings.StartName);

        private static void CreateRespawnMarker()
        {
            GameObject respawnMarker = ObjectCache.RespawnMarker;
            respawnMarker.transform.SetPosition2D(start.x, start.y);
            respawnMarker.transform.SetPositionZ(7.4f);
            respawnMarker.name = RESPAWN_MARKER_NAME;
            respawnMarker.tag = RESPAWN_TAG;
            respawnMarker.SetActive(true);
        }

        // Merge or move this switch block into scene changes
        public static void StartSceneChanges(Scene newScene)
        {
            if (newScene.name != start.sceneName) return;

            CreateRespawnMarker();

            switch (newScene.name)
            {
                case SceneNames.Ruins1_27:
                    PlayerData.instance.hornetFountainEncounter = true;
                    Object.Destroy(GameObject.Find("Fountain Inspect"));
                    break;

                case SceneNames.Abyss_06_Core:
                    PlayerData.instance.abyssGateOpened = true;
                    break;
            }
        }

        public static void StartDataChanges()
        {
            PlayerData.instance.Reset();

            PlayerData.instance.hasCharm = true;

            /*
            if (RandomizerMod.Instance.Settings.FreeLantern)
            {
                PlayerData.instance.hasLantern = true;
            }
            */

            if (RandomizerMod.Instance.Settings.EarlyGeo)
            {
                // added version checking to the early geo randomization
                int geoSeed = RandomizerMod.Instance.Settings.Seed;
                unchecked
                {
                    geoSeed = geoSeed * 17 + 31 * RandomizerMod.Instance.MakeAssemblyHash();
                }

                // added settings checking to early geo randomization
                int settingsSeed = 0;
                if (RandomizerMod.Instance.Settings.RandomizeDreamers) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeSkills) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeCharms) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeKeys) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeGeoChests) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeMaskShards) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeVesselFragments) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeCharmNotches) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizePaleOre) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeRancidEggs) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeRelics) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeMaps) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeStags) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeGrubs) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeWhisperingRoots) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeRocks) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeSoulTotems) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizePalaceTotems) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeLoreTablets) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeLifebloodCocoons) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeGrimmkinFlames) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.RandomizeBossEssence) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.DuplicateMajorItems) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.ShadeSkips) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.AcidSkips) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.SpikeTunnels) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.MildSkips) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.SpicySkips) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.FireballSkips) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;
                if (RandomizerMod.Instance.Settings.DarkRooms) settingsSeed += 1;
                settingsSeed = settingsSeed << 1;

                Random rand = new Random(geoSeed + settingsSeed);
                int startgeo = rand.Next(300, 600);
                PlayerData.instance.AddGeo(startgeo);
            }

            Ref.PD.unchainedHollowKnight = true;
            Ref.PD.encounteredMimicSpider = true;
            Ref.PD.infectedKnightEncountered = true;
            Ref.PD.mageLordEncountered = true;
            Ref.PD.mageLordEncountered_2 = true;

            List<string> startItems = RandomizerMod.Instance.Settings.ItemPlacements.Where(pair => pair.Item2.StartsWith("Equip")).Select(pair => pair.Item1).ToList();
            foreach (string item in startItems)
            {
                GiveAction action = LogicManager.GetItemDef(item).action;
                if (action == GiveAction.Charm) action = GiveAction.EquippedCharm;
                else if (action == GiveAction.SpawnGeo) action = GiveAction.AddGeo;

                GiveItem(action, item, "Equipped");
            }

            for (int i = 1; i < 5; i++)
            {
                if (PlayerData.instance.charmSlotsFilled > PlayerData.instance.charmSlots)
                {
                    PlayerData.instance.charmSlots++;
                    PlayerData.instance.SetBool("salubraNotch" + i, true);
                }
                if (PlayerData.instance.charmSlotsFilled <= PlayerData.instance.charmSlots)
                {
                    PlayerData.instance.overcharmed = false;
                    break;
                }
            }

            PlayerData.instance.respawnScene = start.sceneName;
            PlayerData.instance.respawnMarkerName = RESPAWN_MARKER_NAME;
            PlayerData.instance.respawnType = 0;
            PlayerData.instance.mapZone = start.zone;
        }
    }
}
