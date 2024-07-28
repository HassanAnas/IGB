function scrollToFirstError() {
    delay(500).then(() => {
        var element = document.getElementsByClassName("validation-message")[0];
        if (element != null) {
            var windowHeight = window.innerHeight;
            var elementHeight = element.offsetHeight;
            var offset = (windowHeight - elementHeight) / 2;
            element.parentNode.scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });
        }
    });
}

function delay(time) {
    return new Promise(resolve => setTimeout(resolve, time));
}
