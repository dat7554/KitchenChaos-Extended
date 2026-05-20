# KitchenChaos Extended

A 3D cooking game built in Unity, extended from the [KitchenChaos tutorial](https://www.youtube.com/watch?v=AmGSEH7QcDg) by Code Monkey.

> **Play it here:** [[itch.io link](https://ndat7554.itch.io/kitchenchaos-extended)]

---

## What I Built On Top of the Tutorial

This project started as a tutorial follow-along. After completing it, I extended the codebase with my own systems. Below is a clear separation of what came from the tutorial and what I designed and implemented myself.

### From the Tutorial
- Core player movement and interaction system
- Kitchen counter types (cutting, cooking, plate, trash, delivery)
- Basic recipe matching and delivery logic
- Audio, visual effects, and input rebinding
- ScriptableObject-based ingredient and recipe data

---

### My Extensions

#### Order System
- Orders expire if not completed in time — each with its own independent countdown running simultaneously
- Expired orders disappear from the board and deduct a money penalty

#### Difficulty System
- Three difficulty modes: Easy, Normal, Hard
- Each mode has its own spawn rate, order time limit, max simultaneous orders, recipe pool, and star thresholds
- Harder modes weight the recipe pool toward complex dishes more frequently

#### Upgrade Shop
- Pre-run shop where players spend last run's earnings on upgrades
- Five upgrades: Warming Lamps (longer order timers), Sharp Knives (faster chopping), Hot Stoves (faster cooking), Bigger Board (+1 order slot), Generous Tips (bonus coins per delivery)
- Budget and upgrade costs are tracked per difficulty — quitting mid-run resets the budget

#### Money & Scoring System
- Each recipe has a coin value — completing orders earns, expiring orders loses
- End screen shows earnings and losses separately with a total
- 0–3 star rating based on total money earned, with next star target shown
- Best score saved and displayed per difficulty

#### UI Systems
- Per-order countdown bar shifts green → yellow → red, shakes when critically low
- Coin popups animate in green or red after each delivery or expiry
- Hover and keyboard navigation both show upgrade/difficulty descriptions

#### Scene Management
- Persistent scene holds global systems across all scene transitions
- Additive async scene loading with loading screen

---

## Tech Stack

- **Engine:** Unity version 6000.3.10f1.
- **Patterns used:** Singleton, Observer (C# events), ScriptableObject data architecture
- **Persistence:** Unity PlayerPrefs

---

## What I Learned

- Shipped a complete playable game from tutorial base to extended original features, deployed on itch.io
- Designed and implemented multiple interconnected game systems including order management, difficulty scaling, an upgrade shop economy, and persistent save data
- The difference between `Time.deltaTime` and `Time.unscaledDeltaTime` and when each matters
- Using `PlayerPrefs` for simple cross-session persistence

---

## Credits

Original tutorial by [Code Monkey](https://www.youtube.com/watch?v=AmGSEH7QcDg). All extensions and modifications are my own work.
