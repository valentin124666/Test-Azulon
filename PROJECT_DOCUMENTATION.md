# Test-Azulon Project Documentation | Items System Architecture

## 📋 Project Overview

**Test-Azulon** is a Unity project written in C# (97.4%) with ShaderLab and HLSL elements. The project is built using Zenject-based dependency injection architecture and modular structure. The main focus is implementing a comprehensive item system with support for consumables, equipment, and dynamic effects.

---

## 🏗️ General Project Architecture

```
Assets/Scripts/
├── Core/              # Core kernel components
├── Data/              # Data systems and configurations
├── Items/             # ⭐ ITEM SYSTEM (main focus)
├── Managers/          # Managing modules (GameData, Gameplay, UI, SceneLoader)
├── Services/          # Service layers
├── Modules/           # Modular components
├── Level/             # Level logic
├── Installers/        # Zenject installers for DI
├── General/           # Utilities and helpers
├── UIElements/        # UI components
├── Tools/             # Developer tools
├── FBXPool/           # Object pooling
└── Plugins/           # External plugins
```

---

## ⭐ ITEMS SYSTEM (Items)

### Items System Structure

```
Assets/Scripts/Items/
├── ItemData.cs              # ScriptableObject item definition
├── InventoryItem.cs         # Item instance in inventory
├── ItemSlot.cs              # Equipment slot
├── WorldItem.cs             # Item in game world
├── Effects/                 # Item effects (bonuses, status effects)
│   ├── ItemEffectBase.cs    # Base class for all effects
│   ├── AddArmorEffect.cs    # Armor bonus
│   ├── AddAttackEffect.cs   # Attack bonus
│   ├── AddHealHealthEffect.cs # Healing
│   ├── AddMaxHealthEffect.cs  # Max health increase
│   ├── AddSpeedEffect.cs    # Speed bonus
│   └── AddXpEffect.cs       # Experience bonus
└── Property/                # Item properties
    ├── ItemPropertyBase.cs  # Base class
    ├── ConsumableProperty[] # Consumable properties
    ├── EquippableProperty[] # Equipment properties
    ├── EquipSlotProperty.cs # Equipment slot description
    ├── StackableProperty.cs # Stacking capability
    ├── DurabilityProperty.cs # Durability
    ├── ChargeProperty.cs    # Charges/uses
    ├── UsageCountProperty.cs # Usage counter
    ├── ConsumableItem.cs    # Consumable
    └── EquippableItem.cs    # Equipment item
```

---

## 📊 Detailed Items System Architecture

### 1️⃣ **ItemData.cs** — Main Item Definition

```csharp
/// <summary>
/// Represents the base data for any item in the game.
/// Stores immutable item information and properties.
/// </summary>
[CreateAssetMenu(menuName = "Custom menu/Item/ItemData")]
public class ItemData : ScriptableObject, IItemDataInfo
{
    /// <summary>Unique identifier for this item (GUID)</summary>
    [SerializeField] private string _id;
    
    /// <summary>Display name of the item</summary>
    [SerializeField] private string _name;
    
    /// <summary>Description text for UI</summary>
    [SerializeField] private string _description;
    
    /// <summary>Item icon for inventory UI</summary>
    [SerializeField] private Sprite _icon;
    
    /// <summary>Type of item interaction (Consume or Equip)</summary>
    [SerializeField] private ItemInteractionType _interactionType;

    /// <summary>Properties for consumable items</summary>
    [SerializeField] private ConsumableProperty[] _consumableProperties;
    
    /// <summary>Properties for equippable items</summary>
    [SerializeField] private EquippableProperty[] _equippableProperty;
    
    /// <summary>Effects that trigger when using this item</summary>
    [SerializeField] private ItemEffectBase[] _itemEffects;
    
    /// <summary>Equipment slot restrictions</summary>
    [SerializeField] private EquipSlotProperty _equipSlotProperty;
}
```

**Role:** ScriptableObject that stores all information about an item.

**Key Properties:**
- `Id` — unique GUID for the item
- `Name`, `Description` — textual descriptions
- `Icon` — sprite for UI
- `InteractionType` — item type (Consume or Equip)

**Three types of data based on interaction type:**

| Type | Properties | Example |
|------|-----------|---------|
| **Consume** | `ConsumableProperty[]` | Potions, food, bonus artifacts |
| **Equip** | `EquippableProperty[]` | Weapons, armor, accessories |
| Both | `ItemEffectBase[]` | Effects that activate |

**How it works:**
```csharp
/// <summary>
/// Returns the appropriate properties array based on item type.
/// </summary>
/// <returns>ConsumableProperty[] for consume items, EquippableProperty[] for equipment</returns>
public ItemPropertyBase[] GetProperty()
{
    if (_interactionType == ItemInteractionType.Consume)
        return _consumableProperties;  // Return consumable properties
    else
        return _equippableProperty;    // Return equipment properties
}

/// <summary>
/// Generates a unique GUID for this item if it doesn't have one.
/// Called from context menu in editor.
/// </summary>
[ContextMenu("GenerateId")]
public void GenerateId()
{
    if (!string.IsNullOrEmpty(_id))
    {
        Debug.LogWarning($"ItemData already has ID: {_id}");
        return;
    }
    _id = Guid.NewGuid().ToString("N");
}
```

**Item Interaction Types:**
```csharp
/// <summary>Determines how the player interacts with an item</summary>
public enum ItemInteractionType
{
    /// <summary>Item can be consumed/used once and removed from inventory</summary>
    Consume,
    
    /// <summary>Item can be equipped and provides passive bonuses</summary>
    Equip
}

/// <summary>Possible equipment slots on the character</summary>
public enum EquipmentSlot
{
    /// <summary>Unknown or no slot</summary>
    Unknown,

    /// <summary>Left hand (shield, torch, book)</summary>
    LeftHand,
    
    /// <summary>Right hand (sword, staff, axe)</summary>
    RightHand,
    
    /// <summary>Neck (amulets, chains)</summary>
    Neck,
}
```

---

### 2️⃣ **InventoryItem.cs** — Item Instance in Inventory

```csharp
/// <summary>
/// Represents a concrete instance of an item in the player's inventory.
/// Each inventory item references ItemData but has its own unique ID
/// and instance-specific state.
/// </summary>
public class InventoryItem
{
    /// <summary>Reference to the item's base data template</summary>
    public ItemData Data;
    
    /// <summary>
    /// Unique identifier for this specific item instance.
    /// Allows tracking durability, charges, and other instance-specific properties.
    /// </summary>
    public string InstanceId;

    /// <summary>
    /// Creates a new inventory item instance.
    /// </summary>
    /// <param name="data">The base ItemData template</param>
    /// <param name="instanceId">Unique identifier for this instance</param>
    public InventoryItem(ItemData data, string instanceId)
    {
        Data = data;
        InstanceId = instanceId;
    }
}
```

**Role:** Represents a specific instance of an item in the inventory.

**Key Difference from ItemData:**
- `ItemData` is the **template** (one for all Iron Swords)
- `InventoryItem` is the **concrete instance** (my personal sword with its current state)

**Example:**
```
ItemData "Iron Sword"
├── InventoryItem #1 (InstanceId: "abc123") → with 100/100 durability
├── InventoryItem #2 (InstanceId: "def456") → with 45/100 durability
└── InventoryItem #3 (InstanceId: "ghi789") → with 0/100 durability (broken)
```

---

### 3️⃣ **ItemSlot.cs** — Equipment Slot

```csharp
/// <summary>
/// Represents an equipment slot where an item can be equipped.
/// Stores visual representation and slot-specific properties.
/// </summary>
public class ItemSlot
{
    /// <summary>3D model or visual representation of equipped item</summary>
    public GameObject Equipment;
    
    /// <summary>
    /// Which slot this equipment belongs to.
    /// Determines where the item is equipped on the character.
    /// </summary>
    public EquipmentSlot Slot = 0;

    /// <summary>
    /// Maximum durability value for items in this slot.
    /// -1 means infinite durability.
    /// </summary>
    public int MaxDurability = -1;
    
    /// <summary>
    /// Maximum charges for items in this slot.
    /// -1 means infinite charges.
    /// </summary>
    public int MaxCharges = -1;
}
```

**EquipmentSlot Types:**
```csharp
public enum EquipmentSlot
{
    Unknown,           // Default value
    LeftHand,          // Left hand (shield, torch)
    RightHand,         // Right hand (sword, staff)
    Neck,              // Neck (amulets, chains)
}
```

**Role:** Manages equipment placement and their characteristics.

---

### 4️⃣ **WorldItem.cs** — Item in Game World

```csharp
/// <summary>
/// Represents an item placed in the game world that can be picked up.
/// When player interacts with this, the item is removed from world
/// and added to inventory.
/// </summary>
public class WorldItem : MonoBehaviour
{
    /// <summary>The item data that will be added to inventory when picked up</summary>
    [SerializeField] private ItemData _itemData;

    /// <summary>
    /// Called when player picks up this item.
    /// Removes the world object and returns the item data.
    /// </summary>
    /// <returns>The ItemData to be added to inventory</returns>
    public ItemData GetItemData()
    {
        Destroy(gameObject);  // Remove from world
        return _itemData;     // Return data for inventory
    }
}
```

**Role:** Component attached to GameObjects in the world. When a player picks up an item:
1. `WorldItem.GetItemData()` returns the data
2. The object is destroyed from the scene
3. The item is added to inventory as an `InventoryItem`

---

## 🔧 Property System

### Base Class: **ItemPropertyBase.cs**

```csharp
/// <summary>
/// Base class for all item properties.
/// Properties represent quantifiable characteristics of items
/// such as durability, charges, stack size, etc.
/// </summary>
public abstract class ItemPropertyBase { }
```

Base class for all item properties.

---

### Consumable Properties

#### **ConsumableProperty** — Base Consumable Property
Oriented towards single-use items.

#### **ConsumableItem.cs** — Concrete Implementation
```csharp
/// <summary>
/// Base implementation for consumable items.
/// Represents items that can be used and removed from inventory.
/// </summary>
public class ConsumableItem : ItemPropertyBase { }
```

#### **StackableProperty.cs** — Stacking Capability
```csharp
/// <summary>
/// Allows items to be stacked in a single inventory slot.
/// Useful for potions, ammunition, and other stackable items.
/// </summary>
public class StackableProperty : ItemPropertyBase
{
    /// <summary>Maximum number of items that can stack together</summary>
    public int MaxStackSize;
    
    /// <summary>Current number of items in this stack</summary>
    public int CurrentStack;
}
```

**Example:** 64 healing potions can be in one inventory slot.

#### **UsageCountProperty.cs** — Usage Counter
```csharp
/// <summary>
/// Tracks how many times an item has been used.
/// Useful for items with limited charges or uses.
/// </summary>
public class UsageCountProperty : ItemPropertyBase
{
    /// <summary>How many times this item has already been used</summary>
    public int UsageCount;
    
    /// <summary>Maximum times this item can be used before consuming</summary>
    public int MaxUsageCount;
}
```

**Example:** Magic staff with 10 charges.

---

### Equipment Properties

#### **EquippableItem.cs** — Base Equipment
```csharp
/// <summary>
/// Base implementation for equippable items.
/// Represents items that can be equipped to provide passive bonuses.
/// </summary>
public class EquippableItem : ItemPropertyBase { }
```

#### **DurabilityProperty.cs** — Durability
```csharp
/// <summary>
/// Represents the durability/integrity of an equipped item.
/// Durability decreases with use and can reach zero.
/// Items with zero durability cannot be used.
/// </summary>
public class DurabilityProperty : ItemPropertyBase
{
    /// <summary>Current durability points of the item</summary>
    public int CurrentDurability;
    
    /// <summary>Maximum durability points (starting value)</summary>
    public int MaxDurability;
    
    /// <summary>
    /// Determines if the item can still be used based on durability.
    /// </summary>
    /// <returns>True if durability is greater than 0, false otherwise</returns>
    public bool CanUse()
    {
        return CurrentDurability > 0;
    }
}
```

**Example:** Sword with 100/100 durability. After each hit, durability decreases.

#### **ChargeProperty.cs** — Charges
```csharp
/// <summary>
/// Represents charges or spell uses for magical items.
/// Items can be recharged after depleting charges.
/// </summary>
public class ChargeProperty : ItemPropertyBase
{
    /// <summary>Current number of available charges</summary>
    public int CurrentCharges;
    
    /// <summary>Maximum number of charges the item can hold</summary>
    public int MaxCharges;
}
```

**Example:** Magic staff with 3/5 charges for fireball spell.

#### **EquipSlotProperty.cs** — Equipment Slot
```csharp
/// <summary>
/// Defines which equipment slots this item can be equipped to.
/// Restricts where on the character the item can be placed.
/// </summary>
public class EquipSlotProperty : ItemPropertyBase
{
    /// <summary>Array of allowed slots for this equipment</summary>
    public EquipmentSlot[] AllowedSlots;
}
```

**Example:** Shield can ONLY be equipped to LeftHand.

---

## ⚡ Effects System

### Base Class: **ItemEffectBase.cs**

```csharp
/// <summary>
/// Base class for all item effects.
/// Effects represent bonuses or benefits that activate when using an item.
/// Can be applied when consuming items or when equipping equipment.
/// </summary>
public abstract class ItemEffectBase { }
```

**Role:** Base class for all effects (bonuses) that activate when using an item.

---

### Available Effects:

| Class | Effect | Type |
|-------|--------|------|
| **AddHealHealthEffect** | Heal health points | Consumable |
| **AddMaxHealthEffect** | Permanently increase max HP | Consumable/Equipment |
| **AddArmorEffect** | Armor bonus | Equipment |
| **AddAttackEffect** | Attack bonus | Equipment |
| **AddSpeedEffect** | Movement speed bonus | Equipment |
| **AddXpEffect** | Experience bonus | Consumable |

**Example Implementation: AddArmorEffect.cs**
```csharp
/// <summary>
/// Effect that adds armor/defense when equipped.
/// Provides passive defense bonus to the character.
/// </summary>
public class AddArmorEffect : ItemEffectBase
{
    /// <summary>Amount of armor to add to the character</summary>
    [SerializeField] private int _armorBonus;
    
    /// <summary>
    /// Applies the armor bonus to a character.
    /// </summary>
    /// <param name="character">The character to apply the effect to</param>
    public void ApplyEffect(Character character)
    {
        character.AddArmor(_armorBonus);
    }
}

/// <summary>
/// Effect that adds attack damage when equipped.
/// Increases the character's attack power permanently.
/// </summary>
public class AddAttackEffect : ItemEffectBase
{
    /// <summary>Amount of attack damage to add</summary>
    [SerializeField] private int _attackBonus;
    
    /// <summary>
    /// Applies the attack bonus to a character.
    /// </summary>
    public void ApplyEffect(Character character)
    {
        character.AddAttack(_attackBonus);
    }
}

/// <summary>
/// Effect that heals the character when consumed.
/// Restores health points immediately.
/// </summary>
public class AddHealHealthEffect : ItemEffectBase
{
    /// <summary>Amount of health to restore</summary>
    [SerializeField] private int _healAmount;
    
    /// <summary>
    /// Heals the character by the specified amount.
    /// </summary>
    public void HealCharacter(Character character)
    {
        character.Heal(_healAmount);
    }
}

/// <summary>
/// Effect that increases maximum health when equipped.
/// Provides permanent max HP increase.
/// </summary>
public class AddMaxHealthEffect : ItemEffectBase
{
    /// <summary>Amount to increase max health by</summary>
    [SerializeField] private int _maxHealthIncrease;
    
    /// <summary>
    /// Applies the max health bonus to a character.
    /// </summary>
    public void ApplyEffect(Character character)
    {
        character.AddMaxHealth(_maxHealthIncrease);
    }
}

/// <summary>
/// Effect that increases movement speed when equipped.
/// Provides passive speed bonus.
/// </summary>
public class AddSpeedEffect : ItemEffectBase
{
    /// <summary>Percentage increase to movement speed</summary>
    [SerializeField] private float _speedBonus;
    
    /// <summary>
    /// Applies the speed bonus to a character.
    /// </summary>
    public void ApplyEffect(Character character)
    {
        character.AddSpeed(_speedBonus);
    }
}

/// <summary>
/// Effect that grants experience when consumed.
/// Increases character level progression.
/// </summary>
public class AddXpEffect : ItemEffectBase
{
    /// <summary>Amount of experience to grant</summary>
    [SerializeField] private int _xpAmount;
    
    /// <summary>
    /// Grants experience to the character.
    /// </summary>
    public void GrantXp(Character character)
    {
        character.AddExperience(_xpAmount);
    }
}
```

---

## 🔄 How Everything Works Together

### Item Lifecycle:

```
1. CREATION (in editor)
   ├─ Developer creates ItemData (ScriptableObject)
   ├─ Sets type (Consume or Equip)
   ├─ Adds appropriate Properties
   └─ Adds Effects (bonuses)

2. IN WORLD
   └─ Places WorldItem on scene
      └─ Attaches ItemData to component

3. COLLECTION
   └─ Player touches WorldItem
      ├─ WorldItem.GetItemData() returns data
      ├─ Object is destroyed
      └─ New InventoryItem appears in inventory
         └─ References ItemData
         └─ Receives unique InstanceId

4. USE (Consume)
   ├─ Extract InventoryItem from inventory
   ├─ Read ItemData.GetProperty() → ConsumableProperty[]
   ├─ Apply ItemEffects to character
   ├─ Decrease StackableProperty (if present)
   └─ Remove item when stack = 0

5. EQUIP (Equipment)
   ├─ Extract InventoryItem from inventory
   ├─ Read ItemData.GetProperty() → EquippableProperty[]
   ├─ Check EquipSlotProperty (allowed slots)
   ├─ Place in ItemSlot
   ├─ Continuously apply Effects to character
   ├─ Decrease DurabilityProperty on each hit
   ├─ When durability = 0 → item no longer works
   └─ On unequip, effects are removed
```

---

## 📦 Examples: ItemData Contents for Different Types

### Example 1: Health Potion (Consumable)
```
ItemData: "Health Potion"
├─ ID: "550e8400-e29b-41d4-a716-446655440000"
├─ Name: "Small Health Potion"
├─ Description: "Heals 50 HP"
├─ Icon: [potion bottle sprite]
├─ InteractionType: Consume
├─ ConsumableProperties:
│  └─ StackableProperty { MaxStackSize: 64 }
├─ ItemEffects:
│  └─ AddHealHealthEffect { _healAmount: 50 }
└─ EquipSlotProperty: null

When player uses it:
1. System reads ConsumableProperties
2. Applies AddHealHealthEffect (adds 50 HP)
3. Decreases StackableProperty by 1
4. If stack = 0, item is removed
```

### Example 2: Sword (Equipment)
```
ItemData: "Iron Sword"
├─ ID: "660e8400-e29b-41d4-a716-446655440111"
├─ Name: "Iron Sword"
├─ Description: "Sturdy weapon"
├─ Icon: [sword sprite]
├─ InteractionType: Equip
├─ EquippableProperties:
│  ├─ EquippableItem { }
│  ├─ DurabilityProperty { MaxDurability: 100 }
│  └─ EquipSlotProperty { AllowedSlots: [RightHand] }
├─ ItemEffects:
│  └─ AddAttackEffect { _attackBonus: 15 }
└─ ConsumableProperties: null

When player equips it:
1. System reads EquippableProperties
2. Checks EquipSlotProperty (only RightHand allowed)
3. Places in ItemSlot RightHand
4. Continuously applies AddAttackEffect (+15 attack)
5. On each hit, decreases DurabilityProperty
6. When DurabilityProperty = 0, sword stops working
```

### Example 3: Amulet of Protection (Equipment)
```
ItemData: "Amulet of Protection"
├─ ID: "770e8400-e29b-41d4-a716-446655440222"
├─ Name: "Amulet of Protection"
├─ Description: "Permanent shield"
├─ Icon: [amulet sprite]
├─ InteractionType: Equip
├─ EquippableProperties:
│  ├─ EquippableItem { }
│  └─ EquipSlotProperty { AllowedSlots: [Neck] }
├─ ItemEffects:
│  └─ AddArmorEffect { _armorBonus: 20 }
└─ DurabilityProperty: null (infinite durability)

When player equips it:
1. Places in ItemSlot Neck
2. Continuously adds +20 armor
3. No durability (permanent)
4. On unequip, armor decreases by 20
```

---

## 🎯 Key Concepts

| Concept | Explanation |
|---------|-------------|
| **ItemData** | Template/definition of an item (stored as ScriptableObject) |
| **InventoryItem** | Concrete instance of an item with unique ID |
| **ItemSlot** | Equipment position (LeftHand, RightHand, Neck) with characteristics |
| **WorldItem** | GameObject on scene that can be picked up |
| **Property** | Numerical characteristics (durability, charges, stack) |
| **Effect** | Bonus/effect applied when using item |
| **StackableProperty** | Allows stacking identical items |
| **DurabilityProperty** | Durability that decreases with use |
| **ChargeProperty** | Number of uses before recharging needed |

---

## 📐 Dependencies and Managers

The item system integrates with:

- **GameDataManager** — Loads configs and ItemData
- **GameplayManager** — Coordinates game logic
- **UIManager** — Displays items in interface
- **Zenject** — Manages dependencies

The project uses **async/await** (Cysharp.Threading.Tasks) for asynchronous operations, providing smooth item loading.

---

## 🛠️ MainApp.cs — Application Entry Point

```csharp
/// <summary>
/// Main application entry point that initializes the game.
/// Waits for managers to initialize and starts the game scenario.
/// </summary>
public class MainApp : MonoBehaviour
{
    /// <summary>
    /// Called when the game starts.
    /// Initializes managers and loads initial scenario.
    /// </summary>
    private async void Start()
    {
        // Resolve managers from dependency injection container
        var gameplayManager = ProjectContext.Instance.Container.Resolve<IGameplayManager>();
        var gameDataManager = ProjectContext.Instance.Container.Resolve<IGameDataManager>();

        // Wait for both managers to finish initialization
        await UniTask.WhenAll(
            UniTask.WaitUntil(() => gameplayManager.IsInitialized),
            UniTask.WaitUntil(() => gameDataManager.IsInitialized)
        );
        
        // Get game data and start scenario
        var gameData = ProjectContext.Instance.Container.Resolve<IGameDataManager>()
            .GetDataScriptable<GameData>();
        
        // Start the game with scenario controller
        gameplayManager.GetController<ScenarioController>().DownloadInitialScenario();
        
        // Set initial app state to main menu
        gameplayManager.ChangeAppState(Enumerators.AppState.InMainMenu);
    }
}
```

---

## 🎮 Item System Design Patterns

### 1. **Template Method Pattern**
- `ItemData` serves as the template
- `InventoryItem` is the instantiation
- Each item follows the same structure but with different values

### 2. **Strategy Pattern**
- `ItemPropertyBase` is the strategy interface
- Different properties implement different behaviors
- Effects use polymorphism for different bonus types

### 3. **Factory Pattern**
- Managers create `InventoryItem` instances from `ItemData`
- World items are converted to inventory items

### 4. **Composition Pattern**
- `ItemData` composes multiple `Property` and `Effect` objects
- Flexible combination of properties creates different item types

### 5. **Observer Pattern** (Implicit)
- Managers observe item usage and update character state
- Effect system notifies character of bonus changes

---

## 🚀 Extensibility

The system is highly extensible:

### Adding a New Item Type:
1. Create new ScriptableObject in editor
2. Set ItemData fields (name, description, icon)
3. Choose InteractionType (Consume or Equip)
4. Add Properties based on type
5. Add Effects for bonuses

### Adding a New Property:
1. Create new class extending `ItemPropertyBase`
2. Add serializable fields for configuration
3. Implement logic to apply/remove the property
4. Reference in ItemData

### Adding a New Effect:
1. Create new class extending `ItemEffectBase`
2. Define which characteristic it modifies
3. Implement the effect application logic
4. Add to ItemData's Effects array

---

## 📝 Summary

The Test-Azulon item system is a flexible, extensible architecture that allows:
- ✅ Creating new item types without code changes
- ✅ Combining properties to create unique items
- ✅ Adding effects dynamically
- ✅ Tracking item state per instance
- ✅ Supporting both consumables and equipment
- ✅ Managing durability and charges

This design enables rapid game development and easy iteration on item mechanics! 🎮

---

*Documentation generated for Test-Azulon project*
*Version: 1.0*
*Last Updated: 2026-06-18*
