/*
SOURCES:
https://freesound.org/people/Volvion/sounds/265308/
https://freesound.org/people/LittleRobotSoundFactory/sounds/270336/
http://freesound.org/people/timgormly/sounds/170146
http://freesound.org/people/CGEffex/sounds/107771/
http://freesound.org/people/TheDweebMan/sounds/277213/
http://freesound.org/people/timgormly/sounds/170146/
*/
package {
	
	import flash.display.MovieClip;
	import flash.events.*;

	public class Main extends MovieClip {

		private var screenMenu: Menu;
		private var screenCtrl: Ctrl;
		private var screenGame: Game;
		private var screenGameOver: GameOver;
		private var screenActive: String;
		private var score: uint;

		public function Main() {
			//Screens
			screenMenu = new Menu();
			screenCtrl = new Ctrl();
			screenGame = new Game();
			screenGameOver = new GameOver();
			addChild(screenMenu);
			//Generation
			generateStars(100);
			//Events
			stage.addEventListener(KeyboardEvent.KEY_DOWN, key_down);
			stage.addEventListener(KeyboardEvent.KEY_UP, key_up);
			stage.addEventListener(MouseEvent.MOUSE_UP, mouse_up);
			stage.addEventListener(MouseEvent.MOUSE_DOWN, mouse_down);
		}
		//Generation
		private function generateStars(nbStars: uint): void {
			for (var i: uint = 0; i < nbStars; i++) {
				groupStars_mc.addChild(new Stars());
			}
		}
		//public vars
		public function activeScreen():String{
			return screenActive;
		}
		public function postScore(pts):void{
			score = pts;
		}
		public function getScore():uint{
			return score;
		}
		//CONTROLS
		public function key_up(e: KeyboardEvent): void {
			screenGame.key_up(e.keyCode);
		}
		public function key_down(e: KeyboardEvent): void {
			screenGame.key_down(e.keyCode);
		}
		public function mouse_up(e: MouseEvent): void {
			screenGame.mouse_up();
		}
		public function mouse_down(e: MouseEvent): void {
			screenGame.mouse_down();
		}
		public function mousePosX(): Number {
			return stage.mouseX;
		}
		public function mousePosY(): Number {
			return stage.mouseY;
		}
		public function goto(screenToRemove: MovieClip, screenToAdd: String): void {
			removeChild(screenToRemove);
			screenActive = screenToAdd;
			switch (screenToAdd) {
				case "Menu":
					addChild(screenMenu);
					break;
				case "Ctrl":
					addChild(screenCtrl);
					break;
				case "Game":
					addChild(screenGame);
					break;
				case "GameOver":
					addChild(screenGameOver);
					break;
			}
		}
	} //classe
} //package