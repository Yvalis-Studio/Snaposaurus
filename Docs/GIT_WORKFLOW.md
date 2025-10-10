# 🔄 Git Workflow - Snaposaurus

Guide de workflow Git pour la gestion des branches de développement.

---

## 📋 Structure des branches

- **`main`** : Branche de production, toujours stable
- **`develop`** : Branche de développement principale
- **`feature/xxx`** : Branches de fonctionnalités individuelles

---

## 🔄 Workflow : Merge `develop` dans votre branche de travail

### 1️⃣ Récupérer les dernières modifications du remote

```bash
# Récupérer toutes les modifications du remote (sans les appliquer)
git fetch origin

# S'assurer que develop LOCAL est à jour avec develop REMOTE
git checkout develop
git pull origin develop
# ⚠️ À ce stade : develop local = develop remote
```

### 2️⃣ Retourner sur votre branche de travail

```bash
# Retourner sur votre branche (exemple: feature/ma-feature)
git checkout feature/ma-feature

# Optionnel : vérifier l'état de votre branche locale vs remote
git status
# Si "Your branch is behind" → faire git pull origin feature/ma-feature d'abord
```

### 3️⃣ Merger develop dans votre branche

```bash
# Merger develop LOCAL dans votre branche LOCALE actuelle
git merge develop
# ⚠️ Ceci merge votre develop LOCAL (qui est à jour) dans votre branche LOCAL
```

### 4️⃣ Résoudre les conflits (si nécessaire)

Si des conflits apparaissent :
```bash
# Éditer les fichiers en conflit manuellement
# Puis les ajouter une fois résolus
git add <fichiers-résolus>

# Finaliser le merge
git commit
```

### 5️⃣ Pousser les modifications sur le remote

```bash
# Pousser votre branche LOCALE vers le REMOTE
git push origin feature/ma-feature
# ⚠️ Maintenant : feature/ma-feature local = feature/ma-feature remote
```

---

## ✅ Workflow : Merger votre branche dans `develop`

### 1️⃣ S'assurer que votre branche est à jour avec develop

```bash
# D'abord mettre à jour develop LOCAL depuis develop REMOTE
git checkout develop
git pull origin develop

# Revenir sur votre branche et merger develop dedans
git checkout feature/ma-feature
git merge develop
# ⚠️ Résoudre les conflits si nécessaire
```

### 2️⃣ Pousser votre branche sur le remote

```bash
# Pousser feature/ma-feature LOCAL → feature/ma-feature REMOTE
git push origin feature/ma-feature
```

### 3️⃣ Basculer sur develop LOCAL et merger

```bash
# Aller sur develop LOCAL
git checkout develop

# ⚠️ IMPORTANT : S'assurer que develop LOCAL est toujours à jour
git pull origin develop

# Merger votre branche LOCALE dans develop LOCAL
git merge feature/ma-feature
```

### 4️⃣ Pousser develop LOCAL vers develop REMOTE

```bash
# Pousser develop LOCAL → develop REMOTE
git push origin develop
# ⚠️ Maintenant : develop local = develop remote (avec votre feature)
```

---

## 🚀 Workflow : Merger `develop` dans `main`

### 1️⃣ S'assurer que develop est à jour et stable

```bash
# Aller sur develop
git checkout develop
git pull origin develop

# Vérifier que tout fonctionne (build, tests, etc.)
```

### 2️⃣ Basculer sur main et récupérer les dernières modifications

```bash
git checkout main
git pull origin main
```

### 3️⃣ Merger develop dans main

```bash
git merge develop
```

### 4️⃣ Pousser main

```bash
git push origin main
```

### 5️⃣ Créer un tag de version (optionnel mais recommandé)

```bash
# Créer un tag pour cette version
git tag -a v1.0.0 -m "Release version 1.0.0"

# Pousser le tag
git push origin v1.0.0
```

---

## 🔄 Cycle complet résumé

### Travailler sur une feature

```bash
# 1. Créer/aller sur votre branche
git checkout -b feature/ma-feature

# 2. Coder, commiter régulièrement
git add .
git commit -m "Description des changements"

# 3. Récupérer develop régulièrement
git checkout develop && git pull origin develop
git checkout feature/ma-feature
git merge develop

# 4. Pousser votre travail
git push origin feature/ma-feature
```

### Intégrer dans develop

```bash
# 1. Mettre à jour avec develop
git checkout feature/ma-feature
git merge develop
git push origin feature/ma-feature

# 2. Merger dans develop
git checkout develop
git merge feature/ma-feature
git push origin develop
```

### Release vers main

```bash
# 1. Tester develop
git checkout develop
git pull origin develop
# Vérifier build, tests, QA...

# 2. Merger dans main
git checkout main
git pull origin main
git merge develop
git push origin main

# 3. Créer un tag
git tag -a v1.0.0 -m "Release v1.0.0"
git push origin v1.0.0
```

---

## ⚠️ Bonnes pratiques

### ✅ À FAIRE

- **Toujours `git pull` avant de merger** → pour synchroniser LOCAL avec REMOTE
- **Vérifier `git status`** avant toute opération → pour savoir où vous en êtes
- **Commiter avant de merger** (avoir un working directory propre)
- **Tester après chaque merge** sur votre branche LOCALE avant de push
- **Merger develop régulièrement** dans votre branche pour éviter les gros conflits
- **Utiliser des messages de commit clairs**
- **Faire `git fetch origin`** régulièrement pour voir l'état du remote sans modifier votre local

### ❌ À ÉVITER

- **Ne jamais merger main dans develop** (le flux va de develop → main)
- **Ne jamais merger main dans une feature branch**
- **Ne pas pousser sur main sans tester**
- **Ne pas accumuler trop de commits** avant de merger avec develop
- **Ne jamais faire `git pull` sur une branche avec des modifications non commitées** → risque de conflits
- **Ne pas confondre LOCAL et REMOTE** → `git merge` travaille en LOCAL, `git push/pull` synchronise avec REMOTE

---

## 🆘 Commandes utiles

```bash
# Voir l'état actuel (LOCAL vs REMOTE)
git status
# Indique si votre branche est "ahead" (en avance) ou "behind" (en retard) par rapport au remote

# Voir toutes les branches (locales et remote)
git branch -a

# Voir uniquement les branches locales
git branch

# Voir uniquement les branches remote
git branch -r

# Récupérer l'état du remote sans modifier le local
git fetch origin

# Annuler un merge en cours (si conflits)
git merge --abort

# Voir l'historique avec le graphe
git log --oneline --graph --all

# Voir les différences avant de merger
git diff develop..feature/ma-feature

# Comparer votre branche locale avec sa version remote
git diff feature/ma-feature origin/feature/ma-feature

# Voir quels commits sont sur le remote mais pas en local
git log HEAD..origin/feature/ma-feature

# Voir quels commits sont en local mais pas sur le remote
git log origin/feature/ma-feature..HEAD
```

---

## 📊 Schéma du workflow

```
main (production)
  ↑
  └─── merge (releases)
       |
develop (dev principale)
  ↑    ↓
  └────┴──── merge réguliers
       |
feature/xxx (votre branche)
```

---

## 💡 Comprendre LOCAL vs REMOTE

### Qu'est-ce que LOCAL ?
- **Votre copie** du dépôt sur votre ordinateur
- Toutes les opérations `git commit`, `git merge`, `git checkout` travaillent en LOCAL
- Vous pouvez faire autant de modifications que vous voulez sans affecter le remote

### Qu'est-ce que REMOTE ?
- Le dépôt **sur le serveur** (GitHub, GitLab, etc.)
- Partagé avec toute l'équipe
- Nécessite `git push` pour envoyer vos modifications locales
- Nécessite `git pull` ou `git fetch` pour récupérer les modifications des autres

### Commandes clés

| Commande | Action | Direction |
|----------|--------|-----------|
| `git fetch origin` | Récupère l'état du remote **sans modifier** votre local | REMOTE → LOCAL (info seulement) |
| `git pull origin <branch>` | Récupère ET applique les changements du remote | REMOTE → LOCAL (+ merge auto) |
| `git push origin <branch>` | Envoie vos commits locaux vers le remote | LOCAL → REMOTE |
| `git merge <branch>` | Fusionne une branche dans votre branche actuelle | LOCAL ↔ LOCAL |
| `git commit` | Enregistre vos changements en local | LOCAL seulement |

### Exemple de workflow complet

```bash
# 1. Vous êtes sur feature/ma-feature (LOCAL)
git status
# → "On branch feature/ma-feature"

# 2. Récupérer l'info du remote (sans rien changer)
git fetch origin
# → Maintenant git connait l'état du REMOTE

# 3. Mettre à jour develop LOCAL avec develop REMOTE
git checkout develop
git pull origin develop
# → develop LOCAL = develop REMOTE

# 4. Retourner sur votre branche et merger develop LOCAL dedans
git checkout feature/ma-feature
git merge develop
# → feature/ma-feature LOCAL contient maintenant develop LOCAL

# 5. Pousser votre branche locale vers le remote
git push origin feature/ma-feature
# → feature/ma-feature REMOTE = feature/ma-feature LOCAL
```

### 🚨 Erreurs courantes

#### ❌ Erreur 1 : Oublier de pull avant de merge
```bash
git checkout develop
git merge feature/ma-feature  # ❌ develop LOCAL peut être obsolète !
```
**Solution :**
```bash
git checkout develop
git pull origin develop       # ✅ Mettre à jour develop LOCAL d'abord
git merge feature/ma-feature  # ✅ Maintenant c'est bon
```

#### ❌ Erreur 2 : Pousser sans avoir mergé develop
```bash
# Votre branche est vieille, develop a évolué
git push origin feature/ma-feature  # ❌ Peut créer des conflits plus tard
```
**Solution :**
```bash
git checkout develop
git pull origin develop
git checkout feature/ma-feature
git merge develop              # ✅ Mettre à jour avec develop d'abord
git push origin feature/ma-feature
```

#### ❌ Erreur 3 : Confondre les branches locales et remote
```bash
git merge origin/develop  # ❌ Merger avec la version remote (obsolète)
```
**Solution :**
```bash
git fetch origin          # Récupérer l'info du remote
git checkout develop
git pull origin develop   # Mettre à jour develop local
git checkout feature/ma-feature
git merge develop         # ✅ Merger avec develop LOCAL (à jour)
```

---

**🦖 Happy coding!**
