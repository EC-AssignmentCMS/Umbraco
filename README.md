# Inlämningsuppgift

I denna inlämningsuppgift ska du skapa en webbplats i **Umbraco**, baserad på en given designfil i **Figma**. Målet är att visa att du kan bygga en redaktörsvänlig och strukturerad webbplats med bland annat **Blocklist** och **Element Types**, samt att du förstår hur **navigering**, **formulär** och **globala inställningar** kan implementeras. 

## Design
Du får en Figma-design som visar de sidor du ska bygga i Umbraco.  
Designen behöver inte vara pixelperfekt, men den ska tydligt efterlikna originalet i layout, struktur och innehåll. Bilder kan exporteras direkt från Figma eller hämtas från exempelvis **pexels.com** eller **unsplash.com**.

Nedan följer kriterierna för respektive betygsnivå. Läs igenom noggrant så du vet vilka krav som gäller för **Godkänt (G)** respektive **Väl godkänt (VG)**.

---

## Gemensamma krav för både G och VG
- Webbplatsen ska vara **publicerad på Azure** med en fungerande databaslösning och vara tillgänglig vid rättning. 
- Det ska finnas en användare med fullständiga rättigheter:  
  **E‑post:** `hans@onatrix.com` &nbsp;&nbsp; **Lösenord:** `BytMig123!`

---

## Kriterier för Godkänt (G)
- Webbplatsen följer designfilens samtliga sidor, sektioner och innehållsstruktur, men behöver inte vara responsiv.
- **Block List** och **Element Types** används för att bygga upp sidorna.
- **Posters** hanteras som _child pages_ under relevant sida.
- **Formulär** finns visuellt enligt design, men behöver inte vara fungerande.
- **Dynamisk navigering** ska tillämpas och hämta sidor från innehållsträdet.
- **Site Settings** behöver inte användas, det är tillåtet att **hårdkoda** globala delar (till exempel logotyp, kontaktinformation, sociala länkar).
- **Paginering/Sliders** är frivilligt. 
- **Sökfunktion** i headern behöver inte vara fungerande.

---

## Kriterier för Väl godkänt (VG)
- Webbplatsen följer designfilens samtliga sidor, sektioner, innehållsstruktur **och är responsiv**.
- **Block List** och **Element Types** används för att bygga upp sidor, och **settings** tillämpas på lämpliga delar.
- **Posters** hanteras som _child pages_ under relevant sida och är kopplade till en **List view** för tydligare översikt i backoffice.
- **Formulär** fungerar, valideras, **submissions sparas i backoffice** och en **bekräftelse** skickas till den e‑post som angivits.
- **Dynamisk navigering** ska tillämpas och hämta sidor från innehållsträdet.
- **Site Settings** används för att hantera **globala värden** (till exempel logotyp, kontaktinformation, sociala länkar).
- **Compositions** används där det är lämpligt för att återanvända gemensamma fält mellan sidtyper (till exempel **SEO**, sidrubriker).
- **Paginering/Sliders** är frivilligt. 
- **Sökfunktion** i headern behöver inte vara fungerande.
- **Git** används på ett korrekt sätt: du arbetar i **brancher** och mergar till `main` först när en del är klar.
