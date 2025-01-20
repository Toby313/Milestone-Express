extends Area2D


@export var label: Label
var tween_duration: float = 0.5


func _ready() -> void:
	body_entered.connect(_on_body_entered)
	body_exited.connect(_on_body_exited)
	label.modulate = Color.TRANSPARENT


func _on_body_entered(body: Node) -> void:
	var tween: Tween = create_tween()
	tween.tween_property(label, "modulate", Color.WHITE, tween_duration)


func _on_body_exited(body: Node) -> void:
	var tween: Tween = create_tween()
	tween.tween_property(label, "modulate", Color.TRANSPARENT, tween_duration)
