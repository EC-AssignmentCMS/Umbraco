document.addEventListener('DOMContentLoaded', () => {

    //Footer Copyright Highlighting
    document.querySelectorAll(".copyright-text").forEach(p => {
        const words = p.innerHTML.trim().split(" ");
        if (words.length > 1) {
            const lastWord = words.pop();
            p.innerHTML = `${words.join(" ")} <span class="highlight">${lastWord}</span>`;
        }
    });

    //Scroll at form-submission
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

    const emailRegex = /^(?!\.)[A-Za-z0-9._%+-]+(?<!\.)@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;

    const validators = {
        Name: value => value.trim().length >= 2 || "Name is required",
        Email: value => emailRegex.test(value.trim()) || "Please enter a valid email address",
        NewsletterEmail: value => emailRegex.test(value.trim()) || "Please enter a valid email address",
        Question: value => value.trim().length > 0 || "Please enter a question"
    };

    function getErrorSpan(input) {
        let span = input.parentElement.querySelector('span[asp-validation-for]');
        if (!span) {
            // fallback: skapa ett nytt span om det saknas
            span = document.createElement('span');
            span.setAttribute('asp-validation-for', input.id);
            input.parentElement.appendChild(span);
        }
        return span;
    }

    function showError(input, message) {
        const span = getErrorSpan(input);
        span.textContent = message;
        span.classList.add('field-validation-error');
        input.style.borderColor = 'var(--color-error)';
    }

    function clearError(input) {
        const span = getErrorSpan(input);
        span.textContent = '';
        span.classList.remove('field-validation-error');
        input.style.borderColor = 'var(--border-color)';
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

    [questionForm, newsletterForm].forEach(form => {
        if (!form) return;

        form.querySelectorAll('input, textarea').forEach(input => {
            input.addEventListener('blur', () => validateField(input));
        });

        form.addEventListener('submit', e => {
            let valid = true;
            form.querySelectorAll('input, textarea').forEach(input => {
                if (!validateField(input)) valid = false;
            });
            if (!valid) e.preventDefault();
        });
    });
});
