// TODO: calculate canvas size by screen size
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
const CELL_SIZE = 50; // px

// plus one cell for numbers
let FIELD_WIDTH_CELLS = 10 + 1;
const FIELD_WIDTH = FIELD_WIDTH_CELLS * CELL_SIZE;

const ENEMY_FIELD_WIDTH_CELLS_START = FIELD_WIDTH_CELLS + 2;
const ENEMY_FIELD_WIDTH_CELLS_END = FIELD_WIDTH_CELLS * 2 + 2;

const ENEMY_FIELD_CELL_HELPER = FIELD_WIDTH_CELLS + 2;


// plus one cell for letters
let FIELD_HEIGHT_CELLS = 10 + 1;
const FIELD_HEIGHT = FIELD_HEIGHT_CELLS * CELL_SIZE;


// space for battleships representation
const CANVAS_WIDTH = FIELD_WIDTH_CELLS * CELL_SIZE * 2 + CELL_SIZE * 2; 
const CANVAS_HEIGHT = FIELD_HEIGHT;


const MENU_START_X = FIELD_WIDTH_CELLS * CELL_SIZE;
const MENU_WIDTH = CANVAS_WIDTH - FIELD_WIDTH_CELLS * CELL_SIZE;
const MENU_HEIGHT = FIELD_HEIGHT;
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
let shipsNames = ["Carrier", "Battleship", "Submarine", "Cruiser", "Patrol"];
let shipCount = [1, 1, 1, 1, 1];
let shipLengths = [5, 4, 3, 2, 1];
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
const ALPHABET = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
    'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
let CAN_TOUCH = false;
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
const ALL_MENU_ITEMS = { // add ship settings || check console
    'START MENU': ['NEW GAME', 'FAST GAME', 'LOAD GAME', 'SETTINGS'],
    'NEW GAME': ['PLAYER VS PLAYER', 'PLAYER VS AI', 'BACK'],
    // Fast Game => start random fast game
    // Load Game => load game
    'SETTINGS': ['PLAYERS NAMES', 'CREATE SHIP', 'SHIPS ARRANGEMENT', 'SHIPS COUNT', 'BATTLEFIELD SIZE', 'BACK'],
    // Player VS Player => start game
    // Player VS AI => start game
    // Players Names => prompt
    // Create ship => prompt
    'SHIPS ARRANGEMENT': ['CHANGE SHIPS ARRANGEMENT', 'BACK', 'MAIN MENU'],
    'SHIPS COUNT': [shipsNames, 'BACK', 'MAIN MENU'],
    'BATTLEFIELD SIZE': ['WIDTH', 'HEIGHT', 'BACK', 'MAIN MENU'],
    // Change ships arrangement => alert
    // shipsNames => prompt
    // Width => prompt
    // Height => prompt
};