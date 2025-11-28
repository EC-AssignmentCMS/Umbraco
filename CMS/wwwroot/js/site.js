document.addEventListener('DOMContentLoaded', () => {

    // ===== i18n / Language Handling =====
    let translations = {};
    const currentLang = getCookie('lang') || 'sv';
    
    // Load translations for client-side validation
    async function loadTranslations(lang) {
        try {
            const response = await fetch(`/i18n/${lang}.json`);
            if (response.ok) {
                translations = await response.json();
            }
        } catch (e) {
            console.warn('Could not load translations:', e);
        }
    }

    function getTranslation(key) {
        const keys = key.split('.');
        let value = translations;
        for (const k of keys) {
            if (value && typeof value === 'object' && k in value) {
                value = value[k];
            } else {
                return key;
            }
        }
        return typeof value === 'string' ? value : key;
    }

    function getCookie(name) {
        const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
        return match ? match[2] : null;
    }

    function setCookie(name, value, days) {
        const expires = new Date(Date.now() + days * 864e5).toUTCString();
        document.cookie = `${name}=${value}; expires=${expires}; path=/; SameSite=Lax`;
    }

    // Language selector
    const languageSelect = document.getElementById('language-select');
    if (languageSelect) {
        languageSelect.value = currentLang;
        languageSelect.addEventListener('change', (e) => {
            const newLang = e.target.value;
            setCookie('lang', newLang, 365);
            window.location.reload();
        });
    }

    // Load translations on page load
    loadTranslations(currentLang);

    // ===== Footer Copyright Highlighting =====
    document.querySelectorAll(".copyright-text").forEach(p => {
        const words = p.innerHTML.trim().split(" ");
        if (words.length > 1) {
            const lastWord = words.pop();
            p.innerHTML = `${words.join(" ")} <span class="highlight">${lastWord}</span>`;
        }
    });

    // ===== Scroll position across form submissions =====
    document.addEventListener("submit", function (e) {
        if (e.target.matches("form")) {
            sessionStorage.setItem("scrollY", window.scrollY);
        }
    });

    const scrollY = sessionStorage.getItem("scrollY");
    if (scrollY) {
        window.scrollTo({ top: parseInt(scrollY, 10), behavior: "instant" });
        sessionStorage.removeItem("scrollY");
    }

    // ===== Hybrid Form Validation (Blur + Submit) =====
    const questionForm = document.getElementById('questionForm');
    const newsletterForm = document.getElementById('newsletterForm');
    const callbackForm = document.getElementById('callbackForm');

    const emailRegex = /^(?!\.)[A-Za-z0-9._%+-]+(?<!\.)@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;

    // Per-field validators (id -> validator function returning true or error message)
    function getValidators() {
        return {
            Name: value => value.trim().length >= 2 || getTranslation('forms.validation.nameRequired'),
            Phone: value => value.trim().length >= 2 || getTranslation('forms.validation.phoneRequired'),
            Email: value => emailRegex.test(value.trim()) || getTranslation('forms.validation.emailInvalid'),
            NewsletterEmail: value => emailRegex.test(value.trim()) || getTranslation('forms.validation.emailInvalid'),
            Question: value => value.trim().length > 0 || getTranslation('forms.validation.questionRequired'),
            SelectedOption: value => value && value.trim().length > 0 || getTranslation('forms.validation.selectOption')
        };
    }

    // Find (or create) a validation span that MVC/Unobtrusive expects.
    function getErrorSpan(input) {
        // Place the message span inside the closest field wrapper
        const wrapper = input.closest('.input-group, .select-wrapper') || input.parentElement;

        let span = wrapper.querySelector('span[data-valmsg-for]');
        if (!span) {
            span = document.createElement('span');
            span.setAttribute('data-valmsg-for', input.id);
            span.setAttribute('data-valmsg-replace', 'true');
            span.classList.add('field-validation-valid');
            wrapper.appendChild(span);
        }
        return span;
    }

    function showError(input, message) {
        const span = getErrorSpan(input);
        span.textContent = message;
        span.classList.remove('field-validation-valid');
        span.classList.add('field-validation-error');

        input.style.borderColor = 'var(--color-error)';
        input.setAttribute('aria-invalid', 'true');
    }

    function clearError(input) {
        const span = getErrorSpan(input);
        span.textContent = '';
        span.classList.remove('field-validation-error');
        span.classList.add('field-validation-valid');

        input.style.borderColor = 'var(--border-color)';
        input.removeAttribute('aria-invalid');
    }

    function validateField(input) {
        const id = input.id;
        const value = input.value;
        const validators = getValidators();
        const validator = validators[id];
        if (!validator) return true;

        const result = validator(value);
        if (result !== true) {
            showError(input, result);
            return false;
        } else {
            clearError(input);
            return true;
        }
    }

    function wireForm(form) {
        if (!form) return;

        // Validate on blur for inputs, textareas, and selects
        form.querySelectorAll('input, textarea, select').forEach(input => {
            input.addEventListener('blur', () => validateField(input));
        });

        // Validate all on submit
        form.addEventListener('submit', e => {
            let valid = true;
            form.querySelectorAll('input, textarea, select').forEach(input => {
                if (!validateField(input)) valid = false;
            });
            if (!valid) e.preventDefault();
        });
    }

    [questionForm, newsletterForm, callbackForm].forEach(wireForm);

    // ===== Hamburger Menu =====
    const checkbox = document.getElementById("hamburger-toggle");
    const overlay = document.querySelector(".menu-overlay");
    const body = document.body;

    if (checkbox && overlay) {
        checkbox.addEventListener("change", () => {
            const active = checkbox.checked;
            body.classList.toggle("menu-open", active);
            overlay.hidden = !active;
        });

        overlay.addEventListener("click", () => {
            checkbox.checked = false;
            body.classList.remove("menu-open");
            overlay.hidden = true;
        });

        window.addEventListener("keydown", (e) => {
            if (e.key === "Escape" && checkbox.checked) {
                checkbox.checked = false;
                body.classList.remove("menu-open");
                overlay.hidden = true;
            }
        });
    }
});
