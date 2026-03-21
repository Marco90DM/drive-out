# 🚗 Drive Out

> **Roguelite arcade racing game** — guida il tuo van attraverso tunnel infiniti, affronta boss, evita hazard e sopravvivi il più a lungo possibile.

---

## 📋 Overview

| | |
|---|---|
| **Engine** | Unity 2022.3 LTS |
| **Linguaggio** | C# |
| **Piattaforma target** | Windows (Steam Early Access) |
| **Modalità** | Single player / Co-op 1–4 giocatori (asimmetrico) |
| **Genere** | Roguelite / Arcade Racing |

---

## 🎮 Core Gameplay

- **Guida FPS** in prima persona dentro un tunnel procedurale infinito
- **Hazard** sul percorso: banana peel, engine oil, cardboard, spikes, light flashing
- **Boss** da affrontare ogni X secondi: Goblin Van, Vent Monster
- **Upgrade** randomici offerti da un van di supporto
- **Ruoli cooperativi** (1–4 giocatori): Driver, GPS, Combattente, Riparatore

---

## 🗂️ Struttura del Progetto

```
Assets/
├── Scripts/        # C# gameplay scripts
├── Scenes/         # Unity scene files
├── Prefabs/        # Prefab assets
├── Materials/      # Materiali e shader
├── Textures/       # Texture e sprite
├── Audio/          # SFX e musica
├── Animations/     # Animator controller e clip
├── UI/             # Canvas, prefab UI, font
└── Resources/      # Asset caricati a runtime
Docs/               # Documentazione di design e GDD
```

---

## 🚀 Setup Locale

### Prerequisiti
- Unity **2022.3 LTS** con modulo **Windows Build Support**
- Input System package (installato via Package Manager)
- Git LFS (per asset binari grandi)

### Clone e apertura
```bash
git clone https://github.com/marcodellemonache90/drive-out.git
cd drive-out
```
Apri Unity Hub → **Add project from disk** → seleziona la cartella clonata.

---

## 🌿 Branch Strategy

| Branch | Scopo |
|--------|-------|
| `main` | Build stabile — solo via PR approvata |
| `develop` | Branch di integrazione principale |
| `feature/*` | Feature branch individuali |
| `fix/*` | Bug fix |
| `release/*` | Preparazione release Steam |

---

## 📌 Azure DevOps Backlog

Il backlog del progetto è gestito su Azure DevOps:
👉 `https://dev.azure.com/marcodellemonache90/Drive%20Out`

Sprint attivi:
- **Sprint 00** — Setup e Fondamenta
- **Sprint 01** — Core Loop e Guida FPS
- **Sprint 02** — Boss e Hazard Base
- **Sprint 03** — Multiplayer e Ruoli
- **Sprint 04** — Upgrade e Mappa Procedurale
- **Sprint 05** — Balancing e QA
- **Sprint 06** — Progressione e Polish

---

## 👥 Team

| Ruolo | |
|-------|-|
| Game Developer | @marcodellemonache90 |

---

## 📄 Licenza

Progetto privato — tutti i diritti riservati.
