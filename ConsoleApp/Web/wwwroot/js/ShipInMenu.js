class ShipInMenu {
    constructor(name, size, count) {
        this.type = 'menu_ship';
        this.name = name;
        this.size = size;
        this.count = count;
        this.saveFirstCount = count;
        this.orientation = 'horizontal';
        this.shadow = 'horizontal'
        
        this.x = -1;
        this.y = -1;
        this.choice = false;
        this.next = -1;
        this.previous = -1;
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
    
    resetCount() {
        this.count = this.saveFirstCount;
    }
    
    getCount() {
        return this.count;
    }
    
    plusCount() {
        this.count = this.count + 1;
    }
    
    minusCount() {
        this.count = this.count - 1;
    }
    
    changeShadow() {
        if (this.shadow === 'horizontal') {this.shadow = 'vertical';}
        else {this.shadow = 'horizontal';}
    }
    
    getShadow() {
        return this.shadow;
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
    
    isChosen() {
        return this.choice;
    }
    
    notChosen() {
        this.choice = false;
    }
    
    chosen() {
        this.choice = true;
    }
    
    setNext(ship) {
        this.next = ship;
    }
    
    getNext() {
        return this.next;
    }
    
    setPrevious(ship) {
        this.previous = ship;
    }
    
    getPrevious() {
        return this.previous;
    }
}