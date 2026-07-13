# How to View ER Diagrams

This guide explains how to view the Entity Relationship Diagrams for the CodeWave project in different formats.

---

## Option 1: View Mermaid Diagrams (Recommended - Easiest)

### Method A: GitHub (Best Option)
1. **Push your code to GitHub** (if not already done)
2. **Open the `ER_DIAGRAMS.md` file** on GitHub
3. **GitHub automatically renders Mermaid diagrams** - you'll see beautiful visual diagrams!

### Method B: VS Code
1. **Install Mermaid Preview Extension**:
   - Open VS Code
   - Go to Extensions (Ctrl+Shift+X / Cmd+Shift+X)
   - Search for "Markdown Preview Mermaid Support"
   - Install the extension

2. **View the diagram**:
   - Open `ER_DIAGRAMS.md` in VS Code
   - Press `Ctrl+Shift+V` (Windows/Linux) or `Cmd+Shift+V` (Mac) to open preview
   - The Mermaid diagrams will render automatically

### Method C: Online Mermaid Editor
1. Go to **https://mermaid.live/**
2. Copy the Mermaid code from `ER_DIAGRAMS.md` (the code between ```mermaid and ```)
3. Paste it into the editor
4. View the rendered diagram

### Method D: Markdown Viewer Extensions
- **Markdown Preview Enhanced** (VS Code extension)
- **Markdown All in One** (VS Code extension)
- Both support Mermaid rendering

---

## Option 2: View PlantUML Diagram

### Method A: VS Code with PlantUML Extension
1. **Install PlantUML Extension**:
   - Open VS Code
   - Go to Extensions
   - Search for "PlantUML"
   - Install "PlantUML" by jebbs

2. **View the diagram**:
   - Open `ER_DIAGRAM_PLANTUML.puml`
   - Press `Alt+D` (Windows/Linux) or `Option+D` (Mac) to preview
   - Or right-click and select "Preview PlantUML Diagram"

### Method B: Online PlantUML Server
1. Go to **http://www.plantuml.com/plantuml/uml/**
2. Open `ER_DIAGRAM_PLANTUML.puml` file
3. Copy all the content
4. Paste it into the online editor
5. The diagram will render automatically

### Method C: PlantUML Desktop Application
1. Download PlantUML from: **http://plantuml.com/download**
2. Install Java (required for PlantUML)
3. Open `ER_DIAGRAM_PLANTUML.puml` in the application
4. Export as PNG, SVG, or PDF

---

## Option 3: View DBML Diagram (Best Visual Quality)

### Method A: dbdiagram.io (Recommended)
1. Go to **https://dbdiagram.io/**
2. Click **"Create New Diagram"** or **"Import"**
3. Open `ER_DIAGRAM_DBML.txt` file
4. Copy all the content
5. Paste it into the dbdiagram.io editor
6. The diagram will render automatically with beautiful visuals
7. You can:
   - Export as PNG, PDF, or Postgres/SQL
   - Share the diagram
   - Edit and customize the layout

### Method B: VS Code with DBML Extension
1. **Install DBML Extension**:
   - Open VS Code
   - Go to Extensions
   - Search for "DBML"
   - Install "DBML" extension

2. **View the diagram**:
   - Open `ER_DIAGRAM_DBML.txt`
   - The extension will provide syntax highlighting
   - Use the extension's preview feature if available

---

## Quick Start (Fastest Method)

### For Immediate Viewing:
1. **Go to https://dbdiagram.io/**
2. **Click "Create New Diagram"**
3. **Open `ER_DIAGRAM_DBML.txt`** from your project
4. **Copy all content** and paste into the editor
5. **View the beautiful ER diagram!**

### For GitHub Users:
1. **Push `ER_DIAGRAMS.md` to GitHub**
2. **Open the file on GitHub**
3. **Mermaid diagrams render automatically!**

---

## Comparison of Methods

| Method | Ease of Use | Visual Quality | Export Options |
|--------|-------------|----------------|----------------|
| **dbdiagram.io** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ | PNG, PDF, SQL |
| **GitHub (Mermaid)** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | Screenshot |
| **VS Code Mermaid** | ⭐⭐⭐⭐ | ⭐⭐⭐⭐ | Screenshot |
| **PlantUML Online** | ⭐⭐⭐ | ⭐⭐⭐⭐ | PNG, SVG, PDF |
| **VS Code PlantUML** | ⭐⭐⭐ | ⭐⭐⭐⭐ | PNG, SVG, PDF |

---

## Recommended Workflow

1. **For Quick Viewing**: Use **dbdiagram.io** with the DBML file
2. **For Documentation**: Use **GitHub** with the Mermaid file
3. **For Editing**: Use **VS Code** with appropriate extensions

---

## Troubleshooting

### Mermaid not rendering?
- Make sure you're using a Markdown viewer that supports Mermaid
- Check that the code is between ```mermaid and ``` markers
- Try the online editor at mermaid.live

### PlantUML not working?
- Ensure Java is installed (required for PlantUML)
- Check that the .puml file extension is correct
- Try the online PlantUML server

### DBML not rendering?
- Make sure you're using dbdiagram.io (most compatible)
- Check that the syntax is correct (comments start with //)
- Verify all table definitions are complete

---

## File Locations

All ER diagram files are in the project root:
- `ER_DIAGRAMS.md` - Main documentation with Mermaid diagrams
- `ER_DIAGRAM_PLANTUML.puml` - PlantUML format
- `ER_DIAGRAM_DBML.txt` - DBML format for dbdiagram.io

---

**Happy Diagram Viewing! 🎨**

