package {

	import flash.display.MovieClip;
	import flash.events.*;
	import flash.text.*;

	public class Score_UI extends MovieClip {

		private var score: uint;
		private var newGame: String;
		private var pts:uint = 250;//pts per asteroid destroyed

		public function Score_UI(newG: Boolean = false) {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
		}
		private function addedToStage(e: Event): void {
			newGame = Main(root).activeScreen();
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			if (newGame == "Game") {
				Main(root).postScore(0);
				score = Main(root).getScore();
				addEventListener(Event.ENTER_FRAME, loop);
			} else if (newGame == "GameOver") {
				score = Main(root).getScore();
				score_txt.text = displaySix(score);
			}
		}
		private function loop(e: Event): void {
			score++;
			Main(root).postScore(score);
			score_txt.text = displaySix(score);
		}
		public function killPts():void{
			score+= pts;
			Main(root).postScore(score);
			score_txt.text = displaySix(score);
		}
		private function removedFromStage(e: Event): void {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			removeEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			removeEventListener(Event.ENTER_FRAME, loop);
		}
		private function displaySix(num: uint): String {
			var numString: String;
			if (num < 10) {
				numString = "00000" + num;
			} else if (num < 100) {
				numString = "0000" + num;
			} else if (num < 1000) {
				numString = "000" + num;
			} else if (num < 10000) {
				numString = "00" + num;
			} else if (num < 100000) {
				numString = "0" + num;
			} else {
				numString = "" + num;
			}
			return numString;
		}
	}
}