# Multiplayer Card Game – Unity Assignment

## Unity Version
- Unity 6 (6000.0.38f1)

---

## How to Run the Game

### Method 1: Host and Client on Same Machine
1. Open the project in Unity.
2. Open the **Menu** scene.
3. Press **Play** in the editor.
4. Click **Host**.
5. Build and run a second instance of the game.
6. Click **Client** to connect to the host.

---

### Method 2: Two Different Machines
1. Run the game on the host machine.
2. Click **Host**.
3. Note the host machine’s IP address.
4. On the client machine:
   - Enter the host IP.
   - Click **Client** to connect.

---

## Game Rules
- The game lasts **6 turns**.
- Each turn:
  - Players receive energy equal to the turn number.
  - Players select and play cards.
- When both players end their turn:
  - Cards are revealed.
  - Scores are calculated.
- After Turn 6:
  - Final scores are compared.
  - Winner is displayed.

---

## Controls
- Click a card to select it.
- Click **Play Card** to place it.
- Click **End Turn** to finish your turn.
- If the timer reaches 0, the turn ends automatically.

---

## Assumptions
- Game is designed for **2 players only**.
- Cards are predefined in the JSON card config.
- Both players start with the same card pool.

---

## Limitations
- No reconnection system if a player disconnects mid-game.
- No matchmaking or lobby system.
- Minimal UI polish (assignment-focused).

---

## Networking
- Built using **Mirror Networking**.
- Uses a simple host-client architecture.

---

## Scenes
- `Menu` – Host/Client selection
- `Game` – Main gameplay

---

## Author
- Ramesh Reddy


