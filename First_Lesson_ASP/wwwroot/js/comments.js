document.addEventListener('DOMContentLoaded', function () {
    const commentForm = document.getElementById('comment-form');
    const formMessage = document.getElementById('form-message');

    if (commentForm) {
        commentForm.addEventListener('submit', function (e) {
            e.preventDefault(); // Цей рядок зупиняє перехід на білу сторінку з JSON

            const formData = new FormData(this);

            fetch(this.action, {
                method: 'POST',
                body: formData,
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            })
                .then(response => response.json())
                .then(data => {
                    formMessage.style.display = 'block';
                    formMessage.textContent = data.message;

                    if (data.success) {
                        formMessage.className = "mt-3 alert alert-success";
                        commentForm.reset(); // Очищаємо поля після успіху

                        // Можна додати логіку оновлення списку коментарів без перезавантаження
                    } else {
                        formMessage.className = "mt-3 alert alert-danger";
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    formMessage.style.display = 'block';
                    formMessage.textContent = "Сталася помилка при відправці.";
                    formMessage.className = "mt-3 alert alert-danger";
                });
        });
    }
});