package {
	import flash.display.MovieClip;
	import flash.events.Event;

	public class Tile extends MovieClip {

		private var tileSet: uint;
		private var posX: uint;
		private var posY: uint;

		public function Tile(pX: uint, pY: uint, tile: uint) {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			tileSet = tile;
			posX = pX;
			posY = pY;
		}
		private function addedToStage(e: Event): void {
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			x = posX;
			y = posY;
			show();
		}
		private function show(): void {
			gotoAndStop(tileSet);
		}
		private function removedFromStage(e: Event): void {
			
		}
	}
}