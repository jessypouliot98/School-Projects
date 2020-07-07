/*eslint-env es6, browser*/    //Permet la syntaxe Ecmascript 6 et indique que le contexte est dans un navigateur
/*eslint-disable no-console*/  //Bloque les messages d'erreur au sujet de la console
/*globals Sprite*/             //Permet l'utilisation des globales en question

/**
 * Classe qui represente le personnage du joueur.
 * @extends Sprite
 */
class Perso extends Sprite {
    /**
     * Construit une instance du personnage.
     * @param {string} url - Url de l'image utilisee pour le personnage.
     * @param {Jeu} jeu - L'instance du jeu en cours.
		 * @param {int} vie - nombre de vie restante
     */
	constructor(url, jeu) {
		super("images/" + url); //appel du constructeur de Sprite
    this.jeu = jeu;
		this.viderLaFile();
		this.direction = 1;
		this.vitesse = 0;
    this.inventaire = []; //tableau des objets du joueur
		this.vie = 0; // #tim Jessy, instance la vie par default
	}

	init(props) {
		this.setEvents();
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

	get direction() {
		return this._direction;
	}

	set direction(val) {
		this._direction = val;
		this.image.style.backgroundPositionX = "-"+val+"em";
	}

	get vitesse() {
		return this._vitesse;
	}

	set vitesse(val) {
		this._vitesse = val;
		if (this._vitesse === 0) {
			this.image.style.animationIterationCount = "0";
		} else {
			this.image.style.animationIterationCount = "infinite";
			this.image.style.animationDuration = this.animPas + "ms";
			this.avancer();
		}
	}

	/**
	 * Verifie si le passage est possible ou pas selon un obstacle
     * permet aussi le changement de niveau
	 * @returns {boolean} - true si une interaction a eu lieu
	 */
	verifierBlocage() {
        if(this.cellule.rangee==this.jeu.labyrinthe.largeur-1 && this.cellule.colonne==this.jeu.labyrinthe.hauteur-1 && this.direction==2){
            if(this.verifierObjet("cle")==false){
                this.jeu.ui.afficherMessage("La porte est verrouillée!");
                this.jeu.ui.jouerSFX("porte_barree");

            } else {
								this.suprimerObjet("cle");// #tim Jessy, detruie la clef ors de lutilisation
								this.inventaire._nom = null;
                this.jeu.ui.afficherMessage("Vous avez réussi le niveau "+this.jeu.niveau+"!");
                this.jeu.ui.jouerSFX("succes");

                window.load(); //passage au prochain niveau
            }
            return true; //il y a eu une interaction
        }
        return false; //il n'y a pas eu d'interaction
	}

	/**
	 * Verifie si le perso possede un objet
     * @param {string} nomRecherche - Le com de l'objet recherche
	 * @returns {boolean} - true si le joueur possede l'objet
	 */
	verifierObjet(nomRecherche) {
        for(let i=0; i<this.inventaire.length; i++){ //boucle sur le tableau des sprites de l'inventaire
            if(this.inventaire[i].nom==nomRecherche){
                return true; //l'inventaire contient l'objet
            }
        }
        return false; //l'inventaire ne contient pas l'objet
	}

	// #tim Jessy
	//Detruie la clef lorsque utiliser
	suprimerObjet(nomRecherche) {
				for(let i=0; i<this.inventaire.length; i++){ //boucle sur le tableau des sprites de l'inventaire
						if(this.inventaire[i].nom==nomRecherche){
								this.inventaire[i].nom = null;
						}
				}
	}



	/**
	 * Cette methode verifie si le personnage peut avancer quand
     * la commande a ete lancee ; le mouvement est declenche si possible
	 */
	avancer() {
		let fermee = this.cellule.ouverture(this.direction);
		if (fermee) { //il y a un mur
			return this.finMouvement();
		} else if (this.verifierBlocage()===true) { //la porte est bloquee
            return this.finMouvement();
        }
		this.visuel.style.transitionDuration = (this.animMouvement) + "ms";
		this.cellule = this.cellule.voisine(this.direction);

        console.log("Déplacement: "+["↑","→","↓","←"][this.direction]+" / "+this.cellule.infoDebogage());

		this.visuel.addEventListener('transitionend', this.evt.visuel.transitionend);
	}


    /**
	 * Genere le contenu visuel du personnage
     * et ajoute les ecouteurs qui lui sont utiles
	 * @returns {HTMLElement} - le div et son contenu
     */
	creerVisuel() {
		let resultat;
		resultat = super.creerVisuel(); //appel de la fonction creerVisuel de Sprite
		resultat.classList.add("perso");
		return resultat;
	}


    /**
	 * Change la direction et gere la vitesse d'un mouvement du perso
	 * @returns {Perso} - this
     */
	mouvement(dir) {
		if (dir === undefined) {
			return;
		}
        this.vitesse = 0;
        this.direction = dir;
        this.vitesse = 1;
		return this;
	}

    /**
	 * Methode appelee quand une touche est enfoncee
     * permet de lancer la commande ou de l'ajouter en file
     * si d'autres commandes sont en attente
     * @param   {Event} e - l'evenement du keyDown
	 * @returns {Perso} - this
     */
    ajouterEnFile(e) {
        //console.log("Le 'code' de la touche est '"+e.code+"' et son 'keyCode' est '"+e.keyCode+"' et son 'key' est '"+e.key+"'");
        if(e.code==="KeyM" || e.key==="m" || e.key==="M"){ //dans Safari Mac e.keyCode===91, mais dans EdgeWindows e.keyCode===77
            this.jeu.ui.musique.pause(); //arrete la musique...
        }
				else {
						this.file.pop();// #tim Jessy, empeche de storer un patern de commande
            this.file.push(e);
            if ((this.file.length) === 1 && (this.vitesse===0)) {
                //pas d'attente dans la file ET le perso est pret, on declenche le mouvement
                this.prochainMouvement();
            }
        }
		return this;
	}


    /**
	 * Recupere le prochain evenement en file pour declencher
     * le mouvement correspondant a la touche
	 * @returns {Perso} - this
     */
	prochainMouvement() {
		if (this.file.length === 0) {
            this.vitesse = 0; //il s'arrete
			return this; //fin de la fonction dans ce cas
		}
		let e = this.file[0];

        let directions, dir;

        if(e.code===undefined){
            //patch pour Safari qui ne connait pas la propriete 'code'
            //a verifier: la propriete 'key' est-elle valable dans Safari (serait preferable)
            directions = {
                38:0,
                39:1,
                40:2,
                37:3
            };
            dir = directions[e.keyCode];
        } else {
            directions = {
                ArrowUp:0,
                ArrowRight:1,
                ArrowDown:2,
                ArrowLeft:3
            };
            dir = directions[e.code];
        }
		this.mouvement(dir);
		return this;
	}


    /**
	 * Initialise la file d'attente des evenements a venir
     * (et vide tous les evenements au passage)
     */
    viderLaFile(){
        this.file = [];
    }


    /**
	 * Methode declenchee quand le mouvement se termine
     * (quand le perso arrive dans la nouvelle cellule)
     */
	finMouvement() {
		this.visuel.removeEventListener('transitionend', this.evt.visuel.transitionend);
		this.file.shift();
        this.cellule.verifierObjet();
		this.prochainMouvement();
		//this.vitesse = 0; //maintenant gere dans la fn prochainMouvement
	}


   /**
	 * Donne le droit de deplacer le personnage en ajoutant les ecouteurs
     */
    activer() {
		Sprite.addEventListeners(window, this.evt.window);
        console.log(this.cellule.infoDebogage());
    }


   /**
	 * Retire le droit de deplacer le personnage en eliminant les ecouteurs
     */
    desactiver() {
			this.suprimerObjet("cle");// #tim Jessy, Suppression de linventaire lors de la fin de partie
			Sprite.removeEventListeners(window, this.evt.window);
    }


   /**
	 * Replace le perso a l'entree lors d'un changement de niveau
     */
    replacerChangNiveau(){
        this.viderLaFile();
        this.cellule = this.jeu.labyrinthe.cellule(0,0);
        this.direction = 1;
				this.vitesse = 0;
    }

		/**
 	 * #tim Jessy Pouliot Perd une vie
      */
   perdreVie() {
 		this.vie--;
		document.getElementById('divVie').classList.remove('grow');// #tim Jessy Pouliot Anim perdre vie
		document.getElementById('divVie').classList.add('grow');// #tim Jessy Pouliot Anim perdre vie
   }


    /**
	 * Cree un objet qui regroupe les fonctions
     * qui seront utiles aux differents elements
     */
	setEvents() {
		let sprite = this;
		this.evt = {
			window: {
				keydown: function (e) {
					sprite.ajouterEnFile(e);
				}
			},
			visuel: {
				transitionend: function () {
					sprite.finMouvement();
				}
			}
		};
	}

	static init() {
        //creation de proprietes valables peu importe l'instance de Perso:
		this.prototype.animMouvement = 300;
		this.prototype.animPas = 300;
	}
}

Perso.init();
