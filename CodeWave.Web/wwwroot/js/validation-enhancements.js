// Enhanced Client-Side Validation
// Provides additional validation beyond ASP.NET Core's built-in validation

(function () {
    'use strict';

    // Email validation regex
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    // Password strength validation
    function validatePasswordStrength(password) {
        const minLength = 6;
        const hasUpperCase = /[A-Z]/.test(password);
        const hasLowerCase = /[a-z]/.test(password);
        const hasNumber = /[0-9]/.test(password);
        const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(password);

        const strength = {
            score: 0,
            feedback: []
        };

        if (password.length >= minLength) strength.score++;
        else strength.feedback.push(`Password must be at least ${minLength} characters`);

        if (hasUpperCase) strength.score++;
        else strength.feedback.push('Add uppercase letters');

        if (hasLowerCase) strength.score++;
        else strength.feedback.push('Add lowercase letters');

        if (hasNumber) strength.score++;
        else strength.feedback.push('Add numbers');

        if (hasSpecialChar) strength.score++;
        else strength.feedback.push('Add special characters');

        return strength;
    }

    // Real-time email validation
    function setupEmailValidation() {
        const emailInputs = document.querySelectorAll('input[type="email"]');
        emailInputs.forEach(input => {
            input.addEventListener('blur', function () {
                const email = this.value.trim();
                if (email && !emailRegex.test(email)) {
                    this.setCustomValidity('Please enter a valid email address');
                    showFieldError(this, 'Please enter a valid email address');
                } else {
                    this.setCustomValidity('');
                    clearFieldError(this);
                }
            });
        });
    }

    // Real-time password validation
    function setupPasswordValidation() {
        const passwordInputs = document.querySelectorAll('input[type="password"][name*="Password"]:not([name*="Confirm"])');
        passwordInputs.forEach(input => {
            input.addEventListener('input', function () {
                const password = this.value;
                if (password) {
                    const strength = validatePasswordStrength(password);
                    showPasswordStrength(this, strength);
                } else {
                    hidePasswordStrength(this);
                }
            });
        });
    }

    // Password confirmation matching
    function setupPasswordConfirmation() {
        const passwordInputs = document.querySelectorAll('input[type="password"][name*="Password"]:not([name*="Confirm"])');
        const confirmInputs = document.querySelectorAll('input[type="password"][name*="Confirm"]');

        passwordInputs.forEach(passwordInput => {
            const form = passwordInput.closest('form');
            if (!form) return;

            const confirmInput = form.querySelector('input[type="password"][name*="Confirm"]');
            if (!confirmInput) return;

            [passwordInput, confirmInput].forEach(input => {
                input.addEventListener('input', function () {
                    if (passwordInput.value && confirmInput.value) {
                        if (passwordInput.value !== confirmInput.value) {
                            confirmInput.setCustomValidity('Passwords do not match');
                            showFieldError(confirmInput, 'Passwords do not match');
                        } else {
                            confirmInput.setCustomValidity('');
                            clearFieldError(confirmInput);
                        }
                    }
                });
            });
        });
    }

    // Show field error
    function showFieldError(input, message) {
        clearFieldError(input);
        const errorDiv = document.createElement('div');
        errorDiv.className = 'text-red-500 text-sm mt-1';
        errorDiv.textContent = message;
        input.parentElement.appendChild(errorDiv);
        input.classList.add('border-red-500');
    }

    // Clear field error
    function clearFieldError(input) {
        const errorDiv = input.parentElement.querySelector('.text-red-500');
        if (errorDiv) errorDiv.remove();
        input.classList.remove('border-red-500');
    }

    // Show password strength indicator
    function showPasswordStrength(input, strength) {
        let strengthIndicator = input.parentElement.querySelector('.password-strength');
        if (!strengthIndicator) {
            strengthIndicator = document.createElement('div');
            strengthIndicator.className = 'password-strength mt-2';
            input.parentElement.appendChild(strengthIndicator);
        }

        const strengthText = ['Very Weak', 'Weak', 'Fair', 'Good', 'Strong'];
        const strengthColors = ['bg-red-500', 'bg-orange-500', 'bg-yellow-500', 'bg-blue-500', 'bg-green-500'];

        strengthIndicator.innerHTML = `
            <div class="flex items-center gap-2">
                <div class="flex-1 bg-gray-200 rounded-full h-2">
                    <div class="${strengthColors[strength.score - 1] || 'bg-gray-500'} h-2 rounded-full transition-all" 
                         style="width: ${(strength.score / 5) * 100}%"></div>
                </div>
                <span class="text-sm ${strength.score >= 3 ? 'text-green-600' : 'text-red-600'}">
                    ${strengthText[strength.score - 1] || 'Very Weak'}
                </span>
            </div>
        `;
    }

    // Hide password strength indicator
    function hidePasswordStrength(input) {
        const strengthIndicator = input.parentElement.querySelector('.password-strength');
        if (strengthIndicator) strengthIndicator.remove();
    }

    // Initialize all validations
    function init() {
        setupEmailValidation();
        setupPasswordValidation();
        setupPasswordConfirmation();
    }

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
