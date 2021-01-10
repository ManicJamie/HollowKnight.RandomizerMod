# Randomizer 3

Randomizer 3 expands on previous versions of the Hollow Knight randomizer by allowing you to randomize more items than ever before and, for the first time, to randomize area or room transitions.
- Randomizer 3 requires SeanprCore.dll and Modding Api version 53 or greater to run. These are both automatically downloaded by the ModInstaller.
- There is a new map tracker for area and room randomizer available at https://github.com/homothetyhk/RandomizerTracker/releases
- There is a community randomizer guide published here: https://tinyurl.com/HollowKnightRandomizerGuide

Details on all of the various settings follow:

## Restrictions

The skip settings control which difficult skips the randomizer may require. If you are not familiar with these skips, especially as they are used in speedrunning, you are advised to turn them off.

With skips allowed, the player is advised to take care to not get locked out of certain required pogos. Obtain:
- No more than 1 nail upgrade before claw or wings
- No more than 3 nail upgrades before claw

## Quality of Life

- Salubra Notches: automatically gives you each Salubra charm notch upon acquiring the required number of charms
- Fast Grubfather: all unlocked grub geo rewards are given at once by Grubfather
- Early Geo: start the game with between 300 and 600 geo.
- Extra Platforms: platforms in various places that prevent softlocks. For example, there are several platforms added to Ancient Basin to prevent having to quit-out after checking certain locations without vertical movement.
- 1.2.2.1 Levers: Restores the larger hitboxes of levers from past patches, allowing them to sometimes be hit from the other side. Note that not all levers were fixable. Lever skips are never required in logic.
- Jiji Hints: trade a rancid egg for information on which areas contain which items. Hints are given for progression items, in the order that they were intended to be collected. Already obtained items are skipped.

## Randomization

These settings control which items are randomized.
- Dreamers: Lurien, Monomon, Herrah, and World Sense. World Sense is the Black Egg Temple pickup to view your completion percentage
- Skills: all spells, nail arts, and movement abilities
- Charms
- Keys: all key objects, as well as King's Brand, Godtuner, and Collector's Map
- Mask Shards
- Vessel Fragments
- Pale Ore
- Charm Notches: all charm notches, except those sold by Salubra
- Geo Chests: all geo chests, except the one above Baldur Shell and those in the Junk Pit
- Relics: all wanderers journals, hallownest seals, king's idols, and arcane eggs.
- Rancid Eggs
- Stags
- Maps
- Whispering Roots
- Grubs
- Lifeblood Cocoons
- Soul Totems: all soul totems except those found in White Palace
- Palace Totems: soul totems found in White Palace
- Grimmkin Flames
- Geo Rocks
- Boss Essence: essence drops from Dream Warriors and Dream Bosses

Note: several items are randomized progressively, meaning that collecting any item in a given family always gives the first upgrade, collecting another gives the second upgrade, etc. The families this pertains to are:
	- Dream Nail, Dream Gate, Awoken Dream Nail
	- Mothwing Cloak, Shade Cloak
	- Vengeful Spirit, Shade Soul
	- Desolate Dive, Descending Dark
	- Howling Wraiths, Abyss Shriek
Any of the above pickups may be forced by randomizer logic.

Note: the following items can be used to kill baldurs:
	- All difficulties: Vengeful Spirit (or upgrades), Desolate Dive (or upgrades), Grubberfly's Elegy, Glowing Womb, Dash Slash with Dash
	- With Mild skips: Weaversong, Spore Shroom
	- With Spicy skips: Cyclone Slash, Mark of Pride
	- Not in logic, but feel free to try if you have time on your hands: Longnail, Mothwing Cloak, Sprintmaster+Dashmaster
Baldur hp is reduced to 5 to make slower baldur kills less tedious, and to reduce rng.

Note: the lifeblood door in Abyss opens if you enter the room with a single lifeblood mask. In logic, it requires a lifeblood charm.

Note: while lifeblood cocoons and soul totems normally reset after sitting on a bench, this is no longer true if you randomize them. Instead, picking up a randomized lifeblood cocoon will give a large number of lifeblood masks immediately, and picking up a randomized soul totem will completely fill up all soul.

- Duplicate Major Items: adds second copies of the following important items:
		Dreamer (interchangeable for any of the three dreamers in opening black egg temple), Void Heart,
		Mothwing Cloak, Shade Cloak, Mantis Claw, Monarch Wings, 
		Crystal Heart, Isma's Tear, Dream Nail, 
		Vengeful Spirit, Desolate Dive, Howling Wraiths
	Picking up an excess copy after collecting all other copies of an item gives 100 geo.
	Duplicate items are *not* placed using randomizer logic.
- Randomize Starting Items: begin the game with several items already in the inventory, including at least one vertical movement upgrade.
- Starting Location: you may select a starting location from the menu, or set it to be random. Some locations may be unavailable depending on your settings.
- Create spoiler log: creates a file in the save directory with all item/transition placements

## Additional features

There are four logs created in the save directory to help you with your playthrough.
- Tracker Log: this log continuously records item locations, transition connections, and hints as you discover them.
- Helper Log: this log computes which locations/transitions are accessible with your current equipment.
- Spoiler Log: this log lists the exact locations of every randomized item and/or transition.
- Condensed Spoiler Log: this log condenses the spoiler log to only list the locations of major progression items.

The "cursed" option is a special hard mode for randomizer veterans. Features include:
- Masks, Vessels, Ore, Notches, Geo Chests, Eggs, and Relics are replaced by 1 geo pickups, if randomized.
- Shade Soul, Descending Dark, and Abyss Shriek are removed.
- Major items are less likely to be placed as early progression items.
- Focus (the ability to heal) is randomized, and is no longer available from the start of the game.

## Area/Room randomizer

- Area randomizer randomizes items and connections between areas, which are understood to be any region of the game with a name which appears as onscreen text, excluding dream areas, trams, and elevators.
- Room randomizer randomzies items and nearly every transition between different rooms. Not included are:
    - Warps of any kind, including those entering dream areas
	- Trams and elevators
	- Transitions within Godhome and the Shrine of Believers
	- The transitions leading to Sly's storeroom, Bretta's basement, or to any trial of the colosseum
- The Connected-Area Room randomizer works similarly to Room randomizer, with the additional constraint that it attempts to keep rooms from the same area connected, up to a certain extent, and not affecting single entrance rooms.
Also, note the following:
- Due to an imbalance in the number of left and right transitions, the Divine and Grimm tents are included in the randomization, but their vanilla entrances have been removed, and will not spawn in Dirtmouth
- The nightmare lantern must be lit to obtain Grimmchild
- Sly must be rescued to use his shop

## New game mechanics
- Collecting Grimmchild activates the Nightmare Lantern. If Grimmkin Flames are not randomized, Grimmchild is also given with the first 6 flames already collected.
- You can preview the items at Colosseum, Grey Mourner, and King Fragment by interaction.
- You can preview the item at Grey Prince Zote by reading Bretta's diary.

## Known issues
- Using Benchwarp may cause some room changes to fail to occur. This can be fixed by exiting and reentering the room.
- Minion charms may fail to spawn. This can cause a soft-lock if you attempt to start the Grimm fight without Grimmchild spawned. This can be fixed by reequipping at a bench.