# QTE Sprite Assignment Reference

## Available Sprites in `Assets/Sprites/Keybind/`
- `w.png` - W key
- `a.png` - A key
- `s.png` - S key
- `d.png` - D key
- `z.png` - Z key
- `q.png` - Q key
- `e.png` - E key
- `space.png` - Space bar
- `esc.png` - Escape key

---

## QTEManager Sprite Fields → Sprite Assignment

### QWERTY Normal Sprites:
```
Sprite Up Qwerty    → w.png
Sprite Down Qwerty  → s.png
Sprite Left Qwerty  → a.png
Sprite Right Qwerty → d.png
```

### QWERTY Pressed Sprites:
```
Sprite Up Pressed Qwerty    → (you'll need to create these or use same as normal)
Sprite Down Pressed Qwerty  → (pressed variant of s.png)
Sprite Left Pressed Qwerty  → (pressed variant of a.png)
Sprite Right Pressed Qwerty → (pressed variant of d.png)
```

### AZERTY Normal Sprites:
```
Sprite Up Azerty    → z.png
Sprite Down Azerty  → s.png
Sprite Left Azerty  → q.png
Sprite Right Azerty → d.png
```

### AZERTY Pressed Sprites:
```
Sprite Up Pressed Azerty    → (pressed variant of z.png)
Sprite Down Pressed Azerty  → (pressed variant of s.png)
Sprite Left Pressed Azerty  → (pressed variant of q.png)
Sprite Right Pressed Azerty → (pressed variant of d.png)
```

---

## Note on Pressed States

You mentioned you have normal + pressed/hovered states for sprites.

**If you don't have separate pressed sprite files yet:**
- You can use the same sprite for both normal and pressed
- Or create pressed variants by:
  1. Duplicating the sprite in your image editor
  2. Adding a glow/highlight effect
  3. Importing back to Unity

**The QTEManager already has logic to swap between them automatically** when a key is pressed during the QTE.

---

## Key Mapping Summary

| Direction | QWERTY Key | AZERTY Key | Same? |
|-----------|------------|------------|-------|
| Up        | W          | Z          | ❌     |
| Down      | S          | S          | ✅     |
| Left      | A          | Q          | ❌     |
| Right     | D          | D          | ✅     |

**Note:** S and D are the same in both layouts, but you should still assign them separately for consistency.
