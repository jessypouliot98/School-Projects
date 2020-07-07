package {
	import flash.display.MovieClip;
	import flash.events.Event;

	public class Missile extends MovieClip {

		private var _speed: int = 10;
		private var posX: Number;
		private var posY: Number;
		private var angle: Number;

		public function Missile(mX: Number, mY: Number, pX, pY, double, left = false, right = false) {
			addEventListener(Event.ENTER_FRAME, loop);
			//double proj if power up active
			if (double && left) {
				x = pX - 20;
				y = pY - 10;
			}
			if (double && right) {
				x = pX + 20;
				y = pY - 10;
			} 
			//Single proj
			else if (double == false) {
				x = pX;
				y = pY - 23;
			}
			//Mouse pos
			posX = mX;
			posY = mY;
			//missile angle direction
			var dY: Number = posY - y;
			var dX: Number = posX - x;
			angle = Math.atan2(dY, dX);
			
			//ANTI-SHOOT BEHIND
			var angleRestriction:Number = 70;
			var leftAngle: Number = -(90 - angleRestriction);
			var rightAngle: Number = -(90 + angleRestriction);
			if (angle > degToRad(leftAngle) && angle < degToRad(90)) {
				angle = degToRad(leftAngle);
			}
			if (angle < degToRad(rightAngle) || angle > degToRad(90)) {
				angle = degToRad(rightAngle);
			}
		}
		private function degToRad(deg: Number): Number {
			var rad = deg * Math.PI / 180;
			return rad;
		}
		private function loop(e: Event): void {
			//Move proj
			y += Math.sin(angle) * _speed;
			x += Math.cos(angle) * _speed;
			//OUT OF BOUNDS
			if (y < 0 || x < 0 || x > 800) {
				removeEventListener(Event.ENTER_FRAME, loop);
				MovieClip(parent).removeChild(this);
				return;
			} else {
				//COLLISION
				for (var i: uint = 0; i < MovieClip(parent).groupAsteroid_mc.numChildren; i++) {
					var asteroid = MovieClip(parent).groupAsteroid_mc.getChildAt(i);
					if (asteroid.hitTestObject(this)) {
						//REMOVE ENTITY
						asteroid.destroy();
						MovieClip(parent).removeChild(this);
						removeEventListener(Event.ENTER_FRAME, loop);
						break;
					}
				}
			}
		}
	} //class
} //package