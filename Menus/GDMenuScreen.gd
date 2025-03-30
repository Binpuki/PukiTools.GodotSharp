class_name GDMenuScreen extends GDScreen

## A template for a menu. Relies heavily on Control nodes. Good for when the menu is meant to be the main screen.

@export var initial_focus : Control ## The Control node that's initially focused on when bringing up this menu.
@export var focusable : Array[Control] ## Every Control node in the menu that can be focused on.
@export var unfocus_on_mouse_exit : bool = false ## If toggled, triggers [method update_selection] with null arguments when the mouse exits a menu object.

func _ready() -> void:
	super()
	
	if not is_loaded():
		return
		
	for i in focusable.size():
		var cur : Control = focusable[i]
		cur.mouse_entered.connect(cur.grab_focus)
		cur.focus_entered.connect(func() -> void: update_selection(cur))
	
		if unfocus_on_mouse_exit:
			cur.mouse_exited.connect(func() -> void: update_selection(null))
	
func update_selection(focused : Control) -> void: ## Triggered every time a menu option is focused.
	pass