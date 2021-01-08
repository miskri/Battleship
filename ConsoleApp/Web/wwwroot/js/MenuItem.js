class MenuItem {
    constructor(title) {
        this.title = title;
        this.subItems = null;
        this.position = null;
    }
    
    getTitle() {
        return this.title;
    }
    
    setSubItems(subItems) {
        this.subItems = subItems;
    }
    
    getSubItems() {
        return this.subItems;
    }
    
    setPosition(position) {
        this.position = position;
    }
    
    getPosition() {
        return this.position;
    }
}