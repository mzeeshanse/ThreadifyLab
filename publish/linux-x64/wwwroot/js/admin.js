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

    // Image preview for file uploads
    const imageFileInputs = document.querySelectorAll('input[type="file"][name="imageFile"]');
    imageFileInputs.forEach(input => {
        const previewContainer = input.parentElement.querySelector('#imagePreview');
        const previewImg = previewContainer ? previewContainer.querySelector('#previewImg') : null;
        
        if (!previewContainer || !previewImg) {
            return;
        }

        input.addEventListener('change', function(e) {
            const file = e.target.files[0];
            
            if (file) {
                // Validate file type
                const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/webp'];
                if (!allowedTypes.includes(file.type)) {
                    alert('Please select a valid image file (JPG, PNG, GIF, or WEBP)');
                    this.value = '';
                    previewContainer.style.display = 'none';
                    return;
                }

                // Validate file size (5MB)
                const maxSize = 5 * 1024 * 1024; // 5MB in bytes
                if (file.size > maxSize) {
                    alert('File size must be less than 5MB');
                    this.value = '';
                    previewContainer.style.display = 'none';
                    return;
                }

                // Create preview using FileReader
                const reader = new FileReader();
                reader.onload = function(e) {
                    previewImg.src = e.target.result;
                    previewContainer.style.display = 'block';
                };
                reader.onerror = function() {
                    previewContainer.style.display = 'none';
                };
                reader.readAsDataURL(file);
            } else {
                previewContainer.style.display = 'none';
            }
        });
    });
});

