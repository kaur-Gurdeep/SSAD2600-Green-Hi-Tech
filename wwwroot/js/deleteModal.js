document.addEventListener('DOMContentLoaded', function () {

    function confirmDelete(entityName, entityId, formId) {

        //alert("1. entityName ==> " + JSON.stringify(entityName) + ", entityId" + JSON.stringify(entityId) + ", formId" + JSON.stringify(formId));

        let message = "";
        if (entityName === "role") {
            message = "Are you sure you want to delete the role: " + entityId + "?";
        } else if (entityName === "user") {
            message = "Are you sure you want to delete the user: " + entityId + "?";
        } else {
            message = "Are you sure you want to delete this item?";
        }

        // Set the message to display in the modal
        document.getElementById('modalMessage').textContent = message;

        // Show the modal
        document.getElementById('deleteModal').style.display = "flex";

        // Handle confirmation
        document.getElementById('confirmDeleteBtn').onclick = function () {
            // Submit the form for the corresponding entityId (either userId or roleId)
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
    }
});
