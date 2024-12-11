/* Thêm nút in bài viết */
document.addEventListener("DOMContentLoaded", function () {
    const printButton = document.createElement("button");
    printButton.textContent = "In bài viết";
    printButton.style.cssText = `
        position: fixed;
        bottom: 20px;
        right: 20px;
        background-color: #007bff;
        color: white;
        border: none;
        padding: 10px 15px;
        border-radius: 5px;
        cursor: pointer;
    `;

    printButton.addEventListener("click", function () {
        window.print();
});
document.body.appendChild(printButton);
});
