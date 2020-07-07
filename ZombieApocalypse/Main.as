/*
https://orig00.deviantart.net/87c7/f/2013/215/9/a/rpg_maker_vx___fences_by_ayene_chan-d6gfbfd.png
*/
package {

	import flash.display.MovieClip;
	import flash.events.*;

	public class Main extends MovieClip {
		//screens
		private var screenCine: Cinematic;
		private var screenMenu: Menu;
		private var screenCredit: Credit;
		private var screenCtrl: Ctrl;
		private var screenGame: Game;
		private var screenGameOver: GameOver;

		public function Main() {
			//screens
			screenCine = new Cinematic();
			screenMenu = new Menu();
			screenCredit = new Credit();
			screenCtrl = new Ctrl();
			screenGame = new Game();
			screenGameOver = new GameOver();
			addChild(screenCine);
			//e.controls
			addEventListener(MouseEvent.RIGHT_MOUSE_DOWN, rmDown); //right mouse down
			stage.addEventListener(MouseEvent.RIGHT_CLICK, doNothing);
			addEventListener(MouseEvent.MOUSE_DOWN, mDown); //mouse down
			addEventListener(MouseEvent.RIGHT_MOUSE_UP, rmUp); //right mouse up
			stage.addEventListener(KeyboardEvent.KEY_DOWN, kDown); //keyboard down
			stage.addEventListener(KeyboardEvent.KEY_UP, kUp); //keyboard up
		}
		//SCREEN SWITCHES
		public function goto(screenToRemove: MovieClip, screenToAdd: String): void {
			removeChild(screenToRemove);
			switch (screenToAdd) {
				case "Cinematic":
					addChild(screenCine);
					break;
				case "Menu":
					addChild(screenMenu);
					break;
				case "Ctrl":
					addChild(screenCtrl);
					break;
				case "Credit":
					addChild(screenCredit);
					break;
				case "Game":
					addChild(screenGame);
					break;
				case "GameOver":
					addChild(screenGameOver);
					break;
			}
		}
		//CONTROLS
		//mouse
		//right mouse down // move to && attack enemy
		private function rmDown(e: MouseEvent): void {
			screenGame.rmDown(e);
		}
		private function doNothing(e: MouseEvent): void {
			return;
		}
		//mouse down // attack enemy
		private function mDown(e: MouseEvent): void {
			screenGame.mDown(e);
		}
		private function rmUp(e: MouseEvent): void {
			screenGame.rmUp(e);
		}
		//keyboard
		//key up
		private function kUp(e): void {
			screenGame.kUp(e);
		}
		//key down
		private function kDown(e): void {
			screenGame.kDown(e);
		}
	}
}