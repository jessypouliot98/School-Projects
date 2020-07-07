package {

	import flash.display.MovieClip;
	import flash.events.*;
	import flash.media.*;
	import flash.net.*;
	import flash.display.Stage;
	import flash.utils.*;

	public class Asteroid extends MovieClip {
		//asteroid Speed
		private var _speed: Number;
		private var _speedR: Number;
		//asteroid size
		private var _size: Number = 1.2;
		//life
		private var _life: int = 3;
		//screen width by default
		private var _stageWidth: uint = 800 /*Default*/ ;
		//lanes
		private var _nbLanes: uint = 6;
		private var _laneSize: uint = _stageWidth / _nbLanes;
		//Timer
		private var killTimer: Timer = new Timer(100, 0);
		//sound
		private var _soundHitURL: String = "sound_hit.mp3";
		private var _soundBoomURL: String = "sound_boom.mp3";

		public function Asteroid() {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
		}
		private function addedToStage(e: Event): void {
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.ENTER_FRAME, loop);
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			var _stageWidth = stage.stageWidth;
			spawn();
			y = -100;
		}
		private function spawn(): void {
			_speed = rand(4, 5);
			_speedR = rand(-10, 10) / 10;
			scaleX = scaleY = _size * (rand(-5, 0) / 10 + 1);
			y = -height;
			x = (rand(1, _nbLanes) * _laneSize) - (_laneSize / 2);
		}
		private function rand(min: int, max: int): int {
			return Math.floor(Math.random() * (max - min + 1)) + min;
		}
		public function destroy(): void {
			_life--;
			//if (_life != 0) {
				var soundHit: Sound = new Sound(new URLRequest("sound/" + _soundHitURL));
				var transHit: SoundTransform = new SoundTransform(0.2, 0);
				var channelHit: SoundChannel = soundHit.play(0, 1, transHit);
			//}
			if (_life == 2) {
				gotoAndStop(2);
			}
			if (_life == 1) {
				gotoAndStop(3);
			}
			if (_life == 0) {
				gotoAndPlay(4);
				var soundBoom: Sound = new Sound(new URLRequest("sound/" + _soundBoomURL));
				var transBoom: SoundTransform = new SoundTransform(0.5, 0);
				var channelBoom: SoundChannel = soundBoom.play(0, 1, transBoom);
				MovieClip(parent.parent).AsteroidDestroyed();
				killTimer.addEventListener(TimerEvent.TIMER, kill);
				killTimer.start();
			}
		}
		private function kill(e: TimerEvent): void {
			MovieClip(parent.parent).score_mc.killPts();
			remove();
		}

		private function remove(): void {
			killTimer.stop();
			removeEventListener(Event.ENTER_FRAME, loop);
			MovieClip(parent).removeChild(this);
			return;
		}
		private function loop(e: Event): void {
			//COLLISION
			var aircraft = MovieClip(parent.parent).groupAircraft_mc.getChildAt(0);
			if (aircraft.vBox.hitTestObject(this.box1) == true ||
				aircraft.vBox.hitTestObject(this.box2) == true ||
				aircraft.vBox.hitTestObject(this.box3) == true ||
				aircraft.vBox.hitTestObject(this.box4) == true ||
				aircraft.vBox.hitTestObject(this.box5) == true ||
				aircraft.hBox.hitTestObject(this.box1) == true ||
				aircraft.hBox.hitTestObject(this.box2) == true ||
				aircraft.hBox.hitTestObject(this.box3) == true ||
				aircraft.hBox.hitTestObject(this.box4) == true ||
				aircraft.hBox.hitTestObject(this.box5) == true) {
				aircraft.destroy();
				return;
			}
			//SPEEDS
			y += _speed;
			rotation += _speedR;
			//OUT OF BOUNDS
			if (y > stage.stageHeight + height) {
				remove();
				return;
			}
		}
		private function removedFromStage(e: Event): void {
			removeEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			removeEventListener(Event.ENTER_FRAME, loop);
		}
	} // classe   
} // package