/*eslint-env es6, browser*/    //Permet la syntaxe Ecmascript 6 et indique que le contexte est dans un navigateur
/*eslint-disable no-console*/  //Bloque les messages d'erreur au sujet de la console
/*globals Labyrinthe, Ui, Perso*/ //Permet l'utilisation des globales en question

/** Classe principale, elle controle le deroulement du jeu. */
class Jeu {
    /**
     * Construit une instance du jeu.
     */
    constructor() {
		this.sprites = {};
        this.niveau = 1;
        this.labyrinthe = new Labyrinthe(this);
        this.ui = new Ui(this); //creation de l'instance de l'interface
        this.ui.creerBtnInstruction();// #tim Jessy, Creer le bouton instruction
        this.ui.creerBtnCommencer();
	}


	/**
	 * Ajoute un sprite au jeu
	 * @param   {string}  nom  - Le nom du sprite
	 * @param   {Sprite}  sprite  - Le sprite
	 * @returns {Jeu} - this
	 */
	ajouterSprite(nom, sprite) {
		this.ui.visuel.appendChild(sprite.visuel);
		this.sprites[nom] = nom;
		this[nom] = sprite;
		return this;
	}


    /**
	 * Genere les objets interactifs, puis les affiche
     */
    genererObjets(){
        //creation des objets du niveau:
        this.tObjets = this.labyrinthe.ajouterObjets();
        //affichage des objets:
        for(let i=0; i<this.tObjets.length; i++){
            this.ajouterSprite(this.tObjets[i].nom, this.tObjets[i]);
        }
    }

    // #tim Jessy
    //Affiche l'ecran instruction
    afficherInstruction() {
          this.divJeu = document.getElementById("divJeu"); //reference pour acces rapide
          let divInstruction = document.createElement('div');
          divInstruction.innerHTML = "<p style= 'color: #fff; font-family: Bowlby One SC;'>Le but de ce jeu est de sortir du labyrinthe. Pour se faire vous devez trouver la clé qui dévérouille la porte du labyrinthe avant de la franchir. Chaque niveau de labyrinthe à sa propres clé. En plus de trouver la clé, il faut réussir le niveau avant que le temps ne soit écoulé.</p>" +
          "<p style= 'color: #fff; font-family: Bowlby One SC;'>Bonne Chance!</p>" + "<img src='images/controleJeu.png' alt='image touche clavier'>";// #tim Alex
          divInstruction.id = "divInstruction";
          this.divJeu.appendChild(divInstruction);
          if(document.getElementById('divLabyrinthe')){
            this.ui.creerBtnRecommencer();
          } else {
            this.ui.creerBtnCommencer();
          }
    }


    /**
	 * Prepare le jeu pour la premiere partie du joueur
     */
	prepPremierNiveau() {
        this.perso = new Perso("man1.png", this).init({classe:'homme', cellule: this.labyrinthe.cellule(0,0)}); //creation du perso
        this.ui.jouerMusique(); //lancement de la musique
        this.ui.creerInterface(); //creation de l'interface
    }


    /**
	 * Prepare le niveau pour un changement de niveau
     * @param {number} largeur - La largeur de base du labyrinthe
     * @param {number} hauteur - La hauteur de base du labyrinthe
     */
    prepChangNiveau() {
        this.niveau++;
        console.log("Nouveau niveau = "+this.niveau);

        let niveauPrecedent = document.getElementById("divGrilleNiveau");

        if(niveauPrecedent!==null){
            console.log("Retrait du niveau précédent");
            clearInterval(window.intervalDecompte);
            this.ui.visuel.removeChild(niveauPrecedent);
        }

        this.labyrinthe = new Labyrinthe(this); //permet de creation d'un nouveau labyrinthe

        //repositionnement du perso
        this.perso.replacerChangNiveau();
    }


    /**
	 * Prepare le niveau
     */
	demarrerNiveau() {
        this.ui.creerVisuelLabyrinthe(); //creation du visuel du labyrinthe
        this.ajouterSprite("homme", this.perso); //affichage du perso
        this.genererObjets(); //creation et affichage des objets

        let secDuree = 6 + (this.niveau * 2); // #tim Jessy ajout de temps par rapport a la difficulte

        this.ui.afficherTemps(secDuree);
        this.ui.afficherVie();
        this.ui.afficherCle();
        this.ui.afficherNumNiveau(this.niveau);

        var tempsRef = new Date().getTime();

        this.ui.ajusterZoom();
        this.perso.activer(); //le personnage peut bouger

        window.intervalDecompte = setInterval(function() {
            //attention: dans le contexte de cette fonction @this -> window
            let tempsAct = new Date().getTime() ;
            let secPassees = Math.floor((tempsAct-tempsRef)/1000);
            let secRestantes = secDuree - secPassees;
            this.jeu.ui.afficherTemps(secRestantes);
            if(secRestantes<=0){// #tim Jessy, ajout "=" pour regler compteur negatif
            // #tim Jessy, ajout de reset temps pour chaque vie
              console.log(this.jeu.perso.vie);
              if(this.jeu.perso.vie > 0) {
                console.log("Oof!");
                tempsRef = tempsAct;
                // this.jeu.perso.vie--;
                this.jeu.perso.perdreVie();// #tim Jessy Pouliot fonction perdre vie
                this.jeu.ui.afficherTemps(secRestantes);
                this.jeu.ui.afficherVie();
                this.jeu.ui.jouerSFX('perdVie');// #tim Jessy, ajout son vie perdu
              } else {
                clearInterval(window.intervalDecompte);
                console.log("LabyBoom!");
              //Fin changement
                document.getElementById('divLogo').classList.remove('hide');// #tim Jessy, cacher le menu lors du gameplay

                this.jeu.ui.creerBtnRecommencer();
                this.jeu.ui.creerBtnInstruction();// #tim Jessy, affiche le bouton instruction

                this.jeu.ui.creerAnimBoom();

                this.jeu.ui.jouerSFX("boom");
                this.jeu.perso.desactiver();

                window.intervalAnimBoom = setInterval(function() {
                    //attention: dans le contexte de cette fonction @this -> window
                    this.jeu.ui.supprimerElement("imgBoom");
                    clearInterval(this.intervalAnimBoom);
                },1000);// #tim Jessy, Duree animation boom pour prevenir la boucle d'anim
              }
            }
        }, 1000);
    }


    static init() {

        window.commencer = function() {
            //attention: dans le contexte de cette fonction @this -> l'instance de Ui
            console.log("Commencer");
            this.supprimerElement("btnCommencer");
            this.supprimerElement("btnInstruction");//Jessy Pouliot 1533944
            this.supprimerElement("divInstruction");//Jessy Pouliot 1533944
            //initialisation des parametres:
            this.jeu.prepPremierNiveau();
            this.jeu.demarrerNiveau();
        };

        //Jessy Pouliot 1533944
        window.instruction = function() {
            //attention: dans le contexte de cette fonction @this -> l'instance de Ui
            console.log("Instruction");
            this.supprimerElement("btnCommencer");
            this.supprimerElement("btnRecommencer");
            this.supprimerElement("btnInstruction");
            this.supprimerElement("divInstruction");
            //initialisation des parametres:
            this.jeu.afficherInstruction();
        };

        window.recommencer = function() {
            //attention: dans le contexte de cette fonction @this -> l'instance de Ui
            console.log("Recommencer");
            this.supprimerElement("btnRecommencer");
            this.supprimerElement("btnInstruction");// #tim Jessy, supprimer bouton instruciton
            this.supprimerElement("divInstruction");// #tim Jessy, supprimer ecran instruction
            this.jeu.niveau = 0;
            window.load();
        };

		window.load = function () {
            //attention: dans le contexte de cette fonction @this -> window

            if(!this.jeu){ //le jeu n'existe pas

                //creation d'une instance du jeu (et un 1er labyrinthe):
                this.jeu = new Jeu();

            } else { //le jeu existe deja

                //on passe au niveau suivant:
                this.jeu.prepChangNiveau();

                //on prepare le nouveau niveau:
                this.jeu.demarrerNiveau();
            }

		};

		window.addEventListener("load", window.load);
	}
}
Jeu.init();
