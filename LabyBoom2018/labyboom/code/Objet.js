/*eslint-env es6, browser*/    //Permet la syntaxe Ecmascript 6 et indique que le contexte est dans un navigateur
/*eslint-disable no-console*/  //Bloque les messages d'erreur au sujet de la console
/*globals Sprite*/             //Permet l'utilisation des globales en question

/**
 * Classe qui represente un objet interactif (ex. cle).
 * @extends Sprite
 */
class Objet extends Sprite {
    /**
     * Construit une instance d'un objet.
     * @param {string} url - Url de l'image utilisee pour cet objet.
     */
	constructor(nomObjet, jeu) {
        let url;
        switch(nomObjet){
            case "cle": url = nomObjet+".png"; break;
						case "vie": url = nomObjet+".png"; break;// #tim Jessy, objet de vie
        }
		super("images/" + url); //appel du constructeur de Sprite
        this.jeu = jeu;
        this._nom = nomObjet;
	}

	init(props) {
		super.init(props); //appel de la fonction init de Sprite
		return this;
	}

	get cellule() {
		return this._cellule;
	}

	set cellule(val) {
		this._cellule = val;
		this.x = val.colonne * 2 + 0.5;
		this.y = val.rangee * 2 + 0.5;
	}

	get image() {
		return this.visuel.firstChild;
	}

    get nom() {
        return this._nom;
    }

    set nom(val) {
        this._nom = val;
    }

    /**
	 * Genere le contenu visuel de l'objet
	 * @returns {HTMLElement} - le div et son contenu
     */
	creerVisuel() {
		let resultat;
		resultat = super.creerVisuel(); //appel de la fonction creerVisuel de Sprite
		resultat.classList.add("objet"); //classe css a changer pour objet
		return resultat;
	}


    /**
	 * Execute l'interaction avec l'objet
     * @returns {boolean} - true si l'objet est ramasse ou consomme
	 */
	interagir() {
        let celluleEstVide = false;
        if(this.nom=="cle"){
            this.jeu.ui.afficherMessage("Vous avez trouvé la clé!");
            this.jeu.perso.inventaire.push(this);
						this.jeu.ui.afficherCle();
            this.jeu.ui.jouerSFX(this.nom);
            this.eliminer(); //appel de la methode du sprite
            celluleEstVide = true; //permet ensuite a la cellule de se vider
        }
				if(this.nom=="vie"){
            this.jeu.ui.afficherMessage("Vous avez trouvé une vie!");
						if(this.jeu.perso.vie < 3){// #tim Jessy, ne peux amaser plus de 3 vie supplementaire
							this.jeu.perso.vie++;
						}
						// console.log(this.jeu.perso.vie);
						this.jeu.ui.afficherVie(this.jeu.perso.vie);
            this.jeu.ui.jouerSFX(this.nom);
            this.eliminer(); //appel de la methode du sprite
            celluleEstVide = true; //permet ensuite a la cellule de se vider
        }
        return celluleEstVide;
    }


	static init() {
		//rien a faire
	}
}

Objet.init();
