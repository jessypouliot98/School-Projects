/*eslint-env es6, browser*/    //Permet la syntaxe Ecmascript 6 et indique que le contexte est dans un navigateur
/*eslint-disable no-console*/  //Bloque les messages d'erreur au sujet de la console
/*globals Cellule*/ //Permet l'utilisation des globales en question

/** Classe qui gere l'affichage de l'interface (incluant le labyrinthe). */
class Ui {
    /**
     * Construit une instance de l'interface.
     */
    constructor(jeu) {
        this.jeu = jeu;
		this.sprites = {};

        this.musique = new Audio(); //en attente d'une musique
        this.sfx = new Audio(); //en attente d'un effet sonore
	}


    /**
	 * Supprime tous les elements dont la classe est skin
     */
	eliminerVisuelLabyrintheAnterieur() {
        let lsElements = document.getElementsByClassName('skin'); //tableau de tous les elements de classe skin
        while(lsElements[0]) {
            lsElements[0].parentNode.removeChild(lsElements[0]); //suppression
        }
        this.visuel = "";
	}


    /**
	 * Genere le contenu visuel du niveau
	 * @returns {HTMLElement} - le div et son contenu
     */
	creerVisuelLabyrinthe() {
        console.info("Création de l'habillage visuel"); //info
        document.getElementById('divLogo').classList.add('hide');// #tim Jessy,cache le logo lors du gameplay
        this.labyrinthe = this.jeu.labyrinthe; //pour acces simplifie au labyrinthe

        this.eliminerVisuelLabyrintheAnterieur(); //elimination de tous les elements anterieurs de classe skin

        let contenu;

        this.divLabyrinthe = this.relancerAnimCSS(this.divLabyrinthe); //truc pour declencher l'anim CSS

        this.visuel = document.createElement("div");
		this.visuel.classList.add("skin");

		contenu = document.createElement("div");
		contenu.classList.add("grille");
		contenu.id = "divGrilleNiveau";
		contenu.style.fontSize = Cellule.taille + "px";
		contenu.style.width = this.labyrinthe.largeur + "em";

        //gestion des largeurs et hauteurs des bordures du cadre:
        document.getElementById("divLabyrinthe").style.width = (this.labyrinthe.largeur+2) + "em";
        document.getElementById("divLabyrinthe").style.minWidth = (this.labyrinthe.largeur+2) + "em";
        document.getElementById("cadre_n").style.width = (this.labyrinthe.largeur) + "em";
        document.getElementById("cadre_s").style.width = (this.labyrinthe.largeur)-1 + "em";
        document.getElementById("cadre_o").style.height = (this.labyrinthe.hauteur) + "em";

		this.labyrinthe.parcourirGrille( function (cellule) {
			contenu.appendChild( cellule.visuelCellule() );
		}, this);

		this.visuel.appendChild(contenu);

        document.getElementById("cadre_centre").appendChild(this.visuel);
	}


    /**
	 * Genere les elements HTML requis pour l'affichage du jeu
     */
	creerInterface() {
        console.info("Création des div de l'interface"); //info

        this.divInfos = document.createElement('div');
        document.body.appendChild(this.divInfos);
        this.divInfos.id = "divInfos";

        this.divNumNiveau = document.createElement('div');
        this.divInfos.appendChild(this.divNumNiveau);
        this.divNumNiveau.id = "divNumNiveau";

        this.divChrono = document.createElement('div');
        this.divInfos.appendChild(this.divChrono);
        this.divChrono.id = "divChrono";

        this.divVie = document.createElement('div');
        this.divInfos.appendChild(this.divVie);
        this.divVie.id = "divVie";

        this.divCle = document.createElement('div');
        this.divInfos.appendChild(this.divCle);
        this.divCle.id = "divCle";

        this.divMessage = document.createElement('div');
        document.body.appendChild(this.divMessage);
        this.divMessage.id = "divMessage";

        this.divLabyrinthe = document.createElement('div');
        this.divLabyrinthe.id = "divLabyrinthe";
        let lsIdCadre = ["cadre_no", "cadre_n", "cadre_ne", "cadre_o", "cadre_centre", "cadre_e", "cadre_so", "cadre_s", "cadre_porte", "cadre_se"];
        for(let i=0; i<lsIdCadre.length; i++){
            let divCadre = document.createElement('div');
            divCadre.id = lsIdCadre[i];
            if(i!==4){divCadre.style.backgroundImage = "url(images/"+lsIdCadre[i]+".png)";}
            divCadre.classList.add("cadre");
            this.divLabyrinthe.appendChild(divCadre);
        }

        this.divLabyrinthe.classList.add("animFadeIn"); //ajout de son animation

        document.body.appendChild(this.divLabyrinthe);
	}


    /**
	 * Genere et affiche le bouton pour commencer le jeu
     */
	creerBtnCommencer() {
        this.divJeu = document.getElementById("divJeu"); //reference pour acces rapide
        let btnCommencer = document.createElement('input');
        btnCommencer.type = "button";
        btnCommencer.value = "Jouer";// #tim Jessy, Nom plus approprier pour le bouton jouer
        btnCommencer.id = "btnCommencer";
        btnCommencer.addEventListener("click", window.commencer.bind(this)); //bind permet que la reference this soit transmise a l'execution de la fonction
        this.divJeu.appendChild(btnCommencer);
	}


    // #tim Jessy
    /**
      * Genere et affiche le bouton pour les instruction du jeu
      */
  creerBtnInstruction() {
        this.divJeu = document.getElementById("divJeu"); //reference pour acces rapide
        let btnInstruction = document.createElement('input');
        btnInstruction.type = "button";
        btnInstruction.value = "Instruction";
        btnInstruction.id = "btnInstruction";
        btnInstruction.addEventListener("click", window.instruction.bind(this)); //bind permet que la reference this soit transmise a l'execution de la fonction
        this.divJeu.appendChild(btnInstruction);
  }


    /**
	 * Genere et affiche le bouton pour recommencer le jeu
     */
	creerBtnRecommencer() {
        let btnRecommencer = document.createElement('input');
        btnRecommencer.type = "button";
        btnRecommencer.value = "Rejouer";// #tim Jessy, Renommer bouton recommencer
        btnRecommencer.id = "btnRecommencer";
        btnRecommencer.addEventListener("click", window.recommencer.bind(this)); //bind permet que la reference this soit transmise a l'execution de la fonction
        this.divJeu.appendChild(btnRecommencer);
	}


    /**
	 * Genere et affiche l'animation du boom
     */
	creerAnimBoom() {
        let imgBoom = document.createElement('img');
        imgBoom.src = "images/animboom.gif";
        imgBoom.alt = "Boom!";
        imgBoom.id = "imgBoom";
        this.divJeu.appendChild(imgBoom);
	}


    /**
	 * Supprime le bouton pour commencer le jeu
     * @param {string} leId - Le id de l'element a supprimer
     */
	supprimerElement(leId) {
        try{
            let unElement = document.getElementById(leId);
            unElement.parentElement.removeChild(unElement);
        } catch(erreur) {
            //puisque l'element n'existe pas, il est probablement inutile de le supprimer,
            //mais il faut verifier le code en question!
            console.log("Erreur lors de la suppression de l'élément id='"+leId+"' ("+erreur+").")
        }
	}


	/**
	 * Affiche un message au joueur
	 * @param   {string}  message  - Le texte a afficher
	 */
   afficherMessage(message){
        this.divMessage.classList.add("animPop"); //ajout de son animation
        this.divMessage = this.relancerAnimCSS(this.divMessage); //truc pour declencher l'anim CSS
        this.divMessage.innerHTML=message; //affichage du texte
    }


    /**
	 * Affiche le temps restant
	 * @param   {int}  secRestantes  - Le temps a afficher (secondes)
     */
    afficherTemps(secRestantes) {
        this.divChrono.innerHTML = secRestantes+"s";
    }

    /**
   * Affiche le temps restant
   * @param   {int}  vieRestantes  - Le temps a afficher (secondes)
     */
    afficherVie() {
        this.divVie.innerHTML = "<img src='images/vie.png'> X " + this.jeu.perso.vie;
    }

    /**
   * Affiche le cle
   * @param   {boolean}  aCle  - Le temps a afficher (secondes)
     */
    afficherCle() {
        let cleImg;
        if( this.jeu.perso.verifierObjet("cle") ) {
          cleImg = "<img src='images/cle.png'>";
        } else {
          cleImg = "<img src='images/cle_placeholder.png'>";
        }
        this.divCle.innerHTML = cleImg;
    }


    /**
	 * Affiche le numero du niveau
	 * @param   {int}  numNiveau  - Le numero du niveau
     */
	afficherNumNiveau(numNiveau) {
        this.divNumNiveau.innerHTML = "Niveau&nbsp;"+numNiveau;
    }


    /**
	 * Fait jouer un effet sonore
	 * @param   {string}  nom  - Le nom du son (fichier) a jouer
	 */
	jouerSFX(nom){
        this.sfx.src = "sons/"+nom+".mp3";
        this.sfx.play();
    }


	/**
	 * Fait jouer une musique
	 * @param   {string}  nom  - Le nom du son (fichier) a jouer (optionnel)
	 */
	jouerMusique(nom){
        if(nom==null){ nom = "Death_and_Axes"; } // #tim Alex, changer la trame sonore
        this.musique.src = "sons/"+nom+".mp3";
        this.musique.loop = true;// #tim Jessy, Permet au son de jouer en boucle
        this.musique.play();
    }


    /**
	 * Relance l'animation CSS d'un element
     * Pour utiliser cette methode, il faut faire l'appel comme ceci:
     * elementOriginal = relancerAnimCSS(relancerAnimCSS);
     * @param {HTMLElement} unElementHTML - L'element pour lequel il faut relancer l'animation
     * @return {HTMLElement} - La nouvelle version de l'element
     */
	relancerAnimCSS(unElementHTML) {
        let copieElementHTML = unElementHTML.cloneNode(true); //«clonage» de l'element original (permet l'animation)
        unElementHTML.parentNode.replaceChild(copieElementHTML, unElementHTML); //remplacement de l'element par son clone
        return copieElementHTML; //on retourne le clone pour pouvoir «remplacer» l'original
    }


    /**
	 * Verifie les dimensions d'affichage du labyrinthe et ajuste le zoom du divLabyrinthe
     */
	ajusterZoom() {
        //Verifier bug dimention {bug}
        this.divLabyrinthe.style.transform = "scale(1)"; //zoom temporaire a 100% (pour calculs)

        let ratioLabyrintheH = window.innerHeight/this.divLabyrinthe.clientHeight;
        let ratioLabyrintheW = window.innerWidth/this.divLabyrinthe.clientWidth;
        let ratioLabyrinthe = Math.min(ratioLabyrintheH, ratioLabyrintheW); //le ratio le plus petit est retenu
        if(ratioLabyrinthe<1){
            ratioLabyrinthe -= 0.1; //on reduit un peu plus (bogue occasionnel)
            this.divLabyrinthe.style.transform = "scale("+ ratioLabyrinthe+")"; //zoom out pour eviter le debordement
        } else {
            this.divLabyrinthe.style.transform = "scale(1)";
        }
    }

    static init() {
		//rien a faire
	}
}

Ui.init();
