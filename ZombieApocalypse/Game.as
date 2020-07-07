package {

	import flash.display.MovieClip;
	import flash.events.*;
	import flash.media.*;
	import flash.net.*;
	import flash.ui.*;

	public class Game extends MovieClip {
		//objects
		private var character_mc: Character; //My character
		private const NBENEMY: uint = 6; //nb of total enemy multiplier with round difficulty
		private const MAXENEMY: uint = 10; //nb total enemy on screen at once
		private var totalEnemyGen: uint; //total enemy generated
		//misc
		private var aCharPos: Array; //character pos [x,y]
		private var tileSize: uint = 50; //size of map tile
		private var round: uint; //round level "difficulty"
		private var shiftKey: Boolean = false; //when true, deactivate movement command
		private var clockCount: uint; //clock counter

		public function Game() {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			btn_menu.addEventListener(MouseEvent.CLICK, gotoMenu);
		}
		//Added to stage
		private function addedToStage(e: Event): void {
			//events
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.ENTER_FRAME, loop); //loop
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			//variables
			clockCount = 0; //reset variable
			round = 1; //reset variable
			//generation
			generate();
			//ready
			stage.focus = stage;
		}
		//GENERATIONS
		private function generate(): void {
			character_mc = new Character();
			charPlaceholder.addChild(character_mc); //spawn character
		}
		//create enemies
		private function createEnemy(clock): void {
			//return;

			var currentNbEnemy = mapPlaceholder.enemyPlaceholder.numChildren;

			if (clock % 60 == 0) {
				if (totalEnemyGen < NBENEMY * round) {
					if (currentNbEnemy <= MAXENEMY) {
						totalEnemyGen++;
						mapPlaceholder.enemyPlaceholder.addChild(new Enemy(round, character_mc.getPosMap()));
					}
				} else if (currentNbEnemy == 0) {
					nextWave();
				}
			}
		}
		private function nextWave(): void {
			totalEnemyGen = 0;
			round++;
			var sound: Sound = new Sound(new URLRequest("sound/wave.mp3"));
			var trans: SoundTransform = new SoundTransform(0.7, 0);
			var channel: SoundChannel = sound.play(0, 1, trans);
		}
		//CONTROLS
		//mouse
		//right mouse down // move to && attack enemy
		public function rmDown(e): void {
			if (shiftKey == false && e.target + "" == "[object Tile]") { //shift key is not active
				character_mc.moveChar(e.target.x, e.target.y); //Move character
				mapPlaceholder.addChild(new Move_UI(e.target.x + tileSize / 2, e.target.y + tileSize / 2)); //Click animation at mouse target
			} else if (e.target + "" == "[object Enemy]") { //if Enemy
				character_mc.moveStop(); //stop movement
				character_mc.attack(e.target); //lunches projectile
			}
		}
		//mouse down // attack enemy
		public function mDown(e): void {
			if (e.target + "" == "[object Enemy]") {
				character_mc.moveStop(); //stop movement
				character_mc.attack(e.target); //lunches projectile
			}
		}
		public function rmUp(e): void {

		}
		//keyboard
		//key up
		public function kUp(e): void {
			if (e.keyCode == Keyboard.SHIFT) shiftKey = false; //reset value
		}
		//key down
		public function kDown(e): void {
			if (e.keyCode == Keyboard.S) character_mc.moveStop(); //stop movement
			if (e.keyCode == Keyboard.SHIFT) shiftKey = true; //disable movement key
			if (e.keyCode == Keyboard.Q) character_mc.spellPiercingShot(); //spell fire beam
			if (e.keyCode == Keyboard.W) character_mc.spellFireTrail(); //spell fire trail
			if (e.keyCode == Keyboard.E) character_mc.spellDash(); //spell Dash
			if (e.keyCode == Keyboard.R) character_mc.spellKnockBack(); //spell KnockBack
		}
		//LOOP
		private function loop(e: Event): void {
			//MOVE
			//character
			character_mc.movePos();
			//enemies
			for (var i: uint; i < mapPlaceholder.enemyPlaceholder.numChildren; i++) { //move all enemies on map
				var enemy = mapPlaceholder.enemyPlaceholder; //shortcut
				var gotoX: int = character_mc.getPosMap()[0]; //Find target X
				var gotoY: int = character_mc.getPosMap()[1]; //Find target Y
				enemy.getChildAt(i).moveChar(character_mc.getPosMap()); //Move Enemy
			}
			//Clock
			clock();
		}
		private function clock(): void {
			clockCount++;

			character_mc.regenHP(clockCount);
			character_mc.cooldown(clockCount);

			createEnemy(clockCount);
			for (var i: uint; i < mapPlaceholder.enemyPlaceholder.numChildren; i++) { //move all enemies on map
				var enemy = mapPlaceholder.enemyPlaceholder; //shortcut
				enemy.getChildAt(i).cooldown(clockCount);
			}
		}
		//GETTER
		public function getTileSize(): uint {
			return tileSize;
		}
		//REMOVE
		private function removedFromStage(e: Event): void {
			removeEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			removeEventListener(Event.ENTER_FRAME, loop);
			charPlaceholder.removeChildren();
			mapPlaceholder.tilePlaceholder.removeChildren();
			mapPlaceholder.enemyPlaceholder.removeChildren();
		}
		public function gotoGameOver(): void {
			MovieClip(parent).goto(this, "GameOver");
		}
		private function gotoMenu(e: MouseEvent): void {
			MovieClip(parent).goto(this, "Menu");
		}
	}
}