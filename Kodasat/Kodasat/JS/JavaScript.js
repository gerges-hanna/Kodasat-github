//function myFunction() {
//   // document.getElementById('demo').innerHTML = Date();
//    history.replaceState("hello", null, "Confirm");
//}

//history.replaceState("hello", null, "Confirm");
function preventBack() {
    window.history.forward();
}
setTimeout("preventBack()", 0);
window.onunload = function () { null };