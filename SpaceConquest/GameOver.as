package {
	
	import flash.display.MovieClip;
	import flash.events.*;
	import flash.media.*;
	import flash.net.*;
	
	public class GameOver extends MovieClip {

		//sound
		private var _songURL: String = "song_gameOver.mp3";
		
		public function GameOver() {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			btn_game.addEventListener(MouseEvent.CLICK, gotoGame);
			btn_menu.addEventListener(MouseEvent.CLICK, gotoMenu);
		}
		private function addedToStage(event: Event = null): void {
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			music();
		}
		private function music() {
			var soundtrack: Sound = new Sound(new URLRequest("sound/" + _songURL));
			var trans:SoundTransform = new SoundTransform(0.7, -1); 
			var channelSong:SoundChannel = soundtrack.play(0, 999, trans);
		}
		private function removedFromStage(event: Event): void {
			removeEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			SoundMixer.stopAll();
		}
		private function gotoGame(e: MouseEvent): void {
			MovieClip(parent).goto(this, "Game");
		}
		private function gotoMenu(e: MouseEvent): void {
			MovieClip(parent).goto(this, "Menu");
		}
	} //classe
} //package