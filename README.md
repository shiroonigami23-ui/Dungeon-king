# 👑 Dungeon King: Resurrection

A fast-paced, top-down Action RPG built in Unity. 

## ⚔️ Key Features
* **6 Playable Classes:** Choose between Warrior, Knight, or Mage (Male & Female variants) with unique passives like *Battle Trance* and *Bastion*.
* **Divine Seed System:** Every player gets a unique "Soul Skill" (OP multiplier) generated from their specific hardware ID. No two players have the same "God-Slayer" potential.
* **2-Phase Boss Fight:** Face the Slime King. At 50% HP, he enters Phase 2—growing 2x in size and unleashing Fireball Novas.
* **Procedural Sounds:** 8-bit retro sound system that scales with the action.
* **Save/Load System:** Persistent progress for Gold, Armor, and Leveling via JSON.

## 🕹️ Controls
* **WASD / Arrows:** Movement
* **Mouse:** Aiming & Rotation
* **Left Click:** Attack
* **Space:** Dash (with Invincibility Frames)
* **E:** Interact with Shop / NPCs
* **ESC:** Pause / Settings

## 🛠️ Installation & Setup
1. Clone this repository.
2. Open the project in **Unity 2022.3 LTS** (or newer).
3. Ensure the Scene order in **Build Settings** is:
   1. `MainMenu`
   2. `DungeonLevel1`
4. Assign the 6 `CharacterData` ScriptableObjects to the `Player` prefab in the Inspector.