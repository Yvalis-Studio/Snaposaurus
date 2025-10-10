# 🔄 Git Workflow - Snaposaurus

Guide complet du workflow Git, de la première installation au merge en production.

---

## 📋 Structure des branches

- **`main`** : Branche de production, toujours stable (**VOUS NE TRAVAILLEZ JAMAIS DIRECTEMENT DESSUS**)
- **`develop`** : Branche de développement principale (**BASE DE TOUT VOTRE TRAVAIL**)
- **`feature/xxx`** : Vos branches de fonctionnalités individuelles (créées depuis `develop`)

**⚠️ IMPORTANT : Vous ne clonez pas `main` ! Vous clonez le repo, puis vous travaillez sur `develop` et vos branches `feature/xxx`.**

---

## 🚀 WORKFLOW COMPLET : Du clone au merge en production

### ÉTAPE 0️⃣ : Premier démarrage - Cloner le repo

**Vous ne faites cette étape qu'UNE SEULE FOIS au début.**

```bash
# Cloner le dépôt REMOTE → LOCAL (toutes les branches)
git clone https://github.com/votre-username/snaposaurus.git
cd snaposaurus

# À ce stade :
# - Vous avez tout le repo en LOCAL
# - Vous êtes par défaut sur main LOCAL
# - main LOCAL = main REMOTE
# - develop existe sur le REMOTE mais n'est pas encore sur votre LOCAL

# Créer develop LOCAL à partir de develop REMOTE
git checkout develop
# Ceci crée une branche develop LOCALE qui suit develop REMOTE

# Vérifier que develop LOCAL = develop REMOTE
git pull origin develop
```

**✅ CHECKPOINT : Vous êtes maintenant sur `develop` LOCAL, à jour avec `develop` REMOTE. Vous êtes prêt à travailler.**

---

### ÉTAPE 1️⃣ : Créer votre branche de travail (depuis `develop`)

**Vous faites cette étape à chaque NOUVELLE fonctionnalité.**

```bash
# S'assurer d'être sur develop LOCAL
git checkout develop

# Mettre à jour develop LOCAL avec develop REMOTE
git pull origin develop
# ✅ develop LOCAL = develop REMOTE

# Créer votre branche de travail à partir de develop LOCAL
git checkout -b feature/ma-nouvelle-feature
# ✅ Vous êtes maintenant sur feature/ma-nouvelle-feature LOCAL
# ✅ Cette branche contient tout ce qui était dans develop LOCAL

# Vérifier où vous êtes
git status
# → "On branch feature/ma-nouvelle-feature"
```

**✅ CHECKPOINT : Vous êtes sur votre branche `feature/ma-nouvelle-feature` LOCAL, créée depuis `develop` LOCAL à jour.**

---

### ÉTAPE 2️⃣ : Travailler sur votre branche

**Vous faites cette étape tout au long du développement de votre feature.**

```bash
# Modifier vos fichiers (code, docs, etc.)

# Voir ce qui a changé
git status
git diff

# Ajouter les fichiers modifiés
git add .
# OU ajouter des fichiers spécifiques
git add src/mon-fichier.ts

# Créer un commit LOCAL
git commit -m "Description claire de vos changements"
# ✅ Vos changements sont maintenant dans l'historique LOCAL
# ⚠️ Ils ne sont PAS encore sur le REMOTE

# Pousser votre branche LOCAL vers le REMOTE (pour backup/partage)
git push origin feature/ma-nouvelle-feature
# ✅ feature/ma-nouvelle-feature LOCAL → feature/ma-nouvelle-feature REMOTE
```

**💡 CONSEIL : Commitez et poussez régulièrement (plusieurs fois par jour).**

---

### ÉTAPE 3️⃣ : Synchroniser avec `develop` pendant le développement

**⚠️ IMPORTANT : D'autres développeurs peuvent avoir mergé leurs features dans `develop` REMOTE pendant que vous travaillez. Vous devez RÉGULIÈREMENT (idéalement chaque jour) merger `develop` dans votre branche pour éviter les gros conflits.**

```bash
# 1. Récupérer l'état du REMOTE (sans modifier votre LOCAL)
git fetch origin
# ✅ Git sait maintenant ce qui a changé sur le REMOTE

# 2. Aller sur develop LOCAL
git checkout develop

# 3. Mettre à jour develop LOCAL avec develop REMOTE
git pull origin develop
# ✅ develop LOCAL = develop REMOTE (contient les features des autres)

# 4. Retourner sur votre branche
git checkout feature/ma-nouvelle-feature

# 5. AVANT de merger, vérifier si votre branche LOCAL = branche REMOTE
git status
# Si "Your branch is behind" → faire :
git pull origin feature/ma-nouvelle-feature
# ✅ feature LOCAL = feature REMOTE

# 6. Merger develop LOCAL dans votre branche LOCAL
git merge develop
# ✅ Votre branche LOCAL contient maintenant :
#    - Votre travail
#    - + Les nouvelles features des autres (depuis develop)
```

**Si des conflits apparaissent :**

```bash
# Git vous indique les fichiers en conflit
# Ouvrir les fichiers et résoudre les conflits manuellement
# (chercher les marqueurs <<<<<<, ======, >>>>>>)

# Une fois résolus, ajouter les fichiers
git add <fichiers-résolus>

# Finaliser le merge
git commit
# (Un message de commit par défaut sera proposé)
```

**Après le merge (avec ou sans conflits) :**

```bash
# Pousser votre branche LOCAL vers le REMOTE
git push origin feature/ma-nouvelle-feature
# ✅ feature/ma-nouvelle-feature REMOTE contient maintenant :
#    - Votre travail
#    - + Les nouvelles features de develop
```

**✅ CHECKPOINT : Votre branche est à jour avec `develop`. Vous pouvez continuer à coder.**

---

### ÉTAPE 4️⃣ : Finaliser et préparer le merge vers `develop`

**Une fois votre feature terminée, AVANT de merger dans `develop`, vous devez vous assurer d'être à jour.**

```bash
# 1. Commitez tout votre travail en cours
git status
# Si des fichiers modifiés → git add et git commit

# 2. Mettre à jour develop LOCAL
git checkout develop
git pull origin develop
# ✅ develop LOCAL = develop REMOTE (le plus récent)

# 3. Retourner sur votre branche et merger develop une dernière fois
git checkout feature/ma-nouvelle-feature
git merge develop
# ⚠️ Résoudre les conflits si nécessaire (même processus qu'ÉTAPE 3)

# 4. Pousser votre branche LOCAL vers le REMOTE
git push origin feature/ma-nouvelle-feature
# ✅ feature REMOTE = feature LOCAL (à jour avec develop)
```

**✅ CHECKPOINT : Votre branche est finalisée, à jour avec `develop`, et prête à être intégrée.**

---

### ÉTAPE 5️⃣ : Merger votre branche dans `develop`

**⚠️ ATTENTION : Cette étape modifie `develop` REMOTE, accessible à toute l'équipe. Assurez-vous que votre code fonctionne !**

```bash
# 1. Aller sur develop LOCAL
git checkout develop

# 2. ⚠️ CRUCIAL : Vérifier que develop LOCAL est à jour avec develop REMOTE
git pull origin develop
# ✅ develop LOCAL = develop REMOTE
# (Si quelqu'un a pushé entre-temps, vous récupérez ses changements)

# 3. Merger votre branche LOCAL dans develop LOCAL
git merge feature/ma-nouvelle-feature
# ✅ develop LOCAL contient maintenant :
#    - Tout ce qui était dans develop
#    - + Votre nouvelle feature

# 4. Vérifier que tout fonctionne
# - Lancer le build
# - Lancer les tests
# - Tester manuellement si nécessaire

# 5. Pousser develop LOCAL vers develop REMOTE
git push origin develop
# ✅ develop REMOTE = develop LOCAL (avec votre feature)
```

**✅ CHECKPOINT : Votre feature est maintenant dans `develop` REMOTE, accessible à toute l'équipe !**

**Optionnel : Supprimer votre branche de feature**

```bash
# Supprimer la branche LOCALE
git branch -d feature/ma-nouvelle-feature

# Supprimer la branche REMOTE
git push origin --delete feature/ma-nouvelle-feature
```

---

### ÉTAPE 6️⃣ : Merger `develop` dans `main` (Release en production)

**⚠️ CETTE ÉTAPE EST RÉSERVÉE AUX RELEASES. Ne pas le faire pour chaque feature !**

```bash
# 1. Aller sur develop LOCAL
git checkout develop

# 2. Mettre à jour develop LOCAL avec develop REMOTE
git pull origin develop
# ✅ develop LOCAL = develop REMOTE

# 3. ⚠️ IMPORTANT : Vérifier que tout est stable
# - Build réussi
# - Tests passent
# - QA validée
# - Aucun bug critique

# 4. Aller sur main LOCAL
git checkout main

# 5. Mettre à jour main LOCAL avec main REMOTE
git pull origin main
# ✅ main LOCAL = main REMOTE

# 6. Merger develop LOCAL dans main LOCAL
git merge develop
# ✅ main LOCAL contient maintenant tout develop LOCAL

# 7. Pousser main LOCAL vers main REMOTE
git push origin main
# ✅ main REMOTE = main LOCAL
# 🎉 PRODUCTION MISE À JOUR !

# 8. Créer un tag de version (FORTEMENT RECOMMANDÉ)
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin v1.0.0
# ✅ Tag créé sur le REMOTE
```

**✅ CHECKPOINT : La release est en production (`main`), avec un tag de version !**

---

## 📝 RÉSUMÉ ULTRA-RAPIDE : Les commandes essentielles

### Démarrage initial (une seule fois)

```bash
git clone <url-du-repo>
cd snaposaurus
git checkout develop
git pull origin develop
```

### Créer une nouvelle feature

```bash
git checkout develop
git pull origin develop
git checkout -b feature/ma-feature
```

### Travailler au quotidien

```bash
# Coder, puis :
git add .
git commit -m "Description"
git push origin feature/ma-feature
```

### Synchroniser avec develop (à faire RÉGULIÈREMENT !)

```bash
git checkout develop
git pull origin develop
git checkout feature/ma-feature
git merge develop
git push origin feature/ma-feature
```

### Finaliser et merger dans develop

```bash
# 1. S'assurer d'être à jour
git checkout develop
git pull origin develop
git checkout feature/ma-feature
git merge develop
git push origin feature/ma-feature

# 2. Merger dans develop
git checkout develop
git pull origin develop
git merge feature/ma-feature
git push origin develop
```

### Release vers main (rarement, pour production)

```bash
git checkout develop
git pull origin develop
# Tester !
git checkout main
git pull origin main
git merge develop
git push origin main
git tag -a v1.0.0 -m "Release v1.0.0"
git push origin v1.0.0
```

---

## ⚠️ Règles d'or et bonnes pratiques

### ✅ À FAIRE ABSOLUMENT

1. **TOUJOURS partir de `develop`** (jamais de `main`)
   ```bash
   git checkout develop
   git pull origin develop
   git checkout -b feature/nouvelle-feature
   ```

2. **TOUJOURS `git pull` avant de merger** → Synchroniser LOCAL avec REMOTE
   ```bash
   git pull origin develop  # Avant de merger develop dans votre branche
   ```

3. **TOUJOURS vérifier `git status`** avant toute opération importante
   - Savoir sur quelle branche vous êtes
   - Savoir si vous êtes à jour (ahead/behind)
   - Savoir si vous avez des modifications non commitées

4. **TOUJOURS avoir un working directory propre avant de merger**
   ```bash
   git status  # Doit afficher "working tree clean"
   # Sinon : git add . && git commit -m "..."
   ```

5. **Merger `develop` dans votre branche RÉGULIÈREMENT** (idéalement chaque jour)
   - Évite les gros conflits
   - Garde votre branche à jour

6. **Tester après chaque merge** avant de push

7. **Utiliser des messages de commit clairs et descriptifs**
   ```bash
   ✅ git commit -m "Add cinematic system with Timeline integration"
   ❌ git commit -m "update"
   ```

### ❌ À NE JAMAIS FAIRE

1. **❌ Ne JAMAIS merger `main` dans `develop`**
   - Le flux va uniquement de develop → main (jamais l'inverse)

2. **❌ Ne JAMAIS merger `main` dans une feature branch**
   - Les features se basent sur develop uniquement

3. **❌ Ne JAMAIS travailler directement sur `main`**
   - main = production, en lecture seule pour les développeurs

4. **❌ Ne JAMAIS pousser sur `develop` ou `main` sans tester**
   - Build doit réussir
   - Tests doivent passer

5. **❌ Ne JAMAIS faire `git pull` avec des modifications non commitées**
   - Risque de conflits compliqués
   - Toujours commiter avant de pull

6. **❌ Ne PAS accumuler des jours/semaines de travail sans merger develop**
   - Plus vous attendez, plus les conflits seront gros

7. **❌ Ne PAS confondre LOCAL et REMOTE**
   - `git merge` = travaille en LOCAL uniquement
   - `git push/pull` = synchronise avec REMOTE

---

## 🆘 Commandes utiles de diagnostic

```bash
# 🔍 Voir l'état actuel (branche, modifications, LOCAL vs REMOTE)
git status
# Indique :
# - Sur quelle branche vous êtes
# - Si votre branche est "ahead" (en avance) ou "behind" (en retard) vs REMOTE
# - Quels fichiers sont modifiés

# 📋 Voir toutes les branches (LOCALES et REMOTE)
git branch -a

# 📋 Voir uniquement les branches LOCALES
git branch

# 📋 Voir uniquement les branches REMOTE
git branch -r

# 🔄 Récupérer l'état du REMOTE sans modifier votre LOCAL
git fetch origin
# Très utile pour voir ce qui a changé sur le REMOTE avant de pull

# ❌ Annuler un merge en cours (si conflits trop compliqués)
git merge --abort
# Remet votre branche dans l'état d'avant le merge

# 📜 Voir l'historique avec un graphe visuel
git log --oneline --graph --all

# 🔍 Voir les différences entre deux branches
git diff develop..feature/ma-feature
# Montre ce qui est dans feature mais pas dans develop

# 🔍 Comparer votre branche LOCALE avec sa version REMOTE
git diff feature/ma-feature origin/feature/ma-feature
# Montre les différences entre votre LOCAL et le REMOTE

# 📜 Voir quels commits sont sur le REMOTE mais pas en LOCAL
git log HEAD..origin/feature/ma-feature
# Utile pour savoir ce que vous allez récupérer avec pull

# 📜 Voir quels commits sont en LOCAL mais pas sur le REMOTE
git log origin/feature/ma-feature..HEAD
# Utile pour savoir ce que vous allez envoyer avec push

# 🧹 Supprimer une branche LOCALE (après merge)
git branch -d feature/ma-feature

# 🧹 Supprimer une branche REMOTE (après merge)
git push origin --delete feature/ma-feature

# 🏷️ Lister tous les tags
git tag

# 🔍 Voir les détails d'un tag
git show v1.0.0
```

---

## 📊 Schéma du workflow

```
┌─────────────────────────────────────────────────────────┐
│  REMOTE (GitHub/GitLab)                                  │
│                                                          │
│  main ────────────────────────────────────────────────► │
│   ↑                                                      │
│   │ (merge develop → main pour release)                │
│   │                                                      │
│  develop ──────────────────────────────────────────────► │
│   ↑                                                      │
│   │ (merge feature → develop quand feature terminée)   │
│   │                                                      │
│  feature/cinematic ────────────────────────────────────► │
│  feature/qte ──────────────────────────────────────────► │
│                                                          │
└─────────────────────────────────────────────────────────┘
         ↕ git push / git pull
┌─────────────────────────────────────────────────────────┐
│  LOCAL (Votre ordinateur)                                │
│                                                          │
│  main ────────────────────────────────────────────────► │
│   ↑                                                      │
│   │                                                      │
│  develop ──────────────────────────────────────────────► │
│   ↑    ↓                                                 │
│   │    │ (merge develop → feature régulièrement)        │
│   │    │                                                 │
│  feature/cinematic ────────────────────────────────────► │
│      ↑ VOUS TRAVAILLEZ ICI                               │
│                                                          │
└─────────────────────────────────────────────────────────┘

Flux du code :
1. Vous créez feature/xxx depuis develop
2. Vous codez sur feature/xxx
3. Vous mergez develop dans feature/xxx régulièrement
4. Vous mergez feature/xxx dans develop quand terminé
5. Vous mergez develop dans main pour release
```

---

## 💡 Comprendre LOCAL vs REMOTE (CRUCIAL !)

### 🖥️ Qu'est-ce que LOCAL ?
- **Votre copie** du dépôt sur votre ordinateur
- Toutes les opérations `git commit`, `git merge`, `git checkout` travaillent en LOCAL
- Vous pouvez faire autant de modifications que vous voulez sans affecter le REMOTE
- **Personne d'autre ne voit vos changements LOCAL tant que vous n'avez pas fait `git push`**

### 🌐 Qu'est-ce que REMOTE ?
- Le dépôt **sur le serveur** (GitHub, GitLab, etc.)
- **Partagé avec toute l'équipe**
- Nécessite `git push` pour envoyer vos modifications LOCAL → REMOTE
- Nécessite `git pull` ou `git fetch` pour récupérer les modifications des autres REMOTE → LOCAL

### 🔑 Commandes clés et leurs directions

| Commande | Action | Direction | Impact |
|----------|--------|-----------|--------|
| `git fetch origin` | Récupère l'état du REMOTE **sans modifier** votre LOCAL | REMOTE → LOCAL (info seulement) | Aucun changement visible |
| `git pull origin <branch>` | Récupère ET applique les changements du REMOTE | REMOTE → LOCAL (+ merge auto) | Modifie votre branche LOCAL |
| `git push origin <branch>` | Envoie vos commits LOCAL vers le REMOTE | LOCAL → REMOTE | Modifie le REMOTE (visible par tous) |
| `git merge <branch>` | Fusionne une branche LOCAL dans votre branche LOCAL actuelle | LOCAL ↔ LOCAL | Modifie uniquement votre LOCAL |
| `git commit` | Enregistre vos changements en LOCAL | LOCAL seulement | Aucun impact sur le REMOTE |
| `git checkout` | Change de branche en LOCAL | LOCAL seulement | Aucun impact sur le REMOTE |

### 📝 Exemple complet avec commentaires

```bash
# 1. Vous êtes sur feature/cinematic (LOCAL)
git status
# → "On branch feature/cinematic"
# ✅ Vous savez où vous êtes

# 2. Récupérer l'info du REMOTE (sans rien changer)
git fetch origin
# ✅ Git connait maintenant l'état du REMOTE
# ⚠️ Votre LOCAL n'a PAS changé

# 3. Mettre à jour develop LOCAL avec develop REMOTE
git checkout develop
# ✅ Vous êtes maintenant sur develop LOCAL
git pull origin develop
# ✅ develop LOCAL = develop REMOTE
# (Vous avez maintenant les features que les autres ont mergées)

# 4. Retourner sur votre branche et merger develop LOCAL dedans
git checkout feature/cinematic
# ✅ Vous êtes de retour sur feature/cinematic LOCAL
git merge develop
# ✅ feature/cinematic LOCAL contient maintenant :
#    - Votre travail
#    - + Tout ce qui était dans develop LOCAL (= develop REMOTE)

# 5. Pousser votre branche LOCAL vers le REMOTE
git push origin feature/cinematic
# ✅ feature/cinematic REMOTE = feature/cinematic LOCAL
# ✅ Maintenant les autres peuvent voir votre branche à jour
```

---

## 🚨 Erreurs courantes et solutions

### ❌ Erreur 1 : Oublier de pull avant de merger

```bash
git checkout develop
git merge feature/cinematic  # ❌ develop LOCAL peut être obsolète !
# Résultat : develop LOCAL ne contient pas les derniers changements de l'équipe
```

**✅ Solution :**
```bash
git checkout develop
git pull origin develop       # ✅ develop LOCAL = develop REMOTE
git merge feature/cinematic   # ✅ Maintenant vous mergez avec develop à jour
```

---

### ❌ Erreur 2 : Pousser sans avoir mergé develop

```bash
# Votre branche est vieille, develop a évolué depuis 2 jours
git push origin feature/cinematic  # ❌ Peut créer des conflits plus tard
# Résultat : Votre branche ne contient pas les nouveautés de develop
```

**✅ Solution :**
```bash
git checkout develop
git pull origin develop
git checkout feature/cinematic
git merge develop              # ✅ Mettre à jour avec develop d'abord
git push origin feature/cinematic
```

---

### ❌ Erreur 3 : Confondre branches LOCAL et REMOTE

```bash
git merge origin/develop  # ❌ Merger avec la version REMOTE (qui peut être obsolète)
# Résultat : Vous mergez avec une version REMOTE qui n'est pas à jour dans votre LOCAL
```

**✅ Solution :**
```bash
git fetch origin          # Récupérer l'info du REMOTE
git checkout develop
git pull origin develop   # Mettre à jour develop LOCAL
git checkout feature/cinematic
git merge develop         # ✅ Merger avec develop LOCAL (à jour)
```

---

### ❌ Erreur 4 : Pull avec des modifications non commitées

```bash
# Vous avez modifié des fichiers mais pas committé
git pull origin develop  # ❌ Risque de conflits compliqués
```

**✅ Solution :**
```bash
git status  # Vérifier l'état
git add .
git commit -m "Description des changements"  # ✅ Commiter d'abord
git pull origin develop                      # ✅ Maintenant on peut pull
```

---

### ❌ Erreur 5 : Travailler sur main au lieu de develop

```bash
git checkout main
git checkout -b feature/nouvelle-feature  # ❌ Créé depuis main !
# Résultat : Votre feature ne part pas de develop, mais de la production
```

**✅ Solution :**
```bash
git checkout develop      # ✅ Aller sur develop
git pull origin develop   # ✅ Mettre à jour
git checkout -b feature/nouvelle-feature  # ✅ Créer depuis develop
```

---

## 📖 Glossaire rapide

| Terme | Signification |
|-------|---------------|
| **LOCAL** | Votre ordinateur, votre copie du repo |
| **REMOTE** | Le serveur (GitHub/GitLab), partagé avec l'équipe |
| **origin** | Nom par défaut du REMOTE (exemple : `origin/develop` = branche develop sur le REMOTE) |
| **HEAD** | Pointeur vers votre commit actuel |
| **ahead** | Vous avez des commits en LOCAL qui ne sont pas sur le REMOTE (besoin de `git push`) |
| **behind** | Le REMOTE a des commits que vous n'avez pas en LOCAL (besoin de `git pull`) |
| **working tree clean** | Aucune modification non commitée (tout est propre) |
| **merge** | Fusionner deux branches (intégrer les changements d'une branche dans une autre) |
| **conflict** | Git ne peut pas fusionner automatiquement, vous devez choisir quelle version garder |

---

**🦖 Happy coding!**
