function confirmDelete(entity) {
    // Set the message to display in the modal
    document.getElementById('modalMessage').textContent = "Are you sure you want to delete: " + entity + "?";

    // Show the modal
    document.getElementById('deleteModal').style.display = "flex";

    // If user confirms, submit the form
    document.getElementById('confirmDeleteBtn').onclick = function () {
        document.getElementById('deleteForm_' + entity).submit();
    };

    // If user cancels, close the modal
    document.getElementById('cancelDeleteBtn').onclick = function () {
        document.getElementById('deleteModal').style.display = "none";
    };
}

// Close the modal if clicked outside
window.onclick = function (event) {
    if (event.target == document.getElementById('deleteModal')) {
        document.getElementById('deleteModal').style.display = "none";
    }
}
