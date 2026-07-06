/*
 * CodeWave styled confirm dialog.
 * Replaces the native window.confirm() popup with an on-brand modal.
 *
 * Usage:
 *   const ok = await window.cwConfirm('Are you sure?', {
 *       title: 'Submit Quiz?',
 *       confirmText: 'Submit',
 *       cancelText: 'Cancel',
 *       variant: 'primary' // or 'danger'
 *   });
 *
 * Forms can also opt in declaratively without any extra JS:
 *   <form data-confirm="Delete this project?" data-confirm-variant="danger">
 * The form's normal submit is intercepted, the modal is shown, and the
 * form is submitted for real only if the user confirms.
 */
(function () {
    var overlay, dialog, iconWrap, titleEl, messageEl, cancelBtn, confirmBtn;
    var activeResolve = null;

    var ICONS = {
        primary:
            '<svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">' +
            '<path d="M9.09 9a3 3 0 0 1 5.83 1c0 2-3 2-3 4" stroke="#fff" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>' +
            '<circle cx="12" cy="17" r="0.9" fill="#fff"/>' +
            '</svg>',
        danger:
            '<svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">' +
            '<path d="M12 9v4" stroke="#fff" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>' +
            '<circle cx="12" cy="16.2" r="0.9" fill="#fff"/>' +
            '<path d="M10.29 3.86 1.82 18a1.5 1.5 0 0 0 1.29 2.25h17.78A1.5 1.5 0 0 0 22.18 18L13.71 3.86a1.5 1.5 0 0 0-2.58 0Z" stroke="#fff" stroke-width="1.6" stroke-linejoin="round"/>' +
            '</svg>'
    };

    function injectStyles() {
        if (document.getElementById('cw-confirm-styles')) return;
        var style = document.createElement('style');
        style.id = 'cw-confirm-styles';
        style.textContent =
            '.cw-confirm-overlay{position:fixed;inset:0;z-index:9999;display:flex;align-items:center;justify-content:center;' +
            'background:rgba(12,7,18,0.6);backdrop-filter:blur(6px);-webkit-backdrop-filter:blur(6px);' +
            'opacity:0;pointer-events:none;transition:opacity .2s ease;padding:16px;box-sizing:border-box;font-family:"Inter","Space Grotesk",system-ui,sans-serif;}' +
            '.cw-confirm-overlay.cw-open{opacity:1;pointer-events:auto;}' +
            '.cw-confirm-dialog{width:min(380px,100%);background:#ffffff;border:1px solid rgba(157,100,207,0.18);' +
            'border-radius:1.25rem;padding:28px 24px 24px;box-shadow:0 25px 70px -12px rgba(124,58,237,0.35),0 8px 24px rgba(0,0,0,0.18);' +
            'transform:scale(0.92) translateY(10px);opacity:0;transition:transform .25s cubic-bezier(0.16,1,0.3,1),opacity .2s ease;text-align:center;}' +
            '.cw-confirm-overlay.cw-open .cw-confirm-dialog{transform:scale(1) translateY(0);opacity:1;}' +
            '.cw-confirm-icon{width:56px;height:56px;border-radius:9999px;margin:0 auto 16px;display:flex;align-items:center;justify-content:center;' +
            'background:linear-gradient(135deg,#9d64cf,#7c3aed);box-shadow:0 8px 20px -4px rgba(157,100,207,0.6);}' +
            '.cw-confirm-icon.cw-danger{background:linear-gradient(135deg,#f87171,#dc2626);box-shadow:0 8px 20px -4px rgba(220,38,38,0.5);}' +
            '.cw-confirm-icon svg{width:26px;height:26px;}' +
            '.cw-confirm-title{font-size:1.125rem;font-weight:800;color:#151118;margin:0 0 8px;}' +
            '.cw-confirm-message{font-size:0.875rem;line-height:1.5;color:#776189;margin:0;}' +
            '.cw-confirm-actions{display:flex;gap:12px;margin-top:24px;}' +
            '.cw-confirm-btn{flex:1;padding:11px 16px;border-radius:0.75rem;font-size:0.875rem;font-weight:700;cursor:pointer;' +
            'border:1px solid transparent;transition:transform .12s ease,background .15s ease,opacity .15s ease;}' +
            '.cw-confirm-btn:active{transform:scale(0.96);}' +
            '.cw-confirm-cancel{background:transparent;border-color:#e5e7eb;color:#776189;}' +
            '.cw-confirm-cancel:hover{background:#f2f0f4;}' +
            '.cw-confirm-ok{color:#fff;background:linear-gradient(135deg,#9d64cf,#7c3aed);box-shadow:0 10px 24px -6px rgba(157,100,207,0.55);}' +
            '.cw-confirm-ok:hover{filter:brightness(1.08);}' +
            '.cw-confirm-ok.cw-danger{background:linear-gradient(135deg,#f87171,#dc2626);box-shadow:0 10px 24px -6px rgba(220,38,38,0.45);}' +
            '.cw-confirm-btn:focus-visible{outline:2px solid #9d64cf;outline-offset:2px;}' +
            '@media (prefers-color-scheme: dark){' +
            '.cw-confirm-dialog{background:#1e1427;border-color:rgba(157,100,207,0.25);}' +
            '.cw-confirm-title{color:#fff;}' +
            '.cw-confirm-message{color:#b3a4c2;}' +
            '.cw-confirm-cancel{border-color:rgba(255,255,255,0.1);color:#b3a4c2;}' +
            '.cw-confirm-cancel:hover{background:rgba(255,255,255,0.06);}' +
            '}' +
            'html.dark .cw-confirm-dialog{background:#1e1427;border-color:rgba(157,100,207,0.25);}' +
            'html.dark .cw-confirm-title{color:#fff;}' +
            'html.dark .cw-confirm-message{color:#b3a4c2;}' +
            'html.dark .cw-confirm-cancel{border-color:rgba(255,255,255,0.1);color:#b3a4c2;}' +
            'html.dark .cw-confirm-cancel:hover{background:rgba(255,255,255,0.06);}';
        document.head.appendChild(style);
    }

    function ensureDialog() {
        if (overlay) return;
        injectStyles();

        overlay = document.createElement('div');
        overlay.className = 'cw-confirm-overlay';
        overlay.innerHTML =
            '<div class="cw-confirm-dialog" role="alertdialog" aria-modal="true" aria-labelledby="cw-confirm-title" aria-describedby="cw-confirm-message">' +
            '<div class="cw-confirm-icon"></div>' +
            '<h3 class="cw-confirm-title" id="cw-confirm-title"></h3>' +
            '<p class="cw-confirm-message" id="cw-confirm-message"></p>' +
            '<div class="cw-confirm-actions">' +
            '<button type="button" class="cw-confirm-btn cw-confirm-cancel"></button>' +
            '<button type="button" class="cw-confirm-btn cw-confirm-ok"></button>' +
            '</div>' +
            '</div>';
        document.body.appendChild(overlay);

        dialog = overlay.querySelector('.cw-confirm-dialog');
        iconWrap = overlay.querySelector('.cw-confirm-icon');
        titleEl = overlay.querySelector('.cw-confirm-title');
        messageEl = overlay.querySelector('.cw-confirm-message');
        cancelBtn = overlay.querySelector('.cw-confirm-cancel');
        confirmBtn = overlay.querySelector('.cw-confirm-ok');

        overlay.addEventListener('mousedown', function (e) {
            if (e.target === overlay) settle(false);
        });
        cancelBtn.addEventListener('click', function () { settle(false); });
        confirmBtn.addEventListener('click', function () { settle(true); });
        document.addEventListener('keydown', function (e) {
            if (!overlay.classList.contains('cw-open')) return;
            if (e.key === 'Escape') settle(false);
        });
    }

    function settle(result) {
        if (!activeResolve) return;
        overlay.classList.remove('cw-open');
        document.body.style.overflow = '';
        var resolve = activeResolve;
        activeResolve = null;
        setTimeout(function () { resolve(result); }, 150);
    }

    window.cwConfirm = function (message, options) {
        options = options || {};
        ensureDialog();

        if (activeResolve) settle(false);

        var variant = options.variant === 'danger' ? 'danger' : 'primary';
        titleEl.textContent = options.title || 'Are you sure?';
        messageEl.textContent = message || 'Please confirm this action.';
        cancelBtn.textContent = options.cancelText || 'Cancel';
        confirmBtn.textContent = options.confirmText || 'Confirm';
        iconWrap.innerHTML = ICONS[variant];
        iconWrap.className = 'cw-confirm-icon' + (variant === 'danger' ? ' cw-danger' : '');
        confirmBtn.className = 'cw-confirm-btn cw-confirm-ok' + (variant === 'danger' ? ' cw-danger' : '');

        document.body.style.overflow = 'hidden';
        requestAnimationFrame(function () {
            overlay.classList.add('cw-open');
            confirmBtn.focus();
        });

        return new Promise(function (resolve) {
            activeResolve = resolve;
        });
    };

    // Declarative wiring: <form data-confirm="message" data-confirm-variant="danger" ...>
    document.addEventListener('submit', function (e) {
        var form = e.target;
        if (!(form instanceof HTMLFormElement)) return;
        if (!form.hasAttribute('data-confirm')) return;
        if (form.dataset.cwConfirmed === '1') return;

        e.preventDefault();
        window.cwConfirm(form.getAttribute('data-confirm'), {
            title: form.getAttribute('data-confirm-title'),
            confirmText: form.getAttribute('data-confirm-ok'),
            cancelText: form.getAttribute('data-confirm-cancel'),
            variant: form.getAttribute('data-confirm-variant')
        }).then(function (ok) {
            if (ok) {
                form.dataset.cwConfirmed = '1';
                if (form.requestSubmit) form.requestSubmit();
                else form.submit();
            }
        });
    }, true);
})();
