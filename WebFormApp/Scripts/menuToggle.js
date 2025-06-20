﻿const toggleBtn = document.querySelector('.toggle_btn')
const toggleBtnIcon = document.querySelector('.toggle_btn i')
const dropDownMenu = document.querySelector('.dropdown_menu')

toggleBtn.onclick = function () {
    dropDownMenu.classList.toggle('open')
    const isOpen = dropDownMenu.classList.contains('open')
    if (isOpen) {
        toggleBtnIcon.classList = 'fa-solid fa-xmark'
    } else {
        toggleBtnIcon.classList = 'fa-solid fa-bars'
    }
}
