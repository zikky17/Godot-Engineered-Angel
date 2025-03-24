# Godot-Engineered-Angel

# 🛡️ Engineered Angel – RPG UI System (Godot 4, C#)

Ett modulärt och avancerat UI-system byggt i **Godot 4 med C#** för ett 2D-RPG. Fokus ligger på ett dynamiskt inventorysystem, loot-hantering, vapenutrustning och ett intuitivt användargränssnitt med stöd för högerklicksmenyer, signaler och databaslagring.

## 🎮 Funktioner

- 📦 **Inventorysystem**
  - Dynamiskt `GridContainer` för items
  - Visuell återkoppling med ikoner och tooltips
  - Stackbara objekt (t.ex. potions)

- ⚔️ **Utrustning (Equip System)**
  - Weapon Slot med tooltip
  - Equip/Unequip via högerklick
  - Automatisk visning av utrustat vapen vid spelstart

- 🗑️ **Void Touch (Destroy System)**
  - Högerklick > Destroy Item
  - Item tas bort från UI + databas
  - Scrap-system (VoidScraps) med belöning baserat på tier × quantity

- 🖱️ **PopupMenyer**
  - Anpassade `PopupMenu`-menyer för varje item
  - Positioneras utifrån musens position
  - Alternativ: Equip / Destroy

- 📦 **Persistens via databas**
  - `PlayerInventoryRepository` hanterar:
    - Hämtning av items vid start
    - Spara/ta bort utrustade vapen
    - Lagring av inventory-items

- 💬 **TooltipText**
  - Visar detaljer som attack, försvar, tier och specialeffekt
  - Tooltip uppdateras automatiskt vid equip

🧩 Teknisk stack
Godot 4 (C#)

SQLite (via repository-mönster)

Signalhantering (Connect, Callable)

Custom UI-komponenter (ItemSlot)

Tooltip, popupmenyer och GridContainers

📌 Planerade funktioner
 Support för armor / shield-slots

 Av-equip med klick på WeaponEquipped

 Fler raritetsnivåer (Legendary etc.)

 Drag & drop-stöd i inventory

 Ljud + visuella effekter vid pickup/destroy



