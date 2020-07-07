package {
	import flash.display.MovieClip;
	import flash.events.*;

	public class Menu extends MovieClip {

		public function Menu() {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			btn_ctrl.addEventListener(MouseEvent.CLICK, gotoCtrl);
			btn_credit.addEventListener(MouseEvent.CLICK, gotoCredit);
			btn_cine.addEventListener(MouseEvent.CLICK, gotoCine);
		}
		
		private function addedToStage(e:Event):void {
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
		}

		private function removedFromStage(e:Event):void {
			removeEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
		}
		
		private function gotoCtrl(e:MouseEvent):void {
			MovieClip(parent).goto(this,"Ctrl");
		}
		private function gotoCredit(e:MouseEvent):void {
			MovieClip(parent).goto(this,"Credit");
		}
		private function gotoCine(e:MouseEvent):void {
			MovieClip(parent).goto(this,"Cinematic");
		}
	}
}