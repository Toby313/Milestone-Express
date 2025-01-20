extends CharacterBody2D

@export var speed : float = 300  # speed in pixels/sec
@export var animation_tree: AnimationTree

var input : Vector2

var playback : AnimationNodeStateMachinePlayback

var canPick = true

func _ready():
	playback = animation_tree["parameters/playback"]

func _physics_process(delta: float) -> void:
	input = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
	velocity = input * speed
	move_and_slide()
	select_animation()
	update_animation_parameters()

func select_animation():
	if velocity == Vector2.ZERO:
		playback.travel("Idle")
	else:
		playback.travel("Walking")

func update_animation_parameters():
	if input == Vector2.ZERO:
		return
		
	animation_tree["parameters/Idle/blend_position"] = input
	animation_tree["parameters/Walking/blend_position"] = input
