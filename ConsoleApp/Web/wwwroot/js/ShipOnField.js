class ShipOnField {
    constructor(name, size, orientation) {
        this.type = 'field_ship';
        this.name = name;
        this.size = size;
        this.orientation = orientation;
        
        this.x = -1;
        this.y = -1;
    }
    
    getType() {
        return this.type;
    }

    getName() {
        return this.name;
    }

    getSize() {
        return this.size;
    }

    getOrientation() {
        return this.orientation;
    }

    setX(x) {
        this.x = x;
    }

    getX() {
        return this.x;
    }

    setY(y) {
        this.y = y;
    }

    getY() {
        return this.y;
    }
}