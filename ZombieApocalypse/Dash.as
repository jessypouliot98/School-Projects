package {
	import flash.display.MovieClip;
	import flash.events.*;
	import flash.media.*;
	import flash.net.*;
	import flash.ui.*;

	public class Dash extends MovieClip {
		private var posX: int;
		private var posY: int;

		public function Dash(pX: int, pY: int) {
			mouseEnabled = false;
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			posX = pX;
			posY = pY;
			effect();
		}
		private function effect(): void {//sound effect
			var sound: Sound = new Sound(new URLRequest("sound/dash.mp3"));
			var trans: SoundTransform = new SoundTransform(0.7, 0);
			var channel: SoundChannel = sound.play(0, 1, trans);
		}
		private function addedToStage(e: Event): void {
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			x = posX;
			y = posY;
		}
	}
}