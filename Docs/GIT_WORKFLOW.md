# 🔄 Git Workflow - Snaposaurus

Guide de workflow Git pour la gestion des branches de développement.

---

## 📋 Structure des branches

- **`main`** : Branche de production, toujours stable
- **`develop`** : Branche de développement principale
- **`feature/xxx`** : Branches de fonctionnalités individuelles

---

## 🔄 Workflow : Merge `develop` dans votre branche de travail

### 1️⃣ Récupérer les dernières modifications

```bash
# Récupérer toutes les modifications du remote
git fetch origin

# S'assurer que develop local est à jour
git checkout develop
git pull origin develop
```

### 2️⃣ Retourner sur votre branche de travail

```bash
# Retourner sur votre branche (exemple: feature/ma-feature)
git checkout feature/ma-feature
```

### 3️⃣ Merger develop dans votre branche

```bash
# Merger develop dans votre branche actuelle
git merge develop
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

### 5️⃣ Pousser les modifications

```bash
git push origin feature/ma-feature
```

---

## ✅ Workflow : Merger votre branche dans `develop`

### 1️⃣ S'assurer que votre branche est à jour

```bash
# Suivre les étapes ci-dessus pour merger develop dans votre branche
git checkout feature/ma-feature
git merge develop
```

### 2️⃣ Pousser votre branche

```bash
git push origin feature/ma-feature
```

### 3️⃣ Basculer sur develop et merger

```bash
# Aller sur develop
git checkout develop

# Merger votre branche dans develop
git merge feature/ma-feature
```

### 4️⃣ Pousser develop

```bash
git push origin develop
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

- Toujours `git pull` avant de merger
- Commiter avant de merger (avoir un working directory propre)
- Tester après chaque merge
- Merger develop régulièrement dans votre branche pour éviter les gros conflits
- Utiliser des messages de commit clairs

### ❌ À ÉVITER

- Ne jamais merger main dans develop
- Ne jamais merger main dans une feature branch
- Ne pas pousser sur main sans tester
- Ne pas accumuler trop de commits avant de merger avec develop

---

## 🆘 Commandes utiles

```bash
# Voir l'état actuel
git status

# Voir les branches
git branch -a

# Annuler un merge en cours
git merge --abort

# Voir l'historique
git log --oneline --graph --all

# Voir les différences avant de merger
git diff develop..feature/ma-feature
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

**🦖 Happy coding!**
