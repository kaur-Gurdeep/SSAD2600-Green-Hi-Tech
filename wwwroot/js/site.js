// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//Script for Edit Action Modal

// Show the success modal if there's a success message in TempData
document.addEventListener("DOMContentLoaded", function() {
    // Get the data from the hidden div
    var jsData = document.getElementById('jsData');
    if (jsData) {
        var successMessage = jsData.getAttribute('data-success-message');
        var redirectUrl = jsData.getAttribute('data-redirect-url');

        // Show the success modal if there's a success message
        if (successMessage) {
            var successModal = new bootstrap.Modal(document.getElementById('successModal'));
            successModal.show();

            // Redirect to Index page when modal is closed
            var successModalElement = document.getElementById('successModal');
            successModalElement.addEventListener('hidden.bs.modal', function () {
                window.location.href = redirectUrl; // Redirect to the Index page
            });
        }
    }
});

