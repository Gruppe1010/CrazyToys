
function incOrDecChosenAmount(stockAmount, inc) {
    var chosenAmount = document.getElementById('chosenAmount');
    var oldValue = chosenAmount.value;

    var newVal;

    if (inc) {
        if (oldValue < stockAmount) {
            newVal = parseFloat(oldValue) + 1;
        } else {
            newVal = oldValue;

            alert(`Der er kun ${oldValue} stk. tilbage af denne vare`);
        }

    } else {
        // Don't allow decrementing below zero
        if (oldValue > 0) {
            newVal = parseFloat(oldValue) - 1;
        } else {
            newVal = 0;
        }
    }
    chosenAmount.value = newVal;
}