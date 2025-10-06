# QTE Photo Capture System - Documentation

## Vue d'ensemble

Le système QTE (Quick Time Event) permet au joueur de prendre des photos de dinosaures en appuyant sur une séquence de touches dans un temps limité. Le résultat détermine la qualité de la photo (nette ou floue).

---

## Architecture du système

### Scripts principaux

#### 1. **QTEManager.cs**
Gère la logique principale du QTE.

**Fonctionnalités :**
- Countdown de démarrage (3, 2, 1, GO!)
- Génération aléatoire de séquences de touches
- Timer avec barre de progression
- Détection des inputs clavier (new Input System)
- Affichage des sprites de touches (3 slots visibles + queue)
- Messages de succès/échec
- Support pour recommencer (RestartQTE) ou arrêter (StopQTE)

**Configuration Unity :**
- **Use Text Display** : Désactiver pour cacher les anciens placeholders texte
- **Key Display Slots (3)** : Assigner 3 Images UI pour les touches
- **Sprites** : Assigner z.png, q.png, s.png, d.png pour chaque direction
- **Timer Bar Slider** : Assigner le Slider UI pour la barre de temps
- **Countdown Text** : Texte pour 3,2,1,GO!
- **Success/Fail Images** : Images de résultat

#### 2. **DinosaurQTE.cs**
Lie un dinosaure spécifique au système QTE.

**Fonctionnalités :**
- Détection de fin de QTE
- Affichage de photos (nette/floue)
- Auto-retry en cas d'échec
- Arrêt sur succès

**Configuration Unity :**
- **Dino Name** : Nom du dinosaure (ex: "T-Rex")
- **Clear Photo** : Sprite de photo nette (succès)
- **Blurry Photo** : Sprite de photo floue (échec)
- **QTE Manager** : Référence au QTEManager
- **Auto Retry** : Activer le retry automatique sur échec
- **Retry Delay** : Délai avant retry (défaut: 4s)

#### 3. **PhotoManager.cs**
Gère l'affichage et la collection de photos en session.

**Fonctionnalités :**
- Affichage temporaire de photos
- Stockage en mémoire (session uniquement, pas de sauvegarde disque)
- Collection de toutes les photos prises
- Système singleton (accessible via `PhotoManager.Instance`)

**API Publique :**
```csharp
// Afficher et sauvegarder une photo
PhotoManager.Instance.ShowAndSavePhoto(dinoName, sprite, isSuccess);

// Récupérer toutes les photos
List<DinoPhoto> photos = PhotoManager.Instance.GetAllPhotos();

// Récupérer les photos d'un dino spécifique
List<DinoPhoto> trexPhotos = PhotoManager.Instance.GetPhotosByDino("T-Rex");
```

**Configuration Unity :**
- **Photo Display Image** : Image UI pour afficher la photo
- **Photo Display Panel** : Panel contenant l'image (désactivé au départ)
- **Photo Display Duration** : Durée d'affichage (défaut: 3s)

---

## Scripts de feedback visuel

### 4. **CountdownAnimation.cs**
Anime le countdown 3,2,1,GO!

**Effets :**
- Pulse à chaque changement de chiffre
- Couleurs différentes par chiffre (3=rouge, 2=orange, 1=jaune, GO=vert)

**Configuration :**
- **Pulse On Change** : Activer l'effet pulse
- **Pulse Scale** : Facteur d'agrandissement (défaut: 1.5)
- **Pulse Speed** : Vitesse de l'animation
- **Colors** : Couleurs personnalisables par étape

### 5. **QTESuccessHalo.cs**
Crée un effet de halo sur chaque touche réussie.

**Fonctionnalités :**
- Halo qui apparaît, grandit et fade out
- Configurable (taille, vitesse, couleur)

**Configuration :**
- **Halo Scale** : Taille du halo (défaut: 2)
- **Halo Fade Duration** : Durée du fade (défaut: 0.5s)
- **Halo Color** : Couleur du halo (défaut: vert)

### 6. **ScrollingTexture.cs**
Anime la texture de la barre de timer.

**Fonctionnalités :**
- Scroll horizontal de texture
- Utilise un material instance pour éviter les conflits

**Configuration :**
- **Scroll Speed** : Vitesse de défilement (0.5 = normal, négatif = inverse)

### 7. **CameraFlashEffect.cs**
Effet de flash pour animations (utilisable ailleurs).

**Fonctionnalités :**
- Pulse multiple avec fade in/out
- Scale et intensité animés
- Mode loop continu

---

## Système d'input

### 8. **BackButtonHover.cs** (amélioré)
Gère les sprites de boutons avec états hover et pressed.

**Fonctionnalités :**
- Swap de sprites au survol souris
- Swap de sprites sur pression clavier
- Support multi-touches (z, q, s, d, esc, space, e)
- Animations (rotation, scale, pulse, shake)

**Configuration :**
- **Animation Type** : Type d'animation (ou SpriteSwap)
- **Normal Sprite** : Sprite par défaut
- **Hover Sprite** : Sprite au survol
- **Trigger Key** : Touche associée (ex: "z", "esc", "space")
- **Key Pressed Sprite** : Sprite quand la touche est pressée

**Mapping des touches :**
Le système utilise le **nouveau Input System** de Unity. Les touches sont définies par string :
- "z", "q", "s", "d" pour ZQSD
- "esc" ou "escape" pour Échap
- "space" pour Espace
- "e" pour E

---

## Setup Unity complet

### Canvas QTE (World Space)

```
Canvas QTE (World Space)
├── Background_Image (fond)
├── MaskContainer (Mask pour timer bar)
│   ├── Background (texture colorée qui défile)
│   └── Fill Area (du Slider)
├── QTE_TimerBar (Slider)
│   ├── Background
│   ├── Fill Area
│   │   └── Fill (blanc, Image Type: Filled)
│   └── Border (par-dessus)
├── CountdownText (TextMeshPro + CountdownAnimation)
├── KeysContainer
│   ├── KeySlot_0 (Image)
│   ├── KeySlot_1 (Image)
│   └── KeySlot_2 (Image)
├── SuccessImage (Image, désactivée)
├── FailImage (Image, désactivée)
├── PhotoDisplayPanel (désactivé)
│   └── PhotoImage (grande Image pour photo)
└── ExitButton (Button + QTEExitButton script)
    └── CrossImage (sprite de croix)
```

### GameObjects de scène

```
Scene
├── QTEManager (avec QTEManager + QTESuccessHalo scripts)
├── PhotoManager (avec PhotoManager script)
└── Dinosaure
    └── DinosaurQTE (avec DinosaurQTE script)
```

---

## Timeline d'exécution

### Succès

1. **Début** : Countdown 3s (3, 2, 1, GO!)
2. **QTE Actif** : 5 secondes (configurable)
   - Touches affichées (3 visibles)
   - Timer bar se vide
   - Halos sur touches réussies
3. **Fin réussie** :
   - Message "SUCCESS!" (2s)
   - Photo nette affichée (3s)
   - Photo sauvegardée en mémoire
   - **QTE s'arrête** (pas de retry)

### Échec

1. **Début** : Countdown 3s
2. **QTE Actif** : 5 secondes
3. **Fin échouée** (timer à 0) :
   - Message "FAILED!" (2s)
   - Photo floue affichée (3s)
   - Photo sauvegardée en mémoire
   - **QTE redémarre après 4s** (si Auto Retry activé)

---

## Configuration des touches QTE

Les touches possibles sont définies dans `QTEManager.cs` :
```csharp
string[] possibleKeys = { "up", "down", "left", "right" };
```

**Mapping vers sprites :**
- "up" → z.png
- "down" → s.png
- "left" → q.png
- "right" → d.png

---

## Assets requis

### Sprites de touches
- `z.png`, `q.png`, `s.png`, `d.png` (touches normales)
- Versions "pressed" pour feedback visuel

### Sprites UI
- `ASSET_barre_de_vie_2.png` (timer bar)
- `ASSET_croix.png` (bouton exit)
- Sprites de résultat (success/fail)
- `logo-snaposaurus.png` (optionnel, pour flash)

### Photos de dinosaures
- Photos nettes (une par dinosaure)
- Photos floues (une par dinosaure)

---

## Paramètres configurables

### Timing
- **dbmPull** : Durée du countdown (défaut: 3s)
- **qteTimer** : Durée du QTE (défaut: 5s)
- **qteLength** : Nombre de touches à presser (défaut: 5)
- **photoDisplayDelay** : Délai avant affichage photo (défaut: 1.5s)
- **retryDelay** : Délai avant retry (défaut: 4s)

### Visuel
- **activeKeyScale** : Taille de la touche active (défaut: 1.2)
- **pressedDisplayDuration** : Durée du sprite "pressed" (défaut: 0.2s)
- **resultDisplayDuration** : Durée du message succès/échec (défaut: 2s)

---

## Extensibilité

### Ajouter de nouvelles touches
1. Dans `BackButtonHover.cs`, ajouter la touche dans `IsKeyPressed()` :
```csharp
"a" => keyboard.aKey.isPressed,
```

2. Dans `QTEManager.cs`, ajouter dans `possibleKeys` :
```csharp
string[] possibleKeys = { "up", "down", "left", "right", "a" };
```

3. Ajouter le sprite correspondant et le mapper dans `GetSpriteForKey()`.

### Ajouter des effets visuels
Les scripts de feedback sont modulaires :
- `QTESuccessHalo` peut être remplacé par des particules
- `ScrollingTexture` peut être enrichi avec shader effects
- `CountdownAnimation` peut inclure rotation, bounce, etc.

### Collection de photos
Pour afficher la collection :
```csharp
List<DinoPhoto> collection = PhotoManager.Instance.GetAllPhotos();
foreach (var photo in collection)
{
    Debug.Log($"{photo.dinoName} - Quality: {(photo.isHighQuality ? "High" : "Low")}");
    // Afficher photo.photoSprite dans UI
}
```

---

## Notes techniques

### Input System
Le projet utilise le **nouveau Input System** de Unity (pas l'ancien `Input` class).
- Assurez-vous que "Input System Package" est installé
- Player Settings → Active Input Handling : "Input System Package" (new)

### Canvas Masking
La barre de timer utilise un **Mask** pour cropper la texture qui dépasse :
- MaskContainer a un composant Mask
- Show Mask Graphic est décoché
- Background et Fill sont enfants du Mask

### Singleton Pattern
PhotoManager utilise un singleton pour être accessible globalement :
```csharp
PhotoManager.Instance.ShowAndSavePhoto(...);
```

---

## Troubleshooting

### Les sprites ne s'affichent pas
- Vérifier que les KeyDisplaySlots sont assignés dans QTEManager
- Vérifier que les sprites sont importés en "Sprite (2D and UI)"
- Vérifier la couleur des Images (doit être blanc)

### Le timer bar ne se vide pas correctement
- Fill doit avoir Image Type = "Filled"
- Fill Method = "Horizontal"
- Fill Origin = "Left"
- Anchors en Stretch/Stretch

### Les photos n'apparaissent pas
- PhotoDisplayPanel doit être dernier enfant du Canvas (par-dessus tout)
- Ou utiliser un Canvas séparé avec Sort Order élevé
- Vérifier que PhotoManager.Instance existe dans la scène

### Le QTE ne s'arrête pas sur succès
- Vérifier que `autoRetry` est activé dans DinosaurQTE
- Vérifier que le GameObject du dinosaure reste actif

### Erreur "InvalidOperationException: Input class"
- Le projet utilise le nouveau Input System
- Player Settings → Active Input Handling → "Input System Package"

---

## Crédits

Système développé avec assistance de Claude Code (Anthropic).

