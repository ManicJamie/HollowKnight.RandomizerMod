using System;
using System.Collections.Generic;
using System.Linq;
using RandomizerMod.Randomization;
using SeanprCore;
using UnityEngine;
using static RandomizerMod.LogHelper;
using static RandomizerMod.GiveItemActions;
using Object = UnityEngine.Object;

namespace RandomizerMod.Actions
{
    public abstract class RandomizerAction
    {
        public enum ActionType
        {
            GameObject,
            PlayMakerFSM
        }

        private static readonly List<RandomizerAction> Actions = new List<RandomizerAction>();
        public static Dictionary<string, string> AdditiveBoolNames = new Dictionary<string, string>(); // item name, additive bool name
        public static Dictionary<(string, string), string> ShopItemBoolNames = new Dictionary<(string, string), string>(); // (item name, shop name), shop item bool name

        public abstract ActionType Type { get; }

        public static void ClearActions()
        {
            Actions.Clear();
        }

        public static void CreateActions((string, string)[] items, SaveSettings settings)
        {
            ClearActions();
            
            ShopItemBoolNames = new Dictionary<(string, string), string>();
            AdditiveBoolNames = new Dictionary<string, string>();

            int newShinies = 0;
            string[] shopNames = LogicManager.ShopNames;

            // Loop non-shop items
            foreach ((string newItemName, string location) in items.Where(item => !shopNames.Contains(item.Item2)))
            {
                ReqDef oldItem = LogicManager.GetItemDef(location);
                ReqDef newItem = LogicManager.GetItemDef(newItemName);

                if (!settings.RandomizeMaps && newItem.pool == "Map")
                {
                    continue;
                }
                if (!settings.RandomizeStags && newItem.pool == "Stag") 
                {
                    continue;
                }
                if (!settings.RandomizeRocks && newItem.pool == "Rock") 
                {
                    continue;
                }
                if (!settings.RandomizeSoulTotems && newItem.pool == "Soul") 
                {
                    continue;
                }
                if (!settings.RandomizePalaceTotems && newItem.pool == "PalaceSoul") 
                {
                    continue;
                }
                if (!settings.RandomizeLoreTablets && newItem.pool == "Lore") 
                {
                    continue;
                }

                if (oldItem.replace)
                {
                    string replaceShinyName = "Randomizer Shiny " + newShinies++;
                    if (location == "Dream_Nail" || location == "Mask_Shard-Brooding_Mawlek" || location == "Nailmaster's_Glory" || location == "Godtuner")
                    {
                        replaceShinyName = "Randomizer Shiny"; // legacy name for scene edits
                    }
                    oldItem.fsmName = "Shiny Control";
                    oldItem.type = ItemType.Charm;
                    Actions.Add(new ReplaceObjectWithShiny(oldItem.sceneName, oldItem.objectName, replaceShinyName));
                    oldItem.objectName = replaceShinyName;
                }

                else if (oldItem.newShiny)
                {
                    string newShinyName = "New Shiny " + newShinies++;
                    if (location == "Simple_Key-Lurker")
                    {
                        newShinyName = "New Shiny"; // legacy name for scene edits
                    }
                    Actions.Add(new CreateNewShiny(oldItem.sceneName, oldItem.x, oldItem.y, newShinyName));
                    oldItem.objectName = newShinyName;
                    oldItem.fsmName = "Shiny Control";
                    oldItem.type = ItemType.Charm;
                }

                else if (oldItem.type == ItemType.Geo && newItem.type != ItemType.Geo)
                {
                    Actions.Add(new AddShinyToChest(oldItem.sceneName, oldItem.objectName, oldItem.fsmName,
                        "Randomizer Chest Shiny"));
                    oldItem.objectName = "Randomizer Chest Shiny";
                    oldItem.fsmName = "Shiny Control";
                    oldItem.type = ItemType.Charm;
                } else if (oldItem.type == ItemType.Flame)
                {
                    // Even if the new item is also a flame, this action should still run in order to
                    // guarantee that the player can't be locked out of getting it by upgrading their
                    // Grimmchild.
                    Actions.Add(new ChangeGrimmkinReward(oldItem.sceneName, oldItem.objectName, oldItem.fsmName, newItem.nameKey, newItem.shopSpriteKey, newItem.action, newItemName, location));
                    continue;
                }
                else if (oldItem.pool == "Essence_Boss")
                {
                    Actions.Add(new ChangeBossEssenceReward(oldItem.sceneName, oldItem.objectName, oldItem.fsmName, newItem.nameKey, newItem.shopSpriteKey, newItem.action, newItemName, location));
                    continue;
                }

                // Dream nail needs a special case
                if (location == "Dream_Nail")
                {
                    Actions.Add(new ChangeBoolTest("RestingGrounds_04", "Binding Shield Activate", "FSM", "Check",
                        newItemName, playerdata: false, 
                        altTest:() => RandomizerMod.Instance.Settings.CheckLocationFound(location)));
                    Actions.Add(new ChangeBoolTest("RestingGrounds_04", "Dreamer Plaque Inspect",
                        "Conversation Control", "End", newItemName, playerdata: false,
                        altTest:() => RandomizerMod.Instance.Settings.CheckLocationFound(location)));
                    Actions.Add(new ChangeBoolTest("RestingGrounds_04", "Dreamer Scene 2", "Control", "Init",
                        newItemName, playerdata: false,
                        altTest: () => RandomizerMod.Instance.Settings.CheckLocationFound(location)));
                    Actions.Add(new ChangeBoolTest("RestingGrounds_04", "PreDreamnail", "FSM", "Check",
                        newItemName, playerdata: false,
                        altTest: () => RandomizerMod.Instance.Settings.CheckLocationFound(location)));
                    Actions.Add(new ChangeBoolTest("RestingGrounds_04", "PostDreamnail", "FSM", "Check",
                        newItemName, playerdata: false,
                        altTest: () => RandomizerMod.Instance.Settings.CheckLocationFound(location)));
                }

                switch (newItem.type)
                {
                    default:
                        Actions.Add(new ChangeShinyIntoItem(oldItem.sceneName, oldItem.objectName, oldItem.fsmName,
                            newItem.action, newItemName, location, newItem.nameKey, newItem.shopSpriteKey));
                        if (!string.IsNullOrEmpty(oldItem.altObjectName))
                        {
                            Actions.Add(new ChangeShinyIntoItem(oldItem.sceneName, oldItem.altObjectName, oldItem.fsmName,
                                newItem.action, newItemName, location, newItem.nameKey, newItem.shopSpriteKey));
                        }
                        break;

                    case ItemType.Big:
                    case ItemType.Spell:
                        BigItemDef[] newItemsArray = GetBigItemDefArray(newItemName);

                        Actions.Add(new ChangeShinyIntoBigItem(oldItem.sceneName, oldItem.objectName,
                            oldItem.fsmName, newItemsArray, newItem.action, newItemName, location));
                        if (!string.IsNullOrEmpty(oldItem.altObjectName))
                        {
                            Actions.Add(new ChangeShinyIntoBigItem(oldItem.sceneName, oldItem.altObjectName,
                                oldItem.fsmName, newItemsArray, newItem.action, newItemName, location));
                        }

                        break;

                    case ItemType.Geo:
                        if (oldItem.inChest)
                        {
                            Actions.Add(new ChangeChestGeo(oldItem.sceneName, oldItem.chestName,
                                oldItem.chestFsmName, newItem.geo, newItemName, location));
                        }
                        else if (oldItem.type == ItemType.Geo)
                        {
                            Actions.Add(new ChangeChestGeo(oldItem.sceneName, oldItem.objectName, oldItem.fsmName,
                                    newItem.geo, newItemName, location));
                        }
                        else
                        {
                            Actions.Add(new ChangeShinyIntoGeo(oldItem.sceneName, oldItem.objectName,
                                oldItem.fsmName, newItem.geo, newItemName, location));

                            if (!string.IsNullOrEmpty(oldItem.altObjectName))
                            {
                                Actions.Add(new ChangeShinyIntoGeo(oldItem.sceneName, oldItem.altObjectName,
                                    oldItem.fsmName, newItem.geo, newItemName, location));
                            }
                        }
                        break;
                    case ItemType.Lifeblood:
                        Actions.Add(new ChangeShinyIntoLifeblood(oldItem.sceneName, oldItem.objectName, oldItem.fsmName, newItem.lifeblood, newItemName, location));
                        break;

                    case ItemType.Soul:
                        Actions.Add(new ChangeShinyIntoSoul(oldItem.sceneName, oldItem.objectName,
                            oldItem.fsmName, newItemName, location));

                        if (!string.IsNullOrEmpty(oldItem.altObjectName))
                        {
                            Actions.Add(new ChangeShinyIntoSoul(oldItem.sceneName, oldItem.altObjectName,
                                oldItem.fsmName, newItemName, location));
                        }
                        break;

                }

                if (oldItem.cost != 0 || oldItem.costType != AddYNDialogueToShiny.CostType.Geo)
                {
                    int cost = oldItem.cost;
                    if (oldItem.costType == AddYNDialogueToShiny.CostType.Essence || oldItem.costType == AddYNDialogueToShiny.CostType.Grub)
                    {
                        cost = settings.VariableCosts.First(pair => pair.Item1 == location).Item2;
                    }

                    Actions.Add(new AddYNDialogueToShiny(
                        oldItem.sceneName,
                        oldItem.objectName,
                        oldItem.fsmName,
                        newItem.nameKey,
                        cost,
                        oldItem.costType));
                }
            }

            List<ChangeShopContents> shopActions = new List<ChangeShopContents>();

            // TODO: Change to use additiveItems rather than hard coded
            // No point rewriting this before making the shop component
            foreach ((string shopItem, string shopName) in items.Where(item => shopNames.Contains(item.Item2)))
            {
                ReqDef newItem = LogicManager.GetItemDef(shopItem);

                GiveAction giveAction = newItem.action;
                if (giveAction == GiveAction.SpawnGeo)
                {
                    giveAction = GiveAction.AddGeo;
                }

                string boolName = "RandomizerMod." + giveAction.ToString() + "." + shopItem + "." + shopName;

                ShopItemBoolNames[(shopItem, shopName)] = boolName;
                
                ShopItemDef newItemDef = new ShopItemDef
                {
                    PlayerDataBoolName = boolName,
                    NameConvo = newItem.nameKey,
                    DescConvo = newItem.shopDescKey,
                    RequiredPlayerDataBool = LogicManager.GetShopDef(shopName).requiredPlayerDataBool,
                    RemovalPlayerDataBool = string.Empty,
                    DungDiscount = LogicManager.GetShopDef(shopName).dungDiscount,
                    NotchCostBool = newItem.notchCost,
                    Cost = settings.ShopCosts.First(pair => pair.Item1 == shopItem).Item2,
                    SpriteName = newItem.shopSpriteKey
                };

                if (newItemDef.Cost == 0)
                {
                    newItemDef.Cost = 1;
                    LogWarn($"Found item {shopItem} in {shopName} with no saved cost.");
                }

                if (newItemDef.Cost < 5)
                {
                    newItemDef.DungDiscount = false;
                }

                ChangeShopContents existingShopAction = shopActions.FirstOrDefault(action =>
                    action.SceneName == LogicManager.GetShopDef(shopName).sceneName &&
                    action.ObjectName == LogicManager.GetShopDef(shopName).objectName);

                if (existingShopAction == null)
                {
                    shopActions.Add(new ChangeShopContents(LogicManager.GetShopDef(shopName).sceneName,
                        LogicManager.GetShopDef(shopName).objectName, new[] { newItemDef }));
                }
                else
                {
                    existingShopAction.AddItemDefs(new[] { newItemDef });
                }
            }

            shopActions.ForEach(action => Actions.Add(action));
        }

        public static string GetAdditivePrefix(string itemName)
        {
            return LogicManager.AdditiveItemNames.FirstOrDefault(itemSet =>
                LogicManager.GetAdditiveItems(itemSet).Contains(itemName));
        }

        private static BigItemDef[] GetBigItemDefArray(string itemName)
        {
            itemName = LogicManager.RemoveDuplicateSuffix(itemName);
            string prefix = GetAdditivePrefix(itemName);
            if (prefix != null)
            {
                return LogicManager.GetAdditiveItems(prefix)
                    .Select(LogicManager.GetItemDef)
                    .Select(item => new BigItemDef
                    {
                        Name = itemName,
                        BoolName = item.boolName,
                        SpriteKey = item.bigSpriteKey,
                        TakeKey = item.takeKey,
                        NameKey = item.nameKey,
                        ButtonKey = item.buttonKey,
                        DescOneKey = item.descOneKey,
                        DescTwoKey = item.descTwoKey
                    }).ToArray();
            }

            ReqDef item2 = LogicManager.GetItemDef(itemName);
            return new[]
            {
                new BigItemDef
                {
                    Name = itemName,
                    BoolName = item2.boolName,
                    SpriteKey = item2.bigSpriteKey,
                    TakeKey = item2.takeKey,
                    NameKey = item2.nameKey,
                    ButtonKey = item2.buttonKey,
                    DescOneKey = item2.descOneKey,
                    DescTwoKey = item2.descTwoKey
                }
            };
        }

        private static string GetAdditiveBoolName(string boolName, ref Dictionary<string, int> additiveCounts)
        {
            if (additiveCounts == null)
            {
                additiveCounts = LogicManager.AdditiveItemNames.ToDictionary(str => str, str => 0);
            }

            string prefix = GetAdditivePrefix(boolName);
            if (string.IsNullOrEmpty(prefix))
            {
                return null;
            }

            additiveCounts[prefix] = additiveCounts[prefix] + 1;
            AdditiveBoolNames[boolName] = prefix + additiveCounts[prefix];
            return prefix + additiveCounts[prefix];
        }

        public static void Hook()
        {
            UnHook();

            On.PlayMakerFSM.OnEnable += ProcessFSM;
        }

        public static void UnHook()
        {
            On.PlayMakerFSM.OnEnable -= ProcessFSM;
        }

        public static void ProcessFSM(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM fsm)
        {
            orig(fsm);

            string scene = fsm.gameObject.scene.name;

            foreach (RandomizerAction action in Actions)
            {
                if (action.Type != ActionType.PlayMakerFSM)
                {
                    continue;
                }

                try
                {
                    action.Process(scene, fsm);
                }
                catch (Exception e)
                {
                    LogError(
                        $"Error processing action of type {action.GetType()}:\n{JsonUtility.ToJson(action)}\n{e}");
                }
            }
        }

        public static void EditShinies()
        {
            string scene = Ref.GM.GetSceneNameString();

            foreach (RandomizerAction action in Actions)
            {
                if (action.Type != ActionType.GameObject)
                {
                    continue;
                }

                try
                {
                    action.Process(scene, null);
                }
                catch (Exception e)
                {
                    LogError(
                        $"Error processing action of type {action.GetType()}:\n{JsonUtility.ToJson(action)}\n{e}");
                }
            }
        }

        public abstract void Process(string scene, Object changeObj);
    }
}
