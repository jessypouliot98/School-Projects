package {

	import flash.display.MovieClip;
	import flash.events.*;
	import flash.media.*;
	import flash.net.*;
	import flash.ui.*;

	public class Character extends MovieClip {

		//STATS
		//attacks
		private var attackRange: uint = 150;
		private var attackCooldown: uint = 0;
		private const ACD: uint = 1; //1/4 sec cooldown

		private const QCD: uint = 5 * 4; // 5sec cooldown
		private var qCooldown: uint = 0; //cooldown timer

		private const WCD: uint = 1 * 4; //5sec cooldown
		private var wCooldown: uint = 0; //cooldown timer
		private const WDURATION: uint = 5 * 4; //duration of W actived
		private var wTimer: uint = WDURATION; //W timer

		private const ECD: uint = 15 * 4; //15sec cooldown
		private var eCooldown: uint = 0; //cooldown timer

		private const RCD: uint = 30 * 4; //30sec cooldown
		private var rCooldown: uint = 0; //cooldown timer

		//healtth
		private var maxHP: uint; //THIS total life
		private var currentHP: int; //THIS current life
		private const REGEN: uint = 3; //THIS life regeneration per second
		//movement
		private const SPEED: Number = 3.5; //THIS movement speed
		//movements
		private var _move: Boolean; //is THIS moving yes/no
		private var angle: Number; //movement direction
		private var gotoPos: Array; //move to [x,y] coordinates

		public function Character() {
			addEventListener(Event.ADDED_TO_STAGE, addedToStage);
		}


		//--PLACED ON SCENE--//
		private function addedToStage(e: Event): void {
			//events
			removeEventListener(Event.ADDED_TO_STAGE, addedToStage);
			addEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
			//position
			x = stage.stageWidth / 2;
			y = stage.stageHeight / 2 + 100;
			//variables
			maxHP = 100; //sets total life
			currentHP = maxHP; //resets life to total life
			MovieClip(parent.parent).life_UI.show(currentHP, maxHP); //display life
			_move = false
		}


		//--MOVE--//
		public function moveChar(gotoX: int, gotoY: int): void { //set directions to move to mouse clicked destination
			var tile: uint = MovieClip(parent.parent).getTileSize(); //shortcut for tile

			//sets [x,y] coordinates for THIS to move to & enable movements
			gotoX = gotoX + tile / 2;
			gotoY = gotoY + tile / 2;
			gotoPos = [gotoX, gotoY];

			var dY: Number = gotoY - getPosMap()[1];
			var dX: Number = gotoX - getPosMap()[0];
			angle = Math.atan2(dY, dX);

			_move = true;
		}
		//stop moving
		public function moveStop(): void { //disable movements
			_move = false;
		}
		public function movePos(): void { //Moves "character" (map) to mouse clicked destination & check for obstacles
			var map = MovieClip(parent.parent).mapPlaceholder; //MAP shortcut

			var pX: int = getPosMap()[0]; //posision X on MAP
			var pY: int = getPosMap()[1]; //position Y on MAP

			var tileTopLeft: uint = map.getTileChild(pX, pY, -1, -1); //tile top left of THIS
			var tileTop: uint = map.getTileChild(pX, pY, -1, 0); //tile top of THIS
			var tileTopRight: uint = map.getTileChild(pX, pY, -1, 1); //tile top right of THIS

			var tileLeft: uint = map.getTileChild(pX, pY, 0, -1); //tile left of THIS
			var tileRight: uint = map.getTileChild(pX, pY, 0, 1); //tile right of THIS

			var tileBottomLeft: uint = map.getTileChild(pX, pY, 1, -1); //tile bottom left of THIS
			var tileBottom: uint = map.getTileChild(pX, pY, 1, 0); //tile bottom of THIS
			var tileBottomRight: uint = map.getTileChild(pX, pY, 1, 1); //tile bottom right of THIS

			if (_move) {
				var wall: uint = 11;
				if (angle <= 0) { //-Y
					if (angle <= -Math.PI / 2) { //-X
						//trace("Top left");

						if (map.tilePlaceholder.getChildAt(tileTop).currentFrame >= wall && map.tilePlaceholder.getChildAt(tileTop).hitTestObject(this)) return;
						if (map.tilePlaceholder.getChildAt(tileTopLeft).currentFrame >= wall && map.tilePlaceholder.getChildAt(tileTopLeft).hitTestObject(this)) return;
						if (map.tilePlaceholder.getChildAt(tileLeft).currentFrame >= wall && map.tilePlaceholder.getChildAt(tileLeft).hitTestObject(this)) return;
						moveCharacter(pX, pY);
					} else if (angle >= -Math.PI / 2) { //+X
						//trace("top right");

						if (map.tilePlaceholder.getChildAt(tileTop).currentFrame >= wall && map.tilePlaceholder.getChildAt(tileTop).hitTestObject(this)) return;
						if (map.tilePlaceholder.getChildAt(tileTopRight).currentFrame >= wall && map.tilePlaceholder.getChildAt(tileTopRight).hitTestObject(this)) return;
						if (map.tilePlaceholder.getChildAt(tileRight).currentFrame >= wall && map.tilePlaceholder.getChildAt(tileRight).hitTestObject(this)) return;
						moveCharacter(pX, pY);
					}
				} else if (angle >= 0) { //+Y
					if (angle <= Math.PI / 2) { //-X
						//trace("bottom right");

						if (map.tilePlaceholder.getChildAt(tileBottom).currentFrame >= wall && map.tilePlaceholder.getChildAt(tileBottom).hitTestObject(this)) return;
						if (map.tilePlaceholder.getChildAt(tileBottomRight).currentFrame >= wall && map.tilePlaceholder.getChildAt(tileBottomRight).hitTestObject(this)) return;
						if (map.tilePlaceholder.getChildAt(tileRight).currentFrame >= wall && map.tilePlaceholder.getChildAt(tileRight).hitTestObject(this)) return;
						moveCharacter(pX, pY);
					} else if (angle >= Math.PI / 2) { //+X
						//trace("bottom left");

						if (map.tilePlaceholder.getChildAt(tileBottomLeft).currentFrame >= wall && map.tilePlaceholder.getChildAt(tileBottomLeft).hitTestObject(this)) return;
						if (map.tilePlaceholder.getChildAt(tileBottom).currentFrame >= wall && map.tilePlaceholder.getChildAt(tileBottom).hitTestObject(this)) return;
						if (map.tilePlaceholder.getChildAt(tileLeft).currentFrame >= wall && map.tilePlaceholder.getChildAt(tileLeft).hitTestObject(this)) return;
						moveCharacter(pX, pY);
					}
				}
			}
		}
		private function moveCharacter(pX: int, pY: int) { //actually move THIS
			var map = MovieClip(parent.parent).mapPlaceholder; //MAP shortcut

			rotation = angle * 180 / Math.PI;
			//checks if arrived at destination in X axis //arrived at destination
			if (pX <= gotoPos[0] + SPEED && pX >= gotoPos[0] - SPEED) {
				//checks if arrived at destination in Y axis //arrived at destination
				if (pY >= gotoPos[1] - SPEED && pY <= gotoPos[1] + SPEED) {
					moveStop();
				} else { //not arrived at destination in Y axis
					map.moveMap(angle, SPEED);
				}
			} else { //not arrived at destination in X axis
				map.moveMap(angle, SPEED);
			}
		}


		//--ATTACK X--//
		public function attack(enemy, dmg: uint = 10): void { //normal attack (mouse click left/right)
			if (attackCooldown <= 0) {
				var damage: uint = dmg; //damage of this attack
				var map = MovieClip(parent.parent).mapPlaceholder; //MAP shortcut

				map.addChild(new Projectile(getPosMap()[0], getPosMap()[1], enemy, damage)); //add particles on SCENE
				attackCooldown = ACD;
			}
		}
		public function spellPiercingShot(): void {
			if (qCooldown <= 0) {
				var damage: uint = 1000; //damage of this spell
				var map = MovieClip(parent.parent).mapPlaceholder; //MAP shortcut

				map.addChild(new PiercingShot(getPosMap()[0], getPosMap()[1], stage.mouseX, stage.mouseY, damage)); //add particles on SCENE
				qCooldown = QCD;
			}
		}
		public function spellFireTrail(): void { //activates trail of fire that follows THIS and burns* TARGETS
			if (wCooldown <= 0) {
				wCooldown = WCD;
			}
		}
		private function fireTrail(): void { //sets a trail of fire that follows THIS and burns* TARGETS
			var map = MovieClip(parent.parent).mapPlaceholder; //MAP shortcut
			var damage: uint = 10; //damage of this spell
			map.addChild(new FireTrail(getPosMap()[0], getPosMap()[1], damage)); //add particles on SCENE
		}
		public function spellDash(): void { //dash THIS to last MOUSE CLICKED direction for a small distance
			if (eCooldown <= 0) {
				var map = MovieClip(parent.parent).mapPlaceholder; //MAP shortcut
				map.addChild(new Dash(getPosMap()[0], getPosMap()[1])); //add particles on SCENE
				map.moveMap(angle, 200);
				map.addChild(new Dash(getPosMap()[0], getPosMap()[1])); //add particles on SCENE
				eCooldown = ECD;
			}
		}
		public function spellKnockBack(): void { //Knocks every enemy in range back to the max range of this spell
			if (rCooldown <= 0) {
				//var damage: uint = 5;//damage of this spell
				var map = MovieClip(parent.parent).mapPlaceholder; //MAP shortcut

				map.addChild(new Knockback(getPosMap()[0], getPosMap()[1])); //add particles on SCENE
				rCooldown = RCD;
			}
		}


		//--X ATTACKED THIS--//
		public function hit(dmg: uint): void {
			var sound: Sound = new Sound(new URLRequest("sound/hit.mp3"));
			var trans: SoundTransform = new SoundTransform(0.7, 0);
			var channel: SoundChannel = sound.play(0, 1, trans);
			currentHP -= dmg;
			if (currentHP <= 0) kill();
			MovieClip(parent.parent).life_UI.show(currentHP, maxHP);
		}
		private function kill(): void { //"kills THIS and goes to game over screen"
			MovieClip(parent.parent).gotoGameOver();
		}


		//--GETTER--//
		public function getPosMap(): Array { //get character position on map
			var map = MovieClip(parent.parent).mapPlaceholder.getPos();
			var posX = (-map[0] + x);
			var posY = (-map[1] + y);
			return [posX, posY];
		}


		//--SETTER--//
		public function cooldown(clock): void { //called from GAME each new frame (this is a loop)
			if (clock % 15 == 0) {
				if (attackCooldown > 0) {
					attackCooldown--;
				}
				if (qCooldown > 0) {
					qCooldown--;
				}
				if (wCooldown > 0) {
					wCooldown--;
					wTimer = 0;
				}
				if (wTimer < WDURATION) {
					fireTrail();
					wTimer++;
				}
				if (eCooldown > 0) {
					eCooldown--;
				}
				if (rCooldown > 0) {
					rCooldown--;
				}
			}
		}
		public function regenHP(clock): void { //adds small portion of missing life per second
			if (clock % 15 == 0) {
				if (currentHP >= maxHP) {
					currentHP = maxHP;
					MovieClip(parent.parent).life_UI.show(currentHP, maxHP);
				} else {
					currentHP += REGEN;
					if (currentHP >= maxHP) currentHP = maxHP;
					MovieClip(parent.parent).life_UI.show(currentHP, maxHP);
				}
			}
		}


		//--REMOVED FROM SCENE--//
		private function removedFromStage(e: Event): void {
			removeEventListener(Event.REMOVED_FROM_STAGE, removedFromStage);
		}
	}
}