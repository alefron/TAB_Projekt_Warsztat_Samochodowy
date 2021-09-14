let clientSelector = document.getElementById("clientSelector");
let inputCompany = document.getElementById("inputCompany");
let inputName = document.getElementById("inputName");
let inputSurname = document.getElementById("inputSurname");
function selectClient() {
   
    if (clientSelector.options[clientSelector.selectedIndex].value === "firma") {
       
        inputCompany.disabled = false;
        inputName.disabled = true;
        inputName.value = "";
        inputSurname.disabled = true;
        inputSurname.value = "";
    }
    else if (clientSelector.options[clientSelector.selectedIndex].value === "osoba prywatna") {
        inputCompany.disabled = true;
        inputName.disabled = false;
        inputSurname.disabled = false;
        inputCompany.value = "";
    }
    else {
        inputCompany.disabled = false;
        inputName.disabled = false;
        inputSurname.disabled = false;
    }
}


function initWindow() {
    selectClient();
}

window.onload = initWindow;