package {

	import flash.display.MovieClip;
	import flash.events.*;
	import flash.media.*;
	import flash.net.*;
	import flash.ui.*;

	public class Enemy extends MovieClip {
		//movement
		private var _speed: Number; //movement speed
		private var angle: Number; //direction (move in X angle)
		private var dH: uint; //distance between THIS and Target (enemy & character) (pythagore hypotenus)
		//attack cooldown
		private var attackCooldown: uint = 0;
		private const ACD: uint = 4; //
		//stats
		private const BASEHP: uint = 5; //base life for THIS
		private var maxHP: uint; //Total life BASEHP * difficulty, varies with level difficulty
		private var currentHP: int; //current life
		private var damage: uint = 25; //damage THIS deals to TARGET
		//misc
		private var difficulty: uint; //Difficulty of level is multiplied to life to make THIS stronger
		private var burnDuration: uint; //Duration of  a burning debuff on THIS
		private var burnDamage: uint; //Damage per tick* for the time THIS is burning

		public function Enemy(round: uint, tPos: Array) {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			difficulty = round;

			//position
			var position: uint = rand(0, 3);
			var rand = rand(-500, 500);

			x = tPos[0];
			y = tPos[1];
			if (position == 0) { //top
				y += 500;
				x += rand;
			} else if (position == 1) { //right
				y += rand;
				x += 500;
			} else if (position == 2) { //bottom
				y += -500;
				x += rand;
			} else if (position == 3) { //left
				y += rand;
				x += -500;
			}
		}


		//--PLACED ON SCENE--//
		private function addedToStage(e: Event): void {
			//events
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);

			//life
			maxHP = BASEHP * difficulty; //Sets total life of THIS
			currentHP = maxHP; //Sets THIS to his total life at this creation

			//sets a random speed for THIS at its creation
			var i: uint = rand(1, 3);
			if (i == 1) _speed = 2;
			if (i == 2) _speed = 1.75;
			if (i == 3) _speed = 1.5;

			ambiant();
		}

		private function ambiant(): void {
			var sound: Sound = new Sound(new URLRequest("sound/zombie_ambiant.mp3"));
			var trans: SoundTransform = new SoundTransform(0.5, 0);
			var channel: SoundChannel = sound.play(rand(0, 15000), 999, trans);
		}

		private function effect(): void {
			var sound: Sound = new Sound(new URLRequest("sound/zombie_sound.mp3"));
			var trans: SoundTransform = new SoundTransform(0.5, 0);
			var channel: SoundChannel = sound.play(0, 1, trans);
		}

		//--MOVE--//
		public function moveChar(gotoPos: Array): void { //tells THIS to go to x,y position and attack if THIS touches his TARGET
			var character = MovieClip(parent.parent.parent).charPlaceholder.getChildAt(0);

			var dX: Number = gotoPos[0] - x;
			var dY: Number = gotoPos[1] - y;
			angle = Math.atan2(dY, dX);
			dH = Math.sqrt(Math.pow(dX, 2) + Math.pow(dY, 2));
			rotation = angle * 180 / Math.PI;

			if (character.hitTestObject(this) == true) { //if THIS touches TARGET
				if (attackCooldown <= 0) {
					attack(damage); //atack TARGET
				}
			} else {
				var posT = x + y;
				for (var i: uint = 0; i < MovieClip(parent).numChildren; i++) {
					if (MovieClip(parent).getChildAt(i).hitTestObject(this) == true && MovieClip(parent).getChildAt(i) != this) {
						var enemy = MovieClip(parent).getChildAt(i);

						var vX: int = x - enemy.x;
						var vY: int = y - enemy.y;

						if (vX > 0) {
							x -= _speed;
							if (vY > 0) {
								y += _speed;
							} else {
								y -= _speed;
							}
						} else {
							x += _speed;
							if (vY > 0) {
								y += _speed;
							} else {
								y -= _speed;
							}
						}
					}
				}
				moveEnemy();
			}
		}

		public function moveEnemy(pushed: Boolean = false, distance: uint = 0, slowed: Boolean = false): void { //Modify THIS's position
			if (pushed) { //This got hit by a Knockback*
				x -= Math.cos(-angle) * (distance - dH);
				y += Math.sin(-angle) * (distance - dH);
			} else { //Simply move THIS to TARGET
				if (slowed) {
					x += Math.cos(angle) * _speed * 0.8;
					y += Math.sin(angle) * _speed * 0.8;
				} else {
					x += Math.cos(angle) * _speed;
					y += Math.sin(angle) * _speed;
				}
			}
		}


		//--ATTACK X--//
		private function attack(dmg: uint): void { //THIS attacks TARGET
			var character = MovieClip(parent.parent.parent).charPlaceholder.getChildAt(0);
			character.hit(dmg);
			attackCooldown = ACD;
		}

		//--X ATTACKED THIS--//
		public function hit(dmg: uint): void { //TARGET attacked THIS (reduce current life or kill THIS)
			var sound: Sound = new Sound(new URLRequest("sound/hit.mp3"));
			var trans: SoundTransform = new SoundTransform(0.7, 0);
			var channel: SoundChannel = sound.play(0, 1, trans);
			currentHP -= dmg;
			if (currentHP <= 0) {
				MovieClip(parent).removeChild(this);
			}
		}
		public function ignite(duration: uint, dmg: uint): void { //Sets a burn* on THIS for X duration
			burnDuration = duration;
			burnDamage = dmg;
		}
		private function burn(): void { //Burn* damages THIS for X damage
			hit(burnDamage);
			burnDuration--;
		}

		//--CLOCK--//
		public function cooldown(clock): void { //called from GAME each new frame (this is a loop)
			if (clock % 15 == 0) { //sets a delay of 1/4 of a sec
				if (attackCooldown > 0) { //sets a delay for next attack to TARGET
					attackCooldown--;
				}
				if (burnDuration > 0) { //Burn* THIS while burn* is active
					burn();
				}
			}
			if (clock % rand(360, 720) == 0) {
				effect();
			}
		}

		//--GETTER--//
		public function distance(): uint { //get distance between CHARACTER and THIS
			return dH;
		}

		//--MISC--//
		private function rand(min: int, max: int): int { //easy random number generation
			return Math.floor(Math.random() * (max - min + 1)) + min;
		}
		//--REMOVED FROM SCENE--//
		private function removedFromStage(e: Event): void {
			removeEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			SoundMixer.stopAll();
		}
	}
}