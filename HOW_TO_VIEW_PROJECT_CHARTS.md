# How to View Project Planning Charts

This guide explains how to view the PERT Chart and Gantt Chart for the CodeWave Learning Platform project.

## Files Generated

1. **PROJECT_PLANNING.md** - Comprehensive project planning document with task breakdown table
2. **PERT_CHART.puml** - PERT Network diagram in PlantUML format
3. **GANTT_CHART.puml** - Gantt Chart in PlantUML format
4. **GANTT_CHART.md** - Text-based Gantt Chart visualization

---

## Viewing PlantUML Diagrams

### Option 1: Online PlantUML Server (Recommended)

1. Visit [PlantUML Online Server](http://www.plantuml.com/plantuml/uml/)
2. Copy the contents of `PERT_CHART.puml` or `GANTT_CHART.puml`
3. Paste into the online editor
4. The diagram will be rendered automatically

### Option 2: VS Code Extension

1. Install the "PlantUML" extension in VS Code
   - Open VS Code
   - Go to Extensions (Ctrl+Shift+X / Cmd+Shift+X)
   - Search for "PlantUML" by Jebbs
   - Install the extension

2. Install PlantUML Java (required)
   - Download Java JDK if not already installed
   - Install Graphviz (for diagram rendering)
     - **Windows**: Download from [Graphviz Download](https://graphviz.org/download/)
     - **macOS**: `brew install graphviz`
     - **Linux**: `sudo apt-get install graphviz` (Ubuntu/Debian)

3. View the diagrams
   - Open `PERT_CHART.puml` or `GANTT_CHART.puml` in VS Code
   - Press `Alt+D` (Windows/Linux) or `Option+D` (macOS) to preview
   - Or right-click the file and select "Preview PlantUML Diagram"

### Option 3: Command Line (PlantUML.jar)

1. Download PlantUML JAR file from [PlantUML Releases](https://github.com/plantuml/plantuml/releases)

2. Install Graphviz (see Option 2)

3. Generate diagrams:
   ```bash
   java -jar plantuml.jar PERT_CHART.puml
   java -jar plantuml.jar GANTT_CHART.puml
   ```

4. This will generate PNG images in the same directory

### Option 4: IntelliJ IDEA / JetBrains IDEs

1. Install the "PlantUML integration" plugin
   - File → Settings → Plugins
   - Search for "PlantUML integration"
   - Install and restart

2. Open the `.puml` files and view diagrams directly in the editor

---

## Viewing Text-Based Documents

### Markdown Files

1. **PROJECT_PLANNING.md** - View directly in any markdown viewer or VS Code
2. **GANTT_CHART.md** - View directly in any markdown viewer or VS Code

Most modern IDEs and text editors have built-in markdown preview:
- **VS Code**: Press `Ctrl+Shift+V` (Windows/Linux) or `Cmd+Shift+V` (macOS)
- **GitHub**: Upload to a repository and view directly
- **Online**: Use [Dillinger](https://dillinger.io/) or similar online markdown editor

---

## Diagram Descriptions

### PERT Chart (PERT_CHART.puml)

The PERT (Program Evaluation and Review Technique) Network diagram shows:
- All project tasks and their dependencies
- Task durations and scheduling information (ES, EF, LS, LF)
- Critical path identification (highlighted in red)
- Slack/margin for non-critical tasks

**Key Elements:**
- **Critical Path Tasks** are marked with `[CRITICAL]` and highlighted
- **Early Start (ES)**: Earliest possible start time
- **Early Finish (EF)**: Earliest possible completion time
- **Late Start (LS)**: Latest possible start time without delaying project
- **Late Finish (LF)**: Latest possible completion time
- **Slack**: Time a task can be delayed without affecting project completion

### Gantt Chart (GANTT_CHART.puml)

The Gantt Chart provides a timeline visualization showing:
- Task durations as horizontal bars
- Task dependencies and sequencing
- Parallel work streams
- Critical path highlighting

**Key Elements:**
- **Timeline**: Horizontal axis represents project duration (days)
- **Tasks**: Vertical axis lists all project tasks
- **Critical Path**: Tasks highlighted in red that must be completed on time
- **Dependencies**: Tasks positioned relative to their predecessors

### Task Scheduling Table (PROJECT_PLANNING.md)

The comprehensive table includes:
- Task IDs and descriptions
- Predecessor dependencies
- Duration estimates (in days)
- Early/Late start and finish times
- Critical path identification
- Slack calculations

---

## Quick Reference

| Chart Type | File | Best Viewing Method |
|------------|------|---------------------|
| PERT Network | `PERT_CHART.puml` | PlantUML Online Server or VS Code Extension |
| Gantt Chart | `GANTT_CHART.puml` | PlantUML Online Server or VS Code Extension |
| Task Table | `PROJECT_PLANNING.md` | Markdown viewer (VS Code, GitHub, etc.) |
| Text Gantt | `GANTT_CHART.md` | Markdown viewer |

---

## Project Summary

- **Total Tasks:** 45
- **Project Duration:** 73 days
- **Critical Path Tasks:** 17
- **Parallel Work Streams:** 7

The project follows a clean architecture approach with clear separation of concerns, allowing multiple features to be developed in parallel after the foundational infrastructure is complete.

---

## Troubleshooting

### PlantUML diagrams not rendering

1. **Java not installed**: PlantUML requires Java. Install JDK 8 or higher.
2. **Graphviz not installed**: Required for rendering. Install using package manager or download.
3. **Syntax errors**: Check PlantUML syntax. Use the online server for validation.

### Markdown tables not displaying correctly

1. Ensure your markdown viewer supports tables (most modern viewers do)
2. Try viewing on GitHub or using a dedicated markdown editor
3. Tables are formatted using standard Markdown table syntax

---

## Additional Resources

- [PlantUML Documentation](https://plantuml.com/)
- [PlantUML Language Reference](https://plantuml.com/guide)
- [Graphviz Documentation](https://graphviz.org/documentation/)

---

**Last Updated:** Based on CodeWave Learning Platform SRS v1.0

