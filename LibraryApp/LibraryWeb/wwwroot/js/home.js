document.addEventListener('DOMContentLoaded', () => {
    const rows = document.querySelectorAll(".book-row");
    const detailsDiv = document.getElementById("book-details");

    async function showBookDetails(key) {
        const response = await fetch(`https://localhost:7055/api/books/${key}`);
        const book = await response.json();

        // Создаём общий контейнер с двумя колонками
        let html = `
            <div style="display: flex; gap: 20px; align-items: flex-start;">
                <div style="width: 50%;">
                    <h3>${book.title}</h3>
                    <p><strong>Subtitle:</strong> ${book.subtitle ?? ''}</p>
                    <p><strong>Authors:</strong> ${book.authors.map(a => a.name).join(", ")}</p>
                    <p><strong>First Publish Date:</strong> ${book.firstPublishDate ?? ''}</p>
                    <p><strong>Description:</strong> ${book.description ?? ''}</p>
        `;

        if (book.subjects.length) {
            html += `<p><strong>Subjects:</strong> ${book.subjects.map(s => s.subject).join(", ")}</p>`;
        }

        html += `</div>`; // закрываем блок текста

        // Блок с изображениями
        html += `<div style="width: 50%; display: flex; flex-wrap: wrap; gap: 10px;">`;
        if (book.covers.length) {
            book.covers.forEach(c => {
                html += `<img src="/images/covers/${c.coverFile}.png" alt="Cover" style="max-width:100%; max-height:300px; object-fit:contain; border-radius:8px;" />`;
            });
        }
        html += `</div></div>`; // закрываем контейнер

        detailsDiv.innerHTML = html;
    }

    // Двойной клик по строке
    rows.forEach(row => {
        row.addEventListener("dblclick", () => {
            showBookDetails(row.dataset.key);
        });
    });

    // Поиск
    document.getElementById("search-button").addEventListener("click", () => {
        const titleFilter = document.getElementById("search-title").value.toLowerCase();
        const authorFilter = document.getElementById("search-author").value.toLowerCase();

        document.querySelectorAll("#books-table tbody tr").forEach(row => {
            const title = row.cells[0].innerText.toLowerCase();
            const authors = row.cells[1].innerText.toLowerCase();
            row.style.display = title.includes(titleFilter) && authors.includes(authorFilter) ? "" : "none";
        });
    });
});
