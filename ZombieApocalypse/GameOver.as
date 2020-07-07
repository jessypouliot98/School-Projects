package {
	
	import flash.display.MovieClip;
	import flash.events.*;
	
	public class GameOver extends MovieClip {
		
		public function GameOver() {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			btn_game.addEventListener(MouseEvent.CLICK, gotoGame);
			btn_menu.addEventListener(MouseEvent.CLICK, gotoMenu);
		}
		private function addedToStage(event: Event = null): void {
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
		}
		private function removedFromStage(event: Event): void {
			removeEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
		}
		private function gotoGame(e: MouseEvent): void {
			MovieClip(parent).goto(this, "Game");
		}
		private function gotoMenu(e: MouseEvent): void {
			MovieClip(parent).goto(this, "Menu");
		}
	}
}