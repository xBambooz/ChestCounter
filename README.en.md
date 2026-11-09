> Fork changes: Ryan's fork of [xdoofy92/ChestCounter](https://github.com/xdoofy92/ChestCounter), with teammate tracking and optional replay archiving.

<div align="center">

[Español](README.md) · **English**

# Fortnite Chest Counter

[![version](https://img.shields.io/github/v/release/xdoofy92/ChestCounter?style=flat-square&label=version&color=e8b04b)](../../releases/latest) [![downloads](https://img.shields.io/github/downloads/xdoofy92/ChestCounter/total?style=flat-square&label=downloads&color=e8b04b)](../../releases) ![Windows 10 | 11](https://img.shields.io/badge/Windows-10%20%7C%2011-3a7bd5?style=flat-square) [![license](https://img.shields.io/github/license/xdoofy92/ChestCounter?style=flat-square&label=license&color=555)](LICENSE)

**Counts every chest you open in Fortnite, match after match, with an in-game overlay.**

<img src="docs/overlay-espera.png" alt="Overlay between two matches" width="300">&nbsp;&nbsp;<img src="docs/overlay-en-curso.png" alt="Overlay during a match" width="300">

[**⬇ Download ChestCounter.exe**](../../releases/latest) · Windows 10 / 11 · free · no install

</div>

---

Chest Counter reads the replays Fortnite saves after each match and keeps a running count of the chests you opened, plus the **treasure hunt treasures** your chests gave you. An always-on-top overlay shows your totals while you play, and a detailed report keeps your whole history.

> It counts the chests **you opened**. It does not show where chests are on the map.

## How it works (and why it is safe)

Fortnite can record a **replay file** of every match on your PC. Chest Counter reads these files once the match is over, and nothing else.

- ✅ **Only reads replay files**: the `.replay` files Fortnite writes to `%LOCALAPPDATA%\FortniteGame\Saved\Demos`. They are opened read-only and never modified.
- ✅ **No game modification**: nothing in the Fortnite installation folder is touched, no file is added or patched.
- ✅ **No access to the game's memory**: the tool never reads, writes or injects anything into the Fortnite process. It does not interact with the running game at all.
- ✅ **No data sent anywhere**: no login, no Epic account access, no telemetry. The only network access is the check for a new version on GitHub, which can be turned off in the settings.
- ✅ **Lightweight**: runs at low priority (and in "efficiency mode" on Windows 11), sleeps between matches, and the overlay is only redrawn when a number changes.

> [!IMPORTANT]
> **Replays must be turned on in Fortnite**: *Settings → Game → Replays → Record Replays: **On*** (and *Large Team modes* if you play them). Without replays, the tool has nothing to read.

Because Fortnite only writes the replay's decryption key when the match ends, chests are counted **a few seconds after each match**, not during it. While you play, the overlay shows **● MATCH IN PROGRESS**.

## Getting started

1. Download **[`ChestCounter.exe`](../../releases/latest)**. That single file is all you need.
2. Turn on replays in Fortnite (see above).
3. Run `ChestCounter.exe`.
   > Windows may show *"Windows protected your PC"* because the exe is not signed. Click *More info → Run anyway*.
4. A short setup (Yes / No dialogs) checks your **replays folder**, offers to count the **replays already on your PC**, and offers to **open the counter automatically when Fortnite starts**. The language follows Windows: **Spanish** if Windows is in Spanish, **English** otherwise.

<p align="center"><img src="docs/setup.png" alt="First launch setup" width="760"></p>

If you count your existing replays, a progress bar runs in the window while the report opens in your browser and fills in live.


5. Keep the window open (you can minimize it) while you play. A few seconds after each match, its chests are added:

<p align="center"><img src="docs/main.png" alt="Main window" width="820"></p>

> [!TIP]
> Fortnite only keeps a limited number of unsaved replays and deletes the oldest ones, and a match played while the counter is closed is not counted. Turn on **Open with Fortnite** (offered during the setup, or in the window's settings) so you never have to think about it.

## Open with Fortnite

When this option is on, a tiny invisible process starts with Windows and checks every few seconds whether Fortnite is running. It reads nothing from the game: it only looks at the list of running programs. When Fortnite starts, it opens the counter (minimized, the game keeps the screen). When Fortnite closes, the counter counts your last match and closes itself 30 seconds later; click the notice in its window to keep it open. Turn it off at any time in the settings (the **Settings** button): the invisible process stops and is removed from Windows startup.

## The overlay

- **Big numbers**: your **total** chests (gold) and, in the box, the chests of the **previous** match (or the **current** one while you play).
- **Session**: chests since the tool was started. **Avg.**: average chests per match in this session.
- **Previous** (only during a match): chests of your previous match. **Treasures**: treasures found so far.
- During a match, the **MATCH IN PROGRESS** badge appears and the box shows "…" until the chests can be read; then it switches to **LIVE** with the chests of the current match.

To see it over the game, set Fortnite to **Windowed Fullscreen** (*Settings → Video → Window Mode*): in exclusive fullscreen, Windows cannot draw anything on top of the game. The overlay starts **turned off**: turn it on with the **Overlay** button (green dot = on, red = off) or with Ctrl+Shift+O.

Streaming? While the counter is open, the same overlay is served at **http://localhost:7373/** (copy it from **Settings**). In OBS, add a **browser source** with that address: transparent background, updates by itself.

## Shortcuts

| Shortcut | What it does |
|---|---|
| **Ctrl+Shift+O** | Show / hide the overlay. Works anywhere, even in game. |
| **Ctrl+Shift+L** | Lock / unlock the overlay. Works anywhere. **Locked**: the mouse goes through it and it cannot move (best in game). **Unlocked**: hold the left mouse button to move it. |
| Right-click on the overlay | Hide it. |
| Padlock on the overlay | Lock it. |

If another program already uses Ctrl+Shift+O or Ctrl+Shift+L, the tool shows a notice and the overlay keeps working with its window button.

### Main window

The window has three buttons: **Overlay** (show or hide the overlay; green dot = on, red = off), **Report** (opens `report.html`) and **Settings**. The installed version is shown in the window title. When a new version is available, an install button appears; when Fortnite closes, a notice lets you keep the counter open (click it). The profile selector appears when there are several accounts.

The window can be **resized and maximized**: everything grows in proportion (cards, figures and chart) and no text gets cut off; if a figure does not fit its card, its font shrinks a little.

In **Settings** you can open with Fortnite, open the report at startup, choose the updates, type the **chests opened before the counter** (an estimate added to your total), copy the **overlay address for OBS** and change the replays folder. The installed version is shown at the bottom.

<p align="center"><img src="docs/settings.png" alt="Settings" width="400"></p>

From top to bottom you will see: the total chests, the last match, the session, today, the best day, the average, the best match, the **History** (matches stored and since when), the treasures and the chart of the last 7 days.

## The report

`report.html` opens in your browser and refreshes by itself. It uses the same colors as the window (dark, or light when Windows is in light mode) and adapts to phones:

- **Key figures:** the total chests in large type, with this session's chests and your Victory Royales; next to it, matches, average per match, best match and treasures.
- **Chests per day:** chart of the last 30 days with a scale, the daily average (dashed line) and today in cyan. Hover a bar to see the day and its chests.
- **By chest type** and **where the treasures come from**, with a proportion bar on each row. Ammo boxes, food boxes and loot cannons are listed apart, not in the total.
- **Treasures obtained:** each treasure with its match, the moment and the source chest.
- **Latest matches** (all saved matches): mode, placement (🏆 victory in gold, top 10 in green), chests and treasures.

<p align="center"><img src="docs/report.png" alt="Report" width="820"></p>

## Treasures

- **★★ Golden chest (certain).** When a chest gives a treasure hunt prize (cosmetic, music track, loading screen, vehicle…), the server spawns a golden chest carrying the **prize id**. The tool reads it from the replay.
- **★ Every treasure received (exact moment).** Each treasure you receive is recorded with its exact time and the chest it came from; it only counts if that chest is **yours** (treasures shared by a teammate are not counted). When the item can be identified, it is named; otherwise it shows as **"Treasure gizmo"** (golden firework, portable extractor, locators, extraction accelerator), whose exact type is not in the replay.
- A treasure that came from something other than a chest (loot cannon, ammo box…) is highlighted with ⚑ in the report.

The game only sends prize ids, not names: `treasures.json` maps them to names and can be edited with Notepad. New ids are added automatically and the tool tells you when one needs a name.

## Profiles (several accounts on one PC)

The replays saved on a PC are normally those of its player, so there is **no name to type**: the tool counts the player who recorded each replay. If replays from several accounts are found, each account gets its own profile with its own statistics. The **Profile** text at the top right of the window then becomes a **drop-down list** to choose the account shown (in the window, the overlay, OBS and the report). A match played on another account is saved in that account's profile, never lost.

## Updates

At startup, the counter checks GitHub for a new version. When there is one, it tells you, and the install button downloads it, checks it and restarts the counter: your history and settings are kept. In the settings (the **Settings** button) you can choose between *offered* (default), *installed automatically* (only between two matches) or *off* (no network access at all).

## Your data

Everything is stored in `%APPDATA%\ChestCounter`:

| File | Content |
|---|---|
| `report.html` | The report (opens in your browser). |
| `overlay.html` | File copy of the overlay (for OBS, prefer http://localhost:7373/). |
| `matches.csv` | One line per match, for Excel or Google Sheets. |
| `data.json` | Your full history. **Back it up** if you change PC. |
| `config.json` | Profile, replays folder, overlay position and settings. |
| `treasures.json` | Names of the treasure hunt prizes. |
| `errors.log` | Details of unreadable replays, to attach to a bug report. |

Fortnite deletes old replays, but **your history is not deleted**: each match is saved in `data.json` as soon as it is counted, and the report and statistics come from there. So even when the replay no longer exists, the match stays in your totals.

To start over: run `ChestCounter.exe --reset-config`, or delete that folder.

## FAQ

**Nothing is counted.** Check that replays are turned on in Fortnite, and that the tool is running when the match ends. The window tells you if the replays folder is missing or empty.

**Are ammo boxes counted?** No. Ammo boxes, food boxes and loot cannons are listed separately in the report and are not part of the chest total. Chests you **destroy** instead of opening are counted.

**Creative modes?** Only Battle Royale matches are counted; other modes are skipped.

**"Cannot read this replay".** A Fortnite update can change the replay format; the parser then needs an update. Matches already counted are kept. Please [open an issue](../../issues) and attach `errors.log` (in `%APPDATA%\ChestCounter`).

**Already open?** Only one counter can run at a time: launching it again just tells you it is already open.

**Can a match be counted twice?** No, even if you rename or save the replay.

## Command line options

```
ChestCounter.exe --demos <folder>    other replays folder
ChestCounter.exe --data <folder>     other data folder
ChestCounter.exe --once              analyze the finished replays, then quit
ChestCounter.exe --reset-config      run the setup again
ChestCounter.exe --overlay-preview <file.png>   save previews of the overlay, then quit
```

## Building from source

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (Windows x64), then double-click **`build.bat`**: the exe is written to `dist\ChestCounter.exe`. The GitHub Actions workflow builds the exe on every push and attaches it to a release when a `v*` tag is pushed.

## License and credits

[MIT](LICENSE). Replay parsing is based on [FortniteReplayDecompressor](https://github.com/Shiqan/FortniteReplayDecompressor) by Shiqan (MIT), slightly modified: see [`lib/PATCHES.md`](lib/PATCHES.md).

This project is not affiliated with, endorsed by or sponsored by Epic Games. Fortnite is a trademark of Epic Games, Inc.

## Team tracking and replay archive

The report shows team totals, each match's teammates and totals grouped by teammate display name. The Squad button in the main window shows the same per-match breakdown. Team totals include your own chests and observed teammate chests, with no change to your personal count. Client replays only capture teammates while nearby or spectated, so counts can be incomplete. Teammate treasures are not inferred. Old matches are automatically re-analyzed once when their replay is still available in Demos or the archive.

Settings offers an optional Team overlay line and Archive replays (off by default), an archive folder and a size limit in GB. The default archive is `%LOCALAPPDATA%\ChestCounter\ReplayArchive`. Successfully analyzed, finalized replays are copied there, including replays already in Demos when archiving is turned on; the oldest archived replays are deleted first when the limit is exceeded. Demos is never changed. Archive size is shown when opening settings. Scans include the archive and deduplicate file names and match ids. Saved statistics use all history. Archiving cannot recover already deleted replays, and future game updates may require parser updates.

No Fortnite ini tweak is applied: no supported automatic replay limit setting was verified. Epic distinguishes automatic retention from saved replays, which are unlimited on PC. See [Epic support](https://www.epicgames.com/help/c-34254770/c-33726977/fortnite-a25553277).

Squad ranks observed teammates by account id (name fallback), alongside your highlighted row. It shows matches, total chests and average, ranked by `(total + teammateMean * 5) / (matches + 5)`. Players with fewer than three observed matches appear dimmed at the bottom. Open it with the Squad button in the main window or in the report. Your own row uses your real average, since the replay always has your full data.
