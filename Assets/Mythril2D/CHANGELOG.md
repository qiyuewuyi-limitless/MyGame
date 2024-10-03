# Mythril2D: Changelog
## Version 2.0
### Features
- Added a new monster: water slime (fast, but squishy).
- Added a crafting system with new items:
  - Slime Ball (Dropped by slimes).
  - Water Slime Ball (Dropped by the newly added water slime).
  - Water Bucket (Dropped by water slime or purchased at the store).
  - Bucket (Obtained after using a Water Bucket in a craft).
- Replaced the `ScriptableAction` system with a more versatile command-based system (`ICommand`):
  - Uses a subclass selector to select the command to execute.
  - Allows for easier authoring of new commands.
  - Provides better inspector visuals.
  - Doesn't require a `ScriptableObject` instance (like `ScriptableAction` did).
  - Integrates with Unity's Event System using a `CommandHandler`.
- Added a "GameObjects" palette to quickly populate your maps with prefabs.
- Added a scene overlay to navigate through the available scenes.
- Added new commands to open a shop or craft menu.
- Added item categories and inventory tabs.
- Revamped the monster spawner system:
  - Added 2 separate implementations:
    - `MonsterAreaSpawner`: spawns a monster anywhere within a collider's area.
    - `MonsterSpawner`: spawns a monster at one specific location.
  - Added a max monster count setting to the monster spawner.
- Added `ExecuteCommandIf` to create a branch in a command execution flow.
- Added `PlayDialogueLine` to execute a single line of dialogue without a proper `DialogueSequence`.
- Added a level requirement to quests, making unlocked quests not available until a certain level is reached.
- Added events for when an item is equipped or unequipped.
- Reworked the condition system:
  - Uses a subclass selector to select the condition to evaluate.
  - Supports nesting of conditions, condition lists with operators, and negation.
  - Easily extendable to add new conditions.
  - Improved memory efficiency compared to the previous system in place.
  - Integrates with the command system to check for conditions in any execution flow.
- Reworked the interaction system:
  - Introduced the `Entity` class, serving as the base class for all entities in the game.
  - Extended the interaction system to work with non-NPC entities.
  - Updated all NPCs and interactions to use the new system.
- Reworked the save system to introduce a database to manage references to `ScriptableObject`:
  - Added `DatabaseEntry`: a base class for a `ScriptableObject` that can be referenced by a GUID.
  - Added `DatabaseEntryReference`: a serializable class used to reference a `ScriptableObject` using its GUID (in a save file).

### Fixes
- Fixed a `MissingReferenceException` in some specific cases when using `UINavigationCursor`.
- Fixed an incorrect character reference being sent to `ANPCInteraction.Interact(sender)`.
- Fixed the "Stolen Heirloom" quest by re-adding the missing item in its chest.
- Fixed the Necromancer death animation when finishing the quest "Necromancy In The Vicinity".
- Fixed the `MonsterSpawner` prespawn not properly working with conditional activators.
- Fixed a rare `MissingReferenceException` occurrence with the `UINavigationCursor`.
- Fixed corrupted saved files after restarting the game.

## Version 1.4.1
### Fixes
- Corrected the position of the health bar when loading a save where the player's health is not full.
  
## Version 1.4
### Features
- Added an option for the camera to shake when the player takes a hit and/or when the player hits an enemy:
  - This option is enabled by default only when the player takes a hit.
- Added an option for stat bars (e.g., health bar, mana bar) to shake when their values decrease:
  - This option is enabled by default only for the health bar.
- Added game condition to check if an ability is unlocked.
- Added a line to the event log when an ability is added or removed.
- Added a `ScriptableAction` to add or remove an ability.
- Newly added abilities will automatically be equipped if a slot is available.
- Chests can now contain multiple items and currency.
- Added a "Settings" menu with volume settings for each audio channel.
- Added `playerSpawned` event to the `NotificationSystem`.
- Added "Select" and "Back" input indicators to game menus.
- Added an `EquipmentSpriteLibraryUpdater` component:
  - Different swords will now have different visuals when using sword abilities.
- Added a provocation system for enemies:
  - Hitting an enemy from afar now provokes the enemy even if the player is outside of the enemy detection radius.

### Fixes
- Fixed conditional state machines not being properly updated when some condition where met.
- Fixed enemies sometimes not facing the correct direction in combat.

## Version 1.3
### Features
- Added quest starter items.
- Added conditional loot on monsters.
- Added a death screen.
- Updated the demo game with a new quest that can be initiated by a special item dropped by skeletons (with a 10% drop rate).
- Updated documentation to include a section on save files.

### Fixes
- Minor fixes

## Version 1.2
### Features
- Added dash ability, unlocked by the archer upon reaching level 3.
- Implemented a new AI navigation system using context steering:
  - AI can now avoid simple obstacles (complex shapes, such as mazes, are not supported).
  - AI can no longer see behind objects, allowing the player to hide effectively.
  - When the AI loses sight of the player, it will move to the last known position and reset after a while if it cannot reestablish visual contact.

### Fixes
- Rectified projectile hitbox (preventing instances where they would hit a target behind the caster).
- Resolved a potential path normalization issue on UNIX platforms.
- Fixed player stats updates:
  - Increasing max stats (equip item, apply points...) will now properly adjust current stats by the same amount instead of resetting them to their maximum value.
  - Leveling up now restores the player's stats.
- Rectified typos in the triple shot and corrosion ability sheets.
- Removed unused code.

## Version 1.1
### Features
- Added the ability to execute scriptable actions when a dialogue starts.
- Added a permanent death setting for monsters.
- Added a new scriptable action to either heal or damage the player.

### Fixes
- Fixed a bug where the `booleanChanged` event (renamed to `gameFlagChanged`) wasn't triggering properly, resulting in some broken interactions with conditional activators.
- Fixed a bug where monster projectiles would sometimes remain attached to their caster.
- Fixed a bug causing teleporters without activation settings to teleport the player multiple times.

## Version 1.0
This is the first version of Mythril2D!
