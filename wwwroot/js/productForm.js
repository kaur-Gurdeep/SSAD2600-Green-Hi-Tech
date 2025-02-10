var selectedFiles = [];
// Handles the drop event, adds files to list of selected images
function dropHandler(e) {
    e.preventDefault();
    document.getElementById('drop_zone').style.backgroundColor = 'white';
    console.log("File(s) dropped.")
    if (e.dataTransfer.items) {
        [...e.dataTransfer.items].forEach((item, i) => {
            if (item.kind === "file") {
                const file = item.getAsFile();
                selectedFiles.push(file);
            }
        });
    } else {
        [...e.dataTransfer.files].forEach((file, i) => {
            selectedFiles.push(file);
            console.log(file.kind);
            console.log(file.type);
        });
    }
    selectedFiles.forEach((file) => {
        console.log(file.name);
    });
}
function dragEndHandler(e) {
    e.preventDefault();
    document.getElementById('drop_zone').style.backgroundColor = 'white';
}
function dragOverHandler(e) {
    document.getElementById('drop_zone').style.backgroundColor = 'pink';
    e.preventDefault();
}
function dragEnterHandler(e) {
    e.preventDefault();
    document.getElementById('drop_zone').style.backgroundColor = 'pink';
}
function dragLeaveHandler(e) {
    e.preventDefault();
    document.getElementById('drop_zone').style.backgroundColor = 'white';
}
function sendFilesToController(e) {
    e.preventDefault();

    const form = document.getElementById('input-form');
    const formData = new FormData(form);

    // Append each file to the FormData
    selectedFiles.forEach((file, i) => {
        formData.append('fileImages', file);
    });

    // Send the data to the server via fetch
    fetch(form.action, {
        method: 'POST',
        body: formData,
    })
        .then(response => {
            if (response.ok) {
                console.log("Form submitted successfully!");
                window.location.href = '/Product';
            } else {
                console.error('Failed to submit the form');
            }
        })
        .catch(error => console.error("Error:", error));
}
