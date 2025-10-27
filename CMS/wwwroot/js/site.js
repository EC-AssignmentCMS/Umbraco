document.addEventListener('DOMContentLoaded', () => {


    //Footer Copyright Highlighting
    document.querySelectorAll(".copyright-text").forEach(p => {
        const words = p.innerHTML.trim().split(" ");
        if (words.length > 1) {
            const lastWord = words.pop();
            p.innerHTML = `${words.join(" ")} <span class="highlight">${lastWord}</span>`;
        }
    });
});