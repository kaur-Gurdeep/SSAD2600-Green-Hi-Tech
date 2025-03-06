//Script for Edit Action Modal
// Show the success modal if there's a success message in TempData
document.addEventListener("DOMContentLoaded", function() {
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

// DELETE MODAL
function confirmDelete(roleName, roleId, formId) {
    // Set the message to display in the modal
    document.getElementById('modalMessage').textContent = "Are you sure you want to delete the role: " + roleName + "?";


// Write your JavaScript code.




