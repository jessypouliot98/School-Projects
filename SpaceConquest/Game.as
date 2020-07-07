package {

	import flash.display.MovieClip;
	import flash.events.*;
	import flash.media.*;
	import flash.net.*;
	import flash.ui.*;

	public class Game extends MovieClip {
		//deafault fps
		private var sFPS:uint = 60;
		//sound
		private var _songURL: String = "song_game.mp3";
		//Control vars
		private var _left: Boolean = false;
		private var _right: Boolean = false;
		private var _action: Boolean = false;
		//aircraft object
		private var aircraft_mc: Aircraft;
		//asteroid generation variables
		private var _nbAsteroid: uint;
		private var _nbAsteroidGenerated: uint = 0;
		private var _generateAsteroidTimer: uint = 0;
		private var _asteroidPerSecond: Number = 1.5;
		private var _generateAsteroidDelay: uint = sFPS / _asteroidPerSecond;
		private var _nbAsteroidDestroyed: uint = 0;
		//nb asteroid to PU
		private var _makePowerUp: uint = 10;
		//Score
		private var _score: uint = 0;

		public function Game() {
			//Pages selector
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			btn_menu.addEventListener(MouseEvent.CLICK, gotoMenu);
		}
		//PAGES
		private function addedToStage(e: Event): void {
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.ENTER_FRAME, loop);
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			stage.focus = stage;
			//Variables
			sFPS = stage.frameRate;
			aircraft_mc = new Aircraft();
			_score = 0;
			_nbAsteroidGenerated = 0;
			//Generations
			groupAircraft_mc.addChild(aircraft_mc);
			Main(root).addChild(new Score_UI());
			music();
		}
		private function music() {
			var soundtrack: Sound = new Sound(new URLRequest("sound/" + _songURL));
			var trans:SoundTransform = new SoundTransform(0.7, -1); 
			var channelSong:SoundChannel = soundtrack.play(2000, 999, trans);
		}
		//GENERATIONS
		private function generateAsteroid(): void {
			groupAsteroid_mc.addChild(new Asteroid());
		}
		private function updateAPS():void{
			if(_asteroidPerSecond >= 4){
				return;
			}
			_asteroidPerSecond = _asteroidPerSecond*1.1;
			_generateAsteroidDelay = sFPS / _asteroidPerSecond;
		}
		private function generatePowerUp(): void {
			groupPowerUp_mc.addChild(new PowerUp());
		}
		//POWER UP
		public function AsteroidDestroyed(): void {
			_nbAsteroidDestroyed++
			if (_nbAsteroidDestroyed == _makePowerUp) {
				generatePowerUp();
				_nbAsteroidDestroyed = 0;
			}
		}
		//CONTROLS
		public function key_up(e): void {
			if (e == Keyboard.D) {
				_right = false;
			}
			if (e == Keyboard.A) {
				_left = false;
			}

			if (_right == false && _left == false) {
				aircraft_mc.move_stop();
			}
		}
		public function key_down(e): void {
			if (e == Keyboard.D) {
				if (_right == false) {
					aircraft_mc.move_right();
					_right = true;
				}
			}
			if (e == Keyboard.A) {
				if (_left == false) {
					aircraft_mc.move_left();
					_left = true;
				}
			}
		}
		public function mouse_up(): void {
			_action = false;
		}
		public function mouse_down(): void {
			_action = true;
		}
		//LOOP
		private function loop(e: Event) {
			//ASTEROID GENERATION
			_generateAsteroidTimer++;
			if (_generateAsteroidTimer == _generateAsteroidDelay) {
				generateAsteroid();
				_nbAsteroidGenerated++;
				if(_nbAsteroidGenerated == 10){
					updateAPS();
					_nbAsteroidGenerated = 0;
				}
				_generateAsteroidTimer = 0;
			}
			//AIRCRAFT AUTO-FIRE
			if (_action == true) {
				aircraft_mc.fire(Main(parent).mousePosX(), Main(parent).mousePosY());
			}
			//AIRCRAFT MOVE
			if (_left == true) {
				aircraft_mc.move_left();
			}
			if (_right == true) {
				aircraft_mc.move_right();
			}
		}
		//REMOVE
		private function removedFromStage(e: Event): void {
			removeEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			removeEventListener(Event.ENTER_FRAME, loop);
			groupAsteroid_mc.removeChildren();
			groupPowerUp_mc.removeChildren();
			groupAircraft_mc.removeChildren();
			SoundMixer.stopAll();
		}
		public function gotoGameOver(): void {
			MovieClip(parent).goto(this, "GameOver");
		}
		private function gotoMenu(e: MouseEvent): void {
			MovieClip(parent).goto(this, "Menu");
		}
	} //classe
} //package