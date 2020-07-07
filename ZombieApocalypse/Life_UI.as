package {
	import flash.display.MovieClip;
	import flash.events.Event;

	public class Life_UI extends MovieClip {

		public function Life_UI() {
			mouseEnabled = false;//enable mouse to click under THIS
		}
		
		
		//--DISPLAY--//
		public function show(currentHP: int = 100, maxHP: uint = 100): void {//display the UI life effects
			var percent:Number = (currentHP / maxHP)*100;
			percent = Math.round(percent);
			gotoAndStop(100 - percent);
		}
	}
}