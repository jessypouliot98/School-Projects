/*eslint-env es6, browser*/    //Permet la syntaxe Ecmascript 6 et indique que le contexte est dans un navigateur
/*eslint-disable no-console*/  //Bloque les messages d'erreur au sujet de la console
/*globals Cellule, Curseur, Objet*/   //Permet l'utilisation des globales en question

/**
 * Classe qui represente un labyrinthe avec ses cellules et ses murs.
 * AVERTISSEMENT: LA LOGIQUE DE LA CREATION DU LABYRINTHE NE DOIT PAS ETRE MODIFIEE!
 */
class Labyrinthe {
    /**
     * Construit une instance de labyrinthe.
     * @param {Jeu} jeu - L'instance du jeu en cours.
     */
	constructor(jeu) {
        console.info("Construction d'un nouveau labyrinthe"); //info
        this.jeu = jeu;
        let largeurDeBase = 2;
        let hauteurDeBase = 2;
		this.largeur = this.jeu.niveau + largeurDeBase;
		this.hauteur = this.jeu.niveau + hauteurDeBase;
		this.creer();
		this.ajouterSortie();
	}

	/**
	 * Retourne une grille de la dimension du labyrinthe
	 * dont toutes les cellules sont fermées
	 * @returns {Array} - Un tableau à 2 dimensions d'objets Cellule
	 */
	creerGrille() {
		let resultat, rangee, colonne;
		resultat = [];

		for (rangee = 0; rangee < this.hauteur; rangee++) {
			resultat[rangee] = [];
			for (colonne = 0; colonne < this.largeur; colonne++) {
				let cellule = new Cellule(rangee, colonne, null);
				cellule.labyrinthe = this;
				resultat[rangee][colonne] = cellule;
			}
		}
		return resultat;
	}

	/**
	 * Permet d'exécuter une fonction sur chaque cellule de la grille
     * (cette methode est utilisee pour la creation du visuel)
	 * @param   {function}  fct - La fonction a executer
	 * @param   {object}  thisArg - Le labyrinthe a utiliser
     * @returns {Array} - Un tableau avec le resultat de chaque appel
     * de fonction (il n'y a pas forcement de retour)
	 */
	parcourirGrille(fct, thisArg) {
		let resultat, hauteur, largeur;
		thisArg = thisArg || this;
		hauteur = this.hauteur;
		largeur = this.largeur;
		resultat = [];
		for (let rangee = 0; rangee < hauteur; rangee++) {
			for (let colonne = 0; colonne < largeur; colonne++) {
				resultat.push(fct.call(thisArg, this.cellule(rangee, colonne), rangee, colonne));
			}
		}
		return resultat;
	}

	/**
	 * Retourne une certaine cellule en fonction de la rangee et de la colonne données
	 * @param   {number}  rangee  - La rangee à regarder. On peut également donner un Curseur
	 * @param   {number}  colonne - La colonne à regarder
	 * @returns {boolean} - L'objet Cellule trouvée ou false
	 */
	cellule(rangee, colonne) {
		if (arguments[0] instanceof Curseur) {
            //le parametre rangee a ete utilise pour transmettre un curseur
			return this.cellule(arguments[0].rangee, arguments[0].colonne);
		}
		if (rangee < 0 || rangee >= this.hauteur || colonne < 0 || colonne >= this.largeur) {
            //la cellule demandee est hors de la grille, elle n'existe pas
			return false;
		}
		return this.grille[rangee][colonne];
	}

	/**
	 * Fabrique une grille de labyrinthe avec ses murs
	 * @returns {Labyrinthe} - this
	 */
    creer() {
		let curseur, trajet, nCurseur;

		// On fabrique un labyrinthe vierge rempli de cellule intactes
		this.grille = this.creerGrille();

		curseur = new Curseur( // On démarre le curseur au centre:
			Math.floor(this.hauteur / 2), Math.floor(this.largeur / 2)
		);

		curseur.provenance = this.hasard(4); // On determine une direction au hasard. 0=nord; 1=est; 2=sud; 3=ouest
		trajet = [curseur]; // Permet de revenir en arriere lorsque le curseur est bloqué
		while (trajet.length > 0) {
            //console.log("position du curseur = "+curseur.colonne, curseur.rangee);
            //console.log("provenance = "+["nord","est","sud","ouest"][curseur.provenance]);
			nCurseur = this.trouverOuverture(curseur);
			if (nCurseur) {
				trajet.push(nCurseur); // On garde en memoire le curseur pour le retour
				curseur = nCurseur;
			} else {
				curseur = trajet.pop(); // On retire la cellule du bout (donc retour a la precedente)
			}
		} // On arrete le curseur et le labyrinthe quand le trajet ne contient plus de chemin possible
        //console.log("Laby créé =\n"+this.grille.map(e => e.join(';')).join('\n')); // affichage pour debogage des valeurs des murs du labyrinthe
		return this; // On retourne le labyrinthe
	}

	/**
	 * Tente de trouver une ouverture dans les cellules adjacentes d'un curseur
	 * @param   {Curseur} curseur - Le curseur a tester
	 * @returns {boolean} - Un nouveau curseur
	 */
	trouverOuverture(curseur) {
		while (curseur.tentatives.length) { // On teste les 4 directions pour en trouver une valide
			let nCurseur = curseur.nouvelleTentative();
            //console.log("nCurseur.dir="+["nord","est","sud","ouest"][nCurseur.dir]);

			// Si la case est valide, on ouvre le mur et on déplace le curseur
			let cellule = this.cellule(nCurseur);
            //console.log("Debogage des spirales... cellule?"+(cellule===true)+" cellule.libre?"+(cellule.libre===true));
			if (cellule && cellule.libre) {
				this.ouvrirDevant(curseur);
				this.ouvrirDerriere(nCurseur);
				curseur = nCurseur;
				return nCurseur; // On garde en mémoire le curseur pour le retour
			}
		}
		return false;
	}

	/**
	 * Ajoute l'entree et la sortie
	 * @returns {Labyrinthe} - this
	 */
	ajouterSortie() {
		this.cellule(this.hauteur-1, this.largeur-1).ouvrir("sud");
		return this;
	}

	/**
	 * Ajoute la cle et tous les autres objets requis dans des cellules libres du labyrinthe
     * (excluant les cellules deja occupees, ainsi que l'entree et la sortie)
	 * @returns {Array} - Tableau des objets interactifs du labyrinthe
	 */
	ajouterObjets() {
				// #tim Jessy Pouliot - Ajout de l'objet vie
        let tNomsObjets = ["cle", "vie"]; //d'autres objets pourraient etre ajoutes ici
        let tObjets = []; //tableau des instances des objets qui seront crees
        let tCellules = [].concat.apply([],this.grille); //cree un tableau a une seule dimension a partir du tableau 2d
        tCellules.splice(0,1); //on elimine la cellule de l'entree
        tCellules.splice(tCellules.length-1,1); //on elimine la cellule de sortie
        for(let i=0; i<tNomsObjets.length; i++){
            if(tCellules.length>0){
                let unePos = this.hasard(tCellules.length); //selection d'une pos de cellule
                let uneCellule = tCellules.splice(unePos,1)[0];//extraction de la cellule;
								// #tim Jessy Pouliot - Ajout d'une chance d'avoir vie (balancing)
								var min = 1;
								var max = 100;
								var random = Math.floor(Math.random() * (max - min)) + min;
								if(i == 1 && random <= 50){
									tObjets[i] = new Objet(tNomsObjets[i], this.jeu).init({classe:tNomsObjets[i], cellule: uneCellule});
									uneCellule.changerObjet(tObjets[i]);
								} else if (i != 1) {
									tObjets[i] = new Objet(tNomsObjets[i], this.jeu).init({classe:tNomsObjets[i], cellule: uneCellule});
									uneCellule.changerObjet(tObjets[i]);
								}
            } else {
                console.log("Erreur: pas assez de cellules pour placer tous les objets");
                break; //interruption de la boucle
            }
        }
		return tObjets;
	}

	/**
	 * Ouvre le mur devant le curseur, selon la direction en cours
	 * @param   {Curseur} curseur - Le curseur a utiliser
	 * @returns {Labyrinthe} - this
	 */
	ouvrirDevant(curseur) {
		this.cellule(curseur).ouvrir(curseur.dir);
		return this;
	}

	/**
	 * Ouvre le mur derriere le curseur, selon la direction en cours
	 * @param   {Curseur} curseur - Le curseur a utiliser
     * @returns {Labyrinthe} - this
	 */
	ouvrirDerriere(curseur) {
		this.cellule(curseur).ouvrir(curseur.dirInverse);
		return this;
	}

	/**
	 * Ferme le mur devant le curseur, selon la direction en cours
	 * @param   {Curseur} curseur - Le curseur a utiliser
	 * @returns {Labyrinthe} - this
	 */
	fermerDevant(curseur) {
		this.cellule(curseur).fermer(curseur.dir);
		return this;
	}

	/**
	 * Ferme le mur derriere le curseur, selon la direction en cours
	 * @param   {Curseur} curseur - Le curseur a utiliser
	 * @returns {Labyrinthe} - this
	 */
	fermerDerriere(curseur) {
		this.cellule(curseur).fermer(curseur.dirInverse);
		return this;
	}

	/**
	 * Retourne un nombre entier aleatoire entre 0 et nombre-1
	 * @param   {number}  nombre  - Le nombre de possibilites
	 * @returns {number} - Le nombre entier choisi
	 */
	hasard(nombre) {
		return (Math.floor(Math.random() * nombre));
	}

	/**
	 * Méthode statique qui retourne une direction apres traitement
	 * @param   {*}  direction  - La direction peut etre une chaine ou un entier entre 0 et 3
	 * @returns {number} - Le nombre entier correspondant et valide
	 */
	static parseDirection(direction) {
		if (typeof direction === "string") {
			if (direction[0] === '-') {
				return this.parseDirection(this.directions[direction.substr(1)] + 2);
			} else {
				return this.directions[direction];
			}
		} else if (typeof direction === "number") {
			return ((direction % 4) + 4) % 4;
		} else if (direction.dir !== undefined) {
			return this.parseDirection(direction.dir);
		} else {
			throw "Mauvais type de données";
		}
	}

	static init() {
		this.directions = {nord:0, est:1, sud:2, ouest:3};
	}
}
Labyrinthe.init();
