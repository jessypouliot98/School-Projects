package {
	import flash.display.MovieClip;
	import flash.events.*;
	import flash.media.*;
	import flash.net.*;
	import flash.ui.*;

	public class FireTrail extends MovieClip {
		private var posX: int; //position in X axis
		private var posY: int; //position in Y axis
		private const DURATION: uint = 3 * 4; //3sec //Duration of Burn*
		private const DAMAGE: uint = 2; //Damage of Burn*

		public function FireTrail(pX: int, pY: int, dmg: uint) {
			mouseEnabled = false;
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			posX = pX;
			posY = pY;
		}


		//--PLACED ON SCENE--//
		private function addedToStage(e: Event): void {
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.ENTER_FRAME, loop);
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			x = posX;
			y = posY;
			effect();
		}
		//sound effect
		private function effect(): void {
			var sound: Sound = new Sound(new URLRequest("sound/burning.mp3"));
			var trans: SoundTransform = new SoundTransform(0.7, 0);
			var channel: SoundChannel = sound.play(rand(0,13000), 1, trans);
		}
		//--LOOP--//
		private function loop(e: Event): void { //Checks if it enemy steps on THIS and burns THIS if hit
			for (var i: uint = 0; i < MovieClip(parent).enemyPlaceholder.numChildren; i++) {
				if (MovieClip(parent).enemyPlaceholder.getChildAt(i).hitTestObject(this) == true) {
					MovieClip(parent).enemyPlaceholder.getChildAt(i).ignite(DURATION, DAMAGE);
				}
			}
		}
		//--REMOVED FROM SCENE--//
		private function removedFromStage(e: Event): void {
			removeEventListener(Event.ENTER_FRAME, loop);
			SoundMixer.stopAll();
		}
		//--MISC--//
		private function rand(min: int, max: int): int { //easy random number generation
			return Math.floor(Math.random() * (max - min + 1)) + min;
		}
	}
}