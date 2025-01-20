extends RigidBody2D

var initial_ball_speed = 100
var Ball_speed = initial_ball_speed

var hitcounter = 0
var max_hitcounter = 1
var direction = Vector 2

func _ready():
	
	direction = Vector2(random_x, rand_range(-1, 1))
	direction = direction.normalized() * Ball_speed
	
func _physics_process(delta):
	move_and_collide(direction * delta)
