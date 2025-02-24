// DELETE MODAL
function confirmDelete(roleName, roleId, formId) {
    // Set the message to display in the modal
    document.getElementById('modalMessage').textContent = "Are you sure you want to delete the role: " + roleName + "?";

    // Show the modal
    document.getElementById('deleteModal').style.display = "flex";

    // Handle confirmation
    document.getElementById('confirmDeleteBtn').onclick = function () {
        // Submit the form for the corresponding roleId
        document.getElementById(formId).submit();
    };

    // Close the modal if the user clicks "No"
    document.getElementById('cancelDeleteBtn').onclick = function () {
        document.getElementById('deleteModal').style.display = "none";
    };
}

// Close the modal if clicked outside
window.onclick = function (event) {
    if (event.target == document.getElementById('deleteModal')) {
        document.getElementById('deleteModal').style.display = "none";
    }
};



//UPDATE MODAL