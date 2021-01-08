
let menuShips = [];
let firstPlayerShips = [];
let secondPlayerShips = [];
let firstPlayerCellsStates = [];
let secondPlayerCellsStates = [];

let firstPlayerReady = false;
let secondPlayerReady = false;

let firstPlayerMove = true;
let readyToBattle = false;
let playerWaiting = true;

let menuShipsAreCreated = false;
let dataIsSend = false;

function preparationsScenario() {
    if (!menuShipsAreCreated) {createMenuShips();}
    if (!readyToBattle) {isReady();}
    background(6, 161, 207);
    drawLettersAndNumbers();
    drawShipsMenu();
    drawShipsOnField();
    drawGrid();
    drawChosenShipShadowOnField();
    addShipOnField();
}

function battleScenario() {
    background(6, 161, 207);
    if (playerWaiting) return void showPlayerMoveScreen();
    drawLettersAndNumbers();
    drawShipsOnField();
    drawGrid();
    drawFieldCellStates();
    drawEnemyCellStates();
    drawCursorOnEnemyField();
    addCellState();
}

function showPlayerMoveScreen() {
    let playerName = '';
    if (firstPlayerMove) {
        playerName = playerOneName;
    } else {
        playerName = playerTwoName;
    }

    textSize(2 * CELL_SIZE);
    if (0 <= mx && mx < (CANVAS_WIDTH / CELL_SIZE) && 0 <= my && my < (CANVAS_HEIGHT / CELL_SIZE)) {
        textSize(3 * CELL_SIZE);
    }
    
    fill(255, 255, 255);
    noStroke();
    text(playerName + ' TURN', CANVAS_WIDTH / 2, CANVAS_HEIGHT / 2 - CELL_SIZE / 2);
    textSize(CELL_SIZE - 10);

    if (0 <= cx && cx < (CANVAS_WIDTH / CELL_SIZE) && 0 <= cy && cy < (CANVAS_HEIGHT / CELL_SIZE)) {
        playerWaiting = false;
        [cx, cy] = [-1, -1];
    }
}

function  drawFieldCellStates() {
    let fieldCellStates = [];
    if (firstPlayerMove) {
        fieldCellStates = firstPlayerCellsStates;
    } else {
        fieldCellStates = secondPlayerCellsStates;
    }

    fieldCellStates.forEach(cell => {
        noStroke();
        if (cell.getState() === 'hit') {
            fill(255, 0, 0);
        }
        else if (cell.getState() === 'miss') {
            fill(119, 136, 153);
        }

        rect(cell.getX() * CELL_SIZE - ENEMY_FIELD_CELL_HELPER * CELL_SIZE, cell.getY() * CELL_SIZE, CELL_SIZE, CELL_SIZE);
    })
}

function checkShipHit(x, y) {
    let state = 'miss';
    let opponentsPlayerShips = [];
    if (firstPlayerMove) {
        opponentsPlayerShips = secondPlayerShips;
    }
    else {
        opponentsPlayerShips = firstPlayerShips;
    }
    
    for (let i = 0; i < opponentsPlayerShips.length; i++) {
        let ship = opponentsPlayerShips[i];
        let orientation = ship.getOrientation();
        let [shipStart, shipEnd, shipY, shipX] = [-1, -1, -1, -1];

        switch (orientation) {
            case 'horizontal':
                [shipStart, shipEnd, shipY] =
                    [ship.getX() / CELL_SIZE, ship.getX() / CELL_SIZE + ship.getSize(), ship.getY() / CELL_SIZE];

                shipStart += ENEMY_FIELD_CELL_HELPER;
                shipEnd += ENEMY_FIELD_CELL_HELPER;
                
                if (shipY === y && shipStart <= x && x < shipEnd) {
                    state = 'hit';
                } 

                break;
            case 'vertical':
                [shipStart, shipEnd, shipX] =
                    [ship.getY() / CELL_SIZE, ship.getY() / CELL_SIZE + ship.getSize(), ship.getX() / CELL_SIZE];

                shipX += ENEMY_FIELD_CELL_HELPER;

                if (shipX === x && shipStart <= y && y < shipEnd) {
                    state = 'hit';
                }

                break;
        }
    }
    
    return state;

}

function drawEnemyCellStates() {
    let opponentsCellStates = [];
    if (firstPlayerMove) {
        opponentsCellStates = secondPlayerCellsStates;
    } else {
        opponentsCellStates = firstPlayerCellsStates;
    }

    opponentsCellStates.forEach(cell => {
        noStroke();
        if (cell.getState() === 'hit') {
            fill(255, 0, 0);
        } 
        else if (cell.getState() === 'miss') {
            fill(119, 136, 153);
        }
        
        rect(cell.getX() * CELL_SIZE, cell.getY() * CELL_SIZE, CELL_SIZE, CELL_SIZE);
    })
}

function addCellState() {
    if (ENEMY_FIELD_WIDTH_CELLS_START < cx && cx < ENEMY_FIELD_WIDTH_CELLS_END && 0 < cy && cy < FIELD_HEIGHT_CELLS) {
        let state = checkShipHit(cx, cy);
        let cellState = new CellState(cx, cy, state);
        
        if (firstPlayerMove) {
            secondPlayerCellsStates.push(cellState); 
        } else {
            firstPlayerCellsStates.push(cellState)
        }
        
        playerWaiting = true;
        firstPlayerMove = !firstPlayerMove;
        
        [cx, cy] = [-1, -1];
    }
}

function drawCursorOnEnemyField() {
    if (ENEMY_FIELD_WIDTH_CELLS_START < mx && mx < ENEMY_FIELD_WIDTH_CELLS_END && 0 < my && my < FIELD_HEIGHT_CELLS) {
        noStroke();
        fill(38, 38, 38);
        rect(mx * CELL_SIZE, my * CELL_SIZE, CELL_SIZE, CELL_SIZE);
    }
}

function isReady() {
    if (menuShips.length !== 0) {
        let counter = 0;
        
        for (let i = 0; i < menuShips.length; i++) {
            if (menuShips[i].getCount() > 0) {
                counter++;
            }
        }
        
        if (counter === 0) {
            if (twoPlayersGame) {
                if (!firstPlayerReady) {
                    firstPlayerReady = true;
                    resetMenuShipsCount();
                } else {
                    secondPlayerReady = true;
                    readyToBattle = true;
                }
            } else {
                firstPlayerReady = true;
                readyToBattle = true;
            }
        }
    }
}

function resetMenuShipsCount() {
    menuShips.forEach(ship => {
        ship.resetCount();
    })
}

function createMenuShips() {
    for (let i = 0; i < shipsNames.length; i++) {
        let ship = new ShipInMenu(shipsNames[i], shipLengths[i], shipCount[i]);

        ship.setX(FIELD_WIDTH + CELL_SIZE * 2);
        ship.setY(i * CELL_SIZE);

        if (i === 0) {ship.chosen();}
        
        menuShips.push(ship);
    }

    setPreviousAndNext();
    
    menuShipsAreCreated = true;
}

function setPreviousAndNext() {
    menuShips[0].setPrevious(menuShips.length - 1);
    menuShips[0].setNext(1);
    menuShips[menuShips.length - 1].setPrevious(menuShips.length - 2);
    menuShips[menuShips.length - 1].setNext(0);

    for (let i = 1; i < menuShips.length - 1; i++) {
        menuShips[i].setPrevious(i - 1);
        menuShips[i].setNext(i + 1);
    }
}

function drawShipsOnField() {
    let currentPlayerShips = [];
    
    if (readyToBattle) {
        if (firstPlayerMove) {
            currentPlayerShips = firstPlayerShips;
        }
        else {
            currentPlayerShips = secondPlayerShips;
        }
    }
    else {
        if (!firstPlayerReady) {
            currentPlayerShips = firstPlayerShips;
        } else {
            currentPlayerShips = secondPlayerShips;
        }
    }
    
    if (currentPlayerShips.length > 0) {
        for (let i = 0; i < currentPlayerShips.length; i++) {
            drawShip(currentPlayerShips[i]);
        }
    }
}

function addShipOnField() {
    if (0 < cx && cx < FIELD_WIDTH_CELLS && 0 < cy && cy < FIELD_HEIGHT_CELLS) {
        let chosenShip = getChosenShip();

        if (checkIfShipCanBePlaced(chosenShip, cx, cy) && chosenShip.getCount() > 0) {
            chosenShip.minusCount();

            let orientation = chosenShip.getShadow();

            switch (orientation) {
                case 'horizontal':
                    let shipOnFieldHorizontal = new ShipOnField(chosenShip.getName(), chosenShip.getSize(), 'horizontal');

                    shipOnFieldHorizontal.setX(cx * CELL_SIZE);
                    shipOnFieldHorizontal.setY(cy * CELL_SIZE);

                    if (!firstPlayerReady) {
                        firstPlayerShips.push(shipOnFieldHorizontal);
                    }
                    else {
                        secondPlayerShips.push(shipOnFieldHorizontal);
                    }

                    break;
                case 'vertical':
                    let shipOnFieldVertical = new ShipOnField(chosenShip.getName(), chosenShip.getSize(), 'vertical');

                    shipOnFieldVertical.setX(cx * CELL_SIZE);
                    shipOnFieldVertical.setY(cy * CELL_SIZE);

                    if (firstPlayerReady) {
                        secondPlayerShips.push(shipOnFieldVertical);
                    }
                    else {
                        firstPlayerShips.push(shipOnFieldVertical);
                    }

                    break;
            }
        }

        [cx, cy] = [-1, -1];
    }
}

function checkIfShipCanBePlaced(chosenShip, x, y) {

    if (0 < x && x < FIELD_WIDTH_CELLS && 0 < y && y < FIELD_HEIGHT_CELLS) {
        let currentPlayerShips = firstPlayerShips;
        if (firstPlayerReady) {
            currentPlayerShips = secondPlayerShips;
        }
        
        if (((x + chosenShip.getSize()) > FIELD_WIDTH_CELLS) && (chosenShip.getShadow() === 'horizontal')) {
            return false;
        }

        else if (((y + chosenShip.getSize()) > FIELD_HEIGHT_CELLS) && (chosenShip.getShadow() === 'vertical')) {
            return false;
        }

        else if (currentPlayerShips.length !== 0) {

            for (let i = 0; i < currentPlayerShips.length; i++) {

                // horizontal & horizontal
                if (currentPlayerShips[i].getOrientation() === 'horizontal' && chosenShip.getShadow() === 'horizontal') {
                    let shipStart = currentPlayerShips[i].getX() / CELL_SIZE;
                    let shipEnd = shipStart + currentPlayerShips[i].getSize();
                    let shipY = currentPlayerShips[i].getY() / CELL_SIZE;

                    if (CAN_TOUCH) {
                        if (shipY === y) {
                            if (!(x >= shipEnd)) { 
                                if ((shipStart <= x && x < shipEnd) || (x + chosenShip.getSize()) > shipStart) {
                                    return false;
                                }
                            }
                        }
                    }
                    else {
                        if ((shipY - 1) <= y && y <= (shipY + 1)) {
                            if (!(x > shipEnd)) { 
                                if ((shipStart <= x && x < shipEnd) || (x + chosenShip.getSize()) > shipStart - 1) {
                                    return false;
                                }
                            }
                        }
                    }


                }

                // vertical & vertical
                else if (currentPlayerShips[i].getOrientation() === 'vertical' && chosenShip.getShadow() === 'vertical') {
                    let shipStart = currentPlayerShips[i].getY() / CELL_SIZE;
                    let shipEnd = shipStart + currentPlayerShips[i].getSize();
                    let shipX = currentPlayerShips[i].getX() / CELL_SIZE;

                    if (CAN_TOUCH) {
                        if (shipX === x) {
                            if (!(y >= shipEnd)) { 
                                if ((shipStart <= y && y < shipEnd) || (y + chosenShip.getSize()) > shipStart) {
                                    return false;
                                }
                            }
                        }
                    }
                    else {
                        if ((shipX - 1) <= x && x <= (shipX + 1)) {
                            if (!(y > shipEnd)) { 
                                if ((shipStart <= y && y < shipEnd) || (y + chosenShip.getSize()) >= shipStart) {
                                    return false;
                                }
                            }
                        }
                    }
                }

                // horizontal and vertical
                if (currentPlayerShips[i].getOrientation() === 'horizontal' && chosenShip.getShadow() === 'vertical') {
                    let shipStart = currentPlayerShips[i].getX() / CELL_SIZE;
                    let shipEnd = shipStart + currentPlayerShips[i].getSize();
                    let shipY = currentPlayerShips[i].getY() / CELL_SIZE;

                    if (CAN_TOUCH) {
                        if (shipStart <= x && x < shipEnd) {
                            if (((y + chosenShip.getSize()) > shipY) && (y <= shipY)) {
                                return false;
                            }
                        }
                    }
                    else {
                        if (shipStart - 1 <= x && x < shipEnd + 1) {
                            if ((((y + chosenShip.getSize()) > shipY - 1) && (y <= shipY)) || y === shipY + 1) {
                                return false;
                            }
                        }
                    }
                }

                // vertical and horizontal
                if (currentPlayerShips[i].getOrientation() === 'vertical' && chosenShip.getShadow() === 'horizontal') {
                    let shipStart = currentPlayerShips[i].getY() / CELL_SIZE;
                    let shipEnd = shipStart + currentPlayerShips[i].getSize();
                    let shipX = currentPlayerShips[i].getX() / CELL_SIZE;

                    if (CAN_TOUCH) {
                        if (shipStart <= y && y < shipEnd) {
                            if (((x + chosenShip.getSize()) > shipX) && (x <= shipX)) {
                                return false;
                            }
                        }
                    }
                    else {
                        if (shipStart - 1 <= y && y < shipEnd + 1) {
                            if (((x + chosenShip.getSize()) > shipX - 1) && (x <= shipX + 1)) {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        return true;
    }
}

function drawChosenShipShadowOnField() {
    if (0 < mx && mx < FIELD_WIDTH_CELLS && 0 < my && my < FIELD_HEIGHT_CELLS) {
        let chosenShip = getChosenShip();
        let size = chosenShip.getSize();
        let shadow = chosenShip.getShadow();
        fill(38, 38, 38);
        noStroke();

        switch (shadow) {
            case 'horizontal':
                if (!checkIfShipCanBePlaced(chosenShip, mx, my)) {
                    fill(255, 0, 0);
                    if ((mx + size) > FIELD_WIDTH_CELLS) {
                        size = FIELD_WIDTH_CELLS - mx;
                    }
                }

                rect(mx * CELL_SIZE, my * CELL_SIZE, CELL_SIZE * size, CELL_SIZE);
                break;
            case 'vertical':
                if (!checkIfShipCanBePlaced(chosenShip, mx, my)) {
                    fill(255, 0, 0);
                    if ((my + size) > FIELD_HEIGHT_CELLS) {
                        size = FIELD_HEIGHT_CELLS - my;
                    }
                }

                rect(mx * CELL_SIZE, my * CELL_SIZE, CELL_SIZE, CELL_SIZE * size);
                break;
        }
    }

    offStrokeOffFill();
}

function getChosenShip() {
    for (let i = 0; i < menuShips.length; i++) {
        if (menuShips[i].isChosen()) {
            return menuShips[i];
        }
    }
}

function drawGrid() {
    drawGridLines(0, FIELD_HEIGHT_CELLS + 1, FIELD_HEIGHT,'vertical');
    drawGridLines(0, FIELD_WIDTH_CELLS + 1, FIELD_WIDTH, 'horizontal');
    
    // second field
    if (readyToBattle) {
        drawGridLines(FIELD_HEIGHT_CELLS + 2, FIELD_HEIGHT_CELLS * 2 + 2, FIELD_HEIGHT, 'vertical');
        drawGridLines(0, FIELD_WIDTH_CELLS + 5, FIELD_WIDTH * 2 + 2 * CELL_SIZE, 'horizontal');
       
        // rectangle splits canvas for two fields
        fill(15, 82, 186); 
        noStroke();
        rect(FIELD_WIDTH, 0, 2 * CELL_SIZE, FIELD_HEIGHT);
    }
}

function drawGridLines(counterStart, counterEnd, endPoint, orientation) {
    stroke(255, 255, 255);
    noFill();

    for (let i = counterStart; i < counterEnd; i++) {
        let x = 0;
        let y = 0;

        switch (orientation) {
            case 'vertical':
                x = i * CELL_SIZE
                line(x, y, x, endPoint);
                break;

            case 'horizontal':
                y = i * CELL_SIZE;
                line(x, y, endPoint, y);
                break;
        }
    }
}

function drawLettersAndNumbers() {
    fill(0, 0, 0);
    noStroke();

    for (let c = 0; c < FIELD_WIDTH_CELLS - 1; c++) {
        let x = CELL_SIZE * 1.5 + CELL_SIZE * c;
        let y = CELL_SIZE - CELL_SIZE / 2.5;

        text(ALPHABET[c], x, y);
        
        // second field
        if (readyToBattle) {
            x += FIELD_WIDTH_CELLS * CELL_SIZE + CELL_SIZE * 2;
            text(ALPHABET[c], x, y);
        }   
    }
    
    for (let r = 0; r < FIELD_HEIGHT_CELLS; r++) {
        let x = CELL_SIZE - CELL_SIZE / 2;
        let y = CELL_SIZE / 1.7 + CELL_SIZE * r;

        if (r > 9) {
            x = CELL_SIZE - CELL_SIZE / 1.9;
        }

        if (r !== 0) {
            text(r, x, y);
        }

        // second field
        if (readyToBattle) {
            x += FIELD_WIDTH_CELLS * CELL_SIZE + CELL_SIZE * 2;
            if (r !== 0) {
                text(r, x, y);
            }
        }
    }
}

function drawShipsMenu() {
    fill(173, 216, 230);
    noStroke();
    rect(MENU_START_X, 0, MENU_WIDTH, MENU_HEIGHT);

    for (let i = 0; i < menuShips.length; i++) {
        let ship = menuShips[i];

        fill(38, 38, 38)
        if (ship.getCount() < 10) {
            text(ship.getCount(), FIELD_WIDTH + CELL_SIZE * 1.5, CELL_SIZE * (i + 0.8));
        }
        else {
            text(ship.getCount(), FIELD_WIDTH + CELL_SIZE, CELL_SIZE * (i + 0.8));
        }

        drawShip(ship);
    }
}

function drawShip(ship) {
    let [x, y, len, orientation] = [ship.getX(), ship.getY(), ship.getSize(), ship.getOrientation()];
    let padding = 5;

    fill(38, 38, 38);
    noStroke();

    if (ship.getType() === 'menu_ship') {
        if (ship.isChosen()) {
            padding = 0;
        }
        if (ship.getCount() === 0) {
            fill(255, 0, 0);
            padding = 5;
        }
    }

    switch (orientation) {
        case 'horizontal':
            rect(x + padding, y + padding, CELL_SIZE * len - 2 * padding, CELL_SIZE - 2 * padding);

            if (ship.getType() === 'menu_ship') {
                drawLinesOnTheShip(x, y, len, padding);
            }

            break;
        case 'vertical':
            rect(x + padding, y + padding, CELL_SIZE - 2 * padding, CELL_SIZE * len - 2 * padding);
            break;
    }
}

function drawLinesOnTheShip(x, y, len, padding) {
    stroke(255, 255, 255);
    noFill();

    for (let i = 0; i < len; i++) {
        if (i === 0) {
            line(x + padding + i * CELL_SIZE, y + padding, x + padding + i * CELL_SIZE, y + CELL_SIZE - padding);
        } else {
            line(x + i * CELL_SIZE, y + padding, x + i * CELL_SIZE, y + CELL_SIZE - padding);
        }
    }
}
