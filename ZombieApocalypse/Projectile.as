package {
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.media.*;
	import flash.net.*;
	import flash.ui.*;
	
	public class Projectile extends MovieClip {

		private var _speed: int = 10; //Movement speed
		//private var gotoX: int; //Target position X
		//private var gotoY: int; //Target position Y
		private var fromX: int; //current position X
		private var fromY: int; //current position Y
		private var enemy_mc: Enemy; //Target to follow
		private var travelDistance: uint = 0; //Total distance THIS travelled
		private var maxRange: uint; // max range of projectile before being destroyed
		private var damage: uint; //This damage to TARGET

		public function Projectile(fX: int, fY: int, target, dmg: uint) {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			//gotoX = target.x; //Target position X
			//gotoY = target.y; //Target position Y
			fromX = fX; //current position X
			fromY = fY; //current position Y
			enemy_mc = target; //Target to follow
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
			maxRange = Math.sqrt(Math.pow(enemy_mc.x - x, 2) + Math.pow(enemy_mc.y - y, 2));
		}


		//--MOVEMENT & ATTACK X--/
		private function loop(e: Event): void { //Moves THIS towards TARGET & if THIS hits TARGET, remove THIS and damage TARGET
			//projectile direction
			var dY: Number = enemy_mc.y - y;
			var dX: Number = enemy_mc.x - x;
			var angle: Number = Math.atan2(dY, dX);
			//Move proj
			y += Math.sin(angle) * _speed;
			x += Math.cos(angle) * _speed;
			rotation = radToDeg(angle);
			//COLLISION
			travelDistance += _speed;
			if (enemy_mc.hitTestObject(this) == true) {
				enemy_mc.hit(damage);
				MovieClip(parent).removeChild(this);
			}
			if (travelDistance >= maxRange) {
				MovieClip(parent).removeChild(this);
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