document.addEventListener('DOMContentLoaded', () => {

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

    // Per-field validators (id -> validator)
    const validators = {
        Name: value => value.trim().length >= 2 || "Name is required",
        Phone: value => value.trim().length >= 2 || "Phone number is required",
        Email: value => emailRegex.test(value.trim()) || "Please enter a valid email address",
        NewsletterEmail: value => emailRegex.test(value.trim()) || "Please enter a valid email address",
        Question: value => value.trim().length > 0 || "Please enter a question",
        SelectedOption: value => value && value.trim().length > 0 || "You must select an option"
    };

    // Find (or create) a validation span that MVC/Unobtrusive expects.
    function getErrorSpan(input) {
        // Place the message span inside the closest field wrapper
        const wrapper = input.closest('.input-group, .select-wrapper') || input.parentElement;

        let span = wrapper.querySelector('span[data-valmsg-for]');
        if (!span) {
            span = document.createElement('span');
            span.setAttribute('data-valmsg-for', input.id);
            span.setAttribute('data-valmsg-replace', 'true');
            span.classList.add('field-validation-valid'); // startläge = "valid", dvs reserverar höjd men osynligt via CSS
            wrapper.appendChild(span);
        }
        return span;
    }

    function showError(input, message) {
        const span = getErrorSpan(input);
        span.textContent = message;
        span.classList.remove('field-validation-valid');
        span.classList.add('field-validation-error');

        // Visuell markering av fältet
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
});
