package {
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.media.*;
	import flash.net.*;
	import flash.ui.*;

	public class PiercingShot extends MovieClip {

		private var _speed: int = 10; //Movement speed
		private var gotoX: int; //Target position X
		private var gotoY: int; //Target position Y
		private var fromX: int; //current position X
		private var fromY: int; //current position Y
		private var angle: Number; // angle in rad
		private var travelDistance: uint = 0; //Total distance THIS travelled
		private var damage: uint; //This damage to TARGET
		private var maxRange: uint = 400; // max range projectile can travel before being destroyed

		public function PiercingShot(fX: int, fY: int, mX: int, mY: int, dmg: uint) {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			gotoX = mX; //mouse position X
			gotoY = mY; //mouse position Y
			fromX = fX; //current position X
			fromY = fY; //current position Y
			damage = dmg; //This damage to TARGET
			effect();
		}
		
		private function effect(): void {
			var sound: Sound = new Sound(new URLRequest("sound/fireball.mp3"));
			var trans: SoundTransform = new SoundTransform(0.7, 0);
			var channel: SoundChannel = sound.play(0, 1, trans);
		}
		
		//--ADDED TO SCENE--//
		private function addedToStage(e: Event): void {
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			addEventListener(Event.ENTER_FRAME, loop);
			x = fromX; //sets position X when created
			y = fromY; //sets position Y when created
			gotoX -= MovieClip(parent).getPos()[0];
			gotoY -= MovieClip(parent).getPos()[1];
			//projectile direction
			var dY: Number = gotoY - y;
			var dX: Number = gotoX - x;
			angle = Math.atan2(dY, dX);
		}


		//--MOVEMENT & ATTACK X--/
		private function loop(e: Event): void { //Moves THIS towards TARGET & if THIS hits TARGET, remove THIS and damage TARGET
			//Move proj
			y += Math.sin(angle) * _speed;
			x += Math.cos(angle) * _speed;
			rotation = radToDeg(angle);
			//COLLISION
			travelDistance += _speed;
			var enemy_mc = MovieClip(parent).enemyPlaceholder;
			for (var i: uint = 0; i < enemy_mc.numChildren; i++) {
				if (enemy_mc.getChildAt(i).hitTestObject(this) == true) {
					enemy_mc.getChildAt(i).hit(damage);
				}
				if (travelDistance >= maxRange) {
					MovieClip(parent).removeChild(this);
					return;
				}
			}
		}


		//--REMOVED FROM SCENE--//
		private function removedFromStage(e: Event): void {
			removeEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			removeEventListener(Event.ENTER_FRAME, loop);
		}


		//--SHORTCUTS--//
		private function radToDeg(rad: Number): Number { //converts radian to degrees
			var deg = rad * 180 / Math.PI;
			return deg;
		}
	} //class
} //package