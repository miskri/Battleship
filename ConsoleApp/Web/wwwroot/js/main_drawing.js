let [mx, my] = [-1, -1]
let [cx, cy] = [-1, -1];

function setup() {
    createCanvas(CANVAS_WIDTH, CANVAS_HEIGHT);
    textSize(CELL_SIZE - 10);
}

function draw() {
    if (!gameStarted) {
        textAlign(CENTER, CENTER);
        menuNavigationScenario();
    } else gameScenario()
}

function gameScenario() {
    if (twoPlayersGame) {
        if (!firstPlayerReady) {
            preparationsScenario();
        }
        else if (!secondPlayerReady) {
            preparationsScenario();
        }
        else {
            battleScenario();
        }
    } else {
        if (!firstPlayerReady) {
            preparationsScenario();
        } else {
            battleScenario();
        }
    }
}

function keyPressed() {
    if (gameStarted) {
        if (keyCode === 32) { // space bar keyCode
            getChosenShip().changeShadow();
        }
    }
}

function keyTyped() {
    if (gameStarted) {
        let chosenShip = getChosenShip();
        let index;

        if (key === 'w') {
            index = menuShips.indexOf(menuShips[chosenShip.getPrevious()]);
        }
        if (key === 's') {
            index = menuShips.indexOf(menuShips[chosenShip.getNext()]);
        }

        for (let i = 0; i < menuShips.length; i++) {
            if (key === 'w') {
                if (menuShips[index].getCount() > 0) {
                    chosenShip.notChosen();
                    menuShips[index].chosen();
                    break;
                } else {
                    index = menuShips[index].getPrevious();
                }
            } else if (key === 's') {
                if (menuShips[index].getCount() > 0) {
                    chosenShip.notChosen();
                    menuShips[index].chosen();
                    break;
                } else {
                    index = menuShips[index].getNext();
                }
            }
        }
    }
}

function mouseClicked() {
    [cx, cy] = [Math.floor(mouseX / CELL_SIZE), Math.floor(mouseY / CELL_SIZE)];
}

function mouseDragged() {
    [mx, my] = [Math.floor(mouseX / CELL_SIZE), Math.floor(mouseY / CELL_SIZE)];
}

function mouseMoved() {
    [mx, my] = [Math.floor(mouseX / CELL_SIZE), Math.floor(mouseY / CELL_SIZE)];
}

function offStrokeOffFill() {
    noStroke();
    noFill();
}