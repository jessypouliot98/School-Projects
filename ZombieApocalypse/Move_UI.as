package {
	import flash.display.MovieClip;
	import flash.events.Event;

	public class Move_UI extends MovieClip {
		private var posX:int;
		private var posY:int;

		public function Move_UI(pX:int, pY:int) {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			posX = pX;
			posY = pY;
		}
		private function addedToStage(e: Event): void {
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			x = posX;
			y = posY;
			width = width/2;
			height = height/2;
		}
	}
}