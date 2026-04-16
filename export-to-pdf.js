/**
 * export-to-pdf.js
 * Converts FYP_COMPLETE_DOCUMENT.md → PDF with rendered Mermaid diagrams
 * Run: node export-to-pdf.js
 */

const fs = require('fs');
const path = require('path');
const { marked } = require('marked');
const puppeteer = require('puppeteer');

const INPUT_FILE  = 'FYP_COMPLETE_DOCUMENT.md';
const OUTPUT_PDF  = 'FYP_COMPLETE_DOCUMENT.pdf';
const OUTPUT_HTML = 'FYP_COMPLETE_DOCUMENT.html'; // bonus: also saves the HTML

// ── Read the markdown ──────────────────────────────────────────────────────────
const markdown = fs.readFileSync(path.join(__dirname, INPUT_FILE), 'utf8');

// Convert markdown → HTML (marked preserves ```mermaid blocks as <pre><code class="language-mermaid">)
const bodyHtml = marked.parse(markdown);

// ── Build a full HTML page with Mermaid.js from CDN ───────────────────────────
const fullHtml = `<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>CodeWave – FYP Complete Document</title>
  <style>
    /* ── Page and font ── */
    * { box-sizing: border-box; margin: 0; padding: 0; }
    body {
      font-family: "Segoe UI", Arial, sans-serif;
      font-size: 11pt;
      line-height: 1.65;
      color: #1a1a1a;
      background: #fff;
      padding: 0 40px;
      max-width: 900px;
      margin: 0 auto;
    }

    /* ── Headings ── */
    h1 { font-size: 22pt; margin: 36px 0 10px; color: #1a0050; border-bottom: 3px solid #7c3aed; padding-bottom: 6px; }
    h2 { font-size: 16pt; margin: 28px 0 8px; color: #2d0080; border-bottom: 1px solid #c4b5fd; padding-bottom: 4px; }
    h3 { font-size: 13pt; margin: 20px 0 6px; color: #4c1d95; }
    h4 { font-size: 11pt; margin: 14px 0 4px; color: #5b21b6; }

    /* ── Body text ── */
    p { margin: 8px 0; }
    ul, ol { margin: 8px 0 8px 24px; }
    li { margin: 3px 0; }

    /* ── Tables ── */
    table { border-collapse: collapse; width: 100%; margin: 12px 0; font-size: 10pt; }
    th { background: #7c3aed; color: #fff; padding: 6px 10px; text-align: left; }
    td { padding: 5px 10px; border-bottom: 1px solid #e5e7eb; }
    tr:nth-child(even) td { background: #f5f3ff; }

    /* ── Code blocks ── */
    pre { background: #1e1e2e; color: #cdd6f4; padding: 14px 16px; border-radius: 6px;
          font-family: "Consolas", monospace; font-size: 9pt; overflow: auto;
          margin: 10px 0; white-space: pre-wrap; }
    code { font-family: "Consolas", monospace; font-size: 9.5pt; background: #f3f0ff;
           padding: 1px 4px; border-radius: 3px; }
    pre code { background: none; padding: 0; color: inherit; }

    /* ── Mermaid containers ── */
    .mermaid {
      background: #fff;
      padding: 10px;
      margin: 14px 0;
      text-align: center;
      border: 1px solid #e5e7eb;
      border-radius: 6px;
      page-break-inside: avoid;
    }
    .mermaid svg { max-width: 100%; height: auto; }

    /* ── Horizontal rules ── */
    hr { border: none; border-top: 1px solid #d8b4fe; margin: 24px 0; }

    /* ── Blockquotes ── */
    blockquote { border-left: 4px solid #7c3aed; padding-left: 14px; color: #555; margin: 10px 0; }

    /* ── Print / PDF ── */
    @media print {
      body { padding: 0; max-width: 100%; }
      h1, h2, h3, h4 { page-break-after: avoid; }
      table, .mermaid, pre { page-break-inside: avoid; }
      @page { margin: 20mm 18mm; }
    }
  </style>
</head>
<body>
${bodyHtml}

<!-- Mermaid.js: renders mermaid code blocks automatically -->
<script type="module">
  import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.esm.min.mjs';
  mermaid.initialize({ startOnLoad: false, theme: 'default', securityLevel: 'loose' });

  // Find all <pre><code class="language-mermaid"> and replace with <div class="mermaid">
  document.querySelectorAll('pre code.language-mermaid').forEach(el => {
    const div = document.createElement('div');
    div.className = 'mermaid';
    div.textContent = el.textContent;
    el.parentElement.replaceWith(div);
  });

  await mermaid.run();
  document.title = '__MERMAID_DONE__';
</script>
</body>
</html>`;

// Save HTML file (optional, useful for quick browser preview too)
fs.writeFileSync(path.join(__dirname, OUTPUT_HTML), fullHtml, 'utf8');
console.log(`✅ HTML saved → ${OUTPUT_HTML}`);

// ── Launch puppeteer and export to PDF ────────────────────────────────────────
(async () => {
  console.log('🚀 Launching browser...');
  const browser = await puppeteer.launch({
    headless: 'new',
    args: ['--no-sandbox', '--disable-setuid-sandbox']
  });

  const page = await browser.newPage();

  // Allow CDN requests (Mermaid is loaded from jsdelivr)
  await page.setBypassCSP(true);

  // Load the HTML file
  const htmlPath = 'file:///' + path.join(__dirname, OUTPUT_HTML).replace(/\\/g, '/');
  await page.goto(htmlPath, { waitUntil: 'networkidle0', timeout: 60000 });

  // Wait for Mermaid to finish rendering (title changes to __MERMAID_DONE__)
  console.log('⏳ Waiting for Mermaid diagrams to render...');
  await page.waitForFunction(
    () => document.title === '__MERMAID_DONE__',
    { timeout: 60000 }
  ).catch(() => {
    console.warn('⚠️  Mermaid timeout — some diagrams may not render. Continuing anyway.');
  });

  // Extra wait to ensure all SVGs are fully painted
  await new Promise(r => setTimeout(r, 2000));

  // Export to PDF
  console.log('📄 Generating PDF...');
  await page.pdf({
    path: path.join(__dirname, OUTPUT_PDF),
    format: 'A4',
    margin: { top: '20mm', bottom: '20mm', left: '18mm', right: '18mm' },
    printBackground: true,
    displayHeaderFooter: true,
    headerTemplate: '<span style="font-size:8pt;color:#999;margin-left:18mm;">CodeWave – FYP Complete Document</span>',
    footerTemplate: '<span style="font-size:8pt;color:#999;margin:0 auto;"><span class="pageNumber"></span> / <span class="totalPages"></span></span>',
  });

  await browser.close();

  console.log(`\n✅ Done! PDF saved → ${OUTPUT_PDF}`);
  console.log(`📁 Find it at: ${path.join(__dirname, OUTPUT_PDF)}`);
})();
