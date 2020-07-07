package {
	import flash.display.MovieClip;
	import flash.events.*;

	public class Cinematic extends MovieClip {
		public function Cinematic() {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			btn_menu.addEventListener(MouseEvent.CLICK, gotoMenu);
		}
		private function addedToStage(e:Event):void {
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			trace("cine");
			gotoAndPlay(1);
			play();
		}
		private function removedFromStage(e:Event):void {
			removeEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
		}
		private function gotoMenu(e:MouseEvent):void {
			MovieClip(parent).goto(this,"Menu");
			stop();
		}
	}
}