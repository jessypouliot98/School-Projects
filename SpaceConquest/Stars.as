package {

	import flash.display.MovieClip;
	import flash.events.*;

	public class Stars extends MovieClip {

		private var _speedY: Number;
		private var _speedX: Number;

		public function Stars() {
			addEventListener(Event.ADDED_TO_STAGE, init);
		}
		private function init(e: Event): void {
			removeEventListener(Event.ADDED_TO_STAGE, init);
			addEventListener(Event.ENTER_FRAME, loop);
			respawn();
			y = rand(-height, stage.stageHeight - 1);
		}

		private function respawn(): void {
			_speedY = rand(50, 500) / 220;
			_speedX = rand(-10, 10) / 140;
			scaleX = scaleY = _speedY / 3;
			y = -height;
			x = rand(-width, stage.stageWidth);
		}
		private function rand(min: int, max: int): int {
			return Math.floor(Math.random() * (max - min + 1)) + min;
		}
		private function loop(e: Event): void {
			y += _speedY;
			x += _speedX;
			if (y > stage.stageHeight || x > stage.stageWidth || x < 0) {
				respawn();
			}
		}
	} // classe   
} // package