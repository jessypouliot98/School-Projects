package {
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.media.*;
	import flash.net.*;
	import flash.ui.*;

	public class Knockback extends MovieClip {
		private var posX: int;
		private var posY: int;

		public function Knockback(pX: int, pY: int) {
			mouseEnabled = false;
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			posX = pX;
			posY = pY;
			effect();
		}
		
		private function effect(): void {
			var sound: Sound = new Sound(new URLRequest("sound/boom.mp3"));
			var trans: SoundTransform = new SoundTransform(1, 0);
			var channel: SoundChannel = sound.play(0, 1, trans);
		}

		//--PLACED ON SCENE--//
		private function addedToStage(e: Event): void {
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.ENTER_FRAME, loop);
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage)
			x = posX;
			y = posY;
		}


		//--LOOP--//
		private function loop(e: Event): void { //Checks if it enemy steps on THIS and burns THIS if hit
			for (var i: uint = 0; i < MovieClip(parent).enemyPlaceholder.numChildren; i++) { //searches in every enemy on stage
				var enemy = MovieClip(parent).enemyPlaceholder.getChildAt(i); //shortcut for TARGET ENEMY
				if (MovieClip(parent).enemyPlaceholder.getChildAt(i).hitTestObject(this) == true) { //if THIS hits TARGET
					enemy.moveEnemy(true, width / 2); //push TARGET away from THIS
				}
			}
		}


		//--REMOVED FROM SCENE--//
		private function removedFromStage(e: Event): void {
			removeEventListener(Event.ENTER_FRAME, loop);
		}
	}
}