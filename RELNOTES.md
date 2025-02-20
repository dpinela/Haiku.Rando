# 2.1 (23 July 2023)

New features:

- A new Double Jump Chain skip category, with logic for movement
  that uses Blink or Grapple to refresh the double jump in mid-air.

Bug fixes:

- Assorted base logic fixes.
- The Newtonsoft.JSON DLL is now included in the release zip file;
  some previous versions had not, and did not work correctly when
  installed from Scarab as a result.

# 2.0 (9 June 2023)

New features:

- Works with the new Obscure Information DLC (patch 1.5.1.2). Older versions of the game are no longer supported.
  Rooms and items in Old Arcadia are not randomized.
- Scrap nodes have been added as a new randomizable pool.
  Adjacent nodes are consolidated into a single check with the combined value of all of the nodes.
- The Clock repair is randomizable.
- The new Train Lover Mode starts you in the train with the clock already fixed and one random station unlocked.
- With Skill Chips on, Bulb Relation will serve as a replacement for Bulblet, with the exception of levers that
  specifically require Bulblet.

Bug fixes:

- Going through the Magnet boss room left to right is no longer in logic.
- Fixed false positives in the Sunken Wastes elevator room.
- "Nothing" items are much less likely to be concentrated near the starting area.

# 1.1.2 (13 May 2023)

Bug fixes:
- Lore items now appear in the recent items list like everything else
- Picking up lore, map marker, or Nothing checks will no longer make a certain Rusted Key item disappear

# 1.1.1 (4 May 2023)

Bug fixes:
- The limited subset of shop item in Abandoned Wastes (which in vanilla are duplicates of four train shop items)
  is no longer treated as independent locations; instead these locations always offer the same items as their counterparts in the train.
  This prevents items from being irretrieavably lost once the Clock is repaired.
- Logic no longer allows going up from Sunken Wastes through the elevator without Light under any circumstances

# 1.1 (29 April 2023)

New features:

- A togglable list of the 5 most recently-obtained items (excluding lore tablets).
- Logic for several skip categories, each of which can be enabled individually:
    - BLJs
    - Enemy Pogos
    - Bomb Jumps
    - Dark Rooms (considers all dark rooms traversable without Bulblet, except for the elevator to Sunken Wastes)
    - Hazard Rooms (considers Surface and some parts of Incinerator traversable without their respective sealants)
    - Skill Chips (allows Auto Modifier and Self-Detonation to serve as replacements for Ball and Bomb respectively)

Bug fixes:
- A large number of false negatives and false positives in logic across all areas have been corrected