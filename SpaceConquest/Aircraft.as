package {

	import flash.display.MovieClip;
	import flash.events.*;
	import flash.media.*;
	import flash.net.*;
	import flash.utils.*;

	public class Aircraft extends MovieClip {
		//Default variables
		private var sFps: uint = 60;
		private var sWidth: uint = 800;
		private var sHeight: uint = 600;
		//aircraft speed
		private var _speed: uint = 6;
		private var _speedX: int = 0;
		//aircraft state
		private var _state: Boolean;
		private const ACTIVE: Boolean = true;
		private const DESTROYED: Boolean = false;
		//asteroid weapon
		private var _pps: uint = 4; //projectiles per second
		private var _aspeed: uint = sFps / _pps; //Attack speed
		private var _reload: uint = 0; //reload timer
		private var _fired: Boolean = false; //ready to fire ?
		//misc
		private var _wallOffset: uint = 40; //distance between aircraft and wall
		//power ups
		private var _powerUpTimer: uint = 0;
		private var _powerUpDuration: uint = 6 * sFps;
		private var _powerUpActive: Boolean = false; //POWER UP
		private var _double: Boolean = false; //POWER UP
		private var _fastReload: Boolean = false; //POWER UP
		private var _ppsBoost: uint = 6; //POWER UP projectiles per second
		//Timer
		var killTimer: Timer = new Timer(500, 0)
		//Sound
			private var _soundPewURL: String = "sound_pew.mp3";
		private var _soundPowerURL: String = "sound.mp3";

		public function Aircraft() {
			_aspeed = 15;
			x = 400;
			y = 550;
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
		}
		//Page Selector
		private function addedToStage(e: Event): void {
			//stage variables
			sFps = stage.frameRate;
			sWidth = stage.stageWidth;
			sHeight = stage.stageHeight;
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.ENTER_FRAME, loop);
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			//Aircraft Instructions
			_state = ACTIVE;
			_aspeed = sFps / _pps;
			x = sWidth / 2;
			y = sHeight - 50;
		}
		//POWER UPS
		public function doubleFire(): void {
			if (_powerUpActive) {
				resetPowerUps();
			}
			_double = true;
			_powerUpActive = true;
			soundPower();
		}
		public function fastReload(): void {
			if (_powerUpActive) {
				resetPowerUps();
			}
			_fastReload = true;
			_aspeed = stage.frameRate / _ppsBoost;
			_powerUpActive = true;
			soundPower();
		}
		public function engineUpgrade(): void {
			if (_speed <= 10) {
				_speed += 2;
			}
			soundPower();
		}
		private function soundPower(): void {
			var soundPower: Sound = new Sound(new URLRequest("sound/" + _soundPowerURL));
			var transPower: SoundTransform = new SoundTransform(0.4, 0);
			var channelPower: SoundChannel = soundPower.play(0, 1, transPower);
		}
		private function resetPowerUps() {
			_powerUpTimer = 0;
			_powerUpActive = false;
			_double = false;
			_fastReload = false;
			_aspeed = stage.frameRate / _pps;
		}
		//MOVE CONTROLLS
		public function move_right(): void {
			if (_state != DESTROYED && x < stage.stageWidth - _wallOffset) {
				_speedX = _speed;
				MovieClip(parent)._right = true;
				move();
			} else {
				MovieClip(parent)._right = false;
			}
		}

		public function move_left(): void {
			if (_state != DESTROYED && x > 0 + _wallOffset) {
				_speedX = -_speed;
				MovieClip(parent)._left = true;
				move();
			} else {
				MovieClip(parent)._left = true;
			}
		}

		public function move_stop(): void {
			if (_state != DESTROYED) {
				_speedX = 0;
				move();
			}
		}
		private function move(): void {
			x += _speedX;
		}
		//FIRE MISSILE
		public function fire(mX: Number, mY: Number): void {
			if (_state != DESTROYED) {
				if (_fired == false) {
					_fired = true;
					if (_double) {
						var myMissileLeft: Missile = new Missile(mX, mY, x, y, true, true, false);
						MovieClip(parent.parent).addChild(myMissileLeft);
						var myMissileRight: Missile = new Missile(mX, mY, x, y, true, false, true);
						MovieClip(parent.parent).addChild(myMissileRight);
					} else if (_double == false) {
						var myMissile: Missile = new Missile(mX, mY, x, y, false, false, false);
						MovieClip(parent.parent).addChild(myMissile);
					}
					var soundPew: Sound = new Sound(new URLRequest("sound/" + _soundPewURL));
					var transPew: SoundTransform = new SoundTransform(0.4, 0);
					var channelPew: SoundChannel = soundPew.play(0, 1, transPew);
				}
			}
		}
		//KILL ME
		public function destroy(): void {
			if (_state != DESTROYED) {
				_state = DESTROYED;
				gotoAndStop(2);
				killTimer.start();
				killTimer.addEventListener(TimerEvent.TIMER, kill);
			}
		}
		function kill(e: TimerEvent): void {
			killTimer.stop();
			MovieClip(parent.parent).gotoGameOver();
		}
		//LOOP
		private function loop(e: Event): void {
			//FIRE
			if (_fired) {
				_reload++;
				if (_reload >= _aspeed) {
					_fired = false;
					_reload = 0;
				}
			}
			//POWER UP
			if (_powerUpActive) {
				if (_powerUpTimer < _powerUpDuration) {
					_powerUpTimer++;
				} else {
					resetPowerUps();
				}
			}
		}
		private function removedFromStage(e: Event): void {
			removeEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			removeEventListener(Event.ENTER_FRAME, loop);
		}
	}
}