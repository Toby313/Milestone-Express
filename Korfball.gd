extends RigidBody2D

var picked = false

var input

func _physics_process(delta):
	if picked == true:
		self.position = get_node("../Player/Marker2d").global_position
		
func _input(event):
	if input.is_action_just_pressed("ui_pick"):
		var bodies = $Area2D.get_overlapping_bodies()
		for body in bodies:
			if body.name == 'Player' and get_node("../Player").canPick == true:
				picked = true
				get_node("../Player").canPick = false
				
	if input.is_action_just_pressed("ui_drop") and picked == true:
		picked = false
		get_node("../Player").canPick = false
		if get_node("../Player").sprite.flip_h == false:
				apply_impulse(Vector2(), Vector2(90, -10))
		else:
			apply_impulse(Vector2(), Vector2(-90, -10))
		
	if input.is_action_just_pressed("ui_throw") and picked == true:
		picked = false
		get_node("../Player").canPick = true
		if get_node("../Player").sprite.flip_h == false:
			apply_impulse(Vector2(), Vector2(100, -125))
		else:
			apply_impulse(Vector2(), Vector2(-100, -125))
