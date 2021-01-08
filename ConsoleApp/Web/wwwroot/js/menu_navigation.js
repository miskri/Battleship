
let gameStarted = false;
let menuRootCreated = false;
let twoPlayersGame = false;

let menuItemsHistory = [];
let currentItems = [];
let menuItemsSketch = [];

let customShip = null;

let [playerOneName, playerTwoName] = ['Player 1', 'Player 2'];

function menuNavigationScenario() {
    if (!menuRootCreated) {
        createMenuItems(ALL_MENU_ITEMS['START MENU']);
        menuRootCreated = true;
    }
    else {
        createMenuItems(menuItemsSketch);
        menuItemsSketch = [];
    }
    background(6, 161, 207);
    drawMenuItems();
    actionAfterChoice();
}

function createMenuItems(items) {
    items = items.flat();
    
    for (let i = 0; i < items.length; i++) {
        
        let menuItem = new MenuItem(items[i]);
        
        menuItem.setSubItems(ALL_MENU_ITEMS[menuItem.getTitle()])
        
        menuItem.setPosition(i);
        
        currentItems.push(menuItem);
    }
}

function drawMenuItems() {
    for (let i = 0; i < currentItems.length; i++) {
        drawMenuItem(currentItems[i]);
    }
}

function drawMenuItem(item) {
    stroke(255, 255, 255);
    noFill();
    rect(0, item.getPosition() * CELL_SIZE, CANVAS_WIDTH, CELL_SIZE);

    if (my === item.getPosition() && 0 <= mx && mx < CANVAS_WIDTH / CELL_SIZE) {
        fill(0, 0, 0, 50);
        noStroke();
        rect(0, item.getPosition()  * CELL_SIZE, CANVAS_WIDTH, CELL_SIZE);
    }

    fill(255, 255, 255);
    noStroke();
    text(item.getTitle(), CANVAS_WIDTH / 2, (item.getPosition() + 0.5) * CELL_SIZE);
}

function actionAfterChoice() {
    for (let i = 0; i < currentItems.length; i++) {
        if (cy === i && 0 <= cx && cx < CANVAS_WIDTH / CELL_SIZE) {
            let choice = currentItems[i].getTitle();
            
            if (choice === 'BACK') {
                choiceBack();
            }
            
            else if (choice === 'MAIN MENU') {
                choiceMainMenu();
            }
            
            else if (choice === 'FAST GAME') {
                
            }
            
            else if (choice === 'LOAD GAME') {
                
            }

            else if (choice === 'CREATE SHIP') {
                choiceCreateShip();
            }
            
            else if (choice === 'CHANGE SHIPS ARRANGEMENT') {
                choiceChangeShipsArrangement()
            }
            
            else if (choice === 'WIDTH') {
                choiceWidth();
            }
            
            else if (choice === 'HEIGHT') {
                choiceHeight();
            }
            
            else if (choice === 'PLAYER VS PLAYER' || choice === 'PLAYER VS AI') {
                startGame(choice);
            }
            
            else if (choice === 'PLAYERS NAMES') {
                choicePlayersNames();
            }
            
            else if (shipsNames.includes(choice)) {
                choiceShipName(choice)
            }
            
            else if (['NEW GAME', 'SETTINGS', 'SHIPS ARRANGEMENT', 'SHIPS COUNT', 'BATTLEFIELD SIZE'].includes(choice)) {
                menuItemsHistory.push(currentItems[i].getTitle());
                menuItemsSketch = ALL_MENU_ITEMS[currentItems[i].getTitle()];
                currentItems = [];
            }
            break;
        }
    }
    [cx, cy] = [-1, -1];
}

function choiceShipName(choice) {
    let newCount = '';
    let oldCount = shipCount[shipsNames.indexOf(choice)];
    
    while (newCount.length === 0) {
        newCount = prompt('CURRENT SHIP COUNT: ' + oldCount + '.\n' + 'ENTER NEW SHIP COUNT: ');

        if (newCount === null) {
            break;
        }
        if (!(!isNaN(parseFloat(newCount)) && !isNaN(newCount - 0))) {
            newCount = '';
        }
    }
    
    if (newCount !== null) {
        shipCount[shipsNames.indexOf(choice)] = parseInt(newCount);
    }
}

function choicePlayersNames() {
    playerOneName = prompt('ENTER NEW NAME FOR PLAYER 1', playerOneName);
    playerTwoName = prompt('ENTER NEW NAME FOR PLAYER 2', playerTwoName);
    
    if (playerOneName === null) {playerOneName = 'Player 1';}
    if (playerTwoName === null) {playerTwoName = 'Player 2';}
}

function choiceBack() {
    currentItems = [];
    if (menuItemsHistory.length < 2) {
        menuItemsHistory = [];
        menuItemsSketch = ALL_MENU_ITEMS['START MENU'];
    }
    else {
        menuItemsHistory.pop()
        menuItemsSketch = ALL_MENU_ITEMS[menuItemsHistory.reverse()[0]];
    }
}

function choiceMainMenu() {
    menuItemsHistory = [];
    currentItems = [];
    menuItemsSketch = ALL_MENU_ITEMS['START MENU'];
}

function choiceCreateShip() {
    let [shipLength, shipName] = [null, '']; // convert shipLength to the number
    
    while (shipLength === null || shipLength.length === 0) {
        shipLength = prompt('ENTER CUSTOM SHIP\'S LENGTH (1 <= LENGTH <= 10)');
        
        if (shipLength === null) {
            break;
        }
        
        if (!(!isNaN(parseFloat(shipLength)) && !isNaN(shipLength - 0))) {
            shipLength = '';
            continue;
        }
        if (shipLength < 1 || 10 < shipLength) {
            shipLength = '';
        }
    }
    
    if (shipLength !== null) {
        while (shipName.length === 0) {
            shipName = prompt('ENTER CUSTOM SHIP\'S NAME');

            if (20 < shipName.length) {
                shipName = '';
            }
        }
        
        customShip = [parseInt(shipLength), shipName];
        shipLengths.push(customShip[0]);
        shipsNames.push(customShip[1]);
        shipCount.push(1);
    }
}

function choiceWidth() {
    let width = '';
    
    while (width.length === 0) {
        width = prompt(  'CURRENT WIDTH: ' + (FIELD_WIDTH_CELLS - 1) + '.\n' + 'ENTER NEW BATTLEFIELD WIDTH. (5 <= WIDTH <= 26)');
        
        if (width === null) {
            break;
        }
        if (!(!isNaN(parseFloat(width)) && !isNaN(width - 0))) {
            width = '';
            continue;
        }
        if (width < 5 || 26 < width) {
            width = '';
        }
    }
    
    if (width !== null) {
        if (width.length > 0) {
            FIELD_WIDTH_CELLS = parseInt(width) + 1;
        }
    }
}

function choiceHeight() {
    let height = '';

    while (height.length === 0) {
        height = prompt(  'CURRENT HEIGHT: ' + (FIELD_HEIGHT_CELLS - 1) + '.\n' + 'ENTER NEW BATTLEFIELD HEIGHT. (5 <= WIDTH <= 26)');

        if (height === null) {
            break;
        }
        if (!(!isNaN(parseFloat(height)) && !isNaN(height - 0))) {
            height = '';
            continue;
        }
        if (height < 5 || 26 < height) {
            height = '';
        }
    }

    if (height !== null) {
        if (height.length > 0) {
            FIELD_HEIGHT_CELLS = parseInt(height) + 1;
        }
    }
}

function choiceChangeShipsArrangement() {
    CAN_TOUCH = !CAN_TOUCH;
    alert('SHIPS CAN TOUCH: ' + CAN_TOUCH);
}

function startGame(choice) {
    if (choice === 'PLAYER VS PLAYER') {
        twoPlayersGame = true
    }
    gameStarted = true;
}
