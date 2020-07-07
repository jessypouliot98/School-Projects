/*eslint-env es6, browser*/    //Permet la syntaxe Ecmascript 6 et indique que le contexte est dans un navigateur
/*eslint-disable no-console*/  //Bloque les messages d'erreur au sujet de la console

/** 
 * Classe qui represente un sprite (personnage, objet)
 */
class Sprite { 
    /**
     * Construit une instance de sprite.
     * @param {string} url - Url de l'image utilisee pour afficher le sprite.
     */
	constructor(url) {
		this._x = 0;
		this._y = 0;
		this.url = url;
	}
    
	init(props) {
		this.setProperties(props);
		return this;
	}
    
	get x() {
		return this._x;
	}
    
	set x(val) {
		this._x = val;
		this.visuel.style.left = this.x + "em";
	}
    
	get y() {
		return this._y;
	}
    
	set y(val) {
		this._y = val;
		this.visuel.style.top = this.y + "em";
	}
    
	get visuel() {
		if (!this._visuel) {
			this.visuel = this.creerVisuel();
		}
		return this._visuel;
	}
    
	set visuel(val) {
		val.obj = this;
		this._visuel = val;
	}
    
    
    /**
	 * Permet d'acceder aux proprietes de l'instance 
	 * @returns {object} - Un objet de programmation contennant toutes les proprietes de l'instance
     */
    // AVERTISSEMENT: LA LOGIQUE DE LA FONCTION SUIVANTE NE DOIT PAS ETRE MODIFIEE!
	getProperties(props) { //a clarifier et a documenter
		let resultat;
		resultat = {};
		if (!props || typeof props !== "object") {
			props = Object.keys(this);
		} else if (!(props instanceof Array)) {
			props = Object.keys(props);
		}
		resultat = props.reduce((cumul, prop)=>(cumul[prop] = this.getProperty(prop)), {});
		return resultat;
	}
    
    /**
	 * Permet de modifier l'ensemble des proprietes de l'instance 
	 * @param {object} props - Un objet de programmation contennant toutes les proprietes a modifier
	 * @returns {Sprite} - L'instance du sprite actuel
     */
    // AVERTISSEMENT: LA LOGIQUE DE LA FONCTION SUIVANTE NE DOIT PAS ETRE MODIFIEE!
	setProperties(props) {
		let i;
		if (!props) {
			return this;
		}
		for (i in props) {
			this.setProperty(i, props[i]);
		}
		return this;
	}
    
	getProperty(prop) {
		return this[prop];
	}
    
	setProperty(prop, val) {
		this[prop] = val;
		return this;
	}
    
    /**
	 * Permet d'ajouter un ou plusieurs ecouteurs sur un objet
     * @param   {Objet} obj - L'objet auquel les ecouteurs seront ajoutes
	 * @param   {Objet}  evts - Un objet contenant des evenements et des fonctions associees 
	 * @returns {Sprite} - this
     */
	static addEventListeners(obj, evts) {
		let evt;
		for (evt in evts) {
			obj.addEventListener(evt, evts[evt]);
		}
		return this;
	}
     
    /**
	 * Permet d'eliminer un ou plusieurs ecouteurs sur un objet
     * @param   {Objet} obj - L'objet auquel les ecouteurs seront retires
	 * @param   {Objet}  evts - Un objet contenant des evenements et des fonctions associees 
	 * @returns {Sprite} - this
     */
	static removeEventListeners(obj, evts) {
		let evt;
		for (evt in evts) {
			obj.removeEventListener(evt, evts[evt]);
		}
		return this;
	}
    
    /**
	 * Genere le contenu visuel du sprite
	 * @returns {HTMLElement} - le div et son contenu
     */
	creerVisuel() {
		let resultat;
		resultat = document.createElement("div");
		resultat.classList.add("sprite");
		resultat.classList.add(this.classe);
		resultat.style.left = this.x + "em";
		resultat.style.top = this.y + "em";
		this.divImage = resultat.appendChild(document.createElement("div"));
		this.divImage.style.backgroundImage = "url("+this.url+")";
		return resultat;
	}
    
    /**
	 * Supprime le visuel du sprite
     */
    eliminer() {
        //console.log("ELIMINER le divImage "+this.divImage);
        this.divImage.parentNode.removeChild(this.divImage);
	}
    
	static init() {
		//rien a faire
	}
}
Sprite.init();
