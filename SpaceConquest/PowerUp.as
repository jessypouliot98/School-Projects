package {

	import flash.display.MovieClip;
	import flash.events.*;
	import flash.display.Stage;

	public class PowerUp extends MovieClip {

		private var _speed: Number = 3;
		private var _speedR: Number;
		private var _nbLanes: uint = 6;
		private var _stageWidth: uint = 800 /*Default*/ ;
		private var _laneSize: uint = _stageWidth / _nbLanes;
		private var _size: Number = 1.2;
		//Powers
		private var powerSelect: uint;
		private const fastReload: uint = 1;
		private const doubleFire: uint = 2;
		private const engineUpgrade: uint = 3;

		public function PowerUp() {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			var select = rand(1, 3);
			if (select == fastReload) {
				powerSelect = fastReload;
			}
			else if (select == doubleFire) {
				powerSelect = doubleFire;
			}
			else if (select == engineUpgrade) {
				powerSelect = engineUpgrade;
			}
		}
		private function addedToStage(e: Event): void {
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.ADDED_TO_STAGE, removedFromStage);
			addEventListener(Event.ENTER_FRAME, loop);
			var _stageWidth = stage.stageWidth;
			gotoAndStop(powerSelect);
			spawn();
		}
		private function spawn(): void {
			y = -height;
			x = (rand(1, _nbLanes) * _laneSize) - (_laneSize / 2);
		}
		private function rand(min: int, max: int): int {
			return Math.floor(Math.random() * (max - min + 1)) + min;
		}
		private function loop(e: Event): void {
			//SPEEDS
			y += _speed;
			//COLLISION
			var aircraft = MovieClip(parent.parent).groupAircraft_mc.getChildAt(0)
			if (aircraft.vBox.hitTestObject(this) == true || aircraft.hBox.hitTestObject(this) == true) {
				if (powerSelect == doubleFire) {
					aircraft.doubleFire();
				}
				else if (powerSelect == fastReload) {
					aircraft.fastReload();
				}
				else if(powerSelect == engineUpgrade) {
					aircraft.engineUpgrade();
				}
				MovieClip(parent).removeChild(this);
				removeEventListener(Event.ENTER_FRAME, loop);
				return;
			}
			//OUT OF BOUNDS
			if (y > stage.stageHeight + height) {
				MovieClip(parent).removeChild(this);
				removeEventListener(Event.ENTER_FRAME, loop);
			}
		}
		private function removedFromStage(e: Event): void {
			removeEventListener(Event.ADDED_TO_STAGE, removedFromStage);
			removeEventListener(Event.ENTER_FRAME, loop);
		}
	} // classe   
} // package