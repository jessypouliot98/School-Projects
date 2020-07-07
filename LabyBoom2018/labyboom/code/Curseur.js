/*eslint-env es6, browser*/    //Permet la syntaxe Ecmascript 6 et indique que le contexte est dans un navigateur
/*eslint-disable no-console*/  //Bloque les messages d'erreur au sujet de la console

/** 
 * Classe qui represente un curseur (une position virtuelle et une orientation)
 * Un curseur est utilise par la classe Labyrinthe lors de la creation d'un labyrinthe.
 * AVERTISSEMENT: LA LOGIQUE DE CETTE CLASSE NE DOIT PAS ETRE MODIFIEE!
 */
class Curseur {
    /**
     * Construit une instance de curseur.
     * @param {int} rangee - Position verticale du curseur (debute a 0)
     * @param {int} colonne - Position horizontale du curseur (debute a 0)
     * @param {int} dir - Direction (orientation) du curseur (0, 1, 2 ou 3)
     */
	constructor(rangee, colonne, dir) {
		this.rangee = rangee || 0;
		this.colonne = colonne || 0;
		this.dir = dir || 0;
		this._provenance = 0;
		this.tentatives = [1, 2, 3];
		//this.tentatives.sort(()=>(Math.random() < 0.5)); //ne fonctionne pas sur Safari et Edge
        this.tentatives.sort(function(){return 0.5 - Math.random();});
	}
    
	get dir() {
		return this._dir;
	}
    
	set dir(val) {
		val = val || 0;
		this._dir = ((val % 4) + 4) %4;
	}
    
	get provenance() {
		return this._provenance;
	}
    
	set provenance(val) {
		val = val || 0;
		this._provenance = ((val % 4) + 4) %4;
	}
    
	get dirInverse() {
		return (this.dir + 2) % 4;
	}
    
	/**
	 * Cree un nouveau curseur avec les memes proprietes
	 * @returns {Curseur} - le nouveau curseur
	 */
	clone() {
		return new Curseur(this.rangee, this.colonne, this.dir);
	}
    
	/**
	 * Permet une nouvelle tentative avec un nouveau curseur
	 * @returns {Curseur} - le nouveau curseur
	 */
	nouvelleTentative() {
		let resultat;
        //debugger;
		this.dir = this.provenance + this.tentatives.shift();
		resultat = this.clone().avancer();
		resultat.provenance = this.dirInverse;
		return resultat;
	}
    
	/**
	 * Deplace le curseur du nombre d'unites indique	 
     * @param   {number}  qte - Nombre entier
	 * @returns {Curseur} - this
	 */
	avancer(qte) {
		if (qte === undefined) {
			qte = 1;
		}
		this.rangee += this.tDelta.rangee[this.dir] * qte; // On calcule la nouvelle position du curseur
		this.colonne += this.tDelta.colonne[this.dir] * qte; // On calcule la nouvelle position du curseur
		return this;
	}
    
	static init() {
		this.prototype.tDelta = {
			rangee: [-1, 0, 1, 0], // Permet le déplacement du curseur selon la direction
			colonne: [0, 1, 0, -1] // Permet le déplacement du curseur selon la direction
		};
	}
}
Curseur.init();