class CellState {
    constructor(x, y, state) {
        this.x = x;
        this.y = y;
        this.state = state;
    }
    
    getX() {
        return this.x;
    }
    
    getY() {
        return this.y;
    }
    
    getState() {
        return this.state;
    }
}