/*eslint-env es6, browser*/    //Permet la syntaxe Ecmascript 6 et indique que le contexte est dans un navigateur
/*eslint-disable no-console*/  //Bloque les messages d'erreur au sujet de la console
/*globals Labyrinthe*/ //Permet l'utilisation des globales en question

/**
 * Classe qui represente une cellule de labyrinthe.
 * Une cellule possede 4 murs et elle peut contenir un objet.
 */
class Cellule {
    /**
     * Construit une instance de cellule.
     * @param {int} rangee - Position verticale du curseur (debute a 0)
     * @param {int} colonne - Position horizontale du curseur (debute a 0)
     * @param {string} objet - Objet contenu dans la cellule (ex. Cle)
     */
	constructor(rangee, colonne, objet) {
		this.rangee = rangee || 0;
		this.colonne = colonne || 0;
		this.murs = [1, 1, 1, 1];
		this.labyrinthe = null;
        this.objet = objet;
	}

	get nord() {
		return this.murs[0];
	}

	set nord(val) {
		this.murs[0] = val;
	}

	get est() {
		return this.murs[1];
	}

	set est(val) {
		this.murs[1] = val;
	}

	get sud() {
		return this.murs[2];
	}

	set sud(val) {
		this.murs[2] = val;
	}

	get ouest() {
		return this.murs[3];
	}

	set ouest(val) {
		this.murs[3] = val;
	}

	get libre() {
		return this.murs.join('') === '1111';
	}

	get binaire() {
		let resultat;
		resultat = this.murs.slice(0).reverse().reduce(function (b, m) {
			return b * 2 + m;
		}, 0, this);
		return 15 - resultat;	// Jusqu'a ce qu'on renomme les fichiers
	}

	get nbMurs() {
		let resultat;
		resultat = this.murs.reduce(function (b, m) {
			return b + m;
		}, 0, this);
		return resultat;
	}

	/**
	 * Retourne des infos pour faciliter le reperage lors du debogage
	 * @return   {string} - Message avec chemins et objet
	 */
    infoDebogage() {
		let resultat = "";
        resultat += "Nouvelle position: r"+this.rangee+", c"+this.colonne;
        resultat += " / Chemin(s) possible(s): "+this.ascii();
        //resultat += " / Mur de la cellule: "+this;
        if(this.objet!=null){resultat += " / Objet: "+this.objet.nom;}
        return resultat;
    }

	/**
	 * Retourne les murs quand on utilise la cellule sous forme de chaine (ex. console.log)
	 * @return   {string} - Les murs de la cellule
	 */
    toString() {
		return this.murs.toString();
	}

	/**
	 * Indique quel objet (ex. cle) sera contenu dans la cellule
	 * @param   {Objet} val - Un objet interactif du jeu
	 */
	changerObjet(val) {
		this.objet = val;
    console.log("this.objet="+this.objet);
	}

    /**
	 * Verifie si le personnage rencontre un objet
	 * @returns {boolean} - true si un objet est trouve
	 */
	verifierObjet() {
        if(this.objet!==null){
            if(this.objet.interagir()){
                //si retourne true, il faut eliminer l'objet:
                this.changerObjet(null);
            }
            return true; //il y a eu un objet
        }
        return false; //il n'y a pas eu d'objet
	}

	/**
	 * Retourne ou modifie l'ouverture de la cellule dans une certaine direction
	 * @param   {*}   direction - La direction à traiter
	 * @param   {number}  etat      - Ouvrir ou fermer (0 ou 1)
	 * @returns {Cellule} - this
	 */
	ouverture(direction, etat) {
		direction = Labyrinthe.parseDirection(direction);
		if (etat === undefined) {
			return this.murs[direction];
		}
		this.murs[direction] = etat;
		return this;
	}

	/**
	 * Retourne la cellule voisine selon la position et la direction
	 * @param   {number}  direction  - L'orientation voulue
	 * @returns {Cellule} - La cellule voisine (false si elle n'existe pas)
	 */
	voisine(direction) {
		direction = Labyrinthe.parseDirection(direction);
		switch (direction) {
			case 0:
				return this.labyrinthe.cellule(this.rangee-1, this.colonne);
			case 1:
				return this.labyrinthe.cellule(this.rangee, this.colonne+1);
			case 2:
				return this.labyrinthe.cellule(this.rangee+1, this.colonne);
			case 3:
				return this.labyrinthe.cellule(this.rangee, this.colonne-1);
		}
		return false;
	}

	/**
	 * Ouvre un cote de la cellule, selon la direction
	 * @param   {number}  direction  - L'orientation voulue
	 * @returns {Cellule} - this
	 */
	ouvrir(direction) {
		return this.ouverture(direction, 0);
	}

	/**
	 * Ferme un cote de la cellule, selon la direction
	 * @param   {number}  direction  - L'orientation voulue
	 * @returns {Cellule} - this
	 */
	fermer(direction) { //inutilisee pour le moment
		return this.ouverture(direction, 1);
	}

    /**
	 * Genere le contenu visuel de la cellule
	 * @returns {HTMLElement} - le div et son contenu
     */
	visuelCellule() {
		let resultat;
		let posbg = [
			[0,0],[3,0],[0,1],[3,1],
			[1,0],[2,0],[1,1],[2,1],
			[0,3],[3,3],[0,2],[3,2],
			[1,3],[2,3],[1,2],[2,2],
		];
		let bin = this.binaire;
		resultat = document.createElement("div");
		resultat.style.backgroundPosition = (-posbg[bin][1]) + "em " + (-posbg[bin][0]) + "em ";
        if(this.objet){ //si il y a un objet
            console.log("Objet:"+this.objet+", Rangee:"+this.rangee+", Colonne:"+this.colonne);
            resultat.appendChild(this.objet.visuel);
        }
		return resultat;
	}

    /**
	 * Retourne un caractere ascii qui represente les directions possibles de la cellule
     * Cette fonction est utile pour le debogage seulement!
	 * @returns {string} - le caractere
     */
	ascii() {
		let resultat = "[configuration inconnue]";
        switch(this.toString()){ //selon la chaine qui represente les murs
            case '0,0,0,0': resultat = "┼"; break;
            case '0,1,1,1': resultat = "↑"; break;
            case '1,0,1,1': resultat = "→"; break;
            case '1,1,0,1': resultat = "↓"; break;
            case '1,1,1,0': resultat = "←"; break;
            case '0,1,0,1': resultat = "↕"; break;
            case '1,0,1,0': resultat = "↔"; break;
            case '0,1,0,0': resultat = "┤"; break;
            case '0,0,0,1': resultat = "├"; break;
            case '0,0,1,0': resultat = "┴"; break;
            case '1,0,0,0': resultat = "┬"; break;
            case '0,0,1,1': resultat = "└"; break;
            case '1,0,0,1': resultat = "┌"; break;
            case '1,1,0,0': resultat = "┐"; break;
            case '0,1,1,0': resultat = "┘"; break;
            case '1,1,1,1': resultat = " "; break;
        }
		return resultat;
	}

	static init() {
		this.taille = this.prototype.taille = 64;
	}
}
Cellule.init();
