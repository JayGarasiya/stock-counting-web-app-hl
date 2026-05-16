document.addEventListener("DOMContentLoaded", function () {
    loadCalculator();
});

function loadCalculator() {
    const containers = document.querySelectorAll(".calculator-section .hl_countform_inner");

    containers.forEach(container => {

        // prevent duplicate load
        if (container.dataset.loaded === "true") return;
        container.dataset.loaded = "true";

        container.innerHTML = `
            <div class="calculator">
                <input type="text" class="calc-display" readonly />
                <div class="calc-buttons">
                    <button type="button">7</button>
                    <button type="button">8</button>
                    <button type="button">9</button>
                    <button type="button">/</button>

                    <button type="button">4</button>
                    <button type="button">5</button>
                    <button type="button">6</button>
                    <button type="button">*</button>

                    <button type="button">1</button>
                    <button type="button">2</button>
                    <button type="button">3</button>
                    <button type="button">-</button>

                    <button type="button">0</button>
                    <button type="button">.</button>
                    <button type="button">=</button>
                    <button type="button">+</button>

                    <button type="button" class="ce">CE</button>
                    <button type="button" class="clear">Reset</button>
                </div>
            </div>
        `;

        initCalculator(container);
    });
}

function initCalculator(container) {
    const display = container.querySelector(".calc-display");
    const buttons = container.querySelectorAll("button");

    // Button click support
    buttons.forEach(btn => {
        btn.addEventListener("click", () => {
            handleInput(btn.innerText, display);
        });
    });

    // Keyboard support
    enableKeyboard(display);
}

function handleInput(value, display) {
    const operators = ["+", "-", "*", "/"];
    let current = display.value;

    // RESET (clear all)
    if (value === "Reset") {
        display.value = "";
        return;
    }

    // CE (remove last char)
    if (value === "CE") {
        display.value = current.slice(0, -1);
        return;
    }

    // RESULT
    if (value === "=") {
        try {
            let cleaned = current.replace(/[\+\-\*\/]+$/, "");
            if (cleaned === "") {
                display.value = "";
                return;
            }

            display.value = eval(cleaned);
        } catch {
            display.value = "Error";
        }
        return;
    }

    // Prevent starting with operator
    if (operators.includes(value) && current === "") {
        return;
    }

    // Replace last operator
    if (operators.includes(value)) {
        if (operators.includes(current.slice(-1))) {
            display.value = current.slice(0, -1) + value;
        } else {
            display.value += value;
        }
        return;
    }

    // Prevent multiple dots in same number
    if (value === ".") {
        let parts = current.split(/[\+\-\*\/]/);
        let lastPart = parts[parts.length - 1];
        if (lastPart.includes(".")) return;
    }

    // Numbers
    display.value += value;
}

function enableKeyboard(display) {

    document.addEventListener("keydown", function (e) {

        const active = document.activeElement;

        // Ignore OTHER inputs
        if (active && (active.tagName === "INPUT" || active.tagName === "TEXTAREA") && !active.classList.contains("calc-display")) { return; }

        let key = e.key;

        // =
        if (key === "=") {
            e.preventDefault();
            handleInput("=", display);
            return;
        }

        // Numbers
        if (!isNaN(key)) {
            handleInput(key, display);
        }

        // Operators
        else if (["+", "-", "*", "/"].includes(key)) {
            handleInput(key, display);
        }

        // Dot
        else if (key === ".") {
            handleInput(".", display);
        }

        // Enter = equals
        else if (key === "Enter") {
            e.preventDefault();
            handleInput("=", display);
        }

        // Backspace = CE
        else if (key === "Backspace") {
            handleInput("CE", display);
        }

        // Escape = Reset
        else if (key === "Escape") {
            handleInput("Reset", display);
        }
    });
}

