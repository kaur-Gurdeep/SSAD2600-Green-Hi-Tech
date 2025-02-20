var selectedFiles = [];
let deletedImageIds = "";

// Handles the drop event, adds files to list of selected images
function handleDrop(e) {
    e.preventDefault();
    document.getElementById('drop_zone').style.backgroundColor = 'white';
    console.log("File(s) dropped:")
    if (e.dataTransfer.items) {
        [...e.dataTransfer.items].forEach((item, i) => {
            if (item.kind === "file" && item.type.match("^image/")) {
                const file = item.getAsFile();
                selectedFiles.push(file);
                displayImage(file);
            }
        });
    }
    //selectedFiles.forEach((file) => {
    //    console.log(file.name);
    //});
    const imageCount = document.getElementsByClassName('thumbnail-item').length;
    const noImagesDiv = document.getElementById('no-images');
    if ((selectedFiles.length > 0 || imageCount > 0) && noImagesDiv) {
        document.getElementById('no-images').remove();
    //} else {
    //    document.getElementById('no-images').style.display = 'block';
    }
}
function handleDragEnd(e) {
    e.preventDefault();
    document.getElementById('drop_zone').style.backgroundColor = 'white';
}
function handleDragOver(e) {
    document.getElementById('drop_zone').style.backgroundColor = 'pink';
    e.preventDefault();
}
function handleDragEnter(e) {
    e.preventDefault();
    document.getElementById('drop_zone').style.backgroundColor = 'pink';
}
function handleDragLeave(e) {
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
function removeImage(imageId) {
    if (confirm("Are you sure you want to remove this image?")) {
        // Add the ID to deletedImageIds
        deletedImageIds += ',' + imageId;

        // Update the hidden input field
        document.getElementById('DeletedImageIds').value = deletedImageIds;

        // Remove the image preview from the DOM
        document.querySelector(`div[data-image-id='${imageId}']`).remove();

        const imageCount = document.getElementsByClassName('thumbnail-item').length;
        if (selectedFiles.length == 0 && imageCount == 0) {
            const noImagesDiv = document.createElement('div');
            noImagesDiv.id = 'no-images';
            noImagesDiv.innerHTML = 'No images';
            document.querySelector('.thumbnails').appendChild(noImagesDiv);
        }
    }
}
function displayImage(file) {
    const thumbnails = document.querySelector('.thumbnails');

    const fileReader = new FileReader();

    // this needs to run when an image loads
    fileReader.onload = function (e) {
        // create new image div container
        const newImage = document.createElement('div');
        newImage.classList.add('thumbnail-item');

        // create the img
        const img = document.createElement('img');
        img.src = e.target.result;
        img.style.maxWidth = '100px';
        img.style.maxHeight = '100px';

        // create the button
        const button = document.createElement('button');
        button.innerHTML = 'X';
        button.classList.add('delete-icon');
        button.onclick = function () {
            newImage.remove();
            selectedFiles = selectedFiles.filter(f => f !== file);
        };

        // append the img and button to the container
        newImage.appendChild(img);
        newImage.appendChild(button);

        // append the container to the thumbnails
        thumbnails.appendChild(newImage);
    };

    // read as url?? what does that do?
    fileReader.readAsDataURL(file);
}