// Admin Panel JavaScript

document.addEventListener('DOMContentLoaded', function() {
    // Confirm delete actions
    const deleteForms = document.querySelectorAll('form[onsubmit*="confirm"]');
    deleteForms.forEach(form => {
        form.addEventListener('submit', function(e) {
            if (!confirm('Are you sure you want to delete this item?')) {
                e.preventDefault();
            }
        });
    });

    // Auto-refresh unread message count (every 30 seconds)
    if (window.location.pathname.includes('/Admin')) {
        setInterval(function() {
            fetch('/Admin/GetUnreadCount')
                .then(response => response.json())
                .then(data => {
                    // Update unread count in dashboard if exists
                    const unreadElement = document.querySelector('.dashboard-card .dashboard-content h3');
                    if (unreadElement && window.location.pathname === '/Admin') {
                        // Only update if on dashboard
                        const count = parseInt(data.count) || 0;
                        if (unreadElement.textContent !== count.toString()) {
                            unreadElement.textContent = count;
                        }
                    }
                })
                .catch(err => console.log('Error fetching unread count:', err));
        }, 30000);
    }

    // Form validation
    const adminForms = document.querySelectorAll('.admin-form');
    adminForms.forEach(form => {
        form.addEventListener('submit', function(e) {
            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }
            form.classList.add('was-validated');
        });
    });

    // Image preview for portfolio items
    const imageUrlInputs = document.querySelectorAll('input[type="url"][name="ImageUrl"]');
    imageUrlInputs.forEach(input => {
        const preview = document.createElement('img');
        preview.style.maxWidth = '200px';
        preview.style.maxHeight = '200px';
        preview.style.marginTop = '1rem';
        preview.style.borderRadius = '8px';
        preview.style.display = 'none';
        
        input.parentElement.appendChild(preview);

        input.addEventListener('blur', function() {
            if (this.value && this.validity.valid) {
                preview.src = this.value;
                preview.style.display = 'block';
                preview.onerror = function() {
                    preview.style.display = 'none';
                };
            } else {
                preview.style.display = 'none';
            }
        });
    });
});

