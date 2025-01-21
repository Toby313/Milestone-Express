extends Area2D

@export var next_scene_path: String = "res://Act2.tscn"

@onready var win_area: Area2D = $"winArea."

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	win_area.get_overlapping_areas()
	pass

	#func 
